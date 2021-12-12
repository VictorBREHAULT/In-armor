using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceState : MonoBehaviour
{//Script meant to be attached to a piece fo armor worn by the player
      [SerializeField]
    private int numberOfHitsWithstandable;


    public bool Hit() //Called when an ennemy inflicts damages, return false if the piece is not active
    {
        bool hit = false;

        if (gameObject.activeSelf)
        {
            if (numberOfHitsWithstandable == 0)
            {
                gameObject.SetActive(false);
            }

            else
            {
                numberOfHitsWithstandable -= 1;
            }

            hit = true;
        }

        return hit;
    }
}
