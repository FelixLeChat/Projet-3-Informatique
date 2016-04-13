#include "JoueurManagerLocal.h"
#include "JoueurHumain.h"
#include "JoueurBot.h"
#include "../Reseau/NetworkManager.h"

JoueurManagerLocal::JoueurManagerLocal(int nbJoueurs, bool ai)
{
	ai_ = ai;

	joueurs_.push_back(new JoueurHumain(0,"", false));
	if (nbJoueurs == 2)
	{
		if (ai)
			joueurs_.push_back(new JoueurBot(1, "bot1"));
		else
			joueurs_.push_back(new JoueurHumain(1,"", false));
	}
}

JoueurManagerLocal::~JoueurManagerLocal()
{
}


void JoueurManagerLocal::assignerControles(VisiteurObtenirControles visiteur, ZoneDeJeu* zone)
{
	auto palettesGauches = visiteur.getpalettesGauchesJ1();
	auto palettesGauches2 = visiteur.getpalettesGauchesJ2();
	auto palettesDroites = visiteur.getpalettesDroitesJ1();
	auto palettesDroites2 = visiteur.getpalettesDroitesJ2();
	auto ressorts = visiteur.getressorts();

	if (joueurs_.size() == 1)
	{
		palettesGauches.insert(palettesGauches.end(), palettesGauches2.begin(), palettesGauches2.end());
		palettesDroites.insert(palettesDroites.end(), palettesDroites2.begin(), palettesDroites2.end());
	}
	else
	{
		
		joueurs_[1]->assignerControles(palettesGauches2, palettesDroites2, ressorts);
	}
	joueurs_[0]->assignerControles(palettesGauches, palettesDroites, ressorts);
}