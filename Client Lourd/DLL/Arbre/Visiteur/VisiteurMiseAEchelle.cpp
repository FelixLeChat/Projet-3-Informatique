#include "VisiteurMiseAEchelle.h"
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


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurMiseAEchelle::VisiteurMiseAEchelle(double dy)
///
/// Constructeur par paramètre du visiteur de mise à échelle.
///
/// @param[in] dy : déplacement en y.
///
/// @return Aucune (Constructeur)
///
////////////////////////////////////////////////////////////////////////
VisiteurMiseAEchelle::VisiteurMiseAEchelle(double dy)
{
	this->dy_ = dy;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurMiseAEchelle::traiterNoeud(NoeudAbstrait* noeudBasic)
///
/// Cette fonction traite le noeud en lui assignant un agrandissement.
///
/// @param[in] noeudBasic : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurMiseAEchelle::traiterNoeud(NoeudAbstrait* noeudBasic)
{
	if (noeudBasic->estSelectionne())
	{
		glm::dvec3 nouvelAgrandissement = noeudBasic->obtenirAgrandissement();
		nouvelAgrandissement.x += dy_;
		nouvelAgrandissement.y += dy_;

		if (nouvelAgrandissement.x < 0.5)
		{
			nouvelAgrandissement.x = 0.5;
			nouvelAgrandissement.y = 0.5;
		}
		else
		{
			if (nouvelAgrandissement.x > 2)
			{
				nouvelAgrandissement.x = 2;
				nouvelAgrandissement.y = 2;
			}
		}
		noeudBasic->assignerAgrandissement(nouvelAgrandissement);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurMiseAEchelle::traiterNoeudButoirCirculaire(NoeudButoirCirculaire* noeud)
///
/// Cette fonction traite le noeud en lui assignant un agrandissement.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurMiseAEchelle::traiterNoeudButoirCirculaire(NoeudButoirCirculaire* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurMiseAEchelle::traiterNoeudButoirTriangulaireDroit(NoeudButoirTriangulaireDroit* noeud)
///
/// Cette fonction traite le noeud en lui assignant un agrandissement.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurMiseAEchelle::traiterNoeudButoirTriangulaireDroit(NoeudButoirTriangulaireDroit* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurMiseAEchelle::traiterNoeudButoirTriangulaireGauche(NoeudButoirTriangulaireGauche* noeud)
///
/// Cette fonction traite le noeud en lui assignant un agrandissement.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurMiseAEchelle::traiterNoeudButoirTriangulaireGauche(NoeudButoirTriangulaireGauche* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurMiseAEchelle::traiterNoeudCible(NoeudCible* noeud)
///
/// Cette fonction traite le noeud en lui assignant un agrandissement.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurMiseAEchelle::traiterNoeudCible(NoeudCible* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurMiseAEchelle::traiterNoeudGenerateurBille(NoeudGenerateurBille* noeud)
///
/// Cette fonction traite le noeud en lui assignant un agrandissement.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurMiseAEchelle::traiterNoeudGenerateurBille(NoeudGenerateurBille* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurMiseAEchelle::traiterNoeudPaletteDroitJ1(NoeudPaletteDroitJ1* noeud)
///
/// Cette fonction traite le noeud en lui assignant un agrandissement.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurMiseAEchelle::traiterNoeudPaletteDroitJ1(NoeudPaletteDroitJ1* noeud)
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurMiseAEchelle::traiterNoeudPaletteDroitJ2(NoeudPaletteDroitJ2* noeud)
///
/// Cette fonction traite le noeud en lui assignant un agrandissement.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurMiseAEchelle::traiterNoeudPaletteDroitJ2(NoeudPaletteDroitJ2* noeud)
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurMiseAEchelle::traiterNoeudPaletteGaucheJ1(NoeudPaletteGaucheJ1* noeud)
///
/// Cette fonction traite le noeud en lui assignant un agrandissement.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurMiseAEchelle::traiterNoeudPaletteGaucheJ1(NoeudPaletteGaucheJ1* noeud)
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurMiseAEchelle::traiterNoeudPaletteGaucheJ2(NoeudPaletteGaucheJ2* noeud)
///
/// Cette fonction traite le noeud en lui assignant un agrandissement.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurMiseAEchelle::traiterNoeudPaletteGaucheJ2(NoeudPaletteGaucheJ2* noeud)
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurMiseAEchelle::traiterNoeudRessort(NoeudRessort* noeud)
///
/// Cette fonction traite le noeud en lui assignant un agrandissement.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurMiseAEchelle::traiterNoeudRessort(NoeudRessort* noeud)
{
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurMiseAEchelle::traiterNoeudTrou(NoeudTrou* noeud)
///
/// Cette fonction traite le noeud en lui assignant un agrandissement.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurMiseAEchelle::traiterNoeudTrou(NoeudTrou* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurMiseAEchelle::traiterNoeudMur(NoeudMur* noeud)
///
/// Cette fonction traite le noeud en lui assignant un agrandissement.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurMiseAEchelle::traiterNoeudMur(NoeudMur* noeud)
{
	if (noeud->estSelectionne())
	{
		glm::dvec3 nouvelAgrandissement = noeud->obtenirAgrandissement();
		nouvelAgrandissement.y += dy_;

		if (nouvelAgrandissement.y < 0.5)
		{
			nouvelAgrandissement.y = 0.5;
		}
		else
		{
			if (nouvelAgrandissement.y > 2)
			{
				nouvelAgrandissement.y = 2;
			}
		}
		
		noeud->assignerAgrandissement(nouvelAgrandissement);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurMiseAEchelle::traiterNoeudPortail(NoeudPortail* noeud)
///
/// Cette fonction traite le noeud en lui assignant un agrandissement.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurMiseAEchelle::traiterNoeudPortail(NoeudPortail* noeud)
{
	traiterNoeud(noeud);
}

void VisiteurMiseAEchelle::traiterNoeudChampForce(NoeudChampForce* noeud)
{
	traiterNoeud(noeud);
}

void VisiteurMiseAEchelle::traiterNoeudPlateauDArgent(NoeudPlateauDArgent* noeud)
{
	traiterNoeud(noeud);
}
