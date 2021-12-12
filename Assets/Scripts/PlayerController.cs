using UnityEngine;

[RequireComponent (typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour  // Responsable des contrôles
{//Script meant to be attached to the player, in charge of the controls over the player

    [SerializeField]
    private float speed = 3f;

    private PlayerMotor motor;

    // Start is called before the first frame update
    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Calculate the velocity of the player's movement
        float zMov = Input.GetAxisRaw("Vertical");//We use GetAxisRaw and not GetAxis because the latter adds filters, 
                                                  //which is not desired when we create our own movement system

        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = moveVertical * speed;

        motor.Move(velocity);
    }
}
