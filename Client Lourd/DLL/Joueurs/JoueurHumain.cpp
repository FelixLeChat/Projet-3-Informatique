#include "JoueurHumain.h"
#include "../Event/KeyPressEvent.h"
#include "../Event/EventManager.h"
#include "../Configuration/Config.h"
#include "../Reseau/NetworkManager.h"
#include "../Event/PlayerActionReceiveEvent.h"


JoueurHumain::JoueurHumain(int numJoueur,string id, bool enLigne)
	:IJoueur{numJoueur}
{
	id_ = id;
	estConnecte_ = enLigne;
	EventManager::GetInstance()->subscribe(this, INPUTEVENT);
}

JoueurHumain::~JoueurHumain()
{
	EventManager::GetInstance()->unsubscribe(this, INPUTEVENT);
}

void JoueurHumain::update(IEvent* e)
{
	IJoueur::update(e);
	if (e->getType() == INPUTEVENT)
	{
		auto keyEvent = ((KeyPressEvent*)e);
		int keyPressed = keyEvent->getKeyCode();
		auto config = Config::obtenirInstance();
		if (keyEvent->getAppuye())
		{
			//activer
			if (keyPressed == config->getRes())
				activerRessort();
			else
			{
				if (playerNum_ == 0 || estConnecte_)
				{
					if (keyPressed == config->getPd1())
						activerPalettesDroites();
					else if (keyPressed == config->getPg1())
						activerPalettesGauches();
				}
				else
				{
					if (keyPressed == config->getPd2())
						activerPalettesDroites();
					else if (keyPressed == config->getPg2())
						activerPalettesGauches();
				}
			}
		}
		else
		{
			//desactiver
			if (keyPressed == config->getRes())
				desactiverRessort();
			else
			{
				if (playerNum_ == 0 || estConnecte_)
				{
					if (keyPressed == config->getPd1())
						desactiverPalettesDroites();
					else if (keyPressed == config->getPg1())
						desactiverPalettesGauches();
				}
				else
				{
					if (keyPressed == config->getPd2())
						desactiverPalettesDroites();
					else if (keyPressed == config->getPg2())
						desactiverPalettesGauches();
				}
			}
		}
	}
}

void JoueurHumain::activerPalettesGauches()
{
	IJoueur::activerPalettesGauches();
	if (estConnecte_)
		NetworkManager::getInstance()->playerAction(APPUI_PALETTE_GAUCHE);
}

void JoueurHumain::activerPalettesDroites()
{
	IJoueur::activerPalettesDroites();
	if (estConnecte_)
		NetworkManager::getInstance()->playerAction(APPUI_PALETTE_DROITE);
}

void JoueurHumain::desactiverPalettesGauches()
{
	IJoueur::desactiverPalettesGauches();
	if (estConnecte_)
		NetworkManager::getInstance()->playerAction(RELACHE_PALETTE_GAUCHE);
}

void JoueurHumain::desactiverPalettesDroites()
{
	IJoueur::desactiverPalettesDroites();
	if (estConnecte_)
		NetworkManager::getInstance()->playerAction(RELACHE_PALETTE_DROITE);
}

void JoueurHumain::activerRessort()
{
	IJoueur::activerRessort();
	if (estConnecte_)
		NetworkManager::getInstance()->playerAction(APPUI_RESSORT);
}

void JoueurHumain::desactiverRessort()
{
	IJoueur::desactiverRessort();
	if (estConnecte_)
		NetworkManager::getInstance()->playerAction(RELACHE_RESSORT);
}