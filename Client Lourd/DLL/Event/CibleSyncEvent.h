#pragma once
#include "IEvent.h"

class CibleSyncEvent : public IEvent
{
public :
	CibleSyncEvent(double px, double py)
		:px_{px}, py_{py}
{
	type_ = CIBLESYNC;
}

	double pos_x() const;
	double pos_y() const;
private:
	double px_;
	double py_;
};
