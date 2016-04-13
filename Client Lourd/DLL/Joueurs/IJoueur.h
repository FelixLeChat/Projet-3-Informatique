#ifndef __GAMELOGIC_JOUEUR_INTERFACE_H__
#define __GAMELOGIC_JOUEUR_INTERFACE_H__

#include "../Event/IEventSubscriber.h"
#include "../Arbre/Noeuds/Palette/NoeudPalette.h"
#include "../Arbre/Noeuds/NoeudRessort.h"

class IJoueur : public IEventSubscriber
{
public:
	IJoueur(int playerNum);

	virtual ~IJoueur();

	void assignerControles(vector<NoeudPalette*> paletteGauche, vector<NoeudPalette*> paletteDroite, vector<NoeudRessort*> ressorts, bool fantome = false);

	void assignerId(string id);

	virtual void update(IEvent* e);
	string obtenirId();
	virtual bool estLocal();
	vector<NoeudPalette*> palettes_droites() const;
	vector<NoeudPalette*> palettes_gauches() const;
	vector<NoeudRessort*> ressorts() const;
protected:
	vector<NoeudPalette*> palettesDroites_;
	vector<NoeudPalette*> palettesGauches_;
	vector<NoeudRessort*> ressorts_;

	virtual void activerPalettesGauches();
	virtual void activerPalettesDroites();
	virtual void desactiverPalettesGauches();
	virtual void desactiverPalettesDroites();


	virtual void activerRessort();
	virtual void desactiverRessort();

	string id_;
	int playerNum_; 
};

#endif // !__GAMELOGIC_JOUEUR_INTERFACE_H__
