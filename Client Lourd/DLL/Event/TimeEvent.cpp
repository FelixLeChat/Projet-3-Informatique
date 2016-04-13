#include "TimeEvent.h"

TimeEvent::TimeEvent(double dt)
	:IEvent{}, dt_{ dt }
{
	type_ = TIMEEVENT;
}

TimeEvent::~TimeEvent()
{
}

string TimeEvent::getFriendlyDesc(){
	return  timeSent_ + "Variation de temps : " + to_string(dt_);
}

double TimeEvent::getDt(){
	return dt_;
}