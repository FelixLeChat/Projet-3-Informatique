#include "DisconnectEvent.h"

DisconnectEvent::DisconnectEvent(string userId)
{
	userId_ = userId;
	type_ = DISCONNECTEVENT;
}

string DisconnectEvent::user_id() const
{
	return userId_;
}
