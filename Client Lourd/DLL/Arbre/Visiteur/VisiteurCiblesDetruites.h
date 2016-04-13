#pragma once
#include <vector>
#include "VisiteurAbstrait.h"
#include "../../Event/CibleSyncEvent.h"

class VisiteurCiblesDetruites : public VisiteurAbstrait
{
public:
	void traiterNoeudCible(NoeudCible* n) override;
	std::vector<CibleSyncEvent> getPositionsCibles() const;

private:
	std::vector<CibleSyncEvent> positionCibles_;
};
