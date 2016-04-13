#include "PowerUpPaletteEvent.h"


PowerUpPaletteEvent::PowerUpPaletteEvent(int playerNum, double facteurMultiplicatif)
{
	type_ = PALETTEPOWERUP;
	playerNum_ = playerNum;
	facteurMultiplicatif_ = facteurMultiplicatif;
}

PowerUpPaletteEvent::~PowerUpPaletteEvent()
{}


int PowerUpPaletteEvent::getPlayerNum() const
{
	return playerNum_;
}

double PowerUpPaletteEvent::getFacteur() const
{
	return facteurMultiplicatif_;
}

