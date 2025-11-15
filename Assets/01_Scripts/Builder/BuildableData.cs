using UnityEngine;

[CreateAssetMenu(fileName = "Buildable", menuName = "New Buildable")]
public class BuildableData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public Sprite icon;

    [Header("Prefabs")]
    public GameObject placeablePrefab;   // 실제 설치될 오브젝트
    public GameObject previewPrefab;     // 프리뷰(고스트) 오브젝트

    [Header("Cost")]
    public int requiredWood;             // 필요한 목재 개수
}