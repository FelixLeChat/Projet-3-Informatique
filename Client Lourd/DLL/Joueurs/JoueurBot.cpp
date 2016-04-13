#include "JoueurBot.h"
#include "../Event/EventManager.h"
#include "../Event/TimeEvent.h"
#include <algorithm>
#include "../Reseau/NetworkManager.h"

JoueurBot::JoueurBot(int playerNum, string id, bool estConnecte)
	:IJoueur{playerNum}, estConnecte_{estConnecte}
{
	id_ = id;
	EventManager::GetInstance()->subscribe(this, TIMEEVENT);
	dtDroit_ = 0;
	dtGauche_ = 0;
	counter_ = 0;
}

JoueurBot::~JoueurBot()
{
	EventManager::GetInstance()->unsubscribe(this, TIMEEVENT);
}

void JoueurBot::update(IEvent* e)
{
	IJoueur::update(e);
	if(e->getType() == TIMEEVENT)
	{
		auto te = static_cast<TimeEvent *>(e);
		counter_ += te->getDt();
		// Reste a synchroniser comme joueur humain, ai seulement chez host
		if(counter_ >= 0.05)
		{
			dtDroit_ += counter_;


			if(dtGauche_<3 && any_of(palettesGauches_.begin(), palettesGauches_.end(), [&](NoeudPalette* p) {return p->aBilleAFrapper(); }))
			{
				activerPalettesGauches();
				dtGauche_ += counter_;
			}
			else
			{
				if (dtGauche_ >= 3)
					dtGauche_ += counter_;
				if (dtGauche_ >= 4)
					dtGauche_ = 0;
				desactiverPalettesGauches();
			}

			if (dtDroit_<3 && any_of(palettesDroites_.begin(), palettesDroites_.end(), [&](NoeudPalette* p) {return p->aBilleAFrapper(); }))
			{
				activerPalettesDroites();
				dtDroit_ += counter_;
			}
			else
			{
				desactiverPalettesDroites();
				if (dtDroit_ >= 3)
					dtDroit_ += counter_;
				if (dtDroit_ >= 4)
					dtDroit_ = 0;
			}

			counter_ = 0;
		}
	}
}



void JoueurBot::activerPalettesGauches()
{
	IJoueur::activerPalettesGauches();
	if (estConnecte_)
		NetworkManager::getInstance()->playerAction(APPUI_PALETTE_GAUCHE, id_);
}

void JoueurBot::activerPalettesDroites()
{
	IJoueur::activerPalettesDroites();
	if (estConnecte_)
		NetworkManager::getInstance()->playerAction(APPUI_PALETTE_DROITE, id_);
}

void JoueurBot::desactiverPalettesGauches()
{
	IJoueur::desactiverPalettesGauches();
	if (estConnecte_)
		NetworkManager::getInstance()->playerAction(RELACHE_PALETTE_GAUCHE, id_);
}

void JoueurBot::desactiverPalettesDroites()
{
	IJoueur::desactiverPalettesDroites();
	if (estConnecte_)
		NetworkManager::getInstance()->playerAction(RELACHE_PALETTE_DROITE, id_);
}

void JoueurBot::activerRessort()
{
	IJoueur::activerRessort();
	if (estConnecte_)
		NetworkManager::getInstance()->playerAction(APPUI_RESSORT, id_);
}

void JoueurBot::desactiverRessort()
{
	IJoueur::desactiverRessort();
	if (estConnecte_)
		NetworkManager::getInstance()->playerAction(RELACHE_RESSORT, id_);
}

