using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{//Script meant to be attached to an ennemy, controlling its animations through the Animator component

    private Animator animator;
    private int isRunningHash;

    private GameObject player;

    [SerializeField]
    private float attackRange = 2f;  //Maximum distance where the ennemy's attacks reach the player
        
    void Start()
    {
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning"); //"StringToHash" is used when we interract with Animator's parameters

        player = GameObject.Find("Player");  //"Find(string s)" return the first gameObject named "s" in the hierarchy
    }

    // Update is called once per frame
    void Update()
    {
        //set isRunning false (in the Animator) if the ennemy is close to the player, true if it is far from the player
        animator.SetBool(isRunningHash, !WithinAttackRange());        
    }

    public bool WithinAttackRange()  //return true when the distance between the ennemy and the player is smaller than the attackRange parameter
    {
        return !gameObject || !player ? false : (Vector3.Distance(this.gameObject.transform.position, player.transform.position) < attackRange);
    }
}
