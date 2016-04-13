#ifndef _ARBRE_VISITEUR_VISITEURREINIT_H_
#define _ARBRE_VISITEUR_VISITEURREINIT_H_
#include "VisiteurAbstrait.h"

class VisiteurReinit : public VisiteurAbstrait
{
public:
	/// Constructeur
	VisiteurReinit();
	/// Destructeur
	~VisiteurReinit(){};

	/// Traitement des noeuds

	/// Méthode virtuelle de traitement d'un butoir circulaire
	virtual void traiterNoeudButoirCirculaire(NoeudButoirCirculaire*);
	/// Méthode virtuelle de traitement d'une cible
	virtual void traiterNoeudCible(NoeudCible*);
	/// Méthode virtuelle de traitement de la palette droite du joueur 1
	virtual void traiterNoeudPaletteDroitJ1(NoeudPaletteDroitJ1*);
	/// Méthode virtuelle de traitement de la palette droite du joueur 2
	virtual void traiterNoeudPaletteDroitJ2(NoeudPaletteDroitJ2*);
	/// Méthode virtuelle de traitement de la palette gauche du joueur 1
	virtual void traiterNoeudPaletteGaucheJ1(NoeudPaletteGaucheJ1*);
	/// Méthode virtuelle de traitement de la palette gauche du joueur 2
	virtual void traiterNoeudPaletteGaucheJ2(NoeudPaletteGaucheJ2*);
	/// Méthode virtuelle de traitement du ressort
	virtual void traiterNoeudRessort(NoeudRessort*);
	/// Méthode virtuelle de traitement de portails
	virtual void traiterNoeudPortail(NoeudPortail*);

	virtual void traiterNoeudPlateauDArgent(NoeudPlateauDArgent*);
};
#endif //_ARBRE_VISITEUR_VISITEURREINIT_H_