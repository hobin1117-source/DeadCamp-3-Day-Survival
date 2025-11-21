using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Basic Stat")]
    public float range = 100f;   // 사거리 (나중에 Raycast 쓸 때)
    public int damage = 10;      // 데미지
    public float fireRate = 0.2f; // 연사 속도(쿨타임)

    [Header("Effect")]
    public Animator anim;             // Idle, Shoot 애니메이터
    public ParticleSystem muzzleFlash; // 총구 플래시
    public AudioClip fireSound;        // 총소리

    [Header("Hit Effect")]             // 새로 추가
    public GameObject bloodEffectPrefab; // 좀비 맞았을 때
    public GameObject sparkEffectPrefab; // 다른 오브젝트 맞았을 때
}