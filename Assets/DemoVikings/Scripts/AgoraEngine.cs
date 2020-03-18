using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;

public class AgoraEngine : MonoBehaviour
{
    public string appID;
    public string channel;

    public GameObject inviteButton;
    public GameObject joinButton;

    private IRtcEngine rtcEngine;

    public VideoSurface remoteVideoSurface;

    private Collider otherPlayer;
    private string otherChannel;

    public Text debugText;

    void Start()
    {
        //if(rtcEngine == null)
        //{
            rtcEngine = IRtcEngine.GetEngine(appID);
        //}

        
        rtcEngine.OnUserJoined += OnUserJoinedHandler;

        rtcEngine.EnableVideo();
        rtcEngine.EnableVideoObserver();

        

        rtcEngine.JoinChannel(channel, null, 0);

        remoteVideoSurface.SetEnable(false);
        remoteVideoSurface.gameObject.SetActive(false);


        inviteButton.SetActive(false);
        joinButton.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        rtcEngine.LeaveChannel();
        IRtcEngine.Destroy();
        rtcEngine = null; 
    }

    private void OnUserJoinedHandler(uint uid, int elapsed)
    {
        debugText.text += "\nOtherPlayer UID: " + uid;


        remoteVideoSurface.gameObject.SetActive(true);
        //make new UI surface
        remoteVideoSurface.SetForUser(uid);
        remoteVideoSurface.SetEnable(true);
        remoteVideoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
        remoteVideoSurface.SetGameFps(30);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inviteButton.SetActive(true);

            otherPlayer = other;

            print("otherPlayer = " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inviteButton.SetActive(false);
            joinButton.SetActive(false);

            otherPlayer = null;
        }
    }

    //// Button Events
    public void OnInviteButtonPress()
    {
        
        AgoraEngine otherPlayerAgoraRTC = otherPlayer.gameObject.transform.GetChild(2).GetComponent<AgoraEngine>();
        print(otherPlayerAgoraRTC.gameObject.name);
        otherPlayerAgoraRTC.ShowJoinButton(channel);
        debugText.text += "\nyou have invited " + otherPlayerAgoraRTC.gameObject.name;
    }

    public void OnJoinButtonPress()
    {
        rtcEngine.JoinChannel(otherChannel, null, 0);
    }

    public void ShowJoinButton(string newChannel)
    {
        joinButton.SetActive(true);
        otherChannel = newChannel;
        debugText.text += "\nyou have been invited to channel: " + newChannel;
    }
}
