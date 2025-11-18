using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    // 재생할 오디오 소스 컴포넌트를 연결할 변수
    private AudioSource audioSource;

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
        Debug.Log("트리거에 뭔가 들어옴: " + other.gameObject.name); // 디버깅용

        // 들어온 오브젝트의 태그가 "Enemy"인지 확인합니다.
        // 이때, other.gameObject.tag는 대소문자를 구분하니 정확하게 일치해야 합니다!
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("적이 감지되었습니다: " + other.gameObject.name); // 디버깅용

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
        if (other.gameObject.CompareTag("Player"))
        {
            // AudioSource가 존재하고, 현재 재생 중이라면 오디오를 멈춥니다.
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop(); // 오디오 멈춤!
                Debug.Log("적이 나갔습니다. 오디오 정지!");
            }
        }
    }
}