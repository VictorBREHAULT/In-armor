using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBehavior : MonoBehaviour
{//Script meant to be attached to the player, controlling its attacks

    //[SerializeField]
    //private GameObject weapon;  to be implemented when the weapon system is in place

    private CameraPointer camPointer;

    private float attackRange;

    private GameObject ennemy;

    private Animator swordAnimator;

    private int hitTriggerHash;

    private void Start()
    {
        camPointer = GetComponentInChildren<CameraPointer>();
        attackRange = 2.5f;  // Temporary : will be linked to the weapon
        swordAnimator = GetComponentInChildren<Animator>();
        hitTriggerHash = Animator.StringToHash("hitTrigger");
    }

    private void Update()
    {
        if (camPointer.GetGazedAtObject())  //Define which ennemy is targeted
        {
            if (camPointer.GetGazedAtObject().GetComponent<EnnemyTag>() && ennemy != camPointer.GetGazedAtObject())
                ennemy = camPointer.GetGazedAtObject();
        }

        //The attck is possible only if the ennemy is close to the player
        if (ennemy && System.Math.Abs(ennemy.GetComponent<CapsuleCollider>().transform.position.z - transform.position.z) < attackRange)  // à modifier pour les ennemies ne venant pas du plan x-z
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.D))
            {
                Attack(ennemy);
            }
            
        }
    }

    private void Attack(GameObject ennemy)  //Doit faire intervenir l'arme
    {
        Debug.Log("Attack");
        StartCoroutine(AttackAnimation());

        if (ennemy.activeSelf)
        {
            ennemy.GetComponent<EnnemyBehavior>().Hurted();
        }
        
    }

    IEnumerator AttackAnimation()  //Control the animation of the attack
    {
        swordAnimator.SetBool(hitTriggerHash, true);
        yield return new WaitForSeconds(1);
        swordAnimator.SetBool(hitTriggerHash, false);
    }
}
