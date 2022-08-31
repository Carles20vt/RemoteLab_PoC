using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private void Start()
    {
        OnConnectedToServer();
    }

    private void OnConnectedToServer() 
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect To Server...");
    }
    
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server.");
        base.OnConnectedToMaster();
        var roomOptions = new RoomOptions {MaxPlayers = 10, IsVisible = true, IsOpen = true};

        PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);        
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} joined the room.");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
