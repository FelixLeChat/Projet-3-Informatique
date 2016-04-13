#ifndef __EVENT_MANAGER_H__
#define __EVENT_MANAGER_H__

#include <map>
#include "IEvent.h"
#include "IEventSubscriber.h"
#include <vector>
using namespace std;

class EventManager
{
public:
	EventManager();
	~EventManager();
	bool hasSubscribers(EventTypes type);
	static EventManager* GetInstance();
	void throwEvent(IEvent* e);
	void subscribe(IEventSubscriber* subscriber, EventTypes type);
	void unsubscribe(IEventSubscriber* subscriber, EventTypes type);

private:
	static EventManager* instance_;
	map<EventTypes, vector<IEventSubscriber*>> subscriberMap_;

};

#endif // __EVENT_MANAGER_H__