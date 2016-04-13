#pragma once
#include "IEvent.h"
#include <vector>
#include "PowerUpSyncEvent.h"
#include "NewBallEvent.h"
#include "CibleSyncEvent.h"
#include "PaletteStateSync.h"

class SyncAllEvent:public IEvent
{
public:

	SyncAllEvent(int zone_num, const vector<int>& pointages, const vector<int>& nb_billes_restantes, const vector<NewBallEvent>& billes_en_jeu, const vector<CibleSyncEvent>& cibles_activees, const vector<PowerUpSyncEvent>& power_up_actifs, const vector<PaletteStateSync> palettes)
		: zoneNum_(zone_num),
		  pointages_(pointages),
		  nbBillesRestantes_(nb_billes_restantes),
		  billesEnJeu_(billes_en_jeu),
		  ciblesActivees_(cibles_activees),
		  powerUpActifs_(power_up_actifs),
			palettes_(palettes)
	{
		type_ = SYNCALL;
	}


	int zone_num() const;
	vector<int> pointages() const;
	vector<int> nb_billes_restantes() const;
	vector<NewBallEvent> billes_en_jeu() const;
	vector<CibleSyncEvent> cibles_activees() const;
	vector<PowerUpSyncEvent> power_up_actifs() const;
	vector<PaletteStateSync> palettes() const;
private:
	int zoneNum_;
	vector<int> pointages_;
	vector<int> nbBillesRestantes_;
	vector<NewBallEvent> billesEnJeu_;
	vector<CibleSyncEvent> ciblesActivees_;
	vector<PowerUpSyncEvent> powerUpActifs_;
	vector<PaletteStateSync> palettes_;
};
