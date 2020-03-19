using System.Collections;
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
    public GameObject joinButton;

    private IRtcEngine rtcEngine;

    public VideoSurface remoteVideoSurface;

    private Collider otherPlayer;
    private string otherChannel;

    public Text debugText;

    public static GameObject localPlayerInstance;

    private void Awake()
    {
        if(photonView.isMine)
        {
            AgoraEngine.localPlayerInstance = this.gameObject;
        }
        else
        {
            print("no photon view");
        }
    }

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
        if (photonView.isMine && other.CompareTag("Player"))
        {
            inviteButton.SetActive(true);

            otherPlayer = other;

            print("otherPlayer = " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (photonView.isMine && other.CompareTag("Player"))
        {
            inviteButton.SetActive(false);
            joinButton.SetActive(false);

            otherPlayer = null;
        }
    }

    // Button Events
    public void OnInviteButtonPress()
    {
        if(PhotonNetwork.connected && photonView.isMine)
        {
            print("invite button - photon view is good");
            if(otherPlayer != null)
            {
                print("other player isn't null");
                AgoraEngine otherPlayerAgoraRTC = otherPlayer.gameObject.GetComponent<AgoraEngine>();

                if (otherPlayerAgoraRTC)
                {
                    otherPlayerAgoraRTC.ShowJoinButton(channel);
                    debugText.text += "\nyou have invited " + otherPlayerAgoraRTC.gameObject.name;
                }
                else
                {
                    print("otherPlayerAgoraRTC not found");
                }
            }
            else
            {
                print("other player null");
            }
        }
    }

    public void OnJoinButtonPress()
    {
        if(photonView.isMine)
        {
            rtcEngine.JoinChannel(otherChannel, null, 0);
        }
    }

    public void ShowJoinButton(string newChannel)
    {
        if (photonView.isMine)
        {
            joinButton.SetActive(true);
            otherChannel = newChannel;
            debugText.text += "\nIF you have been invited to channel: " + newChannel; 
        }
        else
        {
            joinButton.SetActive(true);
            otherChannel = newChannel;
            debugText.text += "\nELSE you have been invited to channel: " + newChannel;
        }
    }
}
