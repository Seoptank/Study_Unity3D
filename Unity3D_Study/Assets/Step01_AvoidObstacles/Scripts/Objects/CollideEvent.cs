using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideEvent : MonoBehaviour
{
    private bool            isCollide = false;

    private MeshRenderer meshRenderer;
    private Color        matOriginColor;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        matOriginColor = meshRenderer.material.color;
    }

    private void OnDisable()
    {
        meshRenderer.material.color = matOriginColor;
        isCollide = false;    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && !isCollide)
        {
            isCollide = true;
            meshRenderer.material.color = Color.red;
            Score.collideCount++;
            Debug.Log(Score.collideCount);
        }
    }
}
