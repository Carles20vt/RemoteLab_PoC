using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TreeislandStudio.Engine.Environment;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RemoteLab.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks, IDisposable
    {
        #region Public Properties

        [SerializeField] private GameObject roomUI;
        [SerializeField] private Button connectButton;
        [SerializeField] private List<DefaultRoom> defaultRooms;
        
        #endregion

        #region Private Properties

        private bool isSceneLoading;
        private bool isDebugModeEnabled;

        #endregion
        
        #region Dependencies
        
        /// <summary>
        /// Determines if PUN is enabled or not.
        /// </summary>
        private bool isPunEnabled;

        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="environmentSetUp"></param>
        [Inject]
        private void Initialize(IEnvironmentSetUp environmentSetUp)
        {
            isPunEnabled = environmentSetUp.GameConfiguration.IsMultiPlayerEnabled;
        }

        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        private void Start()
        {
            ConfigureScene();
        }

        private void OnEnable()
        {
            ConfigureScene();
        }
        
        #endregion
        
        #region Events
        
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            
            Debug.Log("Connected To Server.");

            if (!isPunEnabled)
            {
                ShowRoomButtons();
                return;
            }

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

            ShowRoomButtons();
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

        public override void OnDisable()
        {
            if (photonView != null)
            {
                PhotonNetwork.Disconnect();
            }

            base.OnDisable();
        }
        
        #endregion

        #region Public Methods

        public void ConnectToServer(bool isDebugScene)
        {
            isDebugModeEnabled = isDebugScene;
            
            Debug.Log("Try Connect To Server...");

            PhotonNetwork.ConnectUsingSettings();
        }
        
        public void InitializeRoom(int defaultRoomIndex)
        {
            if (isSceneLoading)
            {
                return;
            }
            
            isSceneLoading = true;
            
            var roomSettings = defaultRooms.Find(room => room.SceneIndex == defaultRoomIndex);

            LoadScene(roomSettings);
            JoinRoom(roomSettings);
        }
        
        public void Dispose()
        {
            OnDisable();
        }

        #endregion

        #region Private Methods

        private void ConfigureScene()
        {
            PhotonNetwork.OfflineMode = !isPunEnabled;

            if (isPunEnabled)
            {
                ShowConnectButton();
                return;
            }

            ShowRoomButtons();
        }

        private void ShowConnectButton()
        {
            if (roomUI != null)
            {
                roomUI.SetActive(false);
            }

            if (connectButton != null)
            {
                connectButton.gameObject.SetActive(true);
            }
        }

        private void ShowRoomButtons()
        {
            if (roomUI != null)
            {
                roomUI.SetActive(true);
            }

            if (connectButton != null)
            {
                connectButton.gameObject.SetActive(false);
            }
        }

        private void LoadScene(DefaultRoom roomSettings)
        {
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

            PhotonNetwork.LeaveRoom();
            PhotonNetwork.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
        }

        #endregion
    }
}