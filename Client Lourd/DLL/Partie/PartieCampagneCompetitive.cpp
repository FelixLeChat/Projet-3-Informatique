#include "PartieCampagneCompetitive.h"
#include "EtatOpenGL.h"
#include "../Affichage/Affichage.h"
#include "../Event/KeyPressEvent.h"
#include <FTGL/ftgl.h>
#include "../Sons/ClasseSons.h"
#include "../Configuration/Config.h"
#include "../Event/EventManager.h"
#include "../Joueurs/JoueurManagerEnLigne.h"
#include "../Reseau/NetworkManager.h"


PartieCampagneCompetitive::PartieCampagneCompetitive( vector<string> playerIds, vector<string> paths, int nb_ai, bool estHost, string matchId, bool estRejoin )
{
	for each(auto path in paths)
	{
		auto zone = new ZoneDeJeu(path);
		zone->load();
		zones_.push_back(zone);
	}
	zoneCourante_ = zones_[0];
	matchId_ = matchId;


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
	pointageDerniereZone_ = vector<int>(nbJoueurs_, 0);
	nbBillesEnJeu_ = vector<int>(nbJoueurs_, 0);

	playerIds_ = playerIds;
	partieActuelle_ = 0;
	localPlayerNum_ = 0;

	while (localPlayerNum_ < playerIds_.size() && playerIds[localPlayerNum_] != NetworkManager::getInstance()->getUserId())
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


PartieCampagneCompetitive::~PartieCampagneCompetitive() {
	EventManager::GetInstance()->unsubscribe(this, TIMEEVENT);
	EventManager::GetInstance()->unsubscribe(this, BALLLOST);
	EventManager::GetInstance()->unsubscribe(this, COLLISIONEVENT);
	EventManager::GetInstance()->unsubscribe(this, NEWBALL);
	EventManager::GetInstance()->unsubscribe(this, SCORESYNC);
	EventManager::GetInstance()->unsubscribe(this, SYNCALL);
	EventManager::GetInstance()->unsubscribe(this, SYNCALLREQUEST);
	EventManager::GetInstance()->unsubscribe(this, DISCONNECTEVENT);
}

bool PartieCampagneCompetitive::demarrerPartie()
{
	tempsAvantProchaineBille_ = 5;
	tempsAffichage_ = 5;
	joueurManager_->assignerControles(zoneCourante_->obtenirControles(), zoneCourante_);
	rondeGagnee = false;
	nbBillesEnJeu_ = vector<int>(nbJoueurs_, 0);
	return PartieRapideCompetitive::demarrerPartie();
}


void PartieCampagneCompetitive::analyserPointage(int playerNum)
{
	auto pointageRonde = pointage_[playerNum] - pointageDerniereZone_[playerNum];
	auto pointsDernierAjout = pointageDernierAjout_[playerNum];
	auto nbBillesEnJeu = nbBillesEnJeu_[playerNum];
	auto nbPtsBilleGratuite = zoneCourante_->getPointBilleGratuite();

	// on peut avoir fait assez de points en un seul coup pour ajouter plus d'une bille
	int pointageActuel = pointageRonde - pointsDernierAjout + pointsDernierAjout % nbPtsBilleGratuite; 
	if (pointageRonde >= zoneCourante_->getPointCampagne())
	{
		if (partieActuelle_ < zones_.size() - 1)
		{
			rondeGagnee = true;
			ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_PROCHAIN_NIVEAU, false);
		}
		else
		{
			estTerminee_ = true;
			gagnant_ = playerNum;
			estGagnee_ = joueurManager_->obtenirId(gagnant_) == NetworkManager::getInstance()->getUserId();
			ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_GAME_OVER, false);
		}
	}
	else if (pointageRonde >= nbPtsBilleGratuite)
	{
		while (pointageActuel >= zoneCourante_->getPointBilleGratuite())
		{
			pointageActuel -= zoneCourante_->getPointBilleGratuite();
			nbBillesReserve_.at(playerNum)++;
			ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_BILLE_GRATUITE, false);
		}
		if (nbBillesEnJeu == 1 && Config::obtenirInstance()->getMode2Billes())
		{
			billesALancer_.push(playerNum);
		}
	}
	pointageDernierAjout_.at(playerNum) = pointageRonde;
}

