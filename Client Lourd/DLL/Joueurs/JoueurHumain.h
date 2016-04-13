#ifndef __GAMELOGIC_JOUEUR_HUMAIN_H__
#define __GAMELOGIC_JOUEUR_HUMAIN_H__

#include "IJoueur.h"

class JoueurHumain : public IJoueur
{
public:
	JoueurHumain() = delete;
	JoueurHumain(int numJoueur,string id = "", bool estConnecte = false);

	~JoueurHumain();

	void update(IEvent* e) override;

protected:
	void activerPalettesGauches() override;
	void activerPalettesDroites() override;
	void desactiverPalettesGauches() override;
	void desactiverPalettesDroites() override;


	void activerRessort() override;
	void desactiverRessort() override;

private:
	bool estConnecte_;

};

#endif // !__GAMELOGIC_JOUEUR_HUMAIN_H__
