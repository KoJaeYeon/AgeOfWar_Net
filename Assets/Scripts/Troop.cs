using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Troop : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] float hp;
    [SerializeField] float attack;

    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime);
    }
}
