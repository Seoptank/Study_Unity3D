using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    [SerializeField]
    private float   spinSpeed;
    private Vector3 spinVec;

    private void Start()
    {
        spinVec = new Vector3(0, spinSpeed, 0);
    }
    private void Update()
    {
        transform.Rotate(spinVec * Time.deltaTime);    
    }


}
