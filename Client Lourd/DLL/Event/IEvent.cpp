#include "IEvent.h"
#include "Utilitaire.h"
using namespace std;

IEvent::IEvent()
{
	timeSent_ = utilitaire::getTimeStamp();
}

IEvent::IEvent(EventTypes type)
{
	timeSent_ = utilitaire::getTimeStamp();
	type_ = type;
}

IEvent::~IEvent()
{
}

EventTypes IEvent::getType()
{
	return type_;
}

string IEvent::getFriendlyDesc(){
	return  timeSent_ + "Evenement général";
}
