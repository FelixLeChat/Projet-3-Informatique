#ifndef __EVENT_GAMESTART__
#define __EVENT_GAMESTART__
#include "IEvent.h"

class GameStartEvent : public IEvent
{

public:
	GameStartEvent();
	~GameStartEvent();
private:
	
};

GameStartEvent::GameStartEvent()
{
	type_ = GAMESTART;
}

GameStartEvent::~GameStartEvent()
{
}


#endif // !__EVENT_GAMESTART__
