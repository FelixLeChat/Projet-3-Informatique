#ifndef _ARBRE_VISITEUR_VISITEURECHELLE_H_
#define _ARBRE_VISITEUR_VISITEURECHELLE_H_
#include "VisiteurAbstrait.h"

class VisiteurMiseAEchelle : public VisiteurAbstrait
{
public:
	///Constructeur par paramètre
	VisiteurMiseAEchelle(double dy);
	/// Destructeur
	~VisiteurMiseAEchelle(){};

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
};
#endif //_ARBRE_VISITEUR_VISITEURECHELLE_H_