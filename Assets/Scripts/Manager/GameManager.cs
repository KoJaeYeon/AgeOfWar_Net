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
        StartCoroutine(Earn_Gold());
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
