#ifndef _ARBRE_VISITEUR_VISITEURROTATION_H_
#define _ARBRE_VISITEUR_VISITEURROTATION_H_
#include "VisiteurAbstrait.h"
#include "glm\glm.hpp"

class VisiteurRotation : public VisiteurAbstrait
{
public:
	///Constructeur par paramètre
	VisiteurRotation(double dy, glm::dvec3 pointRotation);
	///Destructeur
	~VisiteurRotation(){};

	///Traitement des noeuds
	virtual void traiterNoeudButoirCirculaire(NoeudButoirCirculaire*);
	virtual void traiterNoeudButoirTriangulaireGauche(NoeudButoirTriangulaireGauche*);
	virtual void traiterNoeudButoirTriangulaireDroit(NoeudButoirTriangulaireDroit*);

	virtual void traiterNoeudCible(NoeudCible*);
	virtual void traiterNoeudGenerateurBille(NoeudGenerateurBille*);

	virtual void traiterNoeudPaletteDroitJ1(NoeudPaletteDroitJ1*);
	virtual void traiterNoeudPaletteDroitJ2(NoeudPaletteDroitJ2*);
	virtual void traiterNoeudPaletteGaucheJ1(NoeudPaletteGaucheJ1*);
	virtual void traiterNoeudPaletteGaucheJ2(NoeudPaletteGaucheJ2*);

	virtual void traiterNoeudRessort(NoeudRessort*);
	virtual void traiterNoeudTrou(NoeudTrou*);

	virtual void traiterNoeudMur(NoeudMur*);
	virtual void traiterNoeudPortail(NoeudPortail*);

	virtual void traiterNoeudChampForce(NoeudChampForce*);
	virtual void traiterNoeudPlateauDArgent(NoeudPlateauDArgent*);

protected:
	virtual void traiterNoeud(NoeudAbstrait*);

private :
	///Variation de positions en y
	double dy_;
	///Le point de rotation
	glm::dvec3 pointRotation_;
};
#endif //_ARBRE_VISITEUR_VISITEURROTATION_H_