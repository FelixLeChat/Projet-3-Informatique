#ifndef _ARBRE_VISITEUR_NIVEAUCARTE_H_
#define _ARBRE_VISITEUR_NIVEAUCARTE_H_
#include "VisiteurAbstrait.h"

class VisiteurNiveauCarte : public VisiteurAbstrait
{
public:
	VisiteurNiveauCarte();

	~VisiteurNiveauCarte() {};

	virtual void traiterNoeudChampForce(NoeudChampForce*);
	virtual void traiterNoeudPlateauDArgent(NoeudPlateauDArgent*);
	int obtenirNiveau();

private:
	int niveau_;
};
#endif //_ARBRE_VISITEUR_NIVEAUCARTE_H_