////////////////////////////////////////////////////////////////////////////////////
/// @file PartieRapide.cpp
/// @author Nicolas Blais, Jérôme Daigle
/// @date 2015-03-17
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "PartieRapide.h"
#include "Configuration/Config.h"
#include "Affichage/Affichage.h"
#include <FTGL/ftgl.h>
#include "Reseau/NetworkManager.h"
#include "Sons/ClasseSons.h"
#include "Joueurs/JoueurManagerLocal.h"
#include "Joueurs/JoueurManagerEnLigne.h"

#include "Event/CollisionEvent.h"
#include "Event/NewBallEvent.h"
#include "Event/EventManager.h"
#include "Event/BallLostEvent.h"
#include "Event/TimeEvent.h"
#include "Event/ScoreSyncEvent.h"
#include "Event/DisconnectEvent.h"
#include "Event/SyncAllRequest.h"


////////////////////////////////////////////////////////////////////////
///
/// @fn PartieRapide::PartieRapide(bool nbJoueurs, bool humain, std::string chemin)
///
/// Constructeur de la partie rapide. Permet d'assigner le nombre de joueurs, 
/// si le deuxième joueur est humain et le chemin du fichier xml.
///
/// @param[in] nbJoueurs : le nombre de joueurs ( 1 si 1 joueur et 0 si 2 joueurs
///			   humain : si le deuxième joueur est un humain(1) ou une intelligence artificielle(0)
///			   chemin : le chemin du fichier xml de la partie
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
PartieRapide::PartieRapide(int nbJoueurs, bool humain, string chemin){
	EventManager::GetInstance()->subscribe(this, TIMEEVENT);
	EventManager::GetInstance()->subscribe(this, BALLLOST);
	EventManager::GetInstance()->subscribe(this, COLLISIONEVENT);
	EventManager::GetInstance()->subscribe(this, NEWBALL);


	zoneCourante_ = new ZoneDeJeu(chemin);
	zoneCourante_->load();

	joueurManager_ =new JoueurManagerLocal(nbJoueurs, !humain); 

	auto controles = zoneCourante_->obtenirControles();
	joueurManager_->assignerControles(controles,zoneCourante_);
	estTerminee_ = false;
	estGagnee_ = false;
	pointage_ = 0;
	pointageDerniereBille_ = 0;

	estHost_ = false;
	partieEnLigne_ = false;

}

PartieRapide::PartieRapide(vector<string> playerIds, string zone, int nb_ai, bool estHost, string matchId, bool estRejoin )
{
	zoneCourante_ = new ZoneDeJeu(zone);
	zoneCourante_->load();
	
	joueurManager_ = new JoueurManagerEnLigne(playerIds,nb_ai,true, estHost);

	auto controles = zoneCourante_->obtenirControles();
	joueurManager_->assignerControles(controles, zoneCourante_);
	estTerminee_ = false;
	estGagnee_ = false;
	pointage_ = 0;
	pointageDerniereBille_ = 0;

	matchId_ = matchId;
	estHost_ = estHost;

	partieEnLigne_ = true;
	estTerminee_ = false;
	estGagnee_ = false;


	if (estRejoin)
	{
		EventManager::GetInstance()->subscribe(this, TIMEEVENT);
		EventManager::GetInstance()->subscribe(this, SYNCALL);
		NetworkManager::getInstance()->requestGlobalSync();
	}
	else 
	{
		EventManager::GetInstance()->subscribe(this, BALLLOST);
		EventManager::GetInstance()->subscribe(this, COLLISIONEVENT);
		EventManager::GetInstance()->subscribe(this, NEWBALL);
		EventManager::GetInstance()->subscribe(this, SCORESYNC);
		EventManager::GetInstance()->subscribe(this, TIMEEVENT);

		
		if (estHost_)
		{
			EventManager::GetInstance()->subscribe(this, SYNCALLREQUEST);
			EventManager::GetInstance()->subscribe(this, DISCONNECTEVENT);
		}
	}
}

PartieRapide::~PartieRapide()
{
	EventManager::GetInstance()->unsubscribe(this, TIMEEVENT);
	EventManager::GetInstance()->unsubscribe(this, BALLLOST);
	EventManager::GetInstance()->unsubscribe(this, COLLISIONEVENT);
	EventManager::GetInstance()->unsubscribe(this, NEWBALL);
	EventManager::GetInstance()->unsubscribe(this, SCORESYNC);
	EventManager::GetInstance()->unsubscribe(this, SYNCALL);
	EventManager::GetInstance()->unsubscribe(this, SYNCALLREQUEST);
	EventManager::GetInstance()->unsubscribe(this, DISCONNECTEVENT);

}

////////////////////////////////////////////////////////////////////////
///
/// @fn bool Partie::demarrerPartie()
///
/// Cette fonction permet de démarrer une partie.
///
/// @param[in] Aucun.
///
/// @return Si une partie est démarrée.
///
////////////////////////////////////////////////////////////////////////
bool PartieRapide::demarrerPartie()
{
	pointage_ = 0;
	pointageDerniereBille_ = 0;

	nbBillesReserve_ = Config::obtenirInstance()->getNbBilles();
	ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_MUSIQUE, true, 0);
	return PartieBase::demarrerPartie();
	
}

NoeudBille* PartieRapide::lancerBille()
{
	if (estHost_ || !partieEnLigne_) // Multijoueur
	{
		tempsAvantProchaineBille_ = 1.5;
		nbBillesEnJeu_++;
		auto bille = zoneCourante_->lancerBille();
		nbBillesReserve_--;
		if(partieEnLigne_)
		{
			auto pos = bille->obtenirPositionRelative();
			auto vit = bille->obtenirVitesse();
			NetworkManager::getInstance()->lancerBille(bille->obtenirId(), pos.x, pos.y, vit.x, vit.y);
		}
		return bille;
	}
	return nullptr;
}

void PartieRapide::perdreBille()
{
	PartieBase::perdreBille();
	if (nbBillesReserve_ == 0 && nbBillesEnJeu_ == 0)
	{
		estTerminee_ = true;
		ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_GAME_OVER, false);
	}
}

void PartieRapide::lancerBilleEnLigne(int id, glm::dvec3 pos, glm::dvec2 vitesse, double scale)
{
	tempsAvantProchaineBille_ = 1.5;
	nbBillesEnJeu_++;
	nbBillesReserve_--;
	auto bille = zoneCourante_->lancerBille(id, pos, vitesse, false);
	bille->assignerAgrandissement(glm::dvec3(scale));
}

//Pour synchroniser lorsque un joueur REjoins une partie
void PartieRapide::toutSynchroniser(SyncAllEvent* SyncEvent)
{
	auto eventManager = EventManager::GetInstance();
	for each(auto bille in SyncEvent->billes_en_jeu())
	{
		eventManager->throwEvent(&bille);
	}
	for each(auto cible in SyncEvent->cibles_activees())
	{
		eventManager->throwEvent(&cible);
	}
	for each(auto powerUpActif in SyncEvent->power_up_actifs())
	{
		eventManager->throwEvent(&powerUpActif);
	}
	for each(auto palette in SyncEvent->palettes())
	{
		eventManager->throwEvent(&palette);
	}

	pointage_ = SyncEvent->pointages()[0];
	nbBillesReserve_ = SyncEvent->nb_billes_restantes()[0];
}

void PartieRapide::envoiSynchronisationGlobale()
{
	auto zoneNum = 0;
	auto pointage = vector<int>(1, pointage_);
	auto nbBillesRest = vector<int>(1, nbBillesReserve_);
	auto billes = zoneCourante_->obtenirEtatBilles();
	auto cibles = zoneCourante_->obtenirEtatCibles();
	auto powerUps = zoneCourante_->obtenirEtatPowerUps();
	auto palettes = static_cast<JoueurManagerEnLigne *> (joueurManager_)->obtenirEtatPalettes();

	NetworkManager::getInstance()->SyncAll(zoneNum, pointage, nbBillesRest, billes, cibles, powerUps, palettes);
}

void PartieRapide::tick(double dt)
{
	if (estHost_ || !partieEnLigne_)
	{
		if (tempsAvantProchaineBille_ > 0)
			tempsAvantProchaineBille_ -= dt;

		if (tempsAvantProchaineBille_ <= 0 &&
			nbBillesReserve_ > 0 &&
			(nbBillesEnJeu_ == 0 ||
				(nbBillesEnJeu_ == 1 && Config::obtenirInstance()->getMode2Billes())))
		{
			lancerBille();
		}
	}
	if(partieEnLigne_)
	{
		NetworkManager::getInstance()->SyncReceivedMessages();
	}
	zoneCourante_->animer(dt);
}


void PartieRapide::afficherPointage() const
{

	glUniform1i(glGetUniformLocation(Affichage::obtenirInstance()->obtenirProgNuanceur(), "avecShader"), false);
	int cloture[4];
	glGetIntegerv(GL_VIEWPORT, cloture);

	FTGLPixmapFont font("C:/Windows/Fonts/Arial.ttf");
	FTPoint coordScore(10, cloture[1] + cloture[3] - 40, 0);
	FTPoint coordVies(10, cloture[1] + cloture[3] - 60, 0);
	if (font.Error()){
		cout << "Probleme de police dans partieRapide.cpp" << endl;
		return;
	}

	glPushAttrib(GL_ALL_ATTRIB_BITS);
	glPushMatrix();
	glLoadIdentity();
	glColor4f(1, 0, 0, 1);
	font.FaceSize(16);

	char score[255];
	char vies[255];
	sprintf_s(score, "Score:  %d", pointage_);
	font.Render(score, -1, coordScore);
	sprintf(vies, "Vies Restantes: %d ", nbBillesReserve_);
	font.Render(vies, -1, coordVies);

	glPopMatrix();
	glPopAttrib();

	glUniform1i(glGetUniformLocation(Affichage::obtenirInstance()->obtenirProgNuanceur(), "avecShader"), true);
}

void PartieRapide::afficher()
{
	PartieBase::afficher();
	//afficherPointage();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void PartieRapide::analyserPointage()
///
/// Analyse du pointage à savoir si le pointage permet d'obtenir une nouvelle bille
///
/// @param[in] Aucun.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void PartieRapide::analyserPointage()
{
	if (!partieEnLigne_ || estHost_)
	{
		int pointageActuel = pointage_ - pointageDerniereBille_ + (pointageDerniereBille_ % (zoneCourante_->getPointBilleGratuite()));
		if (pointage_ >= zoneCourante_->getPointBilleGratuite())
		{
			while (pointageActuel >= zoneCourante_->getPointBilleGratuite())
			{
				pointageActuel -= zoneCourante_->getPointBilleGratuite();
				nbBillesReserve_++;
				ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_BILLE_GRATUITE, false);
			}
			if (nbBillesEnJeu_ == 1 && Config::obtenirInstance()->getMode2Billes())
			{
				lancerBille();
				nbBillesReserve_--;
			}
		}
		pointageDerniereBille_ = pointage_;
	}
}


