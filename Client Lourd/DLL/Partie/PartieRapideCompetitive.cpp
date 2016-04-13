////////////////////////////////////////////////////////////////////////////////////
/// @file PartieRapideCompetitive.cpp
/// @author Nicolas Blais, Jérôme Daigle
/// @date 2015-03-17
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
////////////////////////////////////////////////////////////////////////////////////

#include "PartieRapideCompetitive.h"
#include "../Configuration/Config.h"
#include "../Event/CollisionEvent.h"
#include "../Event/NewBallEvent.h"
#include "../Reseau/NetworkManager.h"
#include "../Sons/ClasseSons.h"
#include "../Event/EventManager.h"
#include "../Joueurs/JoueurManagerEnLigne.h"
#include "../Event/BallLostEvent.h"
#include "../Event/TimeEvent.h"
#include "../Event/SyncAllEvent.h"
#include "../Event/DisconnectEvent.h"
#include "../Event/SyncAllRequest.h"
#include <Event/ScoreSyncEvent.h>

PartieRapideCompetitive::PartieRapideCompetitive(vector<string> playerIds, string zone, int nb_ai, bool estHost, string matchId, bool estRejoin)
{
	zoneCourante_ = new ZoneDeJeu(zone);
	zoneCourante_->load();

	joueurManager_ = new JoueurManagerEnLigne(playerIds, nb_ai, false, estHost);

	auto controles = zoneCourante_->obtenirControles();
	joueurManager_->assignerControles(controles, zoneCourante_);
	estTerminee_ = false;
	estGagnee_ = false;

	matchId_ = matchId;
	estHost_ = estHost;
	nbJoueurs_ = int(playerIds.size()) + nb_ai;


	pointage_ = vector<int>(nbJoueurs_, 0);
	pointageDernierAjout_ = vector<int>(nbJoueurs_, 0);
	nbBillesEnJeu_ = vector<int>(nbJoueurs_, 0);
	billesALancer_ = queue<int>();

	playerIds_ = playerIds;

	localPlayerNum_ = 0;

	while(localPlayerNum_ < playerIds_.size() && playerIds[localPlayerNum_] != NetworkManager::getInstance()->getUserId())
	{
		localPlayerNum_++;
	}
	if (estRejoin && localPlayerNum_ == playerIds_.size())
		localPlayerNum_ = 5; // Spectator


	if (estRejoin)
	{
		EventManager::GetInstance()->subscribe(this, TIMEEVENT);
		EventManager::GetInstance()->subscribe(this, SYNCALL);
		NetworkManager::getInstance()->requestGlobalSync();
	}
	else
	{
		EventManager::GetInstance()->subscribe(this, TIMEEVENT);
		EventManager::GetInstance()->subscribe(this, BALLLOST);
		EventManager::GetInstance()->subscribe(this, COLLISIONEVENT);
		EventManager::GetInstance()->subscribe(this, NEWBALL);
		EventManager::GetInstance()->subscribe(this, SCORESYNC);

		if (estHost_)
		{
			EventManager::GetInstance()->subscribe(this, SYNCALLREQUEST);
			EventManager::GetInstance()->subscribe(this, DISCONNECTEVENT);
		}
	}

}

PartieRapideCompetitive::~PartieRapideCompetitive()
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
bool PartieRapideCompetitive::demarrerPartie()
{
	
	nbBillesReserve_ = vector<int>(nbJoueurs_, Config::obtenirInstance()->getNbBilles());
	for (int i = 0; i < nbJoueurs_; i++)
	{
		billesALancer_.push(i);
	}
	if(Config::obtenirInstance()->getMode2Billes() && nbBillesReserve_[0] >= 2)
	{
		for (int i = 0; i < nbJoueurs_; i++)
		{
			billesALancer_.push(i);
		}
	}
	ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_MUSIQUE, true, 0);
	return PartieBase::demarrerPartie();
}

void PartieRapideCompetitive::checkEndGame()
{
	bool oneWinner = false;
	int winnerId = 0;
	for (int i = 0; i < nbJoueurs_; i++)
	{
		if(nbBillesEnJeu_[i] >0 || nbBillesReserve_[i] > 0)
		{
			if(!oneWinner)
			{
				oneWinner = true;
				winnerId = i;
			}
			else
			{
				return; // Si deja joueur avec au moins une bille en reserve = pas fin
			}
		}
	}
	estTerminee_ = true;
	gagnant_ = winnerId;
	estGagnee_ = joueurManager_->obtenirId(gagnant_) == NetworkManager::getInstance()->getUserId();
	ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_GAME_OVER, false);
}

