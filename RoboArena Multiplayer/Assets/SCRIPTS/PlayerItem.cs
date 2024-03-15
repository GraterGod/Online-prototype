using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public Text playerName;
    Image backgroundImage;

    [Tooltip("Barva kartièky aktuálního hráèe / hráèová kartièka s avatarem v lobby je s touto barvou.")]
    public Color highLightColor;

    public GameObject leftArrow;
    public GameObject RightArrow;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public Image playerAvatar;
    public Sprite[] avatars;

    Player player;
    PhotonView view;
    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        player = _player;
        UpdatePlayerItem(player);
    }

    public void onClickLeftArrow()
    {
        if ((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = avatars.Length - 1;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] - 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);

    }

    public void onClickRightArrow()
    {
        if ((int)playerProperties["playerAvatar"] == avatars.Length - 1)
        {
            playerProperties["playerAvatar"] = 0;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey("playerAvatar"))
        {
            playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
            playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
        }
        else
        {
            playerProperties["playerAvatar"] = 0;
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
    }

    public void ApplyLocalChanges()
    {
        backgroundImage.color = highLightColor; // aktualny hrac ma zvyrazneneho svojeho avatara
        leftArrow.SetActive(true);
        RightArrow.SetActive(true);

        onClickRightArrow(); // posune hned pri nacitani lobby avatar o jedno doprava
    }

    private void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }
}
