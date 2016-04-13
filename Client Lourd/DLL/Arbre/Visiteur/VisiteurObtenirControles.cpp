#include "VisiteurObtenirControles.h"

VisiteurObtenirControles::VisiteurObtenirControles()
{
}

void VisiteurObtenirControles::traiterNoeudPaletteDroitJ1(NoeudPaletteDroitJ1* noeud)
{
	palettesDroitesJ1.push_back((NoeudPalette*)noeud);
}
void VisiteurObtenirControles::traiterNoeudPaletteDroitJ2(NoeudPaletteDroitJ2* noeud)
{
	palettesDroitesJ2.push_back((NoeudPalette*)noeud);

}
void VisiteurObtenirControles::traiterNoeudPaletteGaucheJ1(NoeudPaletteGaucheJ1* noeud)
{
	palettesGauchesJ1.push_back((NoeudPalette*)noeud);

}
void VisiteurObtenirControles::traiterNoeudPaletteGaucheJ2(NoeudPaletteGaucheJ2* noeud)
{
	palettesGauchesJ2.push_back((NoeudPalette*)noeud);

}

void VisiteurObtenirControles::traiterNoeudRessort(NoeudRessort*noeud)
{
	ressorts.push_back(noeud);

}


std::vector<NoeudPalette*> VisiteurObtenirControles::getpalettesDroitesJ1()
{
	return palettesDroitesJ1;
}
std::vector<NoeudPalette*> VisiteurObtenirControles::getpalettesDroitesJ2()
{
	return palettesDroitesJ2;
}
std::vector<NoeudPalette*> VisiteurObtenirControles::getpalettesGauchesJ1()
{
	return palettesGauchesJ1;
}
std::vector<NoeudPalette*> VisiteurObtenirControles::getpalettesGauchesJ2()
{
	return palettesGauchesJ2;
}

std::vector<NoeudRessort*> VisiteurObtenirControles::getressorts()
{
	return ressorts;
}