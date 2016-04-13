#include "JoueurEnLigne.h"
#include "../Event/EventManager.h"
#include "../Event/PlayerActionReceiveEvent.h"

JoueurEnLigne::JoueurEnLigne(int playerNum, string userId)
	:IJoueur{playerNum}
{
	id_ = userId;
	EventManager::GetInstance()->subscribe(this, NETWORKPLAYERACTION);
}

JoueurEnLigne::~JoueurEnLigne()
{
	EventManager::GetInstance()->unsubscribe(this, NETWORKPLAYERACTION);
}

void JoueurEnLigne::update(IEvent* e)
{
	IJoueur::update(e);
	if (e->getType() == NETWORKPLAYERACTION)
	{
		auto ev = static_cast<PlayerActionReceiveEvent*>(e);
		if (ev->getUserId() == id_)
		{
			switch (ev->getAction())
			{
			case APPUI_PALETTE_DROITE:
				activerPalettesDroites();
				break;
			case APPUI_PALETTE_GAUCHE:
				activerPalettesGauches();
				break;
			case APPUI_RESSORT:
				activerRessort();
				break;
			case RELACHE_PALETTE_DROITE:
				desactiverPalettesDroites();
				break;
			case RELACHE_PALETTE_GAUCHE:
				desactiverPalettesGauches();
				break;
			case RELACHE_RESSORT:
				desactiverRessort();
				break;
			default:
				break;
			}
		}
	}
}

bool JoueurEnLigne::estLocal()
{
	return false;
}