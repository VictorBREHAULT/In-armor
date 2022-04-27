using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{//Script meant to be attached to an ennemy, controlling its behavior (i.e. its actions and its state)

    private Animator animator;
    private EnemyMotor motor;
    private int isDyingHash;

    private string ennemyType;

    private GameObject player;
    private int playerCurrentWeaponType;

    private double distanceX;
    private double distanceZ;
    private double distanceY;
    private double distanceToPlayerSquared;
    private double distanceToPlayer;

    private void Start()
    {
        ennemyType = GetComponent<EnnemyTag>().ennemyType;
        animator = GetComponent<Animator>();
        motor = GetComponent<EnemyMotor>();
        isDyingHash = Animator.StringToHash("isDying");
        player = GameObject.Find("Player");
        playerCurrentWeaponType = player.GetComponent<Inventory>().currentWeaponType;
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (playerCurrentWeaponType != player.GetComponent<Inventory>().currentWeaponType)
        {
            playerCurrentWeaponType = player.GetComponent<Inventory>().currentWeaponType;
            ChangeBehavior(playerCurrentWeaponType, ennemyType);
        }   
    }

    public void ChangeBehavior (int playerWeaponType, string ennemyType)
    {
        switch (ennemyType)
        {
            case "thug":

                motor.SetRunningSpeed(4);
                switch (playerWeaponType)
                {
                    case 1: //sword
                        motor.SetIsGoingStraight(true);
                        break;

                    case 2: //knife
                        motor.SetIsGoingStraight(false);
                        motor.SetForwardCoeff(0.25f);
                        motor.SetSideToSideTimeInterval(0.25f);
                        break;

                    case 3: //bow                        
                        if(distanceToPlayer > 5 && distanceToPlayerSquared < 30)
                        {
                            motor.SetIsGoingStraight(false);
                            motor.SetForwardCoeff(0f);
                            motor.SetSideToSideTimeInterval(1f);
                        }
                        else
                        {
                            motor.DiveToTheSide();
                            motor.SetIsGoingStraight(true);
                            motor.SetRunningSpeed(6);
                        }
                        break;

                    case 4: //torch
                        motor.SetIsGoingStraight(true);
                        motor.SetRunningSpeed(2);
                        break;

                    case 5: //whip

                        motor.SetIsGoingStraight(true);
                        if (distanceToPlayer > 6)  // If the thug is out of the whip's range
                        {
                            motor.SetRunningSpeed(0);
                        }
                        else
                        {
                            motor.DiveToTheSide();
                            motor.SetIsGoingStraight(true);
                        }
                        break;
                }

                break;

            case "spider":

                motor.SetRunningSpeed(7);
                switch (playerWeaponType)
                {
                    case 1: //sword
                        motor.SetIsGoingStraight(true);
                        motor.SetIsGoingBackward(false);
                        break;

                    case 2: //knife
                        motor.SetIsGoingStraight(false);
                        motor.SetForwardCoeff(0.5f);
                        motor.SetSideToSideTimeInterval(0.15f);
                        break;

                    case 3: //bow
                        motor.SetIsGoingStraight(true);
                        motor.SetIsGoingBackward(false);
                        break;

                    case 4: //torch
                        if (distanceToPlayer < 10)
                        {
                            motor.SetIsGoingStraight(false);
                            motor.SetForwardCoeff(-0.5f);
                            motor.SetSideToSideTimeInterval(0.75f);
                        }
                        else
                        {
                            motor.SetIsGoingStraight(true);
                            motor.SetIsGoingBackward(true);
                            motor.SetRunningSpeed(0.1f);
                        }
                        break;

                    case 5: //whip
                        motor.SetIsGoingStraight(true);
                        motor.SetIsGoingBackward(false);
                        break;
                }

                break;

            case "eagle":

                motor.SetRunningSpeed(6);
                motor.SetIsFlyingInCircle(false);
                switch (playerWeaponType)
                {
                    case 1: //sword
                        motor.SetIsGoingStraight(true);
                        break;

                    case 2: //knife
                        motor.SetIsGoingStraight(true);
                        motor.SetRunningSpeed(3);
                        break;

                    case 3: //bow
                        motor.SetIsGoingStraight(true);
                        break;

                    case 4: //torch
                        motor.SetIsFlyingInCircle(true);
                        break;

                    case 5: //whip
                        motor.SetIsGoingStraight(false);
                        break;
                }

                break;
        }
    }

    public void InflictDamage()  //The ennemy inflicts damages to one piece of armor according to its type
                                   //Function called through events by the execution of ennemy's attack animations
    {
        Debug.Log(player + " is hurted");

        string armorPiece = null;
        switch (ennemyType)
        {
            case "thug":
                armorPiece = "Plastron completed";
                break;

            case "spider":
                armorPiece = "Right arm";
                break;
        }

        player.GetComponent<PlayerHurtedBehavior>().Damaged(armorPiece);
    }

    public void Hurted(double damage)  //Called when the ennemy is hurted (currently, it could be hurted only by the player)
                                       //Need to be complexified, or deleted if no ennemy could take more than one player's attack
    {
        Death();
    }

    public void Stunned(float time)
    {
        motor.SetIsStunned(true);
        StartCoroutine(TimeToBeUnstunned(time));
    }

    public void Death()  //Called when the ennemy dies
    {
        animator.SetBool(isDyingHash, true);
        StartCoroutine(BodyDisapearance());
    }

    IEnumerator BodyDisapearance()  //Coroutine which make the ennemy disappear when it is dead
    {
        yield return new WaitForSeconds(2.5f);
        this.gameObject.SetActive(false);
    }

    IEnumerator TimeToBeUnstunned(float time)
    {
        yield return new WaitForSeconds(time);
        motor.SetIsStunned(false);
    }
}
