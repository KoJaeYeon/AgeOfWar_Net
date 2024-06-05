using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] Transform m_poolTrans;
    [SerializeField] GameObject[] m_TroopsPrefabs;
    Queue<GameObject>[] pool_Troops;

    private void Awake()
    {
        pool_Troops = new Queue<GameObject>[m_TroopsPrefabs.Length];

        int index = 0;
        foreach (GameObject troopPrefab in m_TroopsPrefabs)
        {
            pool_Troops[index] = new Queue<GameObject>();
            for(int i = 0; i < 100; i++)
            {
                GameObject prefab = Instantiate(troopPrefab, m_poolTrans);
                pool_Troops[index].Enqueue(prefab);
                prefab.SetActive(false);
            }
            index++;
        }
    }

    public GameObject Get_Troop(int id)
    {
        GameObject troop;
        troop = pool_Troops[id].Dequeue();
        pool_Troops[id].Enqueue(troop);

        return troop;
    }
}
