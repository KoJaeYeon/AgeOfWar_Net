using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCave : MonoBehaviour
{
    int hp;
    public void OnDamaged(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {

        }
    }
}
