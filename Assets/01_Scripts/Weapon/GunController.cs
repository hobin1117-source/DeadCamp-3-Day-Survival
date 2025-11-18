using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;      // 지금 들고 있는 총

    private float fireCooldown;  // 남은 쿨타임
    private AudioSource audioSource;

    private int monsterLayer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        monsterLayer = LayerMask.NameToLayer("Monster");
    }

    private void Update()
    {
        // ① 아직 총을 안 든 상태면 그냥 리턴
        if (currentGun == null)
            return;
        // 쿨타임 감소
        if (fireCooldown > 0f)
            fireCooldown -= Time.deltaTime;

        // 마우스 왼쪽 버튼(기본 "Fire1") 입력
        if (Input.GetButton("Fire1"))
        {
            TryShoot();
        }
    }
    public void SetGun(Gun newGun)
    {
        currentGun = newGun;
    }

    public void ClearGun()
    {
        currentGun = null;
    }
    private void TryShoot()
    {
        // ② 혹시라도 여기서도 null 이면 안전하게 리턴
        if (currentGun == null)
            return;
        // 아직 쿨타임이면 발사 X
        if (fireCooldown > 0f)
            return;

        Shoot();
    }

    private void Shoot()
    {
        // 다음 발사까지 쿨타임 설정
        fireCooldown = currentGun.fireRate;

        // 애니메이션 (Animator에 "Shoot" Trigger가 있다고 가정)
        if (currentGun.anim != null)
        {
            currentGun.anim.SetTrigger("Shoot");
        }

        // 머즐 플래시
        if (currentGun.muzzleFlash != null)
        {
            var go = currentGun.muzzleFlash.gameObject;
            if (!go.activeSelf)
                go.SetActive(true);

            currentGun.muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            currentGun.muzzleFlash.Play();
        }

        // 사운드
        if (audioSource == null)
        {
            Debug.LogWarning("GunController: AudioSource가 없습니다.");
        }
        else if (currentGun.fireSound == null)
        {
            Debug.LogWarning("GunController: fireSound 클립이 비어 있습니다.");
        }
        else
        {
            Debug.Log("총소리 출력!");
            audioSource.PlayOneShot(currentGun.fireSound, 0.3f);
        }

        Debug.Log("총 발사! (무한 탄약)");

        // 1) 화면 중앙에서 앞으로 레이 쏘기
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("GunController: Main Camera를 찾을 수 없습니다.");
            return;
        }

        Ray ray = cam.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, currentGun.range))
        {
            Debug.Log("총알이 맞은 대상 : " + hit.collider.name);

            // ★ 1) 맞은 오브젝트의 레이어 확인
            int hitLayer = hit.collider.gameObject.layer;

            if (hitLayer == monsterLayer)
            {
                // ─ 좀비인 경우 (Monster 레이어) ─

                // 데미지 주기 (Monster 스크립트가 달려있다면)
                if (hit.collider.TryGetComponent(out Monster monster))
                {
                    monster.TakePhysicalDamage(currentGun.damage);
                }

                // 피 이펙트 출력
                SpawnHitEffect(currentGun.bloodEffectPrefab, hit.point, hit.normal);
            }
            else
            {
                // ─ 좀비가 아닌 경우 ─
                SpawnHitEffect(currentGun.sparkEffectPrefab, hit.point, hit.normal);
            }
        }
    }

    // 이펙트 찍어내는 공통 함수
    private void SpawnHitEffect(GameObject prefab, Vector3 position, Vector3 normal)
    {
        if (prefab == null) return;

        // 표면 방향을 향하도록 회전
        Quaternion rot = Quaternion.LookRotation(normal);
        GameObject effect = Instantiate(prefab, position, rot);

        Destroy(effect, 2f); // 2초 후 삭제 (필요에 따라 조절)
    }

}
