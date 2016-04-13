#include "BallLostEvent.h"

BallLostEvent::BallLostEvent(int ball, int playerNum, bool fromNetwork)
	: lostBallId_{ ball }, playerNum_{playerNum}, fromNetwork_{ fromNetwork }
{
	type_ = BALLLOST;
}

BallLostEvent::BallLostEvent(NoeudBille* bille)
{
	type_ = BALLLOST;
	lostBallId_ = bille->obtenirId();
	playerNum_ = bille->obtenirPlayerNum();
	fromNetwork_ = false;
}

BallLostEvent::~BallLostEvent()
{
}

string BallLostEvent::getFriendlyDesc(){
	return  timeSent_ + "Balle perdu : TODO ball id";
}

int BallLostEvent::getBallId(){
	return lostBallId_;
}

int BallLostEvent::getPlayerNum()
{
	return playerNum_;
}

int BallLostEvent::isFromNetwork()
{
	return fromNetwork_;
}