void PartieRapide::update(IEvent * e)
{
	switch (e->getType())
	{
	case COLLISIONEVENT:
	{
		CollisionEvent* collision = static_cast<CollisionEvent*>(e);
		string noeudCollision = collision->getTypeNoeud();
		if (noeudCollision == ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE)
			pointage_ += int(zoneCourante_->getPointButoirCercle()*collision->getFacteurMultiplicatif());
		else if (noeudCollision == ArbreRenduINF2990::NOM_BUTOIRTRIANGULAIREDROIT
			|| noeudCollision == ArbreRenduINF2990::NOM_BUTOIRTRIANGULAIREGAUCHE)
			pointage_ += int(zoneCourante_->getPointButoirTriangle()*collision->getFacteurMultiplicatif());
		else if (noeudCollision == ArbreRenduINF2990::NOM_CIBLE)
			pointage_ += int(zoneCourante_->getPointCible()*collision->getFacteurMultiplicatif());

		//lancer event pour sync score - si multijoueur
		if (estHost_)
		{
			NetworkManager::getInstance()->SyncScore(0, pointage_);
			analyserPointage();
		}
	}
		break;
	case BALLLOST: 
	{
		BallLostEvent* ballEvent = static_cast<BallLostEvent*>(e);
		zoneCourante_->perdreBille(ballEvent->getBallId());
		perdreBille();
	}
		break;
	case NEWBALL:
	{
		if (!estHost_ && partieEnLigne_)
		{
			// add playerId a bille and check
			NewBallEvent* ev = static_cast<NewBallEvent *>(e);
			lancerBilleEnLigne(ev->ballId(), glm::dvec3(ev->pos_x(), ev->pos_y(), 0.0), glm::dvec2(ev->vit_x(), ev->vit_y()), ev->scale());
		}
	}
	break;
	case TIMEEVENT:
	{
		TimeEvent* timeEvent = static_cast<TimeEvent*>(e);
		this->tick(timeEvent->getDt());
	}
	break;
	case SCORESYNC:
	{
		if(!estHost_)
		{
			ScoreSyncEvent* sync = static_cast<ScoreSyncEvent*>(e);
			pointage_ = sync->score();
			analyserPointage();
		}
		
	}
	break;
	case SYNCALL:
	{
		if (!estHost_)
		{
			SyncAllEvent* sync = (SyncAllEvent*)e;
			EventManager::GetInstance()->unsubscribe(this, SYNCALL);
			EventManager::GetInstance()->subscribe(this, BALLLOST);
			EventManager::GetInstance()->subscribe(this, COLLISIONEVENT);
			EventManager::GetInstance()->subscribe(this, NEWBALL);
			EventManager::GetInstance()->subscribe(this, SCORESYNC);
			toutSynchroniser(sync);
		}

	}
	break;
	case SYNCALLREQUEST:
	{ // Reconnect
		if (estHost_)
		{
			auto syncRequest = static_cast<SyncAllRequest *>(e);
			envoiSynchronisationGlobale();
			joueurManager_->reconnect(syncRequest->user_id());
			zoneCourante_->assignerBillesLocales(joueurManager_->obtenirPlayerNum(syncRequest->user_id()), false);
		}

	}
	break;
	case DISCONNECTEVENT:
	{
		auto disconnect = static_cast<DisconnectEvent *>(e);
		zoneCourante_->assignerBillesLocales(joueurManager_->obtenirPlayerNum(disconnect->user_id()), true);
	}
	break;
	default: break;
	}
}

bool PartieRapide::obtenirEstGagnee()
{
	return estGagnee_;
}

int PartieRapide::obtenirScoreJoueur(int joueur) const
{
	return pointage_;
}



int PartieRapide::obtenirBillesRestantesJoueur(int joueur_num) const
{
	return nbBillesReserve_;
}

int PartieRapide::obtenirMonPointage()
{
	return pointage_;
}

int PartieRapide::obtenirMonNbBilles()
{
	return nbBillesReserve_;
}


void PartieRapide::reinitialiser() {
	if (!partieEnLigne_)
	{
		zoneCourante_->reinitialiser();
		estGagnee_ = false;
		estTerminee_ = false;
		pointage_ = 0;

		joueurManager_->assignerControles(zoneCourante_->obtenirControles(), zoneCourante_);
	}
}