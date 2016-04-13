#include "CollisionEvent.h"

CollisionEvent::CollisionEvent(string noeudType, double facteurMultiplicatif, int playerNum)
{
	type_ = COLLISIONEVENT;
	typeNoeud_ = noeudType;
	playerNum_ = playerNum;
	facteurMultiplicatif_ = facteurMultiplicatif;
}

string CollisionEvent::getTypeNoeud()
{
	return typeNoeud_;
}

int CollisionEvent::getPlayerNum()
{
	return playerNum_;
}

double CollisionEvent::getFacteurMultiplicatif()
{
	return facteurMultiplicatif_;
}