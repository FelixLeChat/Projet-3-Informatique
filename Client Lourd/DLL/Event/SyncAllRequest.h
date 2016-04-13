#pragma once
#include "IEvent.h"
class SyncAllRequest : public IEvent
{
public:


	explicit SyncAllRequest(const string& user_id)
		: userId_(user_id)
	{
		type_ = SYNCALLREQUEST;
	}


	string user_id() const;
private:
		string userId_;

};


