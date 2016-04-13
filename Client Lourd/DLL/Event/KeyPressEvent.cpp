#include "KeyPressEvent.h"

using namespace std;

KeyPressEvent::KeyPressEvent(int key, bool appuye)
	:IEvent{}, keyCode_{ key }, appuye_{appuye}
{
	type_ = INPUTEVENT;	
}

KeyPressEvent::~KeyPressEvent()
{
}

string KeyPressEvent::getFriendlyDesc(){
	if (appuye_)
		return  timeSent_ + "Touche appuyee : " + to_string(keyCode_);
	return  timeSent_ + "Touche relachee : " + to_string(keyCode_);
}

int KeyPressEvent::getKeyCode(){
	return keyCode_;
}

bool KeyPressEvent::getAppuye(){
	return appuye_;
}
