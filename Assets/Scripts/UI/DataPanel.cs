using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Gold_Text_UI;
    public void OnChange_GoldText(int gold)
    {
        Gold_Text_UI.text = gold.ToString();
    }
}
