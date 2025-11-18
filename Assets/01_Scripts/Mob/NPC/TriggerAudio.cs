using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    // 재생할 오디오 소스 컴포넌트를 연결할 변수
    private AudioSource audioSource;

    // ✨ 인스펙터에서 설정할 "감지 대상" 레이어 마스크를 추가했어!
    // 여기에 플레이어와 몬스터 레이어를 모두 체크해주면 돼!
    [SerializeField]
    private LayerMask detectionLayers;

    // 게임 시작 시 한 번 호출됩니다.
    void Start()
    {
        // 이 스크립트가 붙어있는 오브젝트에서 AudioSource 컴포넌트를 찾아옵니다.
        audioSource = GetComponent<AudioSource>();

        // 만약 AudioSource가 없으면 경고 메시지를 띄웁니다.
        if (audioSource == null)
        {
            Debug.Log("AudioSource 컴포넌트가 없습니다! 'DetectionZone' 오브젝트에 AudioSource를 추가해주세요.");
        }
    }

    // 다른 콜라이더가 이 트리거 안으로 들어왔을 때 호출됩니다.
    // 여기에서 'other'는 트리거 안으로 들어온 오브젝트의 콜라이더를 나타냅니다.
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("트리거에 감지 되었습니다: " + other.gameObject.name);

        // ✨ 레이어 마스크를 사용해서 플레이어와 몬스터 모두 감지하도록 수정!
        // `detectionLayers`에 Player 레이어와 Monster 레이어를 모두 체크해줬다고 가정하고 쓰는 코드야!
        if (((1 << other.gameObject.layer) & detectionLayers) != 0)
        {
            Debug.Log($"Player 또는 Monster 감지되었습니다: {other.gameObject.name}");

            // AudioSource가 존재하고, 현재 재생 중이 아니라면 오디오를 재생합니다.
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play(); // 오디오 재생!
                Debug.Log("오디오가 활성화되었습니다!");
            }
        }
    }

    // 다른 콜라이더가 이 트리거 밖으로 나갔을 때 호출됩니다. (선택 사항)
    // 적이 밖으로 나가면 오디오를 끄고 싶을 때 사용합니다.
    private void OnTriggerExit(Collider other)
    {
        // ✨ 여기도 동일하게 `detectionLayers`를 사용하도록 수정!
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