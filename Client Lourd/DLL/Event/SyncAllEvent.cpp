#include "SyncAllEvent.h"

int SyncAllEvent::zone_num() const
{
	return zoneNum_;
}

vector<int> SyncAllEvent::pointages() const
{
	return pointages_;
}

vector<int> SyncAllEvent::nb_billes_restantes() const
{
	return nbBillesRestantes_;
}

vector<NewBallEvent> SyncAllEvent::billes_en_jeu() const
{
	return billesEnJeu_;
}

vector<CibleSyncEvent> SyncAllEvent::cibles_activees() const
{
	return ciblesActivees_;
}

vector<PowerUpSyncEvent> SyncAllEvent::power_up_actifs() const
{
	return powerUpActifs_;
}

vector<PaletteStateSync> SyncAllEvent::palettes() const
{
	return palettes_;
}
