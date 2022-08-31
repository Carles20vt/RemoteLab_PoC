
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    [FormerlySerializedAs("NetworkPlayerPrefab")] [SerializeField] 
    private GameObject networkPlayerPrefab;
    
    private GameObject spawnedPlayerPrefab;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        spawnedPlayerPrefab = PhotonNetwork.Instantiate(networkPlayerPrefab.name, transform.position, transform.rotation);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }
}
