#ifndef __ARBRE_NOEUDS_NOEUDPALETTEGAUCHEJ1_H__
#define __ARBRE_NOEUDS_NOEUDPALETTEGAUCHEJ1_H__

#include "NoeudPaletteGauche.h"


class NoeudPaletteGaucheJ1 : public NoeudPaletteGauche{
public:
	/// Constructeur.
	NoeudPaletteGaucheJ1(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudPaletteGaucheJ1();

	/// Accepte un visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);
	
};

#endif //__ARBRE_NOEUDS_NOEUDPALETTEGAUCHEJ1_H__