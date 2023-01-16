using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
namespace RemoteLab.Network
{
    public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
    {
    #region Public Properties

        [SerializeField] private GameObject localPlayerPrefab;

    #endregion

    #region Private Properties

        private List<GameObject> spawnedPlayerPrefabs;

    #endregion

        private void Start()
        {
            spawnedPlayerPrefabs = new List<GameObject>();
        }

        public override void OnJoinedRoom()
        {
            var spawnedPlayerPrefab = PhotonNetwork.Instantiate(
                localPlayerPrefab.name,
                transform.position, transform.rotation);

            Debug.Log($"Player {spawnedPlayerPrefab.name} joined the Room");

            spawnedPlayerPrefabs.Add(spawnedPlayerPrefab);

            base.OnJoinedRoom();
        }

        public override void OnLeftRoom()
        {
            foreach (var spawnedPlayerPrefab in spawnedPlayerPrefabs.Where(spawnedPlayerPrefab => !spawnedPlayerPrefab.IsDestroyed()))
            {
                Debug.Log($"Player {spawnedPlayerPrefab.name} will leave the Room");

                PhotonNetwork.Destroy(spawnedPlayerPrefab);
            }

            base.OnLeftRoom();
        }
    }
}