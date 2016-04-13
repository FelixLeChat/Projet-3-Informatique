#pragma once
#include "IEvent.h"

class BallScaleSyncEvent : public IEvent
{
public:

	BallScaleSyncEvent(int ballId, double facteur_scale)
		: ballId_(ballId),
		agrandissement_(facteur_scale)
	{
		type_ = BALLSCALESYNC;
	}

	int ballId() const;
	double agrandissement() const;
private:
	int ballId_;
	double agrandissement_;
};
