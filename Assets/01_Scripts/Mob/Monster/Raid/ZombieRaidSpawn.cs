using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieRaidSpawn : MonoBehaviour
{
    [Header("DayNightCycles 스크립트 넣기")]
    public Day day;
    public int dayCount = 0;

    [Header("Raid Zone 설정")]
    public Collider largeZone; //스폰 가능한 큰 구역
    public Collider safeZone; //스폰 금지 구역

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
        Vector3 pos = GetRandomSpawnPosistion();

        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 2f, NavMesh.AllAreas))
        {
            GameObject prefab = GetRandomZombieWeight(wave.monsters);

            GameObject m = Instantiate(prefab, hit.position, Quaternion.identity);
            currentRaidMonsters++;

            m.GetComponent<Monster>().OnDeath += () =>
            {
                currentRaidMonsters--;
            };
        }
    }

    Vector3 GetRandomSpawnPosistion()
    {
        Bounds b = largeZone.bounds;
        Vector3 pos;

        do
        {
            pos = new Vector3(Random.Range
                (b.min.x, b.max.x),
                b.center.y,
                Random.Range(b.min.z, b.max.z));
        }
        while (safeZone.bounds.Contains(pos));

        return pos;
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
