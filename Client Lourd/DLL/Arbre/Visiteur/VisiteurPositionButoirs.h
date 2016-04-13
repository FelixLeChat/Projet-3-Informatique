#pragma once
#include "VisiteurAbstrait.h"
#include <vector>
#include <glm/vec3.hpp>

class VisiteurPositionButoirs: public VisiteurAbstrait
{

public:
	/// Traitement de noeud selon le visiteur
	void traiterNoeudButoirCirculaire(NoeudButoirCirculaire*) override;
	std::vector<glm::dvec3> obtenirPositions() const;
	
private:
	std::vector<glm::dvec3> positions;
};
