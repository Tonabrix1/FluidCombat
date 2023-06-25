using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float maxHP, currentHP, maxStamina, currentStamina, staminaRegen, HPRegen, regenCycle = 3f;
    float regenCD;
    void Start()
    {
        regenCD = regenCycle;
    }

    void FixedUpdate()
    {
        if (regenCD <= 0) {
            currentStamina = Mathf.Min(currentStamina+staminaRegen, maxStamina);
            currentHP = Mathf.Min(currentHP+HPRegen, maxHP);
            regenCD = regenCycle;
        } else regenCD -= Time.fixedDeltaTime;
    }

    public bool CanUse(float cost) {
        return currentStamina - cost >= 0f;
    }

    public void Use(float cost) {
        currentStamina -= cost;
    }
}
