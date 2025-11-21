using UnityEngine;

[System.Serializable]
public class RaidWave
{
    [Header("이 웨이브에서 나올 몬스터 종류")]
    public MonsterEntry[] monsters;

    [Header("스폰 설정")]
    public int maxMonsters = 20;
    public float spawnInterval = 0.5f; //스폰 간격
}
