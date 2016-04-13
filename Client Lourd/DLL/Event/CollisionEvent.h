#ifndef __EVENT_COLLISIONEVENT_H__
#define __EVENT_COLLISIONEVENT_H__

#include "IEvent.h"

class CollisionEvent :public  IEvent
{
public:
	CollisionEvent(){}

	CollisionEvent(string noeudType, double facteurMultiplicatif = 1, int playerNum = 0);

	~CollisionEvent(){}

	string getTypeNoeud();
	int getPlayerNum();

	double getFacteurMultiplicatif();

private:
	string typeNoeud_;
	int playerNum_;
	double facteurMultiplicatif_;
};

#endif // !__EVENT_COLLISIONEVENT_H__
