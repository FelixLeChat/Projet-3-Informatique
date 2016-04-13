#include "VisiteurReinit.h"
#include "../Noeuds/NoeudAbstrait.h"
#include "../Noeuds/NoeudButoirCirculaire.h"

#include "../Noeuds/NoeudCible.h"

#include "../Noeuds/Palette/NoeudPaletteDroitJ1.h"
#include "../Noeuds/Palette/NoeudPaletteDroitJ2.h"
#include "../Noeuds/Palette/NoeudPaletteGaucheJ1.h"
#include "../Noeuds/Palette/NoeudPaletteGaucheJ2.h"

#include "../Noeuds/NoeudRessort.h"
#include "../Noeuds/NoeudPortail.h"

#include "../Noeuds/PowerUp/NoeudPlateauDArgent.h"


////////////////////////////////////////////////////////////////////////
///
/// @fn VisiteurReinit::VisiteurReinit()
///
/// Constructeur du visiteur de reinitialisation.
///
///
/// @return Aucune (Constructeur)
///
////////////////////////////////////////////////////////////////////////
VisiteurReinit::VisiteurReinit()
{
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurReinit::traiterNoeudButoirCirculaire(NoeudButoirCirculaire* noeud)
///
/// Cette fonction reinitialise le noeud
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurReinit::traiterNoeudButoirCirculaire(NoeudButoirCirculaire* noeud)
{
	noeud->assignerAllumer(false);
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurReinit::traiterNoeudCible(NoeudCible* noeud)
///
///  Cette fonction reinitialise le noeud
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurReinit::traiterNoeudCible(NoeudCible* noeud)
{
	noeud->reinitialiser();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurReinit::traiterNoeudPaletteDroitJ1(NoeudPaletteDroitJ1* noeud)
///
///  Cette fonction reinitialise le noeud
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurReinit::traiterNoeudPaletteDroitJ1(NoeudPaletteDroitJ1* noeud)
{
	noeud->reinitialiser();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurReinit::traiterNoeudPaletteDroitJ2(NoeudPaletteDroitJ2* noeud)
///
///  Cette fonction reinitialise le noeud
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurReinit::traiterNoeudPaletteDroitJ2(NoeudPaletteDroitJ2* noeud)
{
	noeud->reinitialiser();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurReinit::traiterNoeudPaletteGaucheJ1(NoeudPaletteGaucheJ1* noeud)
///
///  Cette fonction reinitialise le noeud
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurReinit::traiterNoeudPaletteGaucheJ1(NoeudPaletteGaucheJ1* noeud)
{
	noeud->reinitialiser();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurReinit::traiterNoeudPaletteGaucheJ2(NoeudPaletteGaucheJ2* noeud)
///
///  Cette fonction reinitialise le noeud
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurReinit::traiterNoeudPaletteGaucheJ2(NoeudPaletteGaucheJ2* noeud)
{
	noeud->reinitialiser();
}

////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurReinit::traiterNoeudRessort(NoeudRessort* noeud)
///
///  Cette fonction reinitialise le noeud
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurReinit::traiterNoeudRessort(NoeudRessort* noeud)
{
	noeud->reinitialiser();
}


////////////////////////////////////////////////////////////////////////
///
/// @fn void VisiteurReinit::traiterNoeudPortail(NoeudPortail* noeud)
///
///  Cette fonction reinitialise le noeud
///
/// @param[in] noeud : le noeud à traiter.
///
/// @return Aucune.
///
////////////////////////////////////////////////////////////////////////
void VisiteurReinit::traiterNoeudPortail(NoeudPortail* noeud)
{
	noeud->reinitialiser();
}

void VisiteurReinit::traiterNoeudPlateauDArgent(NoeudPlateauDArgent* noeud)
{
	noeud->reinitialiser();
}


