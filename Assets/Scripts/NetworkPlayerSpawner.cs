using System.Linq;
using Photon.Pun;
using UnityEngine;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject localPlayerPrefab;
    [SerializeField] private GameObject networkPlayerPrefab;
    
    private GameObject spawnedPlayerPrefab;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        spawnedPlayerPrefab = PhotonNetwork.Instantiate(
            PhotonNetwork.CurrentRoom.Players.Any(player => player.Value.Equals(PhotonNetwork.LocalPlayer)) ? localPlayerPrefab.name : networkPlayerPrefab.name,
            transform.position, transform.rotation);

        Debug.Log($"Player {spawnedPlayerPrefab.name} joined the Room");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        
        Debug.Log($"Player {spawnedPlayerPrefab.name} will leave the Room");
        
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }
}
