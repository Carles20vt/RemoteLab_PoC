using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace RemoteLab.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks, IDisposable
    {
        #region Public Properties

        [SerializeField] private GameObject roomUI;
        [SerializeField] private List<DefaultRoom> defaultRooms;
        
        #endregion

        #region Private Properties

        private bool isSceneLoading;
        private bool isDebugModeEnabled;

        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        #endregion
        
        #region Events
        
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

            if (isDebugModeEnabled && !isSceneLoading)
            {
                LoadDebugScene();
                return;
            }

            roomUI.SetActive(true);
        }
        
        public override void OnJoinedRoom()
        {
            Debug.Log("Joined a Room");
            base.OnJoinedRoom();

            isSceneLoading = false;
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"Player {newPlayer.NickName} joined the room.");
            base.OnPlayerEnteredRoom(newPlayer);
        }

        public override void OnDisable()
        {
            PhotonNetwork.Disconnect();
            base.OnDisable();
        }
        
        #endregion

        #region Public Methods

        public void ConnectToServer(bool isDebugScene)
        {
            isDebugModeEnabled = isDebugScene;
            
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Try Connect To Server...");
        }
        
        public void InitializeRoom(int defaultRoomIndex)
        {
            if (isSceneLoading)
            {
                return;
            }
            
            var roomSettings = defaultRooms[defaultRoomIndex];

            LoadScene(roomSettings);

            JoinRoom(roomSettings);
        }
        
        public void Dispose()
        {
            OnDisable();
        }

        #endregion

        #region Private Methods

        private void LoadScene(DefaultRoom roomSettings)
        {
            isSceneLoading = true;
            PhotonNetwork.LoadLevel(roomSettings.SceneIndex);
        }
        
        private void LoadDebugScene()
        {
            var debugScene = defaultRooms.Find(d => d.Name.Contains("Debug"));
            JoinRoom(debugScene);
        }

        private static void JoinRoom(DefaultRoom roomSettings)
        {
            var roomOptions = new RoomOptions
            {
                MaxPlayers = (byte)roomSettings.MaxPlayer,
                IsVisible = true,
                IsOpen = true
            };

            PhotonNetwork.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
        }

        #endregion
    }
}