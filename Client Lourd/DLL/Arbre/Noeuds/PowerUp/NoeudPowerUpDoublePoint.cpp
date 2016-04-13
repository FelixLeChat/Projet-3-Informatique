#include "NoeudPowerUpDoublePoint.h"


NoeudPowerUpDoublePoint::NoeudPowerUpDoublePoint(const std::string & typeNoeud)
	: NoeudPowerUp{ typeNoeud }
{
}


NoeudPowerUpDoublePoint::~NoeudPowerUpDoublePoint()
{
}

void NoeudPowerUpDoublePoint::appliquerPowerUp(NoeudBille* bille)
{
	bille_ = bille;
	bille_->assignerFacteurPointage(2);
}

void NoeudPowerUpDoublePoint::retirerPowerUp()
{
	if (bille_ != nullptr)
		bille_->assignerFacteurPointage(0.5);
	bille_ = nullptr;
}