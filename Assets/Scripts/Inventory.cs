using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
    //Script describing how the player's inventory works
{
    public List<Weapon> weapons = new List<Weapon>();
    public int currentWeaponIndex = 0;
    public int currentWeaponType = 1;
    public bool aWeaponIsAround = false;  // aWeaponIsAround's value is changed thanks to PickUpWeapon

    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one instance of Inventory in this scene.");
            return;
        }

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!aWeaponIsAround && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Q)))
        {
            GetNextWeapon();
        }
    }

    public void GetNextWeapon()
    {
        if (currentWeaponIndex + 1 >= weapons.Count)
        {
            currentWeaponIndex = 0;
        }            
        else
        {
            currentWeaponIndex++;
        }

        currentWeaponType = weapons[currentWeaponIndex].id;
        
    }

    private void ActualizeWieldedWeapon(int weaponType)  //Do bugs
    {        
        switch (weaponType)
        {
            case 1:
                GameObject.Find("claymore").SetActive(true);
                GameObject.Find("knife").SetActive(false);
                GameObject.Find("bow").SetActive(false);
                GameObject.Find("torch").SetActive(false);
                break;

            case 2:
                GameObject.Find("claymore").SetActive(false);
                GameObject.Find("knife").SetActive(true);
                GameObject.Find("bow").SetActive(false);
                GameObject.Find("torch").SetActive(false);
                break;

            case 3:
                GameObject.Find("claymore").SetActive(false);
                GameObject.Find("knife").SetActive(false);
                GameObject.Find("bow").SetActive(true);
                GameObject.Find("torch").SetActive(false);
                break;

            case 4:
                GameObject.Find("claymore").SetActive(false);
                GameObject.Find("knife").SetActive(false);
                GameObject.Find("bow").SetActive(false);
                GameObject.Find("torch").SetActive(true);
                break;

            case 5:
                GameObject.Find("claymore").SetActive(true);
                GameObject.Find("knife").SetActive(false);
                GameObject.Find("bow").SetActive(false);
                GameObject.Find("torch").SetActive(false);
                break;
        }
    }
        
}
