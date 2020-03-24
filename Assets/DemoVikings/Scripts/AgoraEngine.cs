using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;
using Photon;

// set invite button to interactable.true when I collide with another player

public class AgoraEngine : Photon.MonoBehaviour
{
    public string appID;
    public string channel;

    public Button inviteButton;
    public Button joinButton;

    private IRtcEngine rtcEngine;

    private Collider otherPlayer;
    public string otherChannel;

    public GameObject videoSurfacePrefab;

    static int userCount = 0;

    public uint localuserID;

    private void OnApplicationQuit()
    {
        userCount = 0;
        rtcEngine.LeaveChannel();
        IRtcEngine.Destroy();
        rtcEngine = null; 
    }

    private void OnUserJoinedHandler(uint uid, int elapsed)
    {
        print("user: " + uid + " joined");

        CreateNewUserVideoFrame(uid);
    }

    private void OnLocalUserRegisteredHandler(uint uid, string userAccount)
    {
        if(photonView.isMine)
        {
            localuserID = uid;
            print("my id: " + localuserID);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.isMine && other.CompareTag("Player"))
        {
            inviteButton.interactable = true;

            otherPlayer = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (photonView.isMine && other.CompareTag("Player"))
        {
            inviteButton.interactable = false;
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
                photonView.RPC("ShowJoinButton", PhotonTargets.All, channel);
            }
        }
    }

    public void OnJoinButtonPress()
    {
        print("trying to press join button");
        if(photonView.isMine)
        {
            print("join button pressed");
            rtcEngine.JoinChannel(otherChannel, null, localuserID);
        }
    }

    [PunRPC]
    public void ShowJoinButton(string newChannel)
    {
        if (!photonView.isMine)
        {
            print("you have been invited to channel: " + newChannel);
            joinButton.interactable = true;
            otherChannel = newChannel;
        }
        else
        {
            joinButton.interactable = true;
        }
    }

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        InitializeAgora();
        InitializePlayerCanvas();
    }

    void InitializeAgora()
    {
        rtcEngine = IRtcEngine.GetEngine(appID);

        rtcEngine.OnUserJoined += OnUserJoinedHandler;
        rtcEngine.OnLocalUserRegistered += OnLocalUserRegisteredHandler;
        rtcEngine.EnableVideo();
        rtcEngine.EnableVideoObserver();

        rtcEngine.JoinChannel("agora", null, 0);
    }

    void InitializePlayerCanvas()
    {
        inviteButton.interactable = false;
        joinButton.interactable = false;

        if(!photonView.isMine)
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    void CreateNewUserVideoFrame(uint newUserUid)
    {
        if(photonView.isMine)
        {
            userCount++;

            GameObject newUserVideo = Instantiate(videoSurfacePrefab, transform.GetChild(1));

            newUserVideo.GetComponent<RectTransform>().anchoredPosition += Vector2.right * 120 * userCount;

            VideoSurface videoControls = newUserVideo.GetComponent<VideoSurface>();
            videoControls.SetForUser(newUserUid);
            videoControls.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
            videoControls.SetEnable(true);
        }
    }
}
