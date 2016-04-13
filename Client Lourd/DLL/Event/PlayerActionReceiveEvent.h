#ifndef __EVENT_PLAYERACTIONRECEIVEEVENT__
#define __EVENT_PLAYERACTIONRECEIVEEVENT__

#include "IEvent.h"


enum NetworkPlayerAction
{
	APPUI_PALETTE_GAUCHE,
	APPUI_PALETTE_DROITE,
	APPUI_RESSORT,
	RELACHE_PALETTE_GAUCHE,
	RELACHE_PALETTE_DROITE,
	RELACHE_RESSORT
};

class PlayerActionReceiveEvent : public IEvent
{
public:
	PlayerActionReceiveEvent(NetworkPlayerAction action, string userId);
	~PlayerActionReceiveEvent();

	NetworkPlayerAction getAction();
	string getUserId();

private:
	NetworkPlayerAction action_;
	string userId_;

};


#endif // !__EVENT_PLAYERACTIONRECEIVEEVENT__
