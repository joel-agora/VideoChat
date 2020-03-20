﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;
using Photon;

public class AgoraEngine : Photon.MonoBehaviour
{
    public string appID;
    public string channel;

    public GameObject inviteButton;
    public Button joinButton;

    private IRtcEngine rtcEngine;

    public VideoSurface remoteVideoSurface;

    private Collider otherPlayer;
    public string otherChannel;

    public Text debugText;


    void Start()
    {
        if(PhotonNetwork.connected == true && !photonView.isMine)
        {
            Canvas otherCanvas = transform.GetChild(1).GetComponent<Canvas>();
            if (otherCanvas)
            {
                otherCanvas.enabled = false;
                print("disabling other players canvas");
            }
            else
            {
                print("No canvas found");
                print(otherCanvas.name);
            }
        }

        rtcEngine = IRtcEngine.GetEngine(appID);
        

        
        rtcEngine.OnUserJoined += OnUserJoinedHandler;

        rtcEngine.EnableVideo();
        rtcEngine.EnableVideoObserver();

        

        rtcEngine.JoinChannel(channel, null, 0);

        remoteVideoSurface.SetEnable(false);
        remoteVideoSurface.gameObject.SetActive(false);


        inviteButton.SetActive(false);
        joinButton.interactable = false;

        //joinButton.interactable = true;
    }

    private void OnApplicationQuit()
    {
        rtcEngine.LeaveChannel();
        IRtcEngine.Destroy();
        rtcEngine = null; 
    }

    private void OnUserJoinedHandler(uint uid, int elapsed)
    {
        debugText.text += "\nOtherPlayer JOINED UID: " + uid;


        remoteVideoSurface.gameObject.SetActive(true);
        //make new UI surface
        remoteVideoSurface.SetForUser(uid);
        remoteVideoSurface.SetEnable(true);
        remoteVideoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
        remoteVideoSurface.SetGameFps(30);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.isMine && other.CompareTag("Player"))
        {
            inviteButton.SetActive(true);

            otherPlayer = other;

            //print("otherPlayer = " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (photonView.isMine && other.CompareTag("Player"))
        {
            inviteButton.SetActive(false);
            //joinButton.SetActive(false);

            otherPlayer = null;
        }
    }

    // Button Events
    public void OnInviteButtonPress()
    {
        if(PhotonNetwork.connected && photonView.isMine)
        {
            if (otherPlayer != null)
            {

                //PhotonView photonView = PhotonView.Get(otherPlayer.gameObject.GetComponent<PhotonView>());
                photonView.RPC("ShowJoinButton", PhotonTargets.All, channel);


                //otherPlayerAgoraRTC.ShowJoinButton(channel);
                //debugText.text += "\nyou have invited " + otherPlayer.transform.parent.name;
            }
        }
    }

    public void OnJoinButtonPress()
    {
        print("trying to press join button");
        if(photonView.isMine)
        {
            print("join button pressed");
            rtcEngine.JoinChannel(otherChannel, null, 0);
        }
    }

    [PunRPC]
    public void ShowJoinButton(string newChannel)
    {
        if (!photonView.isMine)
        {
            print("JOIN BUTTON DISPLAY");
            joinButton.interactable = true;
            otherChannel = newChannel;
            debugText.text += "\nELSE you have been invited to channel: " + newChannel;
        }
    }
}
