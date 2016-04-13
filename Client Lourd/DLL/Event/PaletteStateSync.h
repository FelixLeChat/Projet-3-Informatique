#pragma once
#include "IEvent.h"

class NoeudPalette;

class PaletteStateSync : public IEvent
{
public:
	PaletteStateSync(double rotation, double scale, int player_num, bool de_droite, bool actif)
		: IEvent(PALETTESTATESYNC),
		  rotation_(rotation),
		  scale_(scale),
		  playerNum_(player_num),
		  deDroite_(de_droite),
		  actif_(actif)
	{
	}

	PaletteStateSync(NoeudPalette* np, bool deDroite);


	double rotation() const;
	double scale() const;
	int player_num() const;
	bool de_droite() const;
	bool actif() const;
private:
	double rotation_;
	double scale_;
	int playerNum_;
	bool deDroite_;
	bool actif_;
	
};
