#include "NoeudPowerUpGrossissementBille.h"
#include "Reseau/NetworkManager.h"
#include <Event/EventManager.h>
#include <Event/BallLostEvent.h>

NoeudPowerUpGrossissementBille::NoeudPowerUpGrossissementBille(const std::string & typeNoeud)
	: NoeudPowerUp{ typeNoeud }
{
	facteurGrossissement = 2;
	EventManager::GetInstance()->subscribe(this, BALLLOST);
	bille_ = nullptr;
}


NoeudPowerUpGrossissementBille::~NoeudPowerUpGrossissementBille()
{
	EventManager::GetInstance()->unsubscribe(this, BALLLOST);
}

void NoeudPowerUpGrossissementBille::update(IEvent* e)
{
	if(e->getType() == BALLLOST)
	{
		BallLostEvent* ballEvent = static_cast<BallLostEvent*>(e);
		if(bille_ != nullptr && ballEvent->getBallId() == billeId_)
		{
			bille_ = nullptr;
		}
	}
}

void NoeudPowerUpGrossissementBille::appliquerPowerUp(NoeudBille* bille)
{
	bille_ = bille;
	billeId_ = bille->obtenirId();
	bille_->assignerAgrandissement(bille_->obtenirAgrandissement()*facteurGrossissement); // Agrandissement local
	NetworkManager::getInstance()->SyncScaleBille(bille_->obtenirId(), facteurGrossissement);
}

void NoeudPowerUpGrossissementBille::retirerPowerUp()
{
	if (bille_ != nullptr)
	{
		bille_->assignerAgrandissement(bille_->obtenirAgrandissement() / facteurGrossissement);
		NetworkManager::getInstance()->SyncScaleBille(bille_->obtenirId(), 1.0/facteurGrossissement);
	}
	bille_ = nullptr;
}
