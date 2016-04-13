#ifndef __SOCKET_PROXY_H__
#define __SOCKET_PROXY_H__

#include <cpprest/ws_client.h>

#define SERVER_URL U("ws://ec2-52-90-46-132.compute-1.amazonaws.com/Websocket/WsGameHandler.ashx")

using namespace std;
using namespace web::websockets::client;

class NetworkManager;

class SocketProxy
{
public:
	SocketProxy();
	SocketProxy(NetworkManager* listener);
	~SocketProxy();

	void connect();
	void disconnect();

	void listen();

	void send(string message);

	websocket_callback_client client_;
private:
	NetworkManager* manager_;
	
};

#endif //__SOCKET_PROXY_H__