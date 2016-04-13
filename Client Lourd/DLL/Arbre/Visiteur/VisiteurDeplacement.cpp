#include "VisiteurDeplacement.h"
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


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurDeplacement::VisiteurDeplacement(double dx_, double dy_, bool etampe)
///
/// Constructeur par paramètre du visiteur de déplacement.
///
/// @param[in] dx_ : déplacement en x.
/// @param[in] dy_ : déplacement en y.
/// @param[in] etampe : si l'objet est une étampe ou non.
///
/// @return Aucune (Constructeur)
///
////////////////////////////////////////////////////////////////////////
VisiteurDeplacement::VisiteurDeplacement(double dx_, double dy_, bool etampe)
{
	this->dx = dx_;
	this->dy = dy_;
	etampe_ = etampe;
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurDeplacement::traiterNoeud(NoeudAbstrait* noeudBasic)
///
/// Cette fonction traite un noeud en lui assignant une nouvelle position relative.
///
/// @param[in] noeudBasic : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::traiterNoeud(NoeudAbstrait* noeudBasic)
{
	if ((!etampe_ &&noeudBasic->estSelectionne())||(etampe_&&noeudBasic->estEtampe()))
	{
		glm::dvec3 nouvellePosition = noeudBasic->obtenirPositionRelative();
		nouvellePosition.x += dx;
		nouvellePosition.y += dy;
		noeudBasic->assignerPositionRelative(nouvellePosition);
	}
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurDeplacement::traiterNoeudButoirCirculaire(NoeudButoirCirculaire* noeud)
///
/// Cette fonction traite un noeud en lui assignant une nouvelle position relative.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::traiterNoeudButoirCirculaire(NoeudButoirCirculaire* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurDeplacement::traiterNoeudButoirTriangulaireDroit(NoeudButoirTriangulaireDroit* noeud)
///
/// Cette fonction traite un noeud en lui assignant une nouvelle position relative.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::traiterNoeudButoirTriangulaireDroit(NoeudButoirTriangulaireDroit* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurDeplacement::traiterNoeudButoirTriangulaireGauche(NoeudButoirTriangulaireGauche* noeud)
///
/// Cette fonction traite un noeud en lui assignant une nouvelle position relative.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::traiterNoeudButoirTriangulaireGauche(NoeudButoirTriangulaireGauche* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurDeplacement::traiterNoeudCible(NoeudCible* noeud)
///
/// Cette fonction traite un noeud en lui assignant une nouvelle position relative.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::traiterNoeudCible(NoeudCible* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurDeplacement::traiterNoeudGenerateurBille(NoeudGenerateurBille* noeud)
///
/// Cette fonction traite un noeud en lui assignant une nouvelle position relative.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::traiterNoeudGenerateurBille(NoeudGenerateurBille* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurDeplacement::traiterNoeudPaletteDroitJ1(NoeudPaletteDroitJ1* noeud)
///
/// Cette fonction traite un noeud en lui assignant une nouvelle position relative.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::traiterNoeudPaletteDroitJ1(NoeudPaletteDroitJ1* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurDeplacement::traiterNoeudPaletteDroitJ2(NoeudPaletteDroitJ2* noeud)
///
/// Cette fonction traite un noeud en lui assignant une nouvelle position relative.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::traiterNoeudPaletteDroitJ2(NoeudPaletteDroitJ2* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurDeplacement::traiterNoeudPaletteGaucheJ1(NoeudPaletteGaucheJ1* noeud)
///
/// Cette fonction traite un noeud en lui assignant une nouvelle position relative.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::traiterNoeudPaletteGaucheJ1(NoeudPaletteGaucheJ1* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurDeplacement::traiterNoeudPaletteGaucheJ2(NoeudPaletteGaucheJ2* noeud)
///
/// Cette fonction traite un noeud en lui assignant une nouvelle position relative.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::traiterNoeudPaletteGaucheJ2(NoeudPaletteGaucheJ2* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurDeplacement::traiterNoeudRessort(NoeudRessort* noeud)
///
/// Cette fonction traite un noeud en lui assignant une nouvelle position relative.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::traiterNoeudRessort(NoeudRessort* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurDeplacement::traiterNoeudTrou(NoeudTrou* noeud)
///
/// Cette fonction traite un noeud en lui assignant une nouvelle position relative.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::traiterNoeudTrou(NoeudTrou* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurDeplacement::traiterNoeudMur(NoeudMur* noeud)
///
/// Cette fonction traite un noeud en lui assignant une nouvelle position relative.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::traiterNoeudMur(NoeudMur* noeud)
{
	traiterNoeud(noeud);
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurDeplacement::traiterNoeudPortail(NoeudPortail* noeud)
///
/// Cette fonction traite un noeud en lui assignant une nouvelle position relative.
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurDeplacement::traiterNoeudPortail(NoeudPortail* noeud)
{
	traiterNoeud(noeud);
}

void VisiteurDeplacement::traiterNoeudChampForce(NoeudChampForce* noeud)
{
	traiterNoeud(noeud);
}

void VisiteurDeplacement::traiterNoeudPlateauDArgent(NoeudPlateauDArgent* noeud)
{
	traiterNoeud(noeud);
}

