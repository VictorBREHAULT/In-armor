using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySounds : MonoBehaviour
{//Script meant to be attached to an ennemy, controlling the sounds it produces

    [SerializeField]
    private GameObject RunningSound;

    [SerializeField]
    private AudioSource ReadyToAttackSound;

    [SerializeField]
    private AudioSource AttackSound;

    [SerializeField]
    private AudioSource DeathSound;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (RunningSound)
        {
            RunningSound.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (!animator.GetBool("isRunning"))
        {
            if (RunningSound)
            {
                RunningSound.SetActive(false);
            }
        }
    }

    public void DoAttackSound()
    {
        if (AttackSound)
        {
            AttackSound.Play();
        }
    }



    public void DoReadyToAttackSound()
    {
        if (ReadyToAttackSound)
        {
            ReadyToAttackSound.Play();
        }
    }

    public void DoDeathSound()
    {
        if (DeathSound)
        {
            DeathSound.Play();
        }
    }


}
