#define _USE_MATH_DEFINES


#include "VisiteurRotation.h"
#include "../Noeuds/NoeudAbstrait.h"

#include "../Noeuds/NoeudButoirCirculaire.h"
#include "../Noeuds/NoeudButoirTriangulaireDroit.h"
#include "../Noeuds/NoeudButoirTriangulaireGauche.h"

#include "../Noeuds/NoeudCible.h"
#include "../Noeuds/NoeudGenerateurBille.h"

#include "../Noeuds/Palette/NoeudPaletteDroitJ1.h"
#include "../Noeuds/Palette/NoeudPaletteDroitJ2.h"
#include "../Noeuds/Palette/NoeudPaletteGaucheJ1.h"
#include "../Noeuds/Palette/NoeudPaletteGaucheJ2.h"

#include "../Noeuds/NoeudRessort.h"
#include "../Noeuds/NoeudTrou.h"

#include "../Noeuds/NoeudMur.h"
#include "../Noeuds/NoeudPortail.h"

#include "../Noeuds/NoeudChampForce.h"
#include "../Noeuds/PowerUp/NoeudPlateauDArgent.h"

#include <iostream>
#include <cmath>

////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurRotation::VisiteurRotation(double dy, glm::dvec3 pointRotation)
///
/// Constructeur par paramètre du visiteur de rotation.
///
/// @param[in] dy : déplacement en y.
/// @param[in] pointRotation : le point autour duquel se fait la rotation.
///
/// @return Aucune (Constructeur)
///
////////////////////////////////////////////////////////////////////////
VisiteurRotation::VisiteurRotation(double dy, glm::dvec3 pointRotation)
{
	dy_ = dy;
	pointRotation_ = pointRotation;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurRotation::traiterNoeud(NoeudAbstrait* noeudBasic)
///
/// Cette fonction traite le noeud en lui assignant une rotation.
///
/// @param[in] noeudBasic : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurRotation::traiterNoeud(NoeudAbstrait* noeudBasic)
{
	if (noeudBasic->estSelectionne())
	{
		double x = pointRotation_.x - noeudBasic->obtenirPositionRelative().x;
		double y = pointRotation_.y - noeudBasic->obtenirPositionRelative().y;

		glm::dvec3 nouvellePositionRelative(0,0,0);
	
		nouvellePositionRelative.y += x*sin(dy_ / 180 * M_PI);
		nouvellePositionRelative.x += x*cos(dy_ / 180 * M_PI);

		nouvellePositionRelative.y += y*cos(dy_ / 180 * M_PI);
		nouvellePositionRelative.x -= y*sin(dy_ / 180 * M_PI);

		nouvellePositionRelative.x = noeudBasic->obtenirPositionRelative().x + (x - nouvellePositionRelative.x);
		nouvellePositionRelative.y = noeudBasic->obtenirPositionRelative().y + (y - nouvellePositionRelative.y);

		noeudBasic->assignerPositionRelative(nouvellePositionRelative);
				
		noeudBasic->assignerRotation(noeudBasic->obtenirRotation() + dy_);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurRotation::traiterNoeudButoirCirculaire(NoeudButoirCirculaire* noeud)
///
/// Cette fonction traite le noeud en lui assignant une rotation.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurRotation::traiterNoeudButoirCirculaire(NoeudButoirCirculaire* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurRotation::traiterNoeudButoirTriangulaireDroit(NoeudButoirTriangulaireDroit* noeud)
///
/// Cette fonction traite le noeud en lui assignant une rotation.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurRotation::traiterNoeudButoirTriangulaireDroit(NoeudButoirTriangulaireDroit* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurRotation::traiterNoeudButoirTriangulaireGauche(NoeudButoirTriangulaireGauche* noeud)
///
/// Cette fonction traite le noeud en lui assignant une rotation.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurRotation::traiterNoeudButoirTriangulaireGauche(NoeudButoirTriangulaireGauche* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurRotation::traiterNoeudCible(NoeudCible* noeud)
///
/// Cette fonction traite le noeud en lui assignant une rotation.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurRotation::traiterNoeudCible(NoeudCible* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurRotation::traiterNoeudGenerateurBille(NoeudGenerateurBille* noeud)
///
/// Cette fonction traite le noeud en lui assignant une rotation.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurRotation::traiterNoeudGenerateurBille(NoeudGenerateurBille* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurRotation::traiterNoeudPaletteDroitJ1(NoeudPaletteDroitJ1* noeud)
///
/// Cette fonction traite le noeud en lui assignant une rotation.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurRotation::traiterNoeudPaletteDroitJ1(NoeudPaletteDroitJ1* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurRotation::traiterNoeudPaletteDroitJ2(NoeudPaletteDroitJ2* noeud)
///
/// Cette fonction traite le noeud en lui assignant une rotation.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurRotation::traiterNoeudPaletteDroitJ2(NoeudPaletteDroitJ2* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurRotation::traiterNoeudPaletteGaucheJ1(NoeudPaletteGaucheJ1* noeud)
///
/// Cette fonction traite le noeud en lui assignant une rotation.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurRotation::traiterNoeudPaletteGaucheJ1(NoeudPaletteGaucheJ1* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurRotation::traiterNoeudPaletteGaucheJ2(NoeudPaletteGaucheJ2* noeud)
///
/// Cette fonction traite le noeud en lui assignant une rotation.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurRotation::traiterNoeudPaletteGaucheJ2(NoeudPaletteGaucheJ2* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurRotation::traiterNoeudRessort(NoeudRessort* noeud)
///
/// Cette fonction traite le noeud en lui assignant une rotation.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurRotation::traiterNoeudRessort(NoeudRessort* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurRotation::traiterNoeudTrou(NoeudTrou* noeud)
///
/// Cette fonction traite le noeud en lui assignant une rotation.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurRotation::traiterNoeudTrou(NoeudTrou* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurRotation::traiterNoeudMur(NoeudMur* noeud)
///
/// Cette fonction traite le noeud en lui assignant une rotation.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurRotation::traiterNoeudMur(NoeudMur* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurRotation::traiterNoeudPortail(NoeudPortail* noeud)
///
/// Cette fonction traite le noeud en lui assignant une rotation.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurRotation::traiterNoeudPortail(NoeudPortail* noeud)
{
	traiterNoeud(noeud);
}

void VisiteurRotation::traiterNoeudChampForce(NoeudChampForce* noeud)
{
	traiterNoeud(noeud);
}

void VisiteurRotation::traiterNoeudPlateauDArgent(NoeudPlateauDArgent* noeud)
{
	traiterNoeud(noeud);
}

