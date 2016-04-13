#ifndef __GAMELOGIC_JOUEUR_MANAGER_H__
#define __GAMELOGIC_JOUEUR_MANAGER_H__

#include "IJoueurManager.h"

class JoueurManagerLocal : public IJoueurManager
{
public:
	JoueurManagerLocal(){}
	JoueurManagerLocal(int nbJoueurs, bool ai);

	~JoueurManagerLocal();


	void assignerControles(VisiteurObtenirControles visiteur, ZoneDeJeu* zone);

private:
	bool ai_;
};

#endif // !__GAMELOGIC_JOUEUR_MANAGER_H__
