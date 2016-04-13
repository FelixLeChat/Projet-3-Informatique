#ifndef __EVENT_IEVENTSUBSCRIBER_H__
#define __EVENT_IEVENTSUBSCRIBER_H__
#include "IEvent.h"

class IEventSubscriber
{
public:
	IEventSubscriber(){}

	virtual ~IEventSubscriber(){}
	virtual void update(IEvent* e){}
private:

};

#endif // __EVENT_IEVENTSUBSCRIBER_H__
