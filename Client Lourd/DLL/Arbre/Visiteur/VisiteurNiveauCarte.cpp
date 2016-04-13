#include "VisiteurNiveauCarte.h"

VisiteurNiveauCarte::VisiteurNiveauCarte()
{
	niveau_ = 0;
}

int VisiteurNiveauCarte::obtenirNiveau()
{
	return niveau_;
}

void VisiteurNiveauCarte::traiterNoeudChampForce(NoeudChampForce*)
{
	if (niveau_ < 1)
		niveau_ = 1;
}

void VisiteurNiveauCarte::traiterNoeudPlateauDArgent(NoeudPlateauDArgent*)
{
	if (niveau_ < 2)
		niveau_ = 2;
}