#ifndef __EVENT_TIMEEVENT_H__
#define __EVENT_TIMEEVENT_H__

#include "IEvent.h"

#include <string>

class TimeEvent : public IEvent
{
public:
	TimeEvent(double dt);
	~TimeEvent();

	string getFriendlyDesc();
	double getDt();
private:
	double dt_;
};



#endif //__EVENT_TIMEEVENT_H__