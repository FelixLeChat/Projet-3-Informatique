#pragma once
#include "VisiteurAbstrait.h"
#include "Event/PowerUpSyncEvent.h"
#include <vector>

class VisiteurPowerUpVisibles : public VisiteurAbstrait
{
public:
	VisiteurPowerUpVisibles();
	~VisiteurPowerUpVisibles();
	void traiterNoeudPlateauDArgent(NoeudPlateauDArgent* n) override;
	vector<PowerUpSyncEvent> obtenirPowerUps() const;

private:
	vector<PowerUpSyncEvent> PowerUps_;
};

