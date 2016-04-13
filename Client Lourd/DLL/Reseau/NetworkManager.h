#ifndef __NETWORK_MANAGER_H__
#define __NETWORK_MANAGER_H__

#include <cpprest/ws_client.h>
#include "SocketProxy.h"
#include "../Event/PlayerActionReceiveEvent.h"
#include "../Event/PowerUpSyncEvent.h"
#include "../Event/NewBallEvent.h"
#include "../Event/CibleSyncEvent.h"
#include <Event/PaletteStateSync.h>

#define SERVER_URL U("ws://ec2-52-90-46-132.compute-1.amazonaws.com/Websocket/WsGameHandler.ashx")

namespace web
{
	namespace json
	{
		class value;
	}
}

using namespace std;


class NetworkManager
{
public:
	~NetworkManager();
	static NetworkManager* getInstance();

	void deconnexion();
	void jeuHorsLigne();

	bool estJeuEnLigne() const;
	bool isHost() const;
	bool estCompetitif() const;
	void connexion(string userToken, string userId = "");
	string getUserId() const;

	//Tous font les collisions, mais elles sont override par celles du host.
	void requestGlobalSync() const;
	void SyncAll(int zone_num, vector<int> pointage, vector<int> billesReserve, vector<NewBallEvent> billes, vector<CibleSyncEvent> cibles, vector<PowerUpSyncEvent> power_ups, vector<PaletteStateSync> palettes) const;
	void parseSyncAll(web::json::value message) const;
	void syncBille(int ballId, double rotation, double vit_x, double vit_y, double pos_x, double pos_y) const;

	void lancerBille(int ballId, double pos_x, double pos_y, double vit_x, double vit_y, int playerNum = 0) const;
	void SyncCible(double pos_x, double pos_y) const;
	void SyncPowerUp(double posX, double posY, PowerUpType powerUp) const;
	void SyncScore(int playerNum, int pointage) const;

	void SyncScaleBille(int id, double facteur_grossissement) const;
	void SyncScalePalette(int joueurNum, double facteur) const;
	void perdreBille(int ballId, int playerNum) const;
	void startGame(string matchId) const;


	void joinMatch(string matchId, bool estHost, bool estCompetitive);
	void quitMatch();
	void playerAction(NetworkPlayerAction action) const;
	void playerAction(NetworkPlayerAction action, string userId) const;
	void receive(string message);
	void SyncReceivedMessages();
	void parse(string message) const;
	//gameState
	//host is to send game state à chaque +/- 2sec: billes, leurs positions vitesses rotatation id etc, les joueurs, leurs ids, nb ai, les palettes sont elles activées...
	//si rejoin, ou si spectator join, subscribe à gamestateupdate, update game and listen to rest

	//receive


	SocketProxy* socket_;

	enum NetworkMessageType
	{
		LEAVE_GAME, // DO NOT MOVE
		NEW_BALL,
		BALL_LOST,
		START_GAME,
		ENTER_MATCH,
		PLAYER_ACTION,
		SYNC_BILLE,
		SYNC_CIBLE,
		SYNC_POWERUP,
		SYNC_POINTAGE,
		SYNC_SCALE_BILLE,
		SYNC_SCALE_PALETTE,
		SYNC_ALL,
		REQUEST_SYNC
	};

private:
	NetworkManager();
	string userToken_;
	string userId_;
	string matchId_;
	static NetworkManager* instance_;
	bool estConnecte_;
	bool estHost_;
	bool estCompetitif_;
	bool estEnLigne_;
	queue<string> messages_;
	mutex messageLock_;
};


#endif //__NETWORK_MANAGER_H__
