using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] DataPanel DataPanel;
    int gold = 0;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        TcpSender.Instance.SendMyName();
        StartCoroutine(Earn_Gold());
    }

    public void EarnGold(int dieGold)
    {
        gold += dieGold;
        DataPanel.OnChange_GoldText(gold);
    }

    public bool OnSpawnGoldCheck(int gold)
    {
        if(this.gold >= gold)
        {
            this.gold -= gold;
            return true;
        }
        else return false;
    }

    IEnumerator Earn_Gold()
    {
        while (true)
        {
            gold += 5;
            DataPanel.OnChange_GoldText(gold);
            yield return new WaitForSeconds(1);
        }

    }
}
