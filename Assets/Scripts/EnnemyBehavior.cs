using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBehavior : MonoBehaviour
{//Script meant to be attached to an ennemy, controlling its behavior (i.e. its actions and its state)

    private Animator animator;
    private int isDyingHash;

    private GameObject player;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isDyingHash = Animator.StringToHash("isDying");
        player = GameObject.Find("Player");
    }


    public void InflictDamage()  //The ennemy inflicts damages to one piece of armor according to its type
                                   //Function called through events by the execution of ennemy's attack animations
    {
        Debug.Log(player + " is hurted");

        string type = GetComponent<EnnemyTag>().ennemyType;
        string armorPiece = null;
        switch (type)
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

    public void Hurted()  //Called when the ennemy is hurted (currently, it could be hurted only by the player)
                          //Need to be complexified, or deleted if no ennemy could take more than one player's attack
    {
        Death();
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
}
