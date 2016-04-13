#include "PaletteStateSync.h"
#include "Arbre/Noeuds/Palette/NoeudPalette.h"

PaletteStateSync::PaletteStateSync(NoeudPalette* np, bool deDroite):
	rotation_(np->obtenirRotationPalette()),
	scale_(np->obtenirAgrandissement().x),
	playerNum_(np->obtenirPlayerNum()),
	deDroite_(deDroite),
	actif_(np->obtenirEstActif()),
	IEvent(PALETTESTATESYNC)
{

}

double PaletteStateSync::rotation() const
{
	return rotation_;
}

double PaletteStateSync::scale() const
{
	return scale_;
}

int PaletteStateSync::player_num() const
{
	return playerNum_;
}

bool PaletteStateSync::de_droite() const
{
	return deDroite_;
}

bool PaletteStateSync::actif() const
{
	return actif_;
}
