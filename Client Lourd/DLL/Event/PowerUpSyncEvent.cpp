#include "PowerUpSyncEvent.h"

double PowerUpSyncEvent::pos_x() const
{
	return pos_x_;
}

double PowerUpSyncEvent::pos_y() const
{
	return pos_y_;
}

PowerUpType PowerUpSyncEvent::power_up() const
{
	return powerUp_;
}
