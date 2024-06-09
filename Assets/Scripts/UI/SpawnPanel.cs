using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPanel : MonoBehaviour
{
    [SerializeField] Image[] leftSpawnImg;
    [SerializeField] Slider spawnSlider;
    [SerializeField] Color originColor;
    [SerializeField] Color alphaColor;

    private void Awake()
    {
        TurnOn(0);
    }
    public void TurnOn(int index)
    {
        int idx = 0;
        foreach (var item in leftSpawnImg)
        {
            if(idx < index)
            {
                item.color = originColor;
            }
            else
            {
                item.color = alphaColor;
            }
            idx++;
        }
    }

    public void OnChangeSliderValue(float value)
    {
        spawnSlider.value = value;
    }
}
