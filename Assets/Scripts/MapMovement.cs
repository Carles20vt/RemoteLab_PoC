using UnityEngine;

public class MapMovement : MonoBehaviour
{
    #region Public Properties

    [SerializeField] private Transform objectToObserve;
    [SerializeField] private Transform objectToModify;

    #endregion

    // Update is called once per frame
    private void Update()
    {
        //objectToModify.position = new Vector3(objectToObserve.position.x, 0, objectToObserve.position.z);
        //objectToModify.eulerAngles = new Vector3(objectToModify.rotation.eulerAngles.x, objectToObserve.rotation.eulerAngles.y, objectToModify.rotation.eulerAngles.z);
        objectToModify.position = objectToObserve.position;
        objectToModify.rotation = objectToObserve.rotation;
    }
}
