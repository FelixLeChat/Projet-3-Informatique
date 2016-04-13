#pragma once
#include "IEvent.h"

class ScoreSyncEvent : public IEvent
{
public:

	ScoreSyncEvent(int player_num, int score)
		: playerNum_(player_num),
		  score_(score)
	{
		type_ = SCORESYNC;
	}

	int player_num() const;
	int score() const;
private:
	int playerNum_;
	int score_;
};

