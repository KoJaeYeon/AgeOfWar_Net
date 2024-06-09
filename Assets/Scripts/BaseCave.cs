using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseCave : MonoBehaviour
{
    [SerializeField] Slider Slider;
    [SerializeField] TextMeshProUGUI healthText;
    int hp = 500;
    public void OnDamaged(int damage)
    {
        hp -= damage;
        healthText.text = hp.ToString();
        Slider.value = hp/500f;
        if (hp <= 0)
        {

        }
    }
}
