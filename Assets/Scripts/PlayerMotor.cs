using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour  
{//Script meant to be attached to the player, controlling its movements

    private Vector3 velocity;

    private Rigidbody rb;

    public delegate void MovingForward();
    public static event MovingForward OnMovingForward;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        IsWalking();  //Call in Start() to avoid null reference error when the game starts
    }


    public void Move(Vector3 _velocity) //Get the velocity calculated by PlayerController
    {
        velocity = _velocity;
    }


    private void FixedUpdate() //The time between each call of this method is fixed, unlike Update which depends on the framerate
    {
        PerformMovement();
    }

    private void PerformMovement()  //Move the player through its RigidBody
    {
        if (velocity.z > 0)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);  //Time.fixedDeltaTime allows you to make the move over time and not all at once
            OnMovingForward();
        }
    }

    public bool IsWalking()  //True if the player is moving forward, else false
    {
        return velocity.z > 0;
    }
}
