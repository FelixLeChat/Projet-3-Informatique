#include "SocketProxy.h"
#include "NetworkManager.h"

SocketProxy::SocketProxy(NetworkManager* listener)
{
	client_ = websocket_callback_client();
	manager_ = listener;
}
SocketProxy::SocketProxy(): manager_(nullptr)
{
}

SocketProxy::~SocketProxy()
{
}


void SocketProxy::connect()
{
	client_.connect(U("ws://ec2-52-90-46-132.compute-1.amazonaws.com/Websocket/WsGameHandler.ashx")).wait();
}

void SocketProxy::disconnect()
{
	client_.close().wait();
}

void SocketProxy::listen()
{
	client_.set_message_handler([=](websocket_incoming_message recMessage){
		manager_->receive(recMessage.extract_string().get());
	});
}

void SocketProxy::send(string mesStr)
{
	try
	{
		auto message = websocket_outgoing_message();
		message.set_utf8_message(mesStr);
		client_.send(message).wait();
	}
	catch (exception e)
	{
		//concurrency error?
	}

}