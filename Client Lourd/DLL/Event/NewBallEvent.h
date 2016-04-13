#pragma once
#include "IEvent.h"

class NoeudBille;

class NewBallEvent : public IEvent
{
public:
	NewBallEvent(int ballId, double vit_x, double vit_y, double pos_x, double pos_y, int playerNum, double scale =1.0);

	NewBallEvent(NoeudBille* bille);

	~NewBallEvent(){}


	double vit_x() const;
	double vit_y() const;
	double pos_x() const;
	double pos_y() const;
	int ballId() const;
	int playerNum() const;
	double scale() const;
private:
	//todo addPlayerId
	double vit_x_;
	double vit_y_;
	double pos_x_;
	double pos_y_;
	int ballId_;
	int playerNum_;
	double scale_;
};
