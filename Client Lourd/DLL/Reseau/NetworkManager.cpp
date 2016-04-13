#include "NetworkManager.h"
#include <cpprest/json.h>
#include "../Event/GameStartEvent.h"
#include "../Event/BallSync.h"
#include "../Event/NewBallEvent.h"
#include "../Event/EventManager.h"
#include "../Event/BallLostEvent.h"
#include "../Event/CibleSyncEvent.h"
#include "../Event/PowerUpSyncEvent.h"
#include "../Event/ScoreSyncEvent.h"
#include "../Event/BallScaleSyncEvent.h"
#include "../Event/PowerUpPaletteEvent.h"
#include "../Event/DisconnectEvent.h"
#include "../Event/SyncAllEvent.h"
#include "../Event/SyncAllRequest.h"

using namespace web;
using namespace utility::conversions;

NetworkManager* NetworkManager::instance_{nullptr};

//TODO ben eventuel :
// Refactoriser les toJson et fromJson dans les events en tant quel tel. + Factory

NetworkManager::NetworkManager(): socket_(nullptr), estHost_(false)
{
	estConnecte_ = false;
	estCompetitif_ = false;
}

NetworkManager* NetworkManager::getInstance()
{
	if (instance_ == nullptr)
		instance_ = new NetworkManager();
	return instance_;
}

NetworkManager::~NetworkManager()
{
	socket_->disconnect();
}


void NetworkManager::deconnexion()
{
	estConnecte_ = false;
	estCompetitif_ = false;
	userToken_ = "";
	userId_ = "";
	delete socket_;
	socket_ = nullptr;

}

void NetworkManager::jeuHorsLigne()
{
	estEnLigne_ = false;
}

bool NetworkManager::estJeuEnLigne() const
{
	return estEnLigne_;
}

bool NetworkManager::isHost() const
{
	return !estConnecte_ || estHost_;
}

bool NetworkManager::estCompetitif() const
{
	return estCompetitif_;
}

void NetworkManager::connexion(string userToken, string userId)
{
	userToken_ = userToken;
	userId_ = userId;
	if(socket_!= nullptr)
	{
		delete socket_;
		socket_ = nullptr;
	}
	socket_ = new SocketProxy(this);
	socket_->connect();
	socket_->listen();
	estConnecte_ = true;
}

string NetworkManager::getUserId() const
{
	return userId_;
}


void NetworkManager::requestGlobalSync() const
{
	if (estConnecte_)
	{
		json::value message;
		message[L"user_token"] = json::value(to_string_t(userToken_));
		message[L"event_type"] = json::value(REQUEST_SYNC);
		message[L"match_id"] = json::value(to_string_t(matchId_));
		message[L"user_id"] = json::value(to_string_t(userId_));

		socket_->send(to_utf8string(message.serialize()));
	}
}

void NetworkManager::SyncAll(int zone_num, vector<int> pointage, vector<int> billesReserve, vector<NewBallEvent> billes, vector<CibleSyncEvent> cibles, vector<PowerUpSyncEvent> power_ups, vector<PaletteStateSync> palettes) const
{
	if (estConnecte_)
	{
		json::value message;
		message[L"user_token"] = json::value(to_string_t(userToken_));
		message[L"event_type"] = json::value(SYNC_ALL);
		message[L"match_id"] = json::value(to_string_t(matchId_));

		message[L"zone_num"] = json::value(zone_num);

		json::value pointages;
		for (int i = 0; i < pointage.size(); ++i)
		{
			pointages[to_wstring(i)] = json::value(pointage[i]);
		}
		message[L"pointages"] = pointages;

		json::value billesReservesJson;
		for (int i = 0; i < billesReserve.size(); ++i)
		{
			billesReservesJson[to_wstring(i)] = json::value(billesReserve[i]);
		}
		message[L"billes_reserves"] = billesReservesJson;

		json::value billesJson;
		for (int i = 0; i < billes.size(); ++i)
		{
			json::value bille;
			bille[L"item_id"] = json::value(billes[i].ballId());
			bille[L"pos_x"] = json::value(billes[i].pos_x());
			bille[L"pos_y"] = json::value(billes[i].pos_y());
			bille[L"vit_x"] = json::value(billes[i].vit_x());
			bille[L"vit_y"] = json::value(billes[i].vit_y());
			bille[L"player_num"] = json::value(billes[i].playerNum());
			bille[L"scale"] = json::value(billes[i].scale());
			billesJson[to_wstring(i)] = bille;
		}
		message[L"billes_en_jeu"] = billesJson;

		json::value cibleJson;
		for (int i = 0; i < cibles.size(); ++i)
		{
			json::value cible;
			cible[L"pos_x"] = json::value(cibles[i].pos_x());
			cible[L"pos_y"] = json::value(cibles[i].pos_y());
			cibleJson[to_wstring(i)] = cible;
		}
		message[L"cibles"] = cibleJson;

		json::value powerUpJson;
		for (int i = 0; i < power_ups.size(); ++i)
		{
			json::value powerUp;
			powerUp[L"pos_x"] = json::value(power_ups[i].pos_x());
			powerUp[L"pos_y"] = json::value(power_ups[i].pos_y());
			powerUp[L"type"] = json::value(power_ups[i].power_up());
			powerUpJson[to_wstring(i)] = powerUp;
		}
		message[L"power_ups"] = powerUpJson;

		json::value palettesJson;
		for (int i = 0; i < palettes.size(); ++i)
		{
			json::value palette;
			palette[L"player_num"] = json::value(palettes[i].player_num());
			palette[L"de_droite"] = json::value(palettes[i].de_droite());
			palette[L"scale"] = json::value(palettes[i].scale());
			palette[L"rotation"] = json::value(palettes[i].rotation());
			palette[L"est_actif"] = json::value(palettes[i].actif());
			palettesJson[to_wstring(i)] = palette;
		}
		message[L"palettes"] = palettesJson;

		socket_->send(to_utf8string(message.serialize()));
	}
}

void NetworkManager::parseSyncAll(json::value json) const
{
	int zone_num = json[L"zone_num"].as_integer();

	auto pointageJson = json[L"pointages"];
	auto billesReservesJson = json[L"billes_reserves"];
	auto billesJson = json[L"billes_en_jeu"];
	auto ciblesJson = json[L"cibles"];
	auto powerUpsJson = json[L"power_ups"];
	auto palettesJson = json[L"palettes"];

	vector<int> pointage;
	vector<int> billesReserves;
	vector<NewBallEvent> billes;
	vector<CibleSyncEvent> cibles;
	vector<PowerUpSyncEvent> powerUps;
	vector<PaletteStateSync> palettes;

	for (int i = 0; i < pointageJson.size(); i++)
	{
		pointage.push_back(pointageJson[to_wstring(i)].as_integer());
	}

	for (int i = 0; i < billesReservesJson.size(); i++)
	{
		billesReserves.push_back(billesReservesJson[to_wstring(i)].as_integer());
	}

	for (int i = 0; i < billesJson.size(); i++)
	{
		auto billeJson = billesJson[to_wstring(i)];
		billes.push_back(NewBallEvent(
			billeJson[L"item_id"].as_integer(),
			billeJson[L"vit_x"].as_double(),
			billeJson[L"vit_y"].as_double(),
			billeJson[L"pos_x"].as_double(),
			billeJson[L"pos_y"].as_double(),
			billeJson[L"player_num"].as_integer(),
			billeJson[L"scale"].as_double()
		));
	}

	for (int i = 0; i < ciblesJson.size(); i++)
	{
		auto cibleJson = ciblesJson[to_wstring(i)];
		cibles.push_back(CibleSyncEvent(
			cibleJson[L"pos_x"].as_double(),
			cibleJson[L"pos_y"].as_double()
		));
	}

	for (int i = 0; i < powerUpsJson.size(); i++)
	{
		auto powerUpJson = powerUpsJson[to_wstring(i)];
		powerUps.push_back(PowerUpSyncEvent(
			powerUpJson[L"pos_x"].as_double(),
			powerUpJson[L"pos_y"].as_double(),
			PowerUpType(powerUpJson[L"type"].as_integer())
		));
	}

	for (int i = 0; i < palettesJson.size(); i++)
	{
		auto paletteJson = palettesJson[to_wstring(i)];
		palettes.push_back(PaletteStateSync(
			paletteJson[L"rotation"].as_double(),
			paletteJson[L"scale"].as_double(),
			paletteJson[L"player_num"].as_integer(),
			paletteJson[L"de_droite"].as_bool(),
			paletteJson[L"est_actif"].as_bool()
		));
	}
	EventManager::GetInstance()->throwEvent(&SyncAllEvent(zone_num, pointage, billesReserves, billes, cibles, powerUps, palettes));
}

void NetworkManager::syncBille(int ballId, double rotation, double vit_x, double vit_y, double pos_x, double pos_y) const
{
	if (estConnecte_)
	{
		json::value message;
		message[L"user_token"] = json::value(to_string_t(userToken_));
		message[L"event_type"] = json::value(SYNC_BILLE);
		message[L"match_id"] = json::value(to_string_t(matchId_));
		message[L"item_id"] = json::value(ballId);
		message[L"rotation"] = json::value(rotation);
		message[L"pos_x"] = json::value(pos_x);
		message[L"pos_y"] = json::value(pos_y);
		message[L"vit_x"] = json::value(vit_x);
		message[L"vit_y"] = json::value(vit_y);

		socket_->send(to_utf8string(message.serialize()));
	}
}

void NetworkManager::lancerBille(int ballId, double pos_x, double pos_y, double vit_x, double vit_y, int playerNum) const
{
	if (estConnecte_)
	{
		json::value message;
		message[L"user_token"] = json::value(to_string_t(userToken_));
		message[L"event_type"] = json::value(NEW_BALL);
		message[L"match_id"] = json::value(to_string_t(matchId_));
		message[L"item_id"] = json::value(ballId);
		message[L"pos_x"] = json::value(pos_x);
		message[L"pos_y"] = json::value(pos_y);
		message[L"vit_x"] = json::value(vit_x);
		message[L"vit_y"] = json::value(vit_y);
		message[L"player_num"] = json::value(playerNum);

		socket_->send(to_utf8string(message.serialize()));
	}
}

void NetworkManager::SyncCible(double ciblePosX, double ciblePosY) const
{
	if (estConnecte_)
	{
		json::value message;
		message[L"user_token"] = json::value(to_string_t(userToken_));
		message[L"event_type"] = json::value(SYNC_CIBLE);
		message[L"match_id"] = json::value(to_string_t(matchId_));
		message[L"pos_x"] = json::value(ciblePosX);
		message[L"pos_y"] = json::value(ciblePosY);

		socket_->send(to_utf8string(message.serialize()));
	}
}


void NetworkManager::SyncPowerUp(double posX, double posY, PowerUpType powerUp) const
{
	if (estConnecte_)
	{
		json::value message;
		message[L"user_token"] = json::value(to_string_t(userToken_));
		message[L"event_type"] = json::value(SYNC_POWERUP);
		message[L"match_id"] = json::value(to_string_t(matchId_));
		message[L"pos_x"] = json::value(posX);
		message[L"pos_y"] = json::value(posY);
		message[L"power_up"] = json::value(powerUp);

		socket_->send(to_utf8string(message.serialize()));
	}
}

void NetworkManager::SyncScore(int playerNum, int pointage) const
{
	if (estConnecte_)
	{
		json::value message;
		message[L"user_token"] = json::value(to_string_t(userToken_));
		message[L"event_type"] = json::value(SYNC_POINTAGE);
		message[L"match_id"] = json::value(to_string_t(matchId_));
		message[L"player_num"] = json::value(playerNum);
		message[L"pointage"] = json::value(pointage);

		socket_->send(to_utf8string(message.serialize()));
	}
}

