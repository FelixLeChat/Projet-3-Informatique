#ifndef __JOUEURS_ENLIGNE__
#define __JOUEURS_ENLIGNE__

#include "IJoueur.h"

class JoueurEnLigne: public IJoueur
{
public:
	JoueurEnLigne(int playerNum ,string userId);

	~JoueurEnLigne();

	virtual void update(IEvent* e);

	bool estLocal() override;

};

#endif // !__JOUEURS_ENLIGNE__
