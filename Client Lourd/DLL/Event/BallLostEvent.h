#ifndef __EVENT_BALLLOST_H__
#define __EVENT_BALLLOST_H__

#include "IEvent.h"

#include <string>

#include "../Arbre/Noeuds/NoeudBille.h"
#include <queue>

class BallLostEvent : public IEvent
{
public:
	BallLostEvent(int lostBallId, int playerNum, bool fromNetwork=false);
	BallLostEvent(NoeudBille* bille);
	~BallLostEvent();

	string getFriendlyDesc() override;
	int getBallId();
	int getPlayerNum();
	int isFromNetwork();
private:
	int lostBallId_;
	int playerNum_;
	bool fromNetwork_;
};



#endif //__EVENT_BALLLOST_H__