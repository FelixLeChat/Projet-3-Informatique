#pragma once
#include "IJoueur.h"

class JoueurBot : public IJoueur
{
public:
	JoueurBot(int playerNum, string id, bool estConnecte = false);

	~JoueurBot() override;

	virtual void update(IEvent* e);
	void activerPalettesGauches();
	void activerPalettesDroites();
	void desactiverPalettesGauches();
	void desactiverPalettesDroites();
	void activerRessort();
	void desactiverRessort();

private:
	double counter_;
	bool estConnecte_;
	double dtDroit_;
	double dtGauche_;
};