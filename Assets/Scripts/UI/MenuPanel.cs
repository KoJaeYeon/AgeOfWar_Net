using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public void OnClick_SpawnTroop(int id)
    {
        SpawnManager.Instance.Spawn_Troop(id, TroopType.Friend);
    }
}
