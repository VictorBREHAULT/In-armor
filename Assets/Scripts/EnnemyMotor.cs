using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnnemyMotor : MonoBehaviour
{//Script meant to be attached to an ennemy, controlling its movements

    private Animator animator;
    private bool isDying;

    private GameObject player;

    private EnnemyAnimationController ennemyAnimationController;

    [SerializeField]
    private float runningSpeed = 4.0f;  //speed of the ennemy, editable in Unity

    private Vector3 ennemyPlayerVector;    
    private Vector3 movementVector;

    //private double adjustmentAngle;

    private Rigidbody rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        ennemyAnimationController = GetComponent<EnnemyAnimationController>();  
        isDying = animator.GetBool("isDying");

        movementVector = transform.forward * runningSpeed;  //

        player = GameObject.Find("Player");
    }

    //We use FixedUpdate because movements are physic phenomenons
    private void FixedUpdate()
    {
        isDying = animator.GetBool("isDying");

        if (!isDying)  //we don't want ennemies' corpses to move when they are dead
        {
            //Enemy rotation'update so that they always move towards the player

            ennemyPlayerVector = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
            transform.rotation = Quaternion.LookRotation(ennemyPlayerVector);

            // **** ALTERNATIVES ****
            //1)
            //ennemyPlayerVector = player.transform.position - transform.position;  //Need that the z coordinates are equal

            //2)
            //adjustmentAngle = System.Math.Acos(Vector3.Dot(transform.forward, Vector3.Normalize(ennemyPlayerVector)));        
            //transform.Rotate(0, (float)adjustmentAngle, 0);
            //movementVector = Vector3.Normalize(ennemyPlayerVector) * runningSpeed;


            movementVector = transform.forward * runningSpeed;  //movementVector is updated
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
}
