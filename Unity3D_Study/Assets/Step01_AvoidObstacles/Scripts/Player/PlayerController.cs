using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    //private Movement3D movement;

    private void Awake()
    {
        #region ΩÃ±€≈Ê
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this);
        #endregion

        //movement = GetComponent<Movement3D>();
    }

    private void Update()
    {
        //MovePlayer();
    }

    private void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //movement.MoveTo(x, z);
    }
}
