using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtedBehavior : MonoBehaviour
{//Script meant to be attached to the player, controlling its behavior when it is hurted

    public bool Damaged(string armorPiece)
    {
        bool armorTakesTheHit = GameObject.Find(armorPiece) ? GameObject.Find(armorPiece).GetComponent<PieceState>().Hit() : false;

        if (!armorTakesTheHit)
        {
            Debug.Log("Player is dead.");  //The death of the player need to be implemented
        }

        return armorTakesTheHit;
    }
}
