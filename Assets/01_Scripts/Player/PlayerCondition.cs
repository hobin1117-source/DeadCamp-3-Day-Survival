using System;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);

}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uICondition;

    Condition health { get { return uICondition.health; } }
    Condition hunger { get { return uICondition.hunger; } }
    Condition dirsty { get { return uICondition.dirsty; } }

    public float noHungerHealthDecay;
    public float noDrinkHealthDecay;

    public event Action onTakeDamage;

    // Update is called once per frame
    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        dirsty.Subtract(dirsty.passiveValue * Time.deltaTime);

        if (hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (dirsty.curValue == 0f)
        {
            health.Subtract(noDrinkHealthDecay * Time.deltaTime);
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

    public void Drink(float amout)
    {
        dirsty.Add(amout);
    }

    public void Die()
    {
        Debug.Log("ав╬З╢ы.");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

}
