using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorRotations : MonoBehaviour
{//Script meant to be attached to th earmor worn by the player, controlling its rotation following the movement of the camera

    private GameObject cam;

    [SerializeField]
    private float rotXFactor = 0.5f;


    private void Start()
    {
        cam = GameObject.Find("Main Camera");
    }

    private void FixedUpdate()
    {
        float angleX = 0;
        if (cam.transform.rotation.eulerAngles.x < 90)
        {
            angleX = cam.transform.rotation.eulerAngles.x * rotXFactor;
        }

        transform.rotation = Quaternion.Euler(angleX, cam.transform.rotation.eulerAngles.y, 0);
    }
}
