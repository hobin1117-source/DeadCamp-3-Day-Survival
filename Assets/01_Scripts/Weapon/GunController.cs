using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;      // 지금 들고 있는 총

    private float fireCooldown;  // 남은 쿨타임
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
    }
}