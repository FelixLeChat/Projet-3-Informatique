#include "NoeudPaletteDroitJ2.h"
#include <GL/gl.h>

#include "Modele3D.h"
////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPaletteDroitJ2::NoeudPaletteDroitJ2(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPaletteDroitJ2::NoeudPaletteDroitJ2(const std::string& typeNoeud)
: NoeudPaletteDroit{ typeNoeud }
{
	
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPaletteDroitJ2::~NoeudPaletteDroitJ2()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPaletteDroitJ2::~NoeudPaletteDroitJ2()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPaletteDroitJ2::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur.
///
/// @param[in] visiteur : le visiteur à accepter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPaletteDroitJ2::accepterVisiteur(VisiteurAbstrait* visiteur)
{
	visiteur->traiterNoeudPaletteDroitJ2(this);
}



///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
