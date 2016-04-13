#ifndef __ARBRE_NOEUDS_NOEUDPALETTEDROITJ2_H__
#define __ARBRE_NOEUDS_NOEUDPALETTEDROITJ2_H__

#include "NoeudPaletteDroit.h"


class NoeudPaletteDroitJ2 : public NoeudPaletteDroit{
public:
	/// Constructeur.
	NoeudPaletteDroitJ2(
		const std::string& type = std::string{ "" }
	);
	/// Destructeur.
	virtual ~NoeudPaletteDroitJ2();

	/// Accepte le visiteur
	virtual void accepterVisiteur(VisiteurAbstrait*);

	
};

#endif //__ARBRE_NOEUDS_NOEUDPALETTEGAUCHEJ2_H__