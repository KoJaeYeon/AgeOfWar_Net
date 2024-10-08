using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Troop : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] int gold;
    [SerializeField] int dieGold;
    [SerializeField] int hp;
    [SerializeField] int attack;
    [SerializeField] float moveSpeed = 1;

    Animator animator;

    Troop EnemyTroop;
    BaseCave EnemyBase;

    float attackDir = 1;

    int spawnIndex = 0;
    bool canMove = false;

    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(Vector2.right * moveSpeed * attackDir * Time.deltaTime);
    }

    public void OnSetInitData(TroopData troopData)
    {
        gold = troopData.Gold;
        dieGold = troopData.DieGold;
        hp = troopData.Hp;
        attack = troopData.Attack;

    }

    [SerializeField]
    private void OnCalledAnimator_Attack()
    {
        if (EnemyTroop != null)
        {
            EnemyTroop.OnDamaged(attack);
        }
        else if(EnemyBase != null)
        {
            EnemyBase.OnDamaged(attack);
        }
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

    public void OnDamaged(int damage)
    {
        hp-= damage;
        if (hp <= 0)
        {
            if (tag=="Enemy")
            {
                GameManager.Instance.EarnGold(dieGold);
            }
            StartCoroutine(DieDelay());
        }
    }

    IEnumerator DieDelay()
    {
        yield return null;
        yield return null;
        gameObject.SetActive(false);
        yield break;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals(tag))
        {
            Troop troop = collision.GetComponent<Troop>();
            if(troop != null) //null = Base
            {
                if (troop.spawnIndex < spawnIndex)
                {
                    OnSetMoveStop();
                }
            }          
        }
        else //enemy
        {
            Troop troop = collision.GetComponent<Troop>();
            if(troop != null) // Enemy Troop
            {
                EnemyTroop = troop;
                animator.SetBool("Attack", true);
                OnSetMoveStop();
            }
            else
            {
                BaseCave baseCase = collision.GetComponent<BaseCave>();
                if(baseCase != null)
                {
                    EnemyBase = baseCase;
                    animator.SetBool("Attack", true);
                    OnSetMoveStop();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!canMove) { return; }
        if (collision.tag.Equals(tag))
        {
            Troop troop = collision.GetComponent<Troop>();
            if (troop != null)
            {
                if (troop.spawnIndex < spawnIndex)
                {
                    OnSetMoveStart();
                }
            }
        }
        else
        {
            Troop troop = collision.GetComponent<Troop>();
            if (troop != null) // Enemy
            {
                EnemyTroop = null;
                animator.SetBool("Attack", false);
                OnSetMoveStart();
            }
        }
    }
}
