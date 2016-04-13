#ifndef __ARBRE_NOEUDS_NOEUDPALETTEDROITJ1_H__
#define __ARBRE_NOEUDS_NOEUDPALETTEDROITJ1_H__

#include "NoeudPaletteDroit.h"


class NoeudPaletteDroitJ1 : public NoeudPaletteDroit{
public:
	/// Constructeur.
	NoeudPaletteDroitJ1(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudPaletteDroitJ1();

	/// Accepte un visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);
	
};

#endif //__ARBRE_NOEUDS_NOEUDPALETTEGAUCHEJ1_H__