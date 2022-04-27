using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [SerializeField]
    private double damage = 3;
    private Vector3 movementVector;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        movementVector = 10 * transform.up;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PerformMovement();

        if (transform.position.x > 10000 || transform.position.y > 10000 || transform.position.z > 10000)
        {
            Destroy(gameObject);
        }
    }

    private void PerformMovement()  //Move the arrow through its RigidBody
    {
        rb.MovePosition(rb.position + movementVector * Time.fixedDeltaTime); // Time.fixedDeltaTime allows to move continuously (no teleportation)        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            collision.gameObject.GetComponent<EnemyBehavior>().Hurted(damage);
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GetComponentInParent<DestroyItself>().DestroyGameobject();
    }
}
