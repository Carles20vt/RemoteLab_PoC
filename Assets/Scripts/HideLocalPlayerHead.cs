using Photon.Pun;
using UnityEngine;

public class HideLocalPlayerHead : MonoBehaviour
{
    #region Public Properties

    [SerializeField] private GameObject[] gameObjectsToHideFromCamera;
    [SerializeField] private LayerMask layersToHide;

    #endregion
    
    #region Private Properties

    private PhotonView photonView;

    #endregion

    #region Unity Callback
    
    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        HidePlayerHead();
    }
    
    #endregion

    #region Private Methods
    
    private void HidePlayerHead()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        foreach (var gameObjectToHide in gameObjectsToHideFromCamera)
        {
            gameObjectToHide.layer = LayerMaskToLayer(layersToHide);
        }
    }
    
    private static int LayerMaskToLayer(LayerMask layerMask) {
        var layerNumber = 0;
        var layer = layerMask.value;
        while(layer > 0) {
            layer >>= 1;
            layerNumber++;
        }
        return layerNumber - 1;
    }

    #endregion
}
