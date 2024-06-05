using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCol : MonoBehaviour
{
    int hp = 40;
    bool canAttack = true;
    public void Damaged()
    {
        hp -= 10;
        if(hp <= 0)
            gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!canAttack) return;
        Debug.Log("enter");
        canAttack = false;
        collision.GetComponent<testCol>().Damaged();
        StartCoroutine(delay());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("exit");
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(1f);
        canAttack = true;
        yield break;
    }


}