void NetworkManager::SyncScaleBille(int id, double facteur) const
{
	if (estConnecte_)
	{
		json::value message;
		message[L"user_token"] = json::value(to_string_t(userToken_));
		message[L"event_type"] = json::value(SYNC_SCALE_BILLE);
		message[L"match_id"] = json::value(to_string_t(matchId_));
		message[L"bille_id"] = json::value(id);
		message[L"facteur_scale"] = json::value(facteur);

		socket_->send(to_utf8string(message.serialize()));
	}
}

void NetworkManager::SyncScalePalette(int joueurNum, double facteur) const
{
	if (estConnecte_)
	{
		json::value message;
		message[L"user_token"] = json::value(to_string_t(userToken_));
		message[L"event_type"] = json::value(SYNC_SCALE_PALETTE);
		message[L"match_id"] = json::value(to_string_t(matchId_));
		message[L"player_num"] = json::value(joueurNum);
		message[L"facteur_scale"] = json::value(facteur);

		socket_->send(to_utf8string(message.serialize()));
	}
}

void NetworkManager::perdreBille(int ballId, int playerNum) const
{
	if (estConnecte_)
	{
		json::value message;
		message[L"user_token"] = json::value(to_string_t(userToken_));
		message[L"event_type"] = json::value(BALL_LOST);
		message[L"match_id"] = json::value(to_string_t(matchId_));
		message[L"ball_id"] = json::value(ballId);
		message[L"player_num"] = json::value(playerNum);

		socket_->send(to_utf8string(message.serialize()));
	}
}

void NetworkManager::startGame(string matchId) const
{
	if (estConnecte_)
	{
		json::value message;
		message[L"user_token"] = json::value(to_string_t(userToken_));
		message[L"event_type"] = json::value(START_GAME);
		message[L"match_id"] = json::value(to_string_t(matchId));

		socket_->send(to_utf8string(message.serialize()));
	}
}

void NetworkManager::joinMatch(string matchId, bool estHost, bool estCompetitive)
{
	matchId_ = matchId;
	estHost_ = estHost;
	estCompetitif_ = estCompetitive;

	json::value message;
	message[L"user_token"] = json::value(to_string_t(userToken_));
	message[L"match_id"] = json::value(to_string_t(matchId));
	message[L"user_id"] = json::value(to_string_t(userId_));

	if(!estConnecte_)
	{

		if (socket_ == nullptr)
		{
			socket_ = new SocketProxy(this);
		}
		socket_->connect();
		socket_->listen();
		estConnecte_ = true;
	}
	messages_ = queue<string>();

	estEnLigne_ = true;
	socket_->send(to_utf8string(message.serialize()));
}

void NetworkManager::quitMatch()
{
	if (estConnecte_)
	{
		json::value message;
		message[L"user_token"] = json::value(to_string_t(userToken_));
		message[L"user_id"] = json::value(to_string_t(userId_));
		message[L"match_id"] = json::value(to_string_t(matchId_));
		message[L"event_type"] = json::value(LEAVE_GAME);
		socket_->send(to_utf8string(message.serialize()));

		matchId_ = "";
		estHost_ = false;
		estCompetitif_ = false;
		messages_ = queue<string>();
	}
}

void NetworkManager::playerAction(NetworkPlayerAction action) const
{
	if (estConnecte_)
	{
		json::value message;
		message[L"user_token"] = json::value(to_string_t(userToken_));
		message[L"event_type"] = json::value(PLAYER_ACTION);
		message[L"match_id"] = json::value(to_string_t(matchId_));
		message[L"user_id"] = json::value(to_string_t(userId_));
		message[L"player_action"] = json::value(action);

		socket_->send(to_utf8string(message.serialize()));
	}
}

void NetworkManager::playerAction(NetworkPlayerAction action, string userId) const
{
	if (estConnecte_)
	{
		json::value message;
		message[L"user_token"] = json::value(to_string_t(userToken_));
		message[L"event_type"] = json::value(PLAYER_ACTION);
		message[L"match_id"] = json::value(to_string_t(matchId_));
		message[L"user_id"] = json::value(to_string_t(userId));
		message[L"player_action"] = json::value(action);

		socket_->send(to_utf8string(message.serialize()));
	}
}

void NetworkManager::receive(string message)
{
	messageLock_.lock();
	messages_.push(message);
	if (messages_.size() > 200)
		messages_.pop();
	messageLock_.unlock();
}

void NetworkManager::SyncReceivedMessages()
{
	messageLock_.lock();
	while (messages_.size() != 0)
	{
		parse(messages_.front());
		messages_.pop();
	}
	messageLock_.unlock();
}

#pragma region Recieve
void NetworkManager::parse(string message) const
{
	//event LOGIC
	//cout << message << endl;
	try
	{
		auto json = json::value::parse(to_string_t(message));
		auto eventType = json.at(L"event_type").as_integer();

		switch (eventType)
		{
		case LEAVE_GAME:
			EventManager::GetInstance()->throwEvent(&DisconnectEvent(
				to_utf8string(json.at(L"user_id").as_string())
			));
			break;
		case PLAYER_ACTION:
			EventManager::GetInstance()->throwEvent(
				&PlayerActionReceiveEvent(
					NetworkPlayerAction(json.at(L"player_action").as_integer()),
					to_utf8string(json.at(L"user_id").as_string())));
			break;
		case START_GAME:
			cout << "Match " << to_utf8string(json.at(L"match_id").as_string()) << " starts!" << endl;
			EventManager::GetInstance()->throwEvent(&GameStartEvent());
			break;
		case SYNC_BILLE:
			EventManager::GetInstance()->throwEvent(
				&BallSyncEvent(
					json.at(L"item_id").as_integer(),
					json.at(L"rotation").as_double(),
					json.at(L"vit_x").as_double(),
					json.at(L"vit_y").as_double(),
					json.at(L"pos_x").as_double(),
					json.at(L"pos_y").as_double()
				));
			break;
		case SYNC_CIBLE:
			EventManager::GetInstance()->throwEvent(
				&CibleSyncEvent(
					json.at(L"pos_x").as_double(),
					json.at(L"pos_y").as_double()
				));
			break;
		case SYNC_POWERUP:
			EventManager::GetInstance()->throwEvent(
				&PowerUpSyncEvent(
					json.at(L"pos_x").as_double(),
					json.at(L"pos_y").as_double(),
					PowerUpType(json.at(L"power_up").as_integer())
				));
			break;
		case SYNC_SCALE_BILLE:
			EventManager::GetInstance()->throwEvent(
				&BallScaleSyncEvent(
					json.at(L"bille_id").as_integer(),
					json.at(L"facteur_scale").as_double()
				));
			break;
		case SYNC_SCALE_PALETTE:
			EventManager::GetInstance()->throwEvent(
				&PowerUpPaletteEvent(
					json.at(L"player_num").as_integer(),
					json.at(L"facteur_scale").as_double()
				));
			break;
		case SYNC_POINTAGE:
			EventManager::GetInstance()->throwEvent(
				&ScoreSyncEvent(
					json.at(L"player_num").as_integer(),
					json.at(L"pointage").as_integer()
				));
			break;
		case NEW_BALL:
			EventManager::GetInstance()->throwEvent(
				&NewBallEvent(
					json.at(L"item_id").as_integer(),
					json.at(L"vit_x").as_double(),
					json.at(L"vit_y").as_double(),
					json.at(L"pos_x").as_double(),
					json.at(L"pos_y").as_double(),
					json.at(L"player_num").as_integer()));
			break;
		case BALL_LOST:
			EventManager::GetInstance()->throwEvent(
				&BallLostEvent(
					json.at(L"ball_id").as_integer(),
					json.at(L"player_num").as_integer(),
					true));
			break;
		case SYNC_ALL:
			if (EventManager::GetInstance()->hasSubscribers(SYNCALL))
			{
				parseSyncAll(json);
			}
			break;
		case REQUEST_SYNC:
			EventManager::GetInstance()->throwEvent(&SyncAllRequest(to_utf8string(json[L"user_id"].as_string())));
			break;
		default:
			break;
		}
	}
	catch (exception e)
	{
		//Pas du JSON
		cout << "Couldn't parse :" << message << endl;
	}
}
#pragma endregion Receive
