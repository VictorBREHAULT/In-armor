using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyHurtedBehavior : MonoBehaviour
{//Script meant to be attached to an ennemy, controlling its behavior when it is hurted (old version, not used anymore)
    private Animator animator;
    private int isDyingHash;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isDyingHash = Animator.StringToHash("isDying");
    }

    private void Update()
    {
        
    }

    public void Hurted()
    {
        Death();        
    }

    public void Death()
    {
        animator.SetBool(isDyingHash, true);
        StartCoroutine(BodyDisapearance());
    }

    IEnumerator BodyDisapearance()
    {
        yield return new WaitForSeconds(2.5f);
        this.gameObject.SetActive(false);
    }
}
