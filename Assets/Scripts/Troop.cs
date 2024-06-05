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
    [SerializeField] float moveSpeed = 1;

    int spawnIndex = 0;
    bool canMove = false;

    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnSetMoveStart()
    {
        moveSpeed = 1;
        canMove = true;
    }

    public void OnSetMoveStop()
    {
        moveSpeed = 0;
    }

    public void SetSpawnIndex(int index)
    {
        spawnIndex = index;
    }

    public void OnTroopSetFriend(bool friend)
    {
        if(friend)
        {
            friend = true;
            attackDir = 1;
            spriteRenderer.flipX = false;
            tag = "Friend";
        }
        else
        {
            friend = false;
            attackDir = -1;
            spriteRenderer.flipX = true;
            tag = "Enemy";
        }
    }
    void Update()
    {
        transform.Translate(Vector2.right * moveSpeed * attackDir * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals(tag))
        {
            Troop troop = collision.GetComponent<Troop>();
            if(troop.spawnIndex < spawnIndex)
            {
                OnSetMoveStop();
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!canMove) { return; }
        if (collision.tag.Equals(tag))
        {
            Troop troop = collision.GetComponent<Troop>();
            if (troop.spawnIndex < spawnIndex)
            {
                OnSetMoveStart();
            }
        }
    }
}
