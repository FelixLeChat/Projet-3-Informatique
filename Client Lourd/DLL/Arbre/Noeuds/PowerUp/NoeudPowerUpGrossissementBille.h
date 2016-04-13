#ifndef __ARBRE_NOEUDS_POWERUP_GROSSEBILLE_H__
#define __ARBRE_NOEUDS_POWERUP_GROSSEBILLE_H__

#include "NoeudPowerUp.h"
#include "../NoeudBille.h"

class NoeudPowerUpGrossissementBille : public NoeudPowerUp, public IEventSubscriber
{
public:
	/// Constructeur.
	NoeudPowerUpGrossissementBille(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudPowerUpGrossissementBille();

	void update(IEvent * e);

protected:
	virtual void appliquerPowerUp(NoeudBille*);
	virtual void retirerPowerUp();

private:
	NoeudBille* bille_;
	int billeId_;
	double facteurGrossissement;
};

#endif //__ARBRE_NOEUDS_POWERUP_GROSSEBILLE_H__