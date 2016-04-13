#ifndef __ARBRE_NOEUDS_POWERUP_DEMIPOINT_H__
#define __ARBRE_NOEUDS_POWERUP_DEMIPOINT_H__

#include "NoeudPowerUp.h"
#include "../NoeudBille.h"

class NoeudPowerUpDemiPoint : public NoeudPowerUp
{
public:
	/// Constructeur.
	NoeudPowerUpDemiPoint(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudPowerUpDemiPoint();

protected:
	virtual void appliquerPowerUp(NoeudBille*);
	virtual void retirerPowerUp();

private:
	NoeudBille* bille_;
};

#endif //__ARBRE_NOEUDS_POWERUP_DEMIPOINT_H__