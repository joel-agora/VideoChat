using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgoraJoiner : MonoBehaviour
{
    // when I become in range with someone, display button to ask them to join my party


    public GameObject inviteButton;
    public GameObject joinButton;

    public GameObject remoteVideoSurface;

    public Collider otherPlayer;

    string localChannel;

    string remoteInviteChannel;

    // when I have been asked, display a button that says join.

    // Start is called before the first frame update
    void Start()
    {
        //otherPlayer = null;

        //inviteButton.SetActive(false);
        //joinButton.SetActive(false);
        //remoteVideoSurface.SetActive(false);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {
    //        //display invite button
    //        inviteButton.SetActive(true);

    //        otherPlayer = other;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {
    //        otherPlayer = null;

    //        inviteButton.SetActive(false);
    //    }
    //}

    //public void QueryPartyJoin(string newChannel)
    //{
    //    remoteInviteChannel = newChannel;
    //    // here an outsidd player has asked me to join their party
    //    // my join button shows up
    //    joinButton.SetActive(true);
    //}

    //public void OnInviteButtonPress()
    //{
    //    print(gameObject.name + " pressed Invite!");
    //    // send channel name to other player
    //    otherPlayer.GetComponent<AgoraJoiner>().QueryPartyJoin(GetChannel());
    //}

    //// click Join to JoinChannel(newChannel)
    //public void OnJoinButtonPress()
    //{
    //    // AgoraEngine. Join Channel -> Include NEW channel name
    //    transform.GetChild(2).GetComponent<AgoraEngine>().JoinPlayersChat(remoteInviteChannel);
    //}

    //private string GetChannel()
    //{
    //    if(localChannel == null)
    //    {
    //        localChannel = transform.GetChild(2).GetComponent<AgoraEngine>().channel;
    //    }

    //    return localChannel;
    //}
}
