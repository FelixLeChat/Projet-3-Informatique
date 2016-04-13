#include "VisiteurCiblesDetruites.h"
#include "Arbre/Noeuds/NoeudCible.h"

void VisiteurCiblesDetruites::traiterNoeudCible(NoeudCible* n)
{
	if(!n->obtenirEstActif())
	{
		auto pos = n->obtenirPositionRelative();
		positionCibles_.push_back(CibleSyncEvent(pos.x, pos.y));
	}
}

std::vector<CibleSyncEvent> VisiteurCiblesDetruites::getPositionsCibles() const
{
	return positionCibles_;
}
