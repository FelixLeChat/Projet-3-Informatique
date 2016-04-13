#ifndef __EVENT_POWERUPPALETTEEVENT_H__
#define __EVENT_POWERUPPALETTEEVENT_H__

#include "IEvent.h"

class PowerUpPaletteEvent : public IEvent
{
public:
	PowerUpPaletteEvent(int playerNum, double facteurMultiplicatif);
	~PowerUpPaletteEvent();

	int getPlayerNum() const;
	double getFacteur() const;
private:
	int playerNum_;
	double facteurMultiplicatif_;
};



#endif //__EVENT_POWERUPPALETTEEVENT_H__