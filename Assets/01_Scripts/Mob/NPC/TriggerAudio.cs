using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    // 재생할 오디오 소스 컴포넌트를 연결할 변수
    private AudioSource audioSource;

    // 인스펙터에서 설정할 "감지 대상" 레이어 마스크
    [SerializeField]
    private LayerMask detectionLayers;

    // 미리 씬에 배치된 파티클 시스템을 직접 할당할 수 있도록!
    public ParticleSystem soundWaveParticleSystem;

    // 현재 트리거 안에 있는 감지 대상 오브젝트 수를 세는 변수
    private int detectedObjectsCount = 0;

    // ✨ 3D 오디오 관련 설정 변수들 (인스펙터에서 조절 가능)
    [Header("3D Audio Settings")]
    [Tooltip("오디오가 최대로 들리는 최소 거리 (이 거리 안에선 볼륨 최대)")]
    public float audioMinDistance = 1f;
    [Tooltip("오디오가 더 이상 들리지 않는 최대 거리")]
    public float audioMaxDistance = 10f;
    [Tooltip("거리에 따른 볼륨 감소 방식 (선택: Linear, Logarithmic, Custom)")]
    public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;

    // 🔹오디오 피크 감지 관련
    [Header("Dog Animation")]
    public Animator dogAnimator;
    public string barkTriggerName = "Bark";

    [Header("Audio Peak Detection")]
    [Tooltip("이 값보다 볼륨이 클 때를 '짖었다'라고 판단")]
    public float volumeThreshold = 0.1f;
    [Tooltip("연속 트리거 최소 간격(초)")]
    public float minTriggerInterval = 0.25f;

    private float lastTriggerTime = -999f;
    private float[] samples = new float[1024];  // 오디오 샘플 버퍼

    // 게임 시작 시 한 번 호출됩니다.
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource 컴포넌트가 없습니다! 이 스크립트를 붙인 오브젝트에 AudioSource를 추가해주세요.");
            enabled = false;
            return;
        }

        // ✨ AudioSource를 3D 공간 음향 설정으로 변경
        audioSource.spatialBlend = 1f; // 1.0f는 완전한 3D 사운드, 0.0f는 2D 사운드
        audioSource.rolloffMode = rolloffMode;
        audioSource.minDistance = audioMinDistance;
        audioSource.maxDistance = audioMaxDistance;

        if (audioSource.clip == null)
        {
            Debug.LogWarning("AudioSource에 재생할 오디오 클립이 할당되지 않았습니다.");
        }


        if (soundWaveParticleSystem == null)
        {
            soundWaveParticleSystem = GetComponentInChildren<ParticleSystem>();
            if (soundWaveParticleSystem == null)
            {
                Debug.LogWarning("SoundWaveParticleSystem이 할당되지 않았거나 찾을 수 없습니다! 이펙트가 재생되지 않을 수 있습니다.");
            }
        }

        // 파티클 시스템이 있다면, 시작할 때는 일단 멈춰있도록 설정
        if (soundWaveParticleSystem != null)
        {
            soundWaveParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    // 오디오 피크 감지 & 애니메이션/이펙트 트리거
    private void Update()
    {
        // 트리거 안에 몬스터가 없으면 분석 안 함
        if (detectedObjectsCount <= 0) return;
        if (audioSource == null || !audioSource.isPlaying) return;

        // 이 AudioSource에서 현재 출력되는 샘플 데이터 가져오기
        audioSource.GetOutputData(samples, 0);

        // 간단하게 평균 볼륨 계산 (절대값의 평균)
        float sum = 0f;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += Mathf.Abs(samples[i]);
        }
        float avgVolume = sum / samples.Length;

        // 볼륨이 threshold 이상이고, 최소 간격이 지났다면 "짖었다"라고 판단
        if (avgVolume > volumeThreshold && Time.time - lastTriggerTime > minTriggerInterval)
        {
            lastTriggerTime = Time.time;

            // 애니메이션 트리거
            if (dogAnimator != null && !string.IsNullOrEmpty(barkTriggerName))
            {
                dogAnimator.SetTrigger(barkTriggerName);
            }

            // 이펙트 한 번 터뜨리기
            if (soundWaveParticleSystem != null)
            {
                soundWaveParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                soundWaveParticleSystem.Play();
            }
        }
    }

    // 다른 콜라이더가 이 트리거 안으로 들어왔을 때 호출됩니다.
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("트리거에 감지 되었습니다: " + other.gameObject.name);

        if (((1 << other.gameObject.layer) & detectionLayers) != 0)
        {
            detectedObjectsCount++;
            //Debug.Log($"플레이어 또는 몬스터 감지되었습니다: {other.gameObject.name} (현재 {detectedObjectsCount}개)");

            // 첫 번째로 감지된 오브젝트라면 오디오 재생 시작
            if (detectedObjectsCount == 1)
            {
                if (audioSource != null && audioSource.clip != null)
                {
                    audioSource.Play();
                    //Debug.Log("오디오 활성화 및 재생 시작!");
                }
                else if (audioSource == null)
                {
                    //Debug.LogWarning("AudioSource가 없어 오디오를 재생할 수 없습니다.");
                }
            }
        }
    }

    // 다른 콜라이더가 이 트리거 밖으로 나갔을 때 호출됩니다.
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & detectionLayers) != 0)
        {
            detectedObjectsCount--;
            if (detectedObjectsCount < 0) detectedObjectsCount = 0;
            //Debug.Log($"플레이어 또는 몬스터 나갔습니다: {other.gameObject.name} (남은 개수: {detectedObjectsCount})");

            if (detectedObjectsCount == 0)
            {
                if (audioSource != null && audioSource.isPlaying)
                {
                    audioSource.Stop();
                    //Debug.Log("모든 감지 대상 나감. 오디오 정지!");
                }

                // 몬스터가 다 나가면 이펙트도 멈춤
                if (soundWaveParticleSystem != null && soundWaveParticleSystem.isPlaying)
                {
                    soundWaveParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    //Debug.Log("모든 감지 대상 나감. 파티클 시스템 정지!");
                }
            }
        }
    }
}