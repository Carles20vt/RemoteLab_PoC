using Photon.Pun;
using ReadyPlayerMe;
using RemoteLab.Characters;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class RpmAvatarLoader : MonoBehaviour
{
    #region Public Properties

    [SerializeField] private string avatarUrl = "https://d1a370nemizbjq.cloudfront.net/031e2ec8-1716-4f7f-887b-a04891699449.glb";
        //"https://d1a370nemizbjq.cloudfront.net/209a1bc2-efed-46c5-9dfd-edc8a1d9cbe4.glb";

    #endregion
    
    #region Private Properties
    
    private PhotonView photonView;
    private GameObject rpmAvatar;

    #endregion
    
    #region Unity Callback

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        
        LoadRpmAvatar(avatarUrl);

        AddMovement();

        RpcAvatar();
    }

    #endregion
    
    [PunRPC]
    private void ReceiveAvatar(string rpcString,  PhotonMessageInfo info)
    {
        var rpcActorString = rpcString.Split(',')[0];
        int.TryParse(rpcActorString, out var rpcActorId);
        var rpcAvatarUrl = rpcString.Split(',')[1];

        if (rpcActorId != photonView.ViewID)
        {
            LoadRpmAvatar(rpcAvatarUrl);
        }
    }

    private void RpcAvatar()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        
        var playerId = photonView.ViewID;
        var rpcString = playerId + "," + avatarUrl;
        
        photonView.RPC("ReceiveAvatar", RpcTarget.OthersBuffered, rpcString);
    }

    private void LoadRpmAvatar(string rpmAvatarUrl)
    {
        Debug.Log($"Started loading avatar");
        
        var avatarLoader = new AvatarLoader();
        avatarLoader.OnCompleted += AvatarLoadComplete;
        avatarLoader.OnFailed += AvatarLoadFail;
        avatarLoader.LoadAvatar(rpmAvatarUrl);
    }

    private void AvatarLoadComplete(object sender, CompletionEventArgs args)
    {
        rpmAvatar = args.Avatar;
        
        SetRpmAvatarParent();

        Debug.Log($"Avatar {args.Avatar.name} loaded");
    }

    private void SetRpmAvatarParent()
    {
        rpmAvatar.transform.SetParent(transform, true);
        rpmAvatar.transform.localPosition = Vector3.zero;
        rpmAvatar.transform.localRotation = new Quaternion {eulerAngles = Vector3.zero};
    }

    private static void AvatarLoadFail(object sender, FailureEventArgs args)
    {
        Debug.Log($"Avatar loading failed with error message: {args.Message}");
    }
    
    private void AddMovement()
    {
        var movement = gameObject.GetComponent<RpmMovement>();
        movement.enabled = true;
    }
}
