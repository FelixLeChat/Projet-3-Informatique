#include "NewBallEvent.h"
#include "../Arbre/Noeuds/NoeudBille.h"

NewBallEvent::NewBallEvent(int ballId, double vit_x, double vit_y, double pos_x, double pos_y, int playerNum, double scale)
	: vit_x_(vit_x),
	vit_y_(vit_y),
	pos_x_(pos_x),
	pos_y_(pos_y),
	ballId_(ballId),
	playerNum_(playerNum),
	scale_(scale)
{
	type_ = NEWBALL;
}

NewBallEvent::NewBallEvent(NoeudBille* bille)
{
	type_ = NEWBALL;
	vit_x_ = bille->obtenirVitesse().x;
	vit_y_ = bille->obtenirVitesse().y;
	pos_x_ = bille->obtenirPositionRelative().x;
	pos_y_ = bille->obtenirPositionRelative().y;
	ballId_ = bille->obtenirId();
	playerNum_ = bille->obtenirPlayerNum();
	scale_ = bille->obtenirAgrandissement().x;
}

double NewBallEvent::vit_x() const
{
	return vit_x_;
}

double NewBallEvent::vit_y() const
{
	return vit_y_;
}

double NewBallEvent::pos_x() const
{
	return pos_x_;
}

double NewBallEvent::pos_y() const
{
	return pos_y_;
}

int NewBallEvent::ballId() const
{
	return ballId_;
}

int NewBallEvent::playerNum() const
{
	return playerNum_;
}

double NewBallEvent::scale() const
{
	return scale_;
}
