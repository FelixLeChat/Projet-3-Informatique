//////////////////////////////////////////////////////////////////////////////
/// @file VisiteurObtenirControles.h
/// @author Konstantin Fedorov, Jérémie Gagné
/// @date 2015-03-02
/// @version 1.0
///
/// @addtogroup inf2990 INF2990
/// @{
//////////////////////////////////////////////////////////////////////////////
#ifndef _ARBRE_VISITEUR_VisiteurObtenirControles_H_
#define _ARBRE_VISITEUR_VisiteurObtenirControles_H_
#include "VisiteurAbstrait.h"

#include <string>
#include <vector>
///////////////////////////////////////////////////////////////////////////
/// @class VisiteurObtenirControles
/// @brief Le visiteur pour activer un objet
///
/// @author Konstantin Fedorov, Jérémie Gagné
/// @date 2015-03-02
///////////////////////////////////////////////////////////////////////////
class VisiteurObtenirControles : public VisiteurAbstrait
{
public:
	///Constructeur par paramètre
	VisiteurObtenirControles();
	/// Destructeur
	~VisiteurObtenirControles(){};

	/// Traitement des noeuds
	virtual void traiterNoeudPaletteDroitJ1(NoeudPaletteDroitJ1*);
	virtual void traiterNoeudPaletteDroitJ2(NoeudPaletteDroitJ2*);
	virtual void traiterNoeudPaletteGaucheJ1(NoeudPaletteGaucheJ1*);
	virtual void traiterNoeudPaletteGaucheJ2(NoeudPaletteGaucheJ2*);

	virtual void traiterNoeudRessort(NoeudRessort*);

	
	std::vector<NoeudPalette*> getpalettesDroitesJ1();
	std::vector<NoeudPalette*> getpalettesDroitesJ2();
	std::vector<NoeudPalette*> getpalettesGauchesJ1();
	std::vector<NoeudPalette*> getpalettesGauchesJ2();

	std::vector<NoeudRessort*> getressorts();
private :
	/// Indique le type de noeud a activer
	std::vector<NoeudPalette*> palettesDroitesJ1;
	std::vector<NoeudPalette*> palettesDroitesJ2;
	std::vector<NoeudPalette*> palettesGauchesJ1;
	std::vector<NoeudPalette*> palettesGauchesJ2;

	std::vector<NoeudRessort*> ressorts;

};
#endif //_ARBRE_VISITEUR_VisiteurObtenirControles_H_

///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////