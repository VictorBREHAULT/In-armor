using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Inventory/Weapon")]
public class Weapon : ScriptableObject
{
    public int id;
    public string weaponName;
    public GameObject weaponModel;
    public double range;
    public double damage;
}
