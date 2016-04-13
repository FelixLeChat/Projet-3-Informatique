#include "NoeudPowerUpDemiPoint.h"


NoeudPowerUpDemiPoint::NoeudPowerUpDemiPoint(const std::string & typeNoeud)
	: NoeudPowerUp{ typeNoeud }
{
}


NoeudPowerUpDemiPoint::~NoeudPowerUpDemiPoint()
{
}

void NoeudPowerUpDemiPoint::appliquerPowerUp(NoeudBille* bille)
{
	bille_ = bille;
	bille_->assignerFacteurPointage(0.5);
}

void NoeudPowerUpDemiPoint::retirerPowerUp()
{
	if (bille_ != nullptr)
		bille_->assignerFacteurPointage(2);
	bille_ = nullptr;
}
