using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVideoChat : MonoBehaviour
{
    public GameObject panelJoinParty;
    public GameObject panelLeaveParty;

    public Text strangerTextReadout;

    static AgoraEngine agoraEngine = null;

    [SerializeField]
    private string appID;

    public string playerChannel;

    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }

}