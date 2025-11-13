using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageIble
{
    void TakePhysicalDamge(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamageIble
{
    public float hungrySpeed;

    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;

    public event Action onTakeDamage;

    void Update()
    { // 플레이어 상태 변화
        hunger.Subtract(hunger.passiveValue * Time.deltaTime * hungrySpeed);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue == 0f)
        {
            Die();
        }
    }

    public void Heal(float amout)
    {
        health.Add(amout);
    }

    public void Eat(float amout)
    {
        hunger.Add(amout);
    }

    public void Die()
    {
        Debug.Log("Player Died");
    }

    public void TakePhysicalDamge(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }
}
