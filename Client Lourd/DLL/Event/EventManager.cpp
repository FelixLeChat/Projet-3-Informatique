#include "EventManager.h"
#include <iostream>

EventManager* EventManager::instance_{ nullptr };

EventManager::EventManager(){}

EventManager::~EventManager(){}

EventManager* EventManager::GetInstance()
{
	if (instance_ == nullptr)
		instance_ = new EventManager();
	
	return instance_;
}

void EventManager::throwEvent(IEvent *e)
{
	auto type = e->getType();
	//Essentiel d'avoir l'accès à la liste à chaque fois,
	//elle peut être modifiée par un event
	for(int i = 0; i < subscriberMap_[type].size(); i++)
	{
		if(subscriberMap_[type][i]!=nullptr)
			subscriberMap_[type][i]->update(e);
	}
}


void EventManager::subscribe(IEventSubscriber* sub, EventTypes et)
{
	auto it = subscriberMap_.find(et);
	if (it == subscriberMap_.end())
	{
		vector<IEventSubscriber*> newList = vector<IEventSubscriber*>();
		newList.push_back(sub);
		subscriberMap_[et] = newList;
	}
	else
	{
		auto dejaPresent =  std::find(it->second.begin(), it->second.end(), sub);
		if(dejaPresent == it->second.end())
			it->second.push_back(sub);
		else
		{
			cout << "Subscriber écoute déjà " << et << endl;
		}
	}
}


void EventManager::unsubscribe(IEventSubscriber* sub, EventTypes et)
{
	auto it = subscriberMap_.find(et);
	if (it == subscriberMap_.end())
	{
		/*list<IEventSubscriber*> newList = list<IEventSubscriber*>();
		newList.push_front(sub);
		subscriberMap_[et] = newList;*/
		//printf("This event type doesn't have a subscriber yet");
	}
	else
	{
		auto pos = std::find(it->second.begin(), it->second.end(), sub);
		if (pos != it->second.end())
		{
			it->second.erase(pos);
		}
		
	}
}

bool EventManager::hasSubscribers(EventTypes type)
{
	auto it = subscriberMap_.find(type);
	return it != subscriberMap_.end() && it->second.size() != 0;
}