void PartieRapideCompetitive::perdreBille(int playerNum)
{

	nbBillesEnJeu_[playerNum] = nbBillesEnJeu_[playerNum] -1;
	tempsAvantProchaineBille_ = 1;
	if (nbBillesReserve_[playerNum] > 0)
	{
		billesALancer_.push(playerNum);
	}
	else
	{
		if(nbBillesEnJeu_[playerNum] ==0 && nbBillesReserve_[playerNum] == 0)
		{
			checkEndGame();
		}
	}
}

NoeudBille* PartieRapideCompetitive::lancerBille()
{
	//seul le host lance les billes
	if (estHost_) 
	{
		int playerNum = billesALancer_.front();
		billesALancer_.pop();
		if (nbBillesReserve_.at(playerNum) > 0 &&
			(nbBillesEnJeu_.at(playerNum) == 0 || nbBillesEnJeu_.at(playerNum) == 1 && Config::obtenirInstance()->getMode2Billes()))
		{
			tempsAvantProchaineBille_ = 1;
			bool isLocale =joueurManager_->estLocal(playerNum);
			auto bille = zoneCourante_->lancerBille(isLocale);
			bille->assignerPlayerNum(playerNum);
			bille->assignerFantome(playerNum != localPlayerNum_);
			auto pos = bille->obtenirPositionRelative();
			auto vit = bille->obtenirVitesse();
			NetworkManager::getInstance()->lancerBille(bille->obtenirId(), pos.x, pos.y, vit.x, vit.y, playerNum); 
			nbBillesReserve_[playerNum] --;
			nbBillesEnJeu_[playerNum] ++;
			return bille;
		}
	}
	return nullptr;
}


void PartieRapideCompetitive::lancerBilleEnLigne(int id, glm::dvec3 pos, glm::dvec2 vitesse, int playerNum)
{
	tempsAvantProchaineBille_ = 1.5;
	nbBillesReserve_[playerNum] --;
	nbBillesEnJeu_[playerNum] ++;
	bool isLocale = joueurManager_->estLocal(playerNum);
	auto bille = zoneCourante_->lancerBille(id, pos, vitesse,isLocale, playerNum); 
	bille->assignerFantome(playerNum != localPlayerNum_);
}

int PartieRapideCompetitive::obtenirMonPointage()
{
	if(localPlayerNum_ < pointage_.size())
		return pointage_[localPlayerNum_];
	return 0;
}

int PartieRapideCompetitive::obtenirMeilleurPointage()
{
	return *max_element(pointage_.begin(), pointage_.end());
}

int PartieRapideCompetitive::obtenirMonNbBilles()
{
	if (localPlayerNum_ < nbBillesReserve_.size())
		return nbBillesReserve_[localPlayerNum_];
	return 0;
}

void PartieRapideCompetitive::tick(double dt)
{
	if (estHost_)
	{
		if (tempsAvantProchaineBille_ > 0)
			tempsAvantProchaineBille_ -= dt;

		if (tempsAvantProchaineBille_ <= 0 && billesALancer_.size() != 0)
		{
			lancerBille();
		}
	}
	if (!estGagnee_)
	{
		NetworkManager::getInstance()->SyncReceivedMessages();
	}
	zoneCourante_->animer(dt);
}

void PartieRapideCompetitive::reinitialiser()
{
	//Do not reinit
}

void PartieRapideCompetitive::afficher()
{
	PartieBase::afficher();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void PartieRapideCompetitive::analyserPointage()
///
/// Analyse du pointage à savoir si le pointage permet d'obtenir une nouvelle bille
///
/// @param[in] Aucun.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void PartieRapideCompetitive::analyserPointage(int playerNum)
{
	auto pointage = pointage_.at(playerNum);
	auto pointageDerniereBille = pointageDernierAjout_.at(playerNum);
	auto nbBillesEnJeu = nbBillesEnJeu_.at(playerNum);
	auto nbPtsBilleGratuite = zoneCourante_->getPointBilleGratuite();

	int pointageActuel = pointage - pointageDerniereBille + pointageDerniereBille % nbPtsBilleGratuite;
	if (pointage >= nbPtsBilleGratuite)
	{
		while (pointageActuel >= zoneCourante_->getPointBilleGratuite())
		{
			pointageActuel -= zoneCourante_->getPointBilleGratuite();
			nbBillesReserve_.at(playerNum)++;
			if(playerNum == localPlayerNum_)
				ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_BILLE_GRATUITE, false);
		}
		if (nbBillesEnJeu == 1 && Config::obtenirInstance()->getMode2Billes())
		{
			billesALancer_.push(playerNum);
		}
	}
	pointageDernierAjout_.at(playerNum) = pointage;
}


