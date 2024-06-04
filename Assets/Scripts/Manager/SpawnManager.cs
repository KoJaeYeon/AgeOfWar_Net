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

    public void OnClick_FriendSpawn(int id)
    {
        Spawn_Troop(id, TroopType.Friend);
    }
    public void Spawn_Troop(int id, TroopType troopType)
    {
        GameObject troop = PoolManager.Instance.Get_Troop(id);
        troop.SetActive(true);
        Troop troopComp = troop.GetComponent<Troop>();
        if (troopType == TroopType.Friend)
        {
            troop.transform.position = friendSpawnPos.position;
            troopComp.OnTroopSetFriend(true);
        }
        else
        {
            troop.transform.position = enemySpawnPos.position;
            troopComp.OnTroopSetFriend(false);
        }

        
    }
}
