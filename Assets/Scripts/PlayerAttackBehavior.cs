using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PlayerAttackBehavior : MonoBehaviour
{//Script meant to be attached to the player, controlling its attacks

    private Weapon weapon;
    [SerializeField]
    private GameObject arrow;

    private Camera camera;
    private CameraPointer camPointer;

    private double attackRange;
    private double attackDamage;

    private GameObject enemy;

    private Animator weaponAnimator;

    private int hitTriggerHash;

    public bool attackButtonHeld = false;

    private void Start()
    {
        camera = GetComponentInChildren<Camera>();
        camPointer = GetComponentInChildren<CameraPointer>();
        weapon = Inventory.instance.weapons[Inventory.instance.currentWeaponIndex];
        attackRange = weapon.range;
        attackDamage = weapon.damage;
        weaponAnimator = GetComponentInChildren<Animator>();
        hitTriggerHash = Animator.StringToHash("hitTrigger");
    }

    private void Update()
    {
        weapon = Inventory.instance.weapons[Inventory.instance.currentWeaponIndex];
        attackRange = weapon.range;
        weaponAnimator = GetComponentInChildren<Animator>();


        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.D))) // && !attackButtonHeld
        {
            Attack();            
        }

        /* Triggerring of an held attack : PROBLEM, the sequence doesn't work
         
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
        {
            //Coroutine heldAttackPreparation = StartCoroutine(HeldAttackPreparation());
            StartCoroutine(HeldAttackPreparation());
            Stopwatch stopWatch = new Stopwatch(); // Se réinitialise à chaque fois, plutôt utiliser le temps du jeu
            stopWatch.Start();
            if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
            {
                UnityEngine.Debug.Log("C'est arrivé jusqu'ici.");
                StopCoroutine(HeldAttackPreparation());
                stopWatch.Stop();
                HeldAttack(double.Parse(stopWatch.ElapsedMilliseconds.ToString()));                
            }            
        } */
    }

    private void Attack()
    {
        UnityEngine.Debug.Log("Attack");
        StartCoroutine(AttackAnimation());

        //double distanceEnnemySquared = 1000;
        float distanceEnemy = 1000;

        if (camPointer.GetGazedAtObject())  //Define which ennemy is targeted
        {
            if (camPointer.GetGazedAtObject().GetComponent<EnnemyTag>())
            {
                if (enemy != camPointer.GetGazedAtObject())
                {
                    enemy = camPointer.GetGazedAtObject();
                }

                distanceEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            }                         
        }

        switch (weapon.id)
        {
            case 1: //sword
                
                
                if (enemy &&  distanceEnemy < attackRange)
                {
                    if (enemy.activeSelf)
                    {
                        enemy.GetComponent<EnemyBehavior>().Hurted(attackDamage);
                    }
                }                
                break;

            case 2: //knife

                if (enemy && distanceEnemy < attackRange)
                {
                    if (enemy.activeSelf)
                    {
                        enemy.GetComponent<EnemyBehavior>().Hurted(attackDamage);
                    }
                }
                break;

            case 3: //bow

                if (enemy && distanceEnemy < attackRange)
                {
                    if (enemy.activeSelf)
                    {
                        enemy.GetComponent<EnemyBehavior>().Stunned(4);
                    }
                }
                else
                {
                    Instantiate(arrow, transform.position + camera.transform.forward - 0.3f*camera.transform.up, Quaternion.Euler(90f, 0f, 0f));
                }
                break;

            case 4: //torch

                if (enemy && distanceEnemy < attackRange)
                {
                    if (enemy.activeSelf)
                    {
                        enemy.GetComponent<EnemyBehavior>().Hurted(attackDamage);
                    }
                    StartCoroutine(TorchSecondHit(distanceEnemy));
                }
                break;

            case 5: //whip

                GameObject[] enemiesInTheScene = GameObject.FindGameObjectsWithTag("Enemy");
                Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

                foreach (var enemyInTheScene in enemiesInTheScene)
                {
                    if (Vector3.Distance(enemyInTheScene.transform.position, transform.position) < attackRange && GeometryUtility.TestPlanesAABB(planes, enemyInTheScene.GetComponentInChildren<Renderer>().bounds))
                    {
                        if (enemyInTheScene.activeSelf)
                        {
                            enemyInTheScene.GetComponent<EnemyBehavior>().Hurted(attackDamage);
                        }
                    }
                }
                
                break;
        } 
    }

    private void HeldAttack(double time)
    {
        attackButtonHeld = false;
        UnityEngine.Debug.Log("Preparation time : " + time.ToString());
    }


    IEnumerator AttackAnimation()  //Control the animation of the attack
    {
        if(weapon.id == 1 || weapon.id == 5)
        {
            weaponAnimator.SetBool(hitTriggerHash, true);
            yield return new WaitForSeconds(1);
            weaponAnimator.SetBool(hitTriggerHash, false);
        }
        
    }

    /*
    IEnumerator HeldAttackPreparation()
    {
        attackButtonHeld = true;
        UnityEngine.Debug.Log("Blabla");
        yield return new WaitForSeconds(20f);
        HeldAttack(20);
    }
    */

    IEnumerator TorchSecondHit(double distance)
    {
        yield return new WaitForSeconds(1f);

        if (camPointer.GetGazedAtObject())  //Define which ennemy is targeted
        {
            if (camPointer.GetGazedAtObject().GetComponent<EnnemyTag>() && enemy != camPointer.GetGazedAtObject())
                enemy = camPointer.GetGazedAtObject();
        }

        if (enemy && distance < attackRange)
        {
            if (enemy.activeSelf)
            {
                enemy.GetComponent<EnemyBehavior>().Hurted(attackDamage);
            }
        }
    }
}
