using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    // 재생할 오디오 소스 컴포넌트를 연결할 변수
    private AudioSource audioSource;

    // 인스펙터에서 설정할 "감지 대상" 레이어 마스크
    [SerializeField]
    private LayerMask detectionLayers;

    // ✨ 이제 프리팹 대신, 미리 씬에 배치된 파티클 시스템을 직접 할당할 수 있도록!
    // 이 스크립트가 붙은 오브젝트의 자식으로 파티클이 있다면 GetComponentInChildren으로 찾아도 되고,
    // public으로 열어서 인스펙터에서 직접 드래그앤드롭 할당하는게 가장 확실하고 편리해.
    public ParticleSystem soundWaveParticleSystem;

    // 게임 시작 시 한 번 호출됩니다.
    void Start()
    {
        // 이 스크립트가 붙어있는 오브젝트에서 AudioSource 컴포넌트를 찾아옵니다.
        audioSource = GetComponent<AudioSource>();

        // 만약 AudioSource가 없으면 경고 메시지를 띄웁니다.
        if (audioSource == null)
        {
            Debug.Log("AudioSource 컴포넌트가 없습니다! 이 스크립트를 붙인 오브젝트에 AudioSource를 추가해주세요.");
        }

        // ✨ 파티클 시스템이 할당되었는지 확인 (GetComponentInChildren으로 자동으로 찾아올 수도 있어!)
        if (soundWaveParticleSystem == null)
        {
            // 만약 현재 오브젝트의 자식으로 파티클 시스템이 있다면 이렇게 찾아와도 돼!
            soundWaveParticleSystem = GetComponentInChildren<ParticleSystem>();
            if (soundWaveParticleSystem == null)
            {
                Debug.LogWarning("SoundWaveParticleSystem이 할당되지 않았거나 찾을 수 없습니다! 이펙트가 재생되지 않을 수 있습니다.");
            }
        }

        // 파티클 시스템이 있다면, 시작할 때는 일단 멈춰있도록 설정 (선택 사항)
        if (soundWaveParticleSystem != null)
        {
            soundWaveParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // 기존 파티클 클리어하고 멈춤
        }
    }

    // 다른 콜라이더가 이 트리거 안으로 들어왔을 때 호출됩니다.
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("트리거에 감지 되었습니다: " + other.gameObject.name);

        // `detectionLayers`에 포함된 오브젝트만 감지!
        if (((1 << other.gameObject.layer) & detectionLayers) != 0)
        {
            Debug.Log($"Player 또는 Monster 감지되었습니다: {other.gameObject.name}");

            // AudioSource가 존재하고, 현재 재생 중이 아니라면 오디오를 재생합니다.
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play(); // 오디오 재생!
                Debug.Log("오디오가 활성화되었습니다!");

                // ✨ 이미 가지고 있는 파티클 시스템을 재생!
                if (soundWaveParticleSystem != null)
                {
                    // 파티클 시스템이 꺼져있다면 켜주고
                    if (!soundWaveParticleSystem.gameObject.activeInHierarchy)
                    {
                        soundWaveParticleSystem.gameObject.SetActive(true);
                    }

                    // 기존에 남아있던 파티클이 있다면 클리어하고 다시 시작 (선택 사항)
                    soundWaveParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    soundWaveParticleSystem.Play();

                    Debug.Log("사운드 웨이브 파티클 시스템 재생!");
                }
            }
        }
    }

    // 다른 콜라이더가 이 트리거 밖으로 나갔을 때 호출됩니다.
    private void OnTriggerExit(Collider other)
    {
        // `detectionLayers`에 포함된 오브젝트가 나가는 것을 감지!
        if (((1 << other.gameObject.layer) & detectionLayers) != 0)
        {
            // AudioSource가 존재하고, 현재 재생 중이라면 오디오를 멈춥니다.
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop(); // 오디오 멈춤!
                Debug.Log($"Player 또는 Monster 나갔습니다. 오디오 정지!");
            }
        }
    }
}