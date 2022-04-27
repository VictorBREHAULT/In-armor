using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMotor : MonoBehaviour
{//Script meant to be attached to an ennemy, controlling its movements

    private Animator animator;
    private bool isDying;

    [SerializeField]
    private bool isGoingStraight;
    [SerializeField]
    private bool isGoingBackward;
    [SerializeField]
    private bool toTheRight;
    private bool possibilityToChangeDirection;
    private bool isStunned;
    private bool isDiving;
    private bool isFlyingInCircle;

    private GameObject player;

    private string enemyType;

    private EnemyAnimationController ennemyAnimationController;

    [SerializeField]
    private float runningSpeed = 4.0f;  //speed of the ennemy, editable in Unity
    private float forwardCoeff = 0.5f;
    private float sideToSideTimeInterval = 0.75f;

    private Vector3 enemyPlayerVector;    
    private Vector3 movementVector;

    //private double adjustmentAngle;

    private Rigidbody rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        ennemyAnimationController = GetComponent<EnemyAnimationController>(); 
        enemyType = GetComponent<EnnemyTag>().ennemyType;
        isDying = animator.GetBool("isDying");
        isGoingStraight = true;
        isGoingBackward = false;
        toTheRight = true;
        possibilityToChangeDirection = true;
        isStunned = false;
        isDiving = false;
        isFlyingInCircle = false;

        movementVector = transform.forward * runningSpeed; 

        player = GameObject.Find("Player");
    }

    //We use FixedUpdate because movements are physic phenomenons
    private void FixedUpdate()
    {
        isDying = animator.GetBool("isDying");

        if (!isDying && !isStunned)  //we don't want ennemies' corpses to move when they are dead
        {
            if (!isDiving && !isFlyingInCircle)
            {
                if (isGoingStraight)
                {
                    //Ennemy rotation updates so that they always move towards the player

                    enemyPlayerVector = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
                    transform.rotation = Quaternion.LookRotation(enemyPlayerVector);

                    // **** ALTERNATIVES ****
                    //1)
                    //ennemyPlayerVector = player.transform.position - transform.position;  //Need that the z coordinates are equal

                    //2)
                    //adjustmentAngle = System.Math.Acos(Vector3.Dot(transform.forward, Vector3.Normalize(ennemyPlayerVector)));        
                    //transform.Rotate(0, (float)adjustmentAngle, 0);
                    //movementVector = Vector3.Normalize(ennemyPlayerVector) * runningSpeed;

                    if (enemyType == "thug" && (transform.position.x < -1 || transform.position.x > 1))
                    {
                        movementVector = runningSpeed * ReturnOnTheRoad();
                    }

                    else
                    {

                        if (!isGoingBackward)
                        {
                            movementVector = runningSpeed * transform.forward;  //movementVector is updated
                        }
                        else
                        {
                            movementVector = -1 * runningSpeed * transform.forward;
                        }
                    }

                }

                if (!isGoingStraight)
                {
                    movementVector = MoveSideToSide(movementVector, forwardCoeff, sideToSideTimeInterval) * runningSpeed;
                }
            }
                                  

            if (enemyType == "eagle" && (Vector3.Distance(transform.position, player.transform.position)<7))
            {
                movementVector += -10*transform.up;
            }

            if (isFlyingInCircle)
            {
                movementVector = FlyInCircle();
            }

            PerformMovement();
        }
    }

    private void PerformMovement()  //Move the ennemy through its RigidBody
    {
       if (!ennemyAnimationController.WithinAttackRange())
       {
           rb.MovePosition(rb.position + movementVector * Time.fixedDeltaTime); //* Time.fixedDeltaTime allows to move continuously (no teleportation)
       }

    }

    public Vector3 MoveSideToSide(Vector3 movementVector, float forwardCoeff , float sideToSideTimeInterval) 
    {
        if (possibilityToChangeDirection)
            StartCoroutine(ChangingDirection(sideToSideTimeInterval));

        if (!isGoingStraight)
        {
            if (toTheRight)
                movementVector = transform.right + forwardCoeff * transform.forward;
            else
                movementVector = -1*transform.right + forwardCoeff * transform.forward;
        }
        return movementVector;
    }

    
    private Vector3 ReturnOnTheRoad()  //Dedicated to thugs
    {
        Vector3 movementVector;

        if (Vector3.Distance(transform.position, player.transform.position) < 5)
        {
            if (transform.position.x < 0)
            {
                movementVector = Vector3.right + Vector3.forward;                
            }
            else
            {
                movementVector = Vector3.left + Vector3.forward;
            }
        }

        else
        {
            if (transform.position.x < 0)
            {
                movementVector = Vector3.right;
            }
            else
            {
                movementVector = Vector3.left;
            }
        }

        transform.rotation = Quaternion.LookRotation(movementVector);

        return movementVector;
    }

    
    public void DiveToTheSide()
    {
        int side = Random.Range(0, 2);

        StartCoroutine(DiveSpeedTimeSpan(1));
        movementVector = (-1 + 2*side)*5*transform.right;
        transform.rotation = Quaternion.LookRotation(movementVector);

        return ;
    }

    
    public Vector3 FlyInCircle() //Dedicated to eagles
    {
        Vector3 movementVector;

        if (transform.position.y < 5)
        {
            movementVector = runningSpeed*Vector3.up;
        }

        else
        {
            enemyPlayerVector = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
            
            float angle = Mathf.Acos(Vector3.Dot(Vector3.forward, enemyPlayerVector) / Vector3.Magnitude(enemyPlayerVector)); // to redo
            
            if (player.transform.position.x - transform.position.x > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 180f * angle / 3.14f + 90f, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, -180f * angle / 3.14f + 90f, 0f);
            }
            
            movementVector = runningSpeed * transform.forward;
            
            /* Adapt position.z to always have the eagle circling near the player
            float zDistanceDifference = Vector3.Dot(enemyPlayerVector, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 10) - player.transform.position);            
            if (transform.position.z > player.transform.position.z)
            {
                
            }*/
        }

        return movementVector;
    }

    public void SetRunningSpeed(float speed)
    {
        runningSpeed = speed;
    }

    public void SetForwardCoeff(float coeff)
    {
        forwardCoeff = coeff;
    }

    public void SetSideToSideTimeInterval(float interval)
    {
        sideToSideTimeInterval = interval;
    }

    public void SetIsGoingStraight (bool value)
    {
        isGoingStraight = value;
    }
    public void SetIsGoingBackward(bool value)
    {
        isGoingBackward = value;
    }

    public void SetIsStunned(bool value)
    {
        isStunned = value;
    }

    public void SetIsFlyingInCircle(bool value)
    {
        isFlyingInCircle = value;
    }

    IEnumerator ChangingDirection(float time)
    {
        possibilityToChangeDirection = false;
        yield return new WaitForSeconds(time);
        toTheRight = false;
        yield return new WaitForSeconds(2*time);
        toTheRight = true;
        yield return new WaitForSeconds(time);
        possibilityToChangeDirection = true;
                
    }

    IEnumerator DiveSpeedTimeSpan(float time)
    {
        isDiving = true;
        yield return new WaitForSeconds(time);
        isDiving = false;
    }
}
