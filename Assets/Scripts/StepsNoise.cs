using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsNoise : MonoBehaviour
{//Script meant to be attached to the legs of the player, controlling the sound of the steps

    [SerializeField]
    private GameObject Noise;

    private PlayerMotor motor;

    void OnEnable()
    {
        PlayerMotor.OnMovingForward += MakeStepsNoise;
    }

    private void OnDisable()
    {
        PlayerMotor.OnMovingForward -= MakeStepsNoise;
    }
    private void Start()
    {
        Noise.SetActive(false);
        motor = transform.root.GetComponent<PlayerMotor>();
    }

    private void FixedUpdate()
    {
        //Deactivate the sound if the player is not walking anymore
        if (!motor.IsWalking())
        {
            Noise.SetActive(false);
        }
    }

    void MakeStepsNoise()  //Activate the sound
    {
        Noise.SetActive(true);
    }
}
