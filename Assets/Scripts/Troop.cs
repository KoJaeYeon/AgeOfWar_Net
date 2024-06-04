using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Troop : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] float hp;
    [SerializeField] float attack;
    bool friend = true;
    float attackDir = 1;

    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnTroopSetFriend(bool friend)
    {
        if(friend)
        {
            friend = true;
            attackDir = 1;
            spriteRenderer.flipX = false;
        }
        else
        {
            friend = false;
            attackDir = -1;
            spriteRenderer.flipX = true;
        }
    }
    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * attackDir);
    }
}
