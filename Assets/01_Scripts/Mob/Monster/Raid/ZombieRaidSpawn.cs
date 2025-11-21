using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieRaidSpawn : MonoBehaviour
{
    [Header("DayNightCycles 스크립트 넣기")]
    public Day day;
    public int dayCount = 0;

    [Header("Raid 스폰 포인트")]
    public Transform[] spawnPoints;

    [Header("Raid 스폰 설정")]
    public RaidWave[] waves;

    private bool isRaiding = false; //레이드 시작 여부
    private int currentRaidMonsters = 0;

    void Update()
    {
        float hour = day.currentHour;
        bool raidTime = (hour >= 21f || hour < 6f);

        if (raidTime && !isRaiding)
        {
            StartCoroutine(RaidRoutine());
            isRaiding = true;
        }
        else if (!raidTime && isRaiding)
        {
            StopAllCoroutines();
            isRaiding = false;
        }
    }

    IEnumerator RaidRoutine()
    {
        while (true)
        {
            RaidWave wave = waves[Mathf.Clamp(dayCount, 0, waves.Length - 1)];

            if (currentRaidMonsters < wave.maxMonsters)
            {
                SpawnRaidZombie(wave);
            }

            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    void SpawnRaidZombie(RaidWave wave)
    {
        if (spawnPoints.Length == 0) return;

        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 2) NavMesh 위에서 위치 보정
        NavMeshHit hit;
        if (NavMesh.SamplePosition(sp.position, out hit, 3f, NavMesh.AllAreas))
        {
            GameObject prefab = GetRandomZombieWeight(wave.monsters);

            GameObject m = Instantiate(prefab, hit.position, Quaternion.identity);
            currentRaidMonsters++;

            Monster mon = m.GetComponent<Monster>();
            mon.forceChase = true;

            mon.OnDeath += () =>
            {
                currentRaidMonsters--;
            };
        }
    }




    GameObject GetRandomZombieWeight(MonsterEntry[] monsters)
    {
        int total = 0;
        foreach (var m in monsters)
            total += m.weight;

        int random = Random.Range(0, total);
        int current = 0;

        foreach (var m in monsters)
        {
            current += m.weight;
            if (random < current)
                return m.prefabs;
        }
        return monsters[0].prefabs;
    }
}
