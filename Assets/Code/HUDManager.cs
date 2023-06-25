using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Slider HPSlider, StaminaSlider;
    public PlayerStatus player;
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI() {
        HPSlider.maxValue = player.maxHP;
        StaminaSlider.maxValue = player.maxStamina;
        HPSlider.value = player.currentHP;
        StaminaSlider.value = player.currentStamina;
    }

    void Update()
    {
        UpdateUI();
    }
}
