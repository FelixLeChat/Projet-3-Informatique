#include "JoueurManagerEnLigne.h"
#include "../Reseau/NetworkManager.h"
#include "JoueurBot.h"
#include "JoueurHumain.h"
#include "JoueurEnLigne.h"
#include "../Event/EventManager.h"
#include "../Event/DisconnectEvent.h"

JoueurManagerEnLigne::JoueurManagerEnLigne(vector<string> playerIds, int nbAi, bool estCoop, bool estHost)
	: nbAi_{nbAi}, estCoop_{ estCoop }
{
	playerIds_ = playerIds;
	int playerNum = 0;
	for each(auto id in playerIds_)
	{
		if(id == NetworkManager::getInstance()->getUserId())
		{
			joueurs_.push_back(new JoueurHumain(playerNum,id, true));
		}
		else
		{
			joueurs_.push_back(new JoueurEnLigne(playerNum, id));
		}
		playerNum++;
	}
	if (estHost)
	{
		for (int j = 0; j < nbAi; j++)
		{
			string id = "bot" + to_string(playerNum);
			joueurs_.push_back(new JoueurBot(playerNum, id, true)); // Seulement pour le host
			playerNum++;
		}
	}
	else
	{
		for (int j = 0; j < nbAi; j++)
		{
			string id = "bot" + to_string(playerNum);
			joueurs_.push_back(new JoueurEnLigne(playerNum,id)); // Seulement pour le host
			playerNum++;
		}
	}

	if(estHost)
	{
		EventManager::GetInstance()->subscribe(this, DISCONNECTEVENT);
	}
}

JoueurManagerEnLigne::~JoueurManagerEnLigne()
{
	EventManager::GetInstance()->unsubscribe(this, DISCONNECTEVENT);

}

void JoueurManagerEnLigne::update(IEvent* e)
{
	if(e->getType() == DISCONNECTEVENT)
	{
		auto disconnect = static_cast<DisconnectEvent *>(e);
		auto i =obtenirPlayerNum(disconnect->user_id());
		if(i != joueurs_.size())
		{
			auto pGauches = joueurs_[i]->palettes_gauches();
			auto pDroites = joueurs_[i]->palettes_droites();
			auto ressorts = joueurs_[i]->ressorts();
			delete joueurs_[i];
			joueurs_[i] = new JoueurBot(i, disconnect->user_id(),true);
			joueurs_[i]->assignerControles(pGauches, pDroites, ressorts, !estCoop_);

		}
	}
	//Gerer deconnection et reconnection
}

void JoueurManagerEnLigne::assignerControles(VisiteurObtenirControles visiteur, ZoneDeJeu* zone)
{
	auto palettesGauches = visiteur.getpalettesGauchesJ1();
	auto palettesGauches2 = visiteur.getpalettesGauchesJ2();
	auto palettesDroites = visiteur.getpalettesDroitesJ1();
	auto palettesDroites2 = visiteur.getpalettesDroitesJ2();
	auto ressorts = visiteur.getressorts();
	
	if (estCoop_)
	{
		if (joueurs_.size() == 1)
		{
			palettesGauches.insert(palettesGauches.end(), palettesGauches2.begin(), palettesGauches2.end());
			palettesDroites.insert(palettesDroites.end(), palettesDroites2.begin(), palettesDroites2.end());
			//si pas joueur local, assigner palettes fantomes si competitif
			joueurs_[0]->assignerControles(palettesGauches, palettesDroites, ressorts, false);
		}
		else
		{

			if (palettesGauches2.size() == 0)
			{
				palettesGauches2 = zone->dupliquerPalettes(palettesGauches);
			}
			if (palettesDroites2.size() == 0)
			{
				palettesDroites2 = zone->dupliquerPalettes(palettesDroites);
			}

			//si pas joueur local, assigner palettes fantomes si competitif

			if (joueurs_.size() >= 3)
			{
				auto palettesGauches3 = zone->dupliquerPalettes(palettesGauches);
				auto palettesDroites3 = zone->dupliquerPalettes(palettesDroites);
				//si pas joueur local, assigner palettes fantomes si competitif
				joueurs_[2]->assignerControles(palettesGauches3, palettesDroites3, ressorts, false);
			}
			if (joueurs_.size() == 4)
			{

				//si pas joueur local, assigner palettes fantomes si competitif
				auto palettesGauches4 = zone->dupliquerPalettes(palettesGauches2);
				auto palettesDroites4 = zone->dupliquerPalettes(palettesDroites2);
				joueurs_[3]->assignerControles(palettesGauches4, palettesDroites4, ressorts, false);
			}

			joueurs_[0]->assignerControles(palettesGauches, palettesDroites, ressorts, false);

			joueurs_[1]->assignerControles(palettesGauches2, palettesDroites2, ressorts, false);
		}
	}
	else
	{
		auto playerId = NetworkManager::getInstance()->getUserId();
		palettesGauches.insert(palettesGauches.end(), palettesGauches2.begin(), palettesGauches2.end());
		palettesDroites.insert(palettesDroites.end(), palettesDroites2.begin(), palettesDroites2.end());

		joueurs_[0]->assignerControles(palettesGauches, palettesDroites, ressorts, joueurs_[0]->obtenirId() != playerId);

		for(int i = 1; i<joueurs_.size(); i++)
		{
			auto palettesGauches5 = zone->dupliquerPalettes(palettesGauches);
			auto palettesDroites5 = zone->dupliquerPalettes(palettesDroites);
			joueurs_[i]->assignerControles(palettesGauches5, palettesDroites5, ressorts, joueurs_[i]->obtenirId() != playerId);
		}
	}
}

void JoueurManagerEnLigne::reconnect(string userId)
{
	auto i = obtenirPlayerNum(userId);
	if (i != joueurs_.size())
	{
		auto pGauches = joueurs_[i]->palettes_gauches();
		auto pDroites = joueurs_[i]->palettes_droites();
		auto ressorts = joueurs_[i]->ressorts();
		delete joueurs_[i];
		joueurs_[i] = new JoueurEnLigne(i, userId);
		joueurs_[i]->assignerControles(pGauches, pDroites, ressorts, !estCoop_);

	}
}

vector<PaletteStateSync> JoueurManagerEnLigne::obtenirEtatPalettes() const
{
	auto vec = vector<PaletteStateSync>();
	for each(auto joueur in joueurs_)
	{
		vec.push_back(PaletteStateSync(joueur->palettes_droites()[0], true));
		vec.push_back(PaletteStateSync(joueur->palettes_gauches()[0], false));
	}
	return vec;
}
