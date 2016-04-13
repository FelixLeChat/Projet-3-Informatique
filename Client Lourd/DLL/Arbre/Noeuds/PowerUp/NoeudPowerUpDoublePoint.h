#ifndef __ARBRE_NOEUDS_POWERUP_DOUBLEPOINT_H__
#define __ARBRE_NOEUDS_POWERUP_DOUBLEPOINT_H__

#include "NoeudPowerUp.h"
#include "../NoeudBille.h"

class NoeudPowerUpDoublePoint : public NoeudPowerUp
{
public:
	/// Constructeur.
	NoeudPowerUpDoublePoint(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudPowerUpDoublePoint();

protected:
	virtual void appliquerPowerUp(NoeudBille*);
	virtual void retirerPowerUp();

private:
	NoeudBille* bille_;
};

#endif //__ARBRE_NOEUDS_POWERUP_DOUBLEPOINT_H__