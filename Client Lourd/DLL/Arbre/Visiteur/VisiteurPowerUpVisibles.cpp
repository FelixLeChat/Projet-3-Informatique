#include "VisiteurPowerUpVisibles.h"
#include "Arbre/Noeuds/PowerUp/NoeudPlateauDArgent.h"


VisiteurPowerUpVisibles::VisiteurPowerUpVisibles()
{
}


VisiteurPowerUpVisibles::~VisiteurPowerUpVisibles()
{
}

void VisiteurPowerUpVisibles::traiterNoeudPlateauDArgent(NoeudPlateauDArgent* n)
{
	auto pos = n->obtenirPositionRelative();
	PowerUps_.push_back(PowerUpSyncEvent(pos.x, pos.y, n->obtenirTypePowerUp()));
}

vector<PowerUpSyncEvent> VisiteurPowerUpVisibles::obtenirPowerUps() const
{
	return PowerUps_;
}
