#pragma once
#include "IEvent.h"

class PaletteScaleSyncEvent : public IEvent
{
public:

	PaletteScaleSyncEvent(double pos_x, double pos_y, double facteur_scale)
		: pos_x_(pos_x),
		  pos_y_(pos_y),
		  facteurScale_(facteur_scale)
	{
		type_ = PALETTESCALESYNC;
	}

	double pos_x() const;
	double pos_y() const;
	double facteur_scale() const;
private:
	double pos_x_;
	double pos_y_;
	double facteurScale_;
};


