using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorNoise : MonoBehaviour
{ //Script meant to be attached to a piece of the armor worn by the player, controlling the sound produced by it

    private AudioSource noise;

    private PlayerMotor motor;

    private void Start()
    {
        noise = GetComponent<AudioSource>();
        noise.mute = true;  //we ensure that the AudioSource is muted
        motor = transform.root.GetComponent<PlayerMotor>();
    }

    //Here we use FixedUpdate() instead of Update() because this script is linked to physic phenomenon (i.e moving) 
    private void FixedUpdate()
    {
        // the piece of armor produces sound (i.e the AudioSource is "unmuted") when it is moving, and vice-versa
        if (IsMoving())
            noise.mute = false;
        else
            noise.mute = true;
    }

    private bool IsMoving() //return true if the piece is moving, false if it is not
    {                       // Need to be completed by movements other than "player is walking"
        bool isMoving = false;

        if (motor.IsWalking())
        {
            isMoving = true;
        }

        return isMoving;
    }
}
