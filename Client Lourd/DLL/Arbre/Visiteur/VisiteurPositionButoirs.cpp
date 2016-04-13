#include "VisiteurPositionButoirs.h"
#include "Arbre/Noeuds/NoeudButoirCirculaire.h"

void VisiteurPositionButoirs::traiterNoeudButoirCirculaire(NoeudButoirCirculaire* noeud)
{
	if(noeud->estAllume())
		positions.push_back(noeud->obtenirPositionRelative());
}

std::vector<glm::dvec3> VisiteurPositionButoirs::obtenirPositions() const
{
	return positions;
}
