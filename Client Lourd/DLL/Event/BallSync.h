#pragma once
#include "IEvent.h"

class BallSyncEvent : public IEvent
{
public:

	BallSyncEvent(int ballId, double rot, double vit_x, double vit_y, double pos_x, double pos_y)
		: rot_(rot),
		  vit_x_(vit_x),
		  vit_y_(vit_y),
		  pos_x_(pos_x),
		  pos_y_(pos_y),
		ballId_(ballId)
	{
		type_ = BALLSYNC;
	}

	BallSyncEvent(BallSyncEvent & eve)
		:rot_(eve.rot_),
		vit_x_(eve.vit_x_),
		vit_y_(eve.vit_y_),
		pos_x_(eve.pos_x_),
		pos_y_(eve.pos_y_),
		ballId_(eve.ballId_)
	{
		type_ = BALLSYNC;
	}

	~BallSyncEvent(){}


	double rot() const;
	double vit_x() const;
	double vit_y() const;
	double pos_x() const;
	double pos_y() const;
	int ballId() const;
private:
	double rot_;
	double vit_x_;
	double vit_y_;
	double pos_x_;
	double pos_y_;
	int ballId_;
};


