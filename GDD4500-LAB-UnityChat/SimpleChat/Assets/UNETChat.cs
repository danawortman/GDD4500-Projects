using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class UNETChat : Chat {

    private const short chatMessage = 131;

    private const short loginMessage = 132;

    // Use this for initialization
    void Start () {

        //if the client is also the server
        if (NetworkServer.active)
        {
            //registering the server handler
            NetworkServer.RegisterHandler(chatMessage, ServerReceiveMessage);
            NetworkServer.RegisterHandler(loginMessage, ServerReceiveLoginMessage);
        }

        NetworkManager.singleton.client.RegisterHandler(chatMessage, ReceiveMessage);
        NetworkManager.singleton.client.RegisterHandler(loginMessage, ReceiveLoginMessage);
    }

    public override void SendMessage(UnityEngine.UI.InputField input)
    {
        StringMessage myMessage = new StringMessage();
        //getting the value of the input
        myMessage.value = input.text;

        //sending to server
        NetworkManager.singleton.client.Send(chatMessage, myMessage);
        Debug.Log("SendMessage: " + myMessage.value);
    }

    private void ReceiveMessage(NetworkMessage message)
    {
        //reading message
        string text = message.ReadMessage<StringMessage>().value;

        AddMessage(text);
        Debug.Log("ReceiveMessage: " + text);
    }

    private void ServerReceiveMessage(NetworkMessage message)
    {
        StringMessage myMessage = new StringMessage();
        //we are using the connectionId as player name only to exemplify
        myMessage.value = message.conn.connectionId + ": " + message.ReadMessage<StringMessage>().value;

        //sending to all connected clients
        NetworkServer.SendToAll(chatMessage, myMessage);
        Debug.Log("ServerReceiveMessage: " + myMessage.value);
    }

    public override void SendMessage()
    {
        LoginMessage myMessage = new LoginMessage();
        //getting the value of the input
        myMessage.username = usernameInput.text;
        myMessage.password = passwordInput.text;

        //sending to server
        NetworkManager.singleton.client.Send(loginMessage, myMessage);
        Debug.Log("SendMessage: " + myMessage.username + " " + myMessage.password);
    }

    private void ReceiveLoginMessage(NetworkMessage message)
    {
        //reading message
        LoginMessage login = message.ReadMessage<LoginMessage>();

        usernameInput.text = login.username;
        passwordInput.text = login.password;

        Debug.Log("ReceiveLoginMessage: " + login.username + " " + login.password);
        Debug.Log("After ReceiveLoginMessage: " + usernameInput.text + " " + passwordInput.text);
    }

    private void ServerReceiveLoginMessage(NetworkMessage message)
    {
        LoginMessage login = message.ReadMessage<LoginMessage>();

        //we are using the connectionId as player name only to exemplify
        string text = message.conn.connectionId + ": " + login.username + " "  + login.password;

        usernameInput.text = login.username;
        passwordInput.text = login.password;
        

        //sending to all connected clients
        NetworkServer.SendToAll(loginMessage, login);
        Debug.Log("ServerReceiveLoginMessage: " + text);
    }

}
