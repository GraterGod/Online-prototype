using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Text roomName;
    LobbyManager manager;

    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<LobbyManager>();
    }

    public void OnClickItem()
    {
        manager.JoinRoom(roomName.text);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
