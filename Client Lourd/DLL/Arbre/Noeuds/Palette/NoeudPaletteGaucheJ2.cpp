#include "NoeudPaletteGaucheJ2.h"
#include <GL/gl.h>

#include "Modele3D.h"

////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPaletteGaucheJ2::NoeudPaletteGaucheJ2(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPaletteGaucheJ2::NoeudPaletteGaucheJ2(const std::string& typeNoeud)
: NoeudPaletteGauche{ typeNoeud }
{
	
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPaletteGaucheJ2::~NoeudPaletteGaucheJ2()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPaletteGaucheJ2::~NoeudPaletteGaucheJ2()
{
}






////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPaletteGaucheJ2::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur.
///
/// @param[in] visiteur : le visiteur à accepter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPaletteGaucheJ2::accepterVisiteur(VisiteurAbstrait* visiteur)
{
	visiteur->traiterNoeudPaletteGaucheJ2(this);
}



///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
