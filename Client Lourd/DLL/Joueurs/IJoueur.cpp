#include "IJoueur.h"
#include "../Event/EventManager.h"
#include "../Event/PowerUpPaletteEvent.h"

IJoueur::IJoueur(int playerNum):
	playerNum_{playerNum}
{
	palettesDroites_ = vector<NoeudPalette*>();
	palettesGauches_ = vector<NoeudPalette*>();
	ressorts_ = vector<NoeudRessort*>();
	EventManager::GetInstance()->subscribe(this, PALETTEPOWERUP);
}

IJoueur::~IJoueur()
{
	EventManager::GetInstance()->unsubscribe(this, PALETTEPOWERUP);
}

void IJoueur::assignerId(string id)
{
	id_ = id;
}


void IJoueur::assignerControles(vector<NoeudPalette*> palettesGauches, vector<NoeudPalette*> palettesDroites, vector<NoeudRessort*> ressorts, bool fantome)
{
	palettesGauches_.clear();
	palettesDroites_.clear();
	palettesDroites_ = palettesDroites;
	palettesGauches_ = palettesGauches;

	for each(auto palette in palettesDroites_)
	{
		palette->assignerPlayerNum(playerNum_);
		palette->assignerFantome(fantome);
	}
	for each(auto palette in palettesGauches_)
	{
		palette->assignerPlayerNum(playerNum_);
		palette->assignerFantome(fantome);
	}
	ressorts_ = ressorts;
}

void IJoueur::activerPalettesDroites()
{
	for each (auto palette in palettesDroites_)
	{
		palette->activer();
	}
}

void IJoueur::activerPalettesGauches()
{
	for each (auto palette in palettesGauches_)
	{
		palette->activer();
	}
}

void IJoueur::desactiverPalettesDroites()
{
	for each (auto palette in palettesDroites_)
	{
		palette->desactiver();
	}
}

void IJoueur::desactiverPalettesGauches()
{
	for each (auto palette in palettesGauches_)
	{
		palette->desactiver();
	}
}


void IJoueur::activerRessort()
{
	for each (auto ressort in ressorts_)
	{
		ressort->activer();
	}
}

void IJoueur::desactiverRessort()
{
	for each (auto ressort in ressorts_)
	{
		ressort->desactiver();
	}
}

void IJoueur::update(IEvent* e)
{
	if (e->getType() == PALETTEPOWERUP)
	{
		PowerUpPaletteEvent* powerUpEvent = (PowerUpPaletteEvent*)e;
		if (powerUpEvent->getPlayerNum() == playerNum_)
		{
			for each(NoeudPalette* p in palettesDroites_)
			{
				p->assignerAgrandissement(p->obtenirAgrandissement()*powerUpEvent->getFacteur());
			}
			for each(NoeudPalette* p in palettesGauches_)
			{
				p->assignerAgrandissement(p->obtenirAgrandissement()*powerUpEvent->getFacteur());
			}
		}
	}
}

string IJoueur::obtenirId()
{
	return id_;
}

bool IJoueur::estLocal()
{
	return true;
}

vector<NoeudPalette*> IJoueur::palettes_droites() const
{
	return palettesDroites_;
}

vector<NoeudPalette*> IJoueur::palettes_gauches() const
{
	return palettesGauches_;
}

vector<NoeudRessort*> IJoueur::ressorts() const
{
	return ressorts_;
}