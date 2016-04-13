#include "NoeudPaletteDroitJ1.h"


#include "Utilitaire.h"

#include <windows.h>
#include <GL/gl.h>
#include <cmath>

#include "Modele3D.h"
#include "OpenGL_Storage/ModeleStorage_Liste.h"
#include <iostream>



////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPaletteDroitJ1::NoeudPaletteDroitJ1(const std::string& typeNoeud)
///
/// Ce constructeur ne fait qu'appeler la version de la classe et base
/// et donner des valeurs par défaut aux variables membres.
///
/// @param[in] typeNoeud : Le type du noeud.
///
/// @return Aucune (constructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPaletteDroitJ1::NoeudPaletteDroitJ1(const std::string& typeNoeud)
: NoeudPaletteDroit{ typeNoeud }
{
	
}


////////////////////////////////////////////////////////////////////////
///
/// @fn NoeudPaletteDroitJ1::~NoeudPaletteDroitJ1()
///
/// Ce destructeur désallouee la liste d'affichage du cube.
///
/// @return Aucune (destructeur).
///
////////////////////////////////////////////////////////////////////////
NoeudPaletteDroitJ1::~NoeudPaletteDroitJ1()
{
}




////////////////////////////////////////////////////////////////////////
///
/// @fn void NoeudPaletteDroitJ1::accepterVisiteur(VisiteurAbstrait* visiteur)
///
/// Cette fonction accepte un visiteur
///
/// @param[in] visiteur : le visiteur à accepter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void NoeudPaletteDroitJ1::accepterVisiteur(VisiteurAbstrait* visiteur)
{
	visiteur->traiterNoeudPaletteDroitJ1(this);
}


///////////////////////////////////////////////////////////////////////////////
/// @}
///////////////////////////////////////////////////////////////////////////////
