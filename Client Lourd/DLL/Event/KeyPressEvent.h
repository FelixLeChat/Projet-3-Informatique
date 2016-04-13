#ifndef __EVENT_KEYPRESSEVENT_H__
#define __EVENT_KEYPRESSEVENT_H__
#include <string>
#include "IEvent.h"
using namespace std;

class KeyPressEvent : public IEvent
{
public:
	KeyPressEvent(int keyCode, bool appuye);
	~KeyPressEvent();
	string getFriendlyDesc() override;
	int getKeyCode();
	bool getAppuye();
private:
	int keyCode_;
	bool appuye_;

};

#endif //__EVENT_KEYPRESSEVENT_H__