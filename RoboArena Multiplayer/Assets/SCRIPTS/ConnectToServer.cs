using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public InputField usernameInput;
    public Text buttonText;


    public void OnClickConnect()
    {
        if (usernameInput.text.Length >= 1) // jestli je nìco napsané v inputfieldu username (jestli je tam alespoò jeden znak)
        {
            PhotonNetwork.NickName = usernameInput.text;
            buttonText.text = "Connecting...";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Lobby");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}
