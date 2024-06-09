using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TroopType
{
    Friend,
    Enemy
}
public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] Transform friendSpawnPos;
    [SerializeField] Transform enemySpawnPos;
    [SerializeField] SpawnPanel SpawnPanel;

    Queue<Troop> distanceQueue_Friend = new Queue<Troop>();
    Queue<Troop> distanceQueue_Enemy = new Queue<Troop>();

    Dictionary<int, int> NeedSpawnGold = new Dictionary<int, int>();

    Queue<int> spawnDelayQueue = new Queue<int>();
    float spawnTime = 0;

    int spawnIndex = 0;


    private void Awake()
    {
        NeedSpawnGold.Add(0, 15);
    }
    private void Update()
    {
        if(distanceQueue_Friend.Count > 0)
        {
            Troop friend_Troop = distanceQueue_Friend.Peek();
            friend_Troop.OnSetMoveStart();

            if(!friend_Troop.gameObject.activeSelf)
            {
                distanceQueue_Friend.Dequeue();
            }
            else if(friend_Troop.transform.position.x > friendSpawnPos.position.x + 0.2f)
            {
                distanceQueue_Friend.Dequeue();
            }
        }

        if (distanceQueue_Enemy.Count > 0)
        {
            Troop enemy_Troop = distanceQueue_Enemy.Peek();
            enemy_Troop.OnSetMoveStart();

            if (!enemy_Troop.gameObject.activeSelf)
            {
                distanceQueue_Enemy.Dequeue();
            }
            else if (enemy_Troop.transform.position.x < enemySpawnPos.position.x - 0.2f)
            {
                distanceQueue_Enemy.Dequeue();
            }
        }

        if(spawnDelayQueue.Count > 0)
        {
            spawnTime += Time.deltaTime / 3;
            SpawnPanel.OnChangeSliderValue(spawnTime);
            if(spawnTime > 1)
            {
                spawnTime = 0;
                SpawnPanel.OnChangeSliderValue(spawnTime);
                int id = spawnDelayQueue.Dequeue();
                Spawn_Troop(id, TroopType.Friend);
                SpawnPanel.TurnOn(spawnDelayQueue.Count);
            }
        }
    }

    public void OnCalled_FriendSpawn(int id)
    {
        if (spawnDelayQueue.Count > 5)
        {
            return;
        }
        if (!GameManager.Instance.OnSpawnGoldCheck(NeedSpawnGold[id]))
        {
            return;
        }
        spawnDelayQueue.Enqueue(id);
        SpawnPanel.TurnOn(spawnDelayQueue.Count);
    }

    public void OnClick_EnemySpawn(int id)
    {
        Spawn_Troop(id, TroopType.Enemy);
    }

    public void Spawn_Troop(int id, TroopType troopType)
    {
        GameObject troop = PoolManager.Instance.Get_Troop(id);
        troop.SetActive(true);
        Troop troopComp = troop.GetComponent<Troop>();
        troopComp.OnSetInitData(DatabaseManager.Instance.GetTroopData(id));
        troopComp.OnSetMoveStop();
        troopComp.SetSpawnIndex(spawnIndex++);
        if (troopType == TroopType.Friend)
        {
            troop.transform.position = friendSpawnPos.position;
            troopComp.OnTroopSetFriend(true);
            distanceQueue_Friend.Enqueue(troopComp);
            TcpSender.Instance.SendMsg($"[º“»Ø]/{id}");
        }
        else
        {
            troop.transform.position = enemySpawnPos.position;
            troopComp.OnTroopSetFriend(false);
            distanceQueue_Enemy.Enqueue(troopComp);
        }

        
    }
}
