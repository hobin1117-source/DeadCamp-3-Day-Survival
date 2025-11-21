using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour, IDamagable
{
    [Header("바리게이트 상태")]
    public int maxHP = 50;
    private int currentHP;

    void Awake()
    {
        currentHP = maxHP;
    }

    public void TakePhysicalDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator HitShake()
    {
        Vector3 originalPlos = transform.localPosition;

        float timer = 0f;
        float duration = 0.15f; //흔들리는 시간
        float strength = 0.05f; //흔들리는 강도

        while (timer < duration)
        {
            timer += Time.deltaTime;
            transform.localPosition = originalPlos + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.localPosition = originalPlos;
    }
}
