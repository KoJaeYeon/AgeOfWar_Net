using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] Transform m_poolTrans;
    [SerializeField] GameObject[] m_TroopsPrefabs;
    Stack<GameObject>[] pool_Troops;

    private void Awake()
    {
        pool_Troops = new Stack<GameObject>[m_TroopsPrefabs.Length];

        int index = 0;
        foreach (GameObject troopPrefab in m_TroopsPrefabs)
        {
            pool_Troops[index] = new Stack<GameObject>();
            for(int i = 0; i < 100; i++)
            {
                GameObject prefab = Instantiate(troopPrefab, m_poolTrans);
                pool_Troops[index].Push(prefab);
                prefab.SetActive(false);
            }
            index++;
        }
    }

    public GameObject Get_Troop(int id)
    {
        GameObject troop;
        if(pool_Troops[id].TryPop(out troop)) //²¨³ÂÀ» ¶§
        {
            return troop;
        }
        else // ²¨³»Áö ¸øÇßÀ» ¶§
        {
            GameObject prefab = Instantiate(m_TroopsPrefabs[id], m_poolTrans);
            return prefab;
        }
    }
}
