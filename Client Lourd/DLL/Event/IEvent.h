#ifndef __EVENT_IEVENT_H__
#define __EVENT_IEVENT_H__
#include <string>

using namespace std;

enum EventTypes
{
	COLLISIONEVENT,
	SYNCEVENT,
	INPUTEVENT,
	TIMEEVENT,
	BALLLOST,
	NETWORKPLAYERACTION,
	GAMESTART,
	NEWBALL,
	PALETTEPOWERUP,
	BALLSYNC,
	CIBLESYNC,
	POWERUPSYNC,
	SCORESYNC,
	BALLSCALESYNC,
	PALETTESCALESYNC,
	DISCONNECTEVENT,
	SYNCALL,
	SYNCALLREQUEST,
	PALETTESTATESYNC
};
class IEvent
{
public:
	IEvent();
	IEvent(EventTypes type);
	virtual ~IEvent();

	EventTypes getType();
	virtual string getFriendlyDesc();

protected:
	EventTypes type_;
	string timeSent_;

};



#endif //__EVENT_IEVENT_H__