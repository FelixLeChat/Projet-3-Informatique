#include "NoeudPaletteGaucheJ1.h"

#include "Utilitaire.h"

#include <windows.h>
#include <GL/gl.h>
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_Storage/ModeleStorage_Liste.h"
#include <iostream>


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPaletteGaucheJ1::NoeudPaletteGaucheJ1(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPaletteGaucheJ1::NoeudPaletteGaucheJ1(const std::string& typeNoeud)
: NoeudPaletteGauche{ typeNoeud }
{
	
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPaletteGaucheJ1::~NoeudPaletteGaucheJ1()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPaletteGaucheJ1::~NoeudPaletteGaucheJ1()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPaletteGaucheJ1::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur
///
/// @param[in] visiteur : le visiteur à accepter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPaletteGaucheJ1::accepterVisiteur(VisiteurAbstrait* visiteur)
{
	visiteur->traiterNoeudPaletteGaucheJ1(this);
}





///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
