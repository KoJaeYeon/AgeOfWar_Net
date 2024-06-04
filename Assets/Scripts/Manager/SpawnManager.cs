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

    public void OnClick_FriendSpawn(int id)
    {
        Spawn_Troop(id, TroopType.Friend);
    }
    public void Spawn_Troop(int id, TroopType troopType)
    {
        GameObject troop = PoolManager.Instance.Get_Troop(0);
        troop.SetActive(true);
        troop.transform.position = friendSpawnPos.position;
    }
}