void PartieRapideCompetitive::toutSynchroniser(SyncAllEvent* SyncEvent)
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
	pointage_ = SyncEvent->pointages();
	nbBillesReserve_ = SyncEvent->nb_billes_restantes();


}

void PartieRapideCompetitive::envoiSynchronisationGlobale()
{
	auto zoneNum = 0;
	auto pointage = pointage_;
	auto nbBillesRest = nbBillesReserve_;
	auto billes = zoneCourante_->obtenirEtatBilles();
	auto cibles = zoneCourante_->obtenirEtatCibles();
	auto powerUps = zoneCourante_->obtenirEtatPowerUps();
	auto palettes = static_cast<JoueurManagerEnLigne *> (joueurManager_)->obtenirEtatPalettes();

	NetworkManager::getInstance()->SyncAll(zoneNum, pointage, nbBillesRest, billes, cibles, powerUps, palettes);
}


void PartieRapideCompetitive::update(IEvent * e)
{
	switch (e->getType())
	{
	case COLLISIONEVENT:
	{
		//ajouter score seulement si bille est locale
		CollisionEvent* collision = static_cast<CollisionEvent*>(e);
		if (joueurManager_->estLocal(collision->getPlayerNum()))
		{
			string noeudCollision = collision->getTypeNoeud();
			int playerNum = collision->getPlayerNum();
			if (noeudCollision == ArbreRenduINF2990::NOM_BUTOIRCIRCULAIRE)
				pointage_.at(playerNum) += zoneCourante_->getPointButoirCercle();
			else if (noeudCollision == ArbreRenduINF2990::NOM_BUTOIRTRIANGULAIREDROIT
				|| noeudCollision == ArbreRenduINF2990::NOM_BUTOIRTRIANGULAIREGAUCHE)
				pointage_.at(playerNum) += zoneCourante_->getPointButoirTriangle();
			else if (noeudCollision == ArbreRenduINF2990::NOM_CIBLE)
				pointage_.at(playerNum) += zoneCourante_->getPointCible();

			//lancer event pour sync score - si multijoueur
			NetworkManager::getInstance()->SyncScore(playerNum, pointage_[playerNum]);
			analyserPointage(playerNum);
		}
	}
	break;
	case SCORESYNC:
	{
		ScoreSyncEvent* sync = static_cast<ScoreSyncEvent*>(e);
		pointage_[sync->player_num()] = sync->score();
		analyserPointage(sync->player_num());

	}
	break;
	case BALLLOST:
	{
		BallLostEvent* ballLost = static_cast<BallLostEvent*>(e);
		auto playerNum = ballLost->getPlayerNum();

		zoneCourante_->perdreBille(ballLost->getBallId());
		perdreBille(playerNum);
		
	}
	break;
	case NEWBALL:
	{
		if (!estHost_)
		{
			NewBallEvent* ev = static_cast<NewBallEvent *>(e);
			lancerBilleEnLigne(ev->ballId(), glm::dvec3(ev->pos_x(), ev->pos_y(), 0.0), glm::dvec2(ev->vit_x(), ev->vit_y()), ev->playerNum());
		}
	}
	break;
	case TIMEEVENT:
	{
		TimeEvent* timeEvent = static_cast<TimeEvent*>(e);
		this->tick(timeEvent->getDt());
	}
	break;
	case SYNCALL:
	{
		if (!estHost_)
		{
			SyncAllEvent* sync = static_cast<SyncAllEvent*>(e);
			EventManager::GetInstance()->unsubscribe(this, SYNCALL);
			EventManager::GetInstance()->subscribe(this, BALLLOST);
			EventManager::GetInstance()->subscribe(this, COLLISIONEVENT);
			EventManager::GetInstance()->subscribe(this, GAMESTART);
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

bool PartieRapideCompetitive::obtenirEstGagnee()
{
	return estGagnee_;
}

int PartieRapideCompetitive::obtenirJoueurGagnant() const
{
	return gagnant_;
}

int PartieRapideCompetitive::obtenirScoreJoueur(int joueur) const
{
	return pointage_[joueur];
}

int PartieRapideCompetitive::obtenirBillesRestantesJoueur(int joueur_num) const
{
	return nbBillesReserve_[joueur_num];
}
