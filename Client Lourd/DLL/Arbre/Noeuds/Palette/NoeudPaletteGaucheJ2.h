#ifndef __ARBRE_NOEUDS_NOEUDPALETTEGAUCHEJ2_H__
#define __ARBRE_NOEUDS_NOEUDPALETTEGAUCHEJ2_H__

#include "NoeudPaletteGauche.h"


class NoeudPaletteGaucheJ2 : public NoeudPaletteGauche{
public:
	/// Constructeur.
	NoeudPaletteGaucheJ2(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudPaletteGaucheJ2();

	///Accepte un visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);

};

#endif //__ARBRE_NOEUDS_NOEUDPALETTEGAUCHEJ2_H__