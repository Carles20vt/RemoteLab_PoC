using Photon.Pun;
using UnityEngine;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject localPlayerPrefab;
    
    private GameObject spawnedPlayerPrefab;
    public override void OnJoinedRoom()
    {
        spawnedPlayerPrefab = PhotonNetwork.Instantiate(
            localPlayerPrefab.name,
            transform.position, transform.rotation);

        Debug.Log($"Player {spawnedPlayerPrefab.name} joined the Room");

        base.OnJoinedRoom();
    }

    public override void OnLeftRoom()
    {        
        Debug.Log($"Player {spawnedPlayerPrefab.name} will leave the Room");
        
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
        
        base.OnLeftRoom();
    }
}
