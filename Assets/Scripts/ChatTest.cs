using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;

public class ChatTest : MonoBehaviour, IChatClientListener
{
    ChatClient chat;

    private void Start ()
    {
        chat = new ChatClient (this);
        //chat.ChatRegion = "au";
        chat.Connect (PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues (PhotonNetwork.AuthValues.UserId));
    }

    private void Update () => chat.Service ();

    public void PublishMessage (string message)
    {
        chat.PublishMessage ("PublicChat", message);
    }

    public void DebugReturn (DebugLevel level, string message)
    {
        Debug.Log (message);
    }

    public void OnChatStateChange (ChatState state)
    {
        throw new System.NotImplementedException ();
    }

    public void OnConnected ()
    {
        chat.Subscribe ("PublicChat");
    }

    public void OnDisconnected ()
    {
        throw new System.NotImplementedException ();
    }

    public void OnGetMessages (string channelName, string[] senders, object[] messages)
    {
        foreach (string message in messages)
            print (message);
        //throw new System.NotImplementedException ();
    }

    public void OnPrivateMessage (string sender, object message, string channelName)
    {
        throw new System.NotImplementedException ();
    }

    public void OnStatusUpdate (string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException ();
    }

    public void OnSubscribed (string[] channels, bool[] results)
    {
        throw new System.NotImplementedException ();
    }

    public void OnUnsubscribed (string[] channels)
    {
        throw new System.NotImplementedException ();
    }

    public void OnUserSubscribed (string channel, string user)
    {
        throw new System.NotImplementedException ();
    }

    public void OnUserUnsubscribed (string channel, string user)
    {
        throw new System.NotImplementedException ();
    }
}
