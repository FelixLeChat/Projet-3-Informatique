#pragma once
#include "IEvent.h"

class DisconnectEvent : public IEvent
{
public:
	DisconnectEvent(string userId);

private:
	string userId_;
public:
	string user_id() const;
};
