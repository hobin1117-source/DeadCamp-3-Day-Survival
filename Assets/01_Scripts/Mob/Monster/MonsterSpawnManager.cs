using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.AI;

[System.Serializable]
public class MonsterEntry
{
    public GameObject prefabs;
    public int weight = 1;
}

public class MonsterSpawnManager : MonoBehaviour
{
    [Header("몬스터 종류 & 스폰 비율")]
    public MonsterEntry[] monsters;

    [Header("스폰 설정")]
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    public int maxMonsters = 10;

    private int currentMonsters = 0;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (currentMonsters < maxMonsters)
            {
                SpawnMonster();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnMonster()
    {
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

        NavMeshHit hit; //navmesh위에서 스폰 위치를 찾음

        if (NavMesh.SamplePosition(sp.position, out hit, 3f, NavMesh.AllAreas))
        {
            GameObject prefab = GetRandomMonsterWeight();

            GameObject m = Instantiate(prefab, hit.position, Quaternion.identity);
            currentMonsters++;

            m.GetComponent<Monster>().OnDeath += () =>
            {
                currentMonsters--;
            };
        }
    }

    GameObject GetRandomMonsterWeight()
    {
        int totalWeight = 0;

        foreach (var entry in monsters)
        {
            totalWeight += entry.weight;
        }

        int randomValue = Random.Range(0, totalWeight);
        int current = 0;

        foreach (var entry in monsters)
        {
            current += entry.weight;
            if (randomValue < current)
                return entry.prefabs;
        }
        return monsters[0].prefabs;
    }
}