void PartieCampagneCompetitive::afficherInformation() const
{

	if (tempsAffichage_ > 0)
	{

		glUniform1i(glGetUniformLocation(Affichage::obtenirInstance()->obtenirProgNuanceur(), "avecShader"), false);
		int cloture[4];
		glGetIntegerv(GL_VIEWPORT, cloture);
		double centreX = cloture[0] + cloture[2] / 2.0;
		double centreY = cloture[1] + cloture[3] / 2.0;

		glMatrixMode(GL_PROJECTION);
		glPushMatrix();
		glLoadIdentity();

		glOrtho(cloture[0], cloture[0] + cloture[2], cloture[1], cloture[1] + cloture[3], -1, 1);

		glMatrixMode(GL_MODELVIEW);
		glPushMatrix();
		glLoadIdentity();

		glDisable(GL_DEPTH_TEST);


		glColor3f(1, 0, 0);
		glBegin(GL_QUADS);
		glVertex2f(float(centreX) - 160, float(centreY) + 75);
		glVertex2f(float(centreX) - 160, float(centreY) - 75);
		glVertex2f(float(centreX) + 160, float(centreY) - 75);
		glVertex2f(float(centreX) + 160, float(centreY) + 75);
		glEnd();
		glColor3f(1, 1, 1);



		EtatOpenGL etat;
		FTGLPixmapFont font("C:/Windows/Fonts/Impact.ttf");
		FTPoint coordNom(centreX - 130, centreY + 40, 50);
		FTPoint coordDiff(centreX - 130, centreY + 10, 50);
		FTPoint coordPointage(centreX - 130, centreY - 20, 50);
		FTPoint coordPointage2(centreX - 130, centreY - 45, 50);
		if (font.Error()) {
			cout << "Probleme de police dans PartieCampagneCompetitive.cpp" << endl;
			return;
		}

		glPushAttrib(GL_ALL_ATTRIB_BITS);
		glLoadIdentity();
		glColor3f(0, 1, 0);
		font.FaceSize(20);

		char nom[255];
		char difficulte[255];
		char pointage[255];

		sprintf_s(nom, "Nom de la partie: %s", zoneCourante_->obtenirChemin().c_str());
		font.Render(nom, -1, coordNom);
		sprintf_s(difficulte, "Niveau de difficulte: %d ", zoneCourante_->getDifficulte());
		font.Render(difficulte, -1, coordDiff);
		sprintf_s(pointage, "Pointage necessaire pour");
		font.Render(pointage, -1, coordPointage);
		sprintf_s(pointage, "passer au prochain niveau: %d", zoneCourante_->getPointCampagne());
		font.Render(pointage, -1, coordPointage2);

		glPopAttrib();
		glPopMatrix();
		glMatrixMode(GL_PROJECTION);
		glPopMatrix();
		glEnable(GL_DEPTH_TEST);
		glUniform1i(glGetUniformLocation(Affichage::obtenirInstance()->obtenirProgNuanceur(), "avecShader"), true);
	}

}

void PartieCampagneCompetitive::tick(double dt) {
	if (rondeGagnee)
	{
		zoneCourante_ = zones_[++partieActuelle_];
		pointageDerniereZone_ = pointage_;
		zoneCourante_->reinitialiser();
		demarrerPartie();
	}
	if (tempsAffichage_ > 0)
		tempsAffichage_ -= dt;

	if (estHost_ && tempsAffichage_ <=0)
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

void PartieCampagneCompetitive::update(IEvent* e)
{
	PartieRapideCompetitive::update(e);
}

void PartieCampagneCompetitive::afficher()
{
	PartieRapideCompetitive::afficher();
	//afficherInformation();
}


bool PartieCampagneCompetitive::obtenirRondeGagnee() const
{
	return tempsAffichage_ > 0;
}

int PartieCampagneCompetitive::obtenirDifficulteZone() const
{
	return zoneCourante_->getDifficulte();
}

int PartieCampagneCompetitive::obtenirPointsAAtteindre() const
{
	return zoneCourante_->getPointCampagne();
}