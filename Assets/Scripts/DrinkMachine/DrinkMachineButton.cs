using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkMachineButton : MonoBehaviour
{

    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        ButtonPressed();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }

        ButtonPressed();
    }

    private void ButtonPressed()
    {
        meshRenderer.material.color = Color.red;
        Debug.Log("Button pressed.");
    }
}
