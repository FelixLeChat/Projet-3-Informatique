#pragma once
#include "IEvent.h"



enum PowerUpType
{
	DOUBLE_PTS,
	MOITIE_PTS,
	DOUBLE_TAILE_BILLE,
	DOUBLE_TAILLE_PALETTES,
	NB_TYPES
};
class PowerUpSyncEvent : public IEvent
{
public:

	
	PowerUpSyncEvent(double pos_x, double pos_y, PowerUpType powerUp):
		pos_x_{pos_x}, pos_y_{pos_y}, powerUp_{powerUp}
	{
		type_ = POWERUPSYNC;
	}

	double pos_x() const;
	double pos_y() const;
	PowerUpType power_up() const;

private:
	double pos_x_;
	double pos_y_;
	PowerUpType powerUp_;
};
