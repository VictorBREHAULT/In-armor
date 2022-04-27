using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
    //Script meant to be attached to weapons lying on the floor, in order to be picked by the player
{
    private bool isInRange = false;
    public Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Q)))
        {
            TakeWeapon();
        }
    }

    private void TakeWeapon()
    {
        Inventory.instance.weapons.Add(weapon);
        Inventory.instance.currentWeaponIndex = Inventory.instance.weapons.Count - 1;
        Destroy(gameObject);
        Inventory.instance.aWeaponIsAround = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            Inventory.instance.aWeaponIsAround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            Inventory.instance.aWeaponIsAround = false;
        }
    }
}
