using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RemoteLab.Characters
{
    public class CollisionWarning : MonoBehaviour
    {
        [SerializeField] private float fadeSpeed;
        [SerializeField] private float collisionRadiustoEnter, collisionRadiustoExit;
        [SerializeField] private LayerMask colisionLayer;

        private Material fadeMaterial;
        private float collisionRadius;

        private void Awake()
        {
            fadeMaterial = GetComponent<Renderer>().material;
            collisionRadius = collisionRadiustoEnter;
        }

        private void FixedUpdate()
        {
            float currentAlpha = fadeMaterial.GetFloat("_AlphaValue");
            if (Physics.CheckSphere(transform.position, collisionRadius, colisionLayer))
            {

                if(currentAlpha < 1)
                {
                    collisionRadius = collisionRadiustoExit;
                    float targetAlpha = Mathf.MoveTowards(currentAlpha, 1, fadeSpeed);
                    fadeMaterial.SetFloat("_AlphaValue", targetAlpha);
                }
            }
            else
            {
                if (currentAlpha > 0)
                {
                    collisionRadius = collisionRadiustoEnter;
                    float targetAlpha = Mathf.MoveTowards(currentAlpha, 0, fadeSpeed);
                    fadeMaterial.SetFloat("_AlphaValue", targetAlpha);
                }
            }
        }

    }
}
