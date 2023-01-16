using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
namespace RemoteLab.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
    #region Public Properties
    
        [SerializeField] private GameObject roomUI;
        [SerializeField] private List<DefaultRoom> defaultRooms;

    #endregion

        public void ConnectToServer() 
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Try Connect To Server...");
        }
    
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Server.");
            base.OnConnectedToMaster();
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            Debug.Log("We joined the lobby");

            roomUI.SetActive(true);
        }

        public void InitializeRoom(int defaultRoomIndex)
        {
            var roomSettings = defaultRooms[defaultRoomIndex];

            LoadScene(roomSettings);

            JoinRoom(roomSettings);
        }

        private void LoadScene(DefaultRoom roomSettings)
        {
            PhotonNetwork.LoadLevel(roomSettings.SceneIndex);
        }

        private void JoinRoom(DefaultRoom roomSettings)
        {
            var roomOptions = new RoomOptions
            {
                MaxPlayers = (byte)roomSettings.MaxPlayer, 
                IsVisible = true, 
                IsOpen = true
            };

            PhotonNetwork.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default); 
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
}
