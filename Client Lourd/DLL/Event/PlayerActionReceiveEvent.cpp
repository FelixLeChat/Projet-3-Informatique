#include "PlayerActionReceiveEvent.h"

PlayerActionReceiveEvent::PlayerActionReceiveEvent(NetworkPlayerAction action, string userToken)
{
	action_ = action;
	userId_ = userToken;
	type_ = NETWORKPLAYERACTION;
}

PlayerActionReceiveEvent::~PlayerActionReceiveEvent()
{
}

NetworkPlayerAction PlayerActionReceiveEvent::getAction()
{
	return action_;
}

string PlayerActionReceiveEvent::getUserId()
{
	return userId_;
}