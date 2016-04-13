#ifndef __ARBRE_NOEUDS_POWERUP_GROSSEPALETTE_H__
#define __ARBRE_NOEUDS_POWERUP_GROSSEPALETTE_H__

#include "NoeudPowerUp.h"
#include "../NoeudBille.h"
#include "../Palette/NoeudPalette.h"

class NoeudPowerUpGrossissementPalette : public NoeudPowerUp
{
public:
	/// Constructeur.
	NoeudPowerUpGrossissementPalette(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudPowerUpGrossissementPalette();

protected:
	virtual void appliquerPowerUp(NoeudBille*);
	virtual void retirerPowerUp();

private:
	vector<NoeudPalette*> palettes_;
	double facteurGrossissement;
	int joueurNum_;
};

#endif //__ARBRE_NOEUDS_POWERUP_GROSSEPALETTE_H__