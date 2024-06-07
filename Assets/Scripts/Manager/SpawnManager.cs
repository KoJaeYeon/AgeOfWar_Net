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

    Queue<Troop> distanceQueue_Friend = new Queue<Troop>();
    Queue<Troop> distanceQueue_Enemy = new Queue<Troop>();

    Dictionary<int, int> NeedSpawnGold = new Dictionary<int, int>();

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
    }

    public void OnClick_FriendSpawn(int id)
    {
        Spawn_Troop(id, TroopType.Friend);        
    }

    public void OnClick_EnemySpawn(int id)
    {
        Spawn_Troop(id, TroopType.Enemy);
    }

    public void Spawn_Troop(int id, TroopType troopType)
    {
        if(troopType == TroopType.Friend)
        {
            if(!GameManager.Instance.OnSpawnGoldCheck(NeedSpawnGold[id]))
            {
                return;
            }
        }

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
