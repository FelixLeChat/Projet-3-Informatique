#include "BallSync.h"

double BallSyncEvent::rot() const
{
	return rot_;
}

double BallSyncEvent::vit_x() const
{
	return vit_x_;
}

double BallSyncEvent::vit_y() const
{
	return vit_y_;
}

double BallSyncEvent::pos_x() const
{
	return pos_x_;
}

double BallSyncEvent::pos_y() const
{
	return pos_y_;
}

int BallSyncEvent::ballId() const
{
	return ballId_;
}
