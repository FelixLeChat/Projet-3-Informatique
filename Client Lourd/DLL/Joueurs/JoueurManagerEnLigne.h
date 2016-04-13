#pragma once
#include "IJoueurManager.h"
#include "../Event/IEventSubscriber.h"
#include <Event/PaletteStateSync.h>

class JoueurManagerEnLigne : public IJoueurManager, IEventSubscriber // Listen to disconnects and reconnets
{
public:
	JoueurManagerEnLigne(vector<string> playerIds, int nbAi, bool estCoop, bool estHost);
	~JoueurManagerEnLigne();
	void update(IEvent * e) override;
	void assignerControles(VisiteurObtenirControles visiteur, ZoneDeJeu* zone) override;
	void reconnect(string userId) override;
	vector<PaletteStateSync> obtenirEtatPalettes() const;
private:
	vector<string> playerIds_;
	int nbAi_;
	bool estCoop_;
	
};
