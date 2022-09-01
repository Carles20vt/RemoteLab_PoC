using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    #region Public Properties

    [SerializeField] private bool showController;
    [SerializeField] private InputDeviceCharacteristics controllerCharacteristics;
    [SerializeField] private List<GameObject> controllerPrefabs;
    [SerializeField] private GameObject handModelPrefab;

    #endregion

    #region Private Properties

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;
    private static readonly int Grip = Animator.StringToHash("Grip");
    private static readonly int Trigger = Animator.StringToHash("Trigger");

    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        TryInitialize();
    }

    private void TryInitialize()
    {
        var inputDevices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, inputDevices);

        foreach (var item in inputDevices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (inputDevices.Count <= 0)
        {
            return;
        }
        
        targetDevice = inputDevices[0];
        
        var prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
        
        if (prefab)
        {
            spawnedController = Instantiate(prefab, transform);
        }
        else
        {
            Debug.Log("Did not find corresponding controller model");
        }

        spawnedHandModel = Instantiate(handModelPrefab, transform);
        handAnimator = spawnedHandModel.GetComponent<Animator>();
    }

    private void UpdateHandAnimation()
    {
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat(Trigger, triggerValue);
        }
        else
        {
            handAnimator.SetFloat(Trigger, 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out var gripValue))
        {
            handAnimator.SetFloat(Grip, gripValue);
        }
        else
        {
            handAnimator.SetFloat(Grip, 0);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(!targetDevice.isValid)
        {
            TryInitialize();
            return;
        }
        
        if (showController)
        {
            if(spawnedHandModel)
                spawnedHandModel.SetActive(false);
            if(spawnedController)
                spawnedController.SetActive(true);
        }
        else
        {
            if (spawnedHandModel)
                spawnedHandModel.SetActive(true);
            if (spawnedController)
                spawnedController.SetActive(false);
            UpdateHandAnimation();
        }
    }
}
