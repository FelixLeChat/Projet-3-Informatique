#include "PartieBase.h"
#include "../Event/TimeEvent.h"
#include "../Event/BallLostEvent.h"
#include "../Sons/ClasseSons.h"
#include "../Event/EventManager.h"
#include "../Joueurs/JoueurManagerLocal.h"


PartieBase::PartieBase(string chemin)
{
	estTerminee_ = false;
	EventManager::GetInstance()->subscribe(this, TIMEEVENT);
	EventManager::GetInstance()->subscribe(this, BALLLOST);

	zoneCourante_ = new ZoneDeJeu(chemin);
	zoneCourante_->load();
	joueurManager_ = new JoueurManagerLocal(1, false);

	reassignerPalettes();
}

PartieBase::~PartieBase()
{
	EventManager::GetInstance()->unsubscribe(this, TIMEEVENT);
	EventManager::GetInstance()->unsubscribe(this, BALLLOST);

	delete joueurManager_;
	delete zoneCourante_;
}


bool PartieBase::demarrerPartie()
{
	tempsAvantProchaineBille_ = 3;
	nbBillesEnJeu_ = 0;
	ClasseSons::obtenirInstance()->jouerSon(TYPE_SON_DEMARRER_PARTIE, false);
	return true;
}


void PartieBase::perdreBille()
{
	tempsAvantProchaineBille_ = 1;
	nbBillesEnJeu_--;
}


NoeudBille* PartieBase::lancerBille()
{
	tempsAvantProchaineBille_ = 1.5;
	nbBillesEnJeu_++;
	return zoneCourante_->lancerBille();
}

void PartieBase::reinitialiser()
{
	zoneCourante_->reinitialiser(); 
}

void PartieBase::afficher()
{
	zoneCourante_->afficher();
}

void PartieBase::tick(double dt)
{
	if (tempsAvantProchaineBille_ > 0)
		tempsAvantProchaineBille_ -= dt;

	if (tempsAvantProchaineBille_ <= 0 &&  nbBillesEnJeu_ == 0)
	{
		lancerBille();
	}
	zoneCourante_->animer(dt);
}



void PartieBase::update(IEvent* e)
{
	switch (e->getType())
	{
		case TIMEEVENT:
		{
			TimeEvent* timeEvent = (TimeEvent*)e;
			this->tick(timeEvent->getDt());
			break;
		}
		case BALLLOST:
		{
			BallLostEvent* ballEvent = (BallLostEvent*)e;
			perdreBille();
			zoneCourante_->perdreBille(ballEvent->getBallId());
		}
	}
}

int PartieBase::obtenirScoreJoueur(int joueur) const
{
	return 0;
}

int PartieBase::obtenirBillesRestantesJoueur(int joueur_num) const
{
	return 0;
}

ZoneDeJeu* PartieBase::obtenirZoneDeJeu() const
{
	return zoneCourante_;
}

int PartieBase::obtenirMeilleurPointage()
{
	return 0;
}

int PartieBase::obtenirMonPointage()
{
	return 0;
}

int PartieBase::obtenirMonNbBilles()
{
	return MAXINT;
}

void PartieBase::reassignerPalettes() const
{

	auto controles = zoneCourante_->obtenirControles();
	joueurManager_->assignerControles(controles, zoneCourante_);
}

bool PartieBase::obtenirRondeGagnee() const
{
	return false;
}

int PartieBase::obtenirDifficulteZone() const
{
	return 0;
}

int PartieBase::obtenirPointsAAtteindre() const
{
	return 0;
}

string PartieBase::obtenirNomZone() const
{
	auto nom = zoneCourante_->obtenirChemin();
	nom = nom.substr(nom.find_last_of('\\') + 1);
	return nom;
}

bool PartieBase::obtenirEstGagnee()
{
	return false;
}

bool PartieBase::obtenirEstTerminee() const
{
	if (estTerminee_)
	{
		ClasseSons::obtenirInstance()->arreterSon(TYPE_SON_MUSIQUE);
	}
	return estTerminee_;
}
