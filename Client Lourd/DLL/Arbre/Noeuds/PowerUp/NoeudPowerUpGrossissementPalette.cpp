#include "NoeudPowerUpGrossissementPalette.h"
#include "Event/EventManager.h"
#include "Event/PowerUpPaletteEvent.h"
#include "Reseau/NetworkManager.h"

NoeudPowerUpGrossissementPalette::NoeudPowerUpGrossissementPalette(const std::string & typeNoeud)
	: NoeudPowerUp{ typeNoeud }
{
	palettes_ = vector<NoeudPalette*>();
}


NoeudPowerUpGrossissementPalette::~NoeudPowerUpGrossissementPalette()
{
}

void NoeudPowerUpGrossissementPalette::appliquerPowerUp(NoeudBille* bille)
{
	joueurNum_ = bille->obtenirDernierFrappeur();
	EventManager::GetInstance()->throwEvent(&PowerUpPaletteEvent(joueurNum_, 1.6));
	NetworkManager::getInstance()->SyncScalePalette(joueurNum_, 1.6);
}

void NoeudPowerUpGrossissementPalette::retirerPowerUp()
{
	EventManager::GetInstance()->throwEvent(&PowerUpPaletteEvent(joueurNum_, 0.625));
	NetworkManager::getInstance()->SyncScalePalette(joueurNum_, 0.625);
}
