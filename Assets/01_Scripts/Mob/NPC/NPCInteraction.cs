using UnityEngine;
using UnityEngine.UI; // UI 요소를 사용하려면 이 네임스페이스가 필요합니다.

public class NpcInteraction : MonoBehaviour
{
    // === 인스펙터에서 설정할 변수들 ===
    [Header("UI 설정")]
    public GameObject interactionUI; // 띄우고 싶은 UI 패널/캔버스
    public float detectionRange = 5.0f; // 플레이어를 감지할 거리 (반경)

    // === 내부 변수들 ===
    private Transform playerTransform;
    private bool isPlayerInRange = false;

    void Start()
    {
        // 씬에서 "Player" 태그를 가진 오브젝트를 찾아 Transform을 저장합니다.
        // 플레이어 오브젝트에 'Player' 태그가 설정되어 있어야 합니다.
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("씬에 'Player' 태그를 가진 오브젝트를 찾을 수 없습니다!");
        }

        // 시작할 때 UI를 비활성화합니다.
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }

    void Update()
    {
        if (playerTransform == null || interactionUI == null)
        {
            return; // 플레이어나 UI가 설정되지 않았다면 아무것도 하지 않습니다.
        }

        // 1. 현재 NPC와 플레이어 사이의 거리를 계산합니다.
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // 2. 거리가 감지 범위 이내인지 확인합니다.
        if (distance <= detectionRange)
        {
            // 플레이어가 범위 안에 들어왔을 때
            if (!isPlayerInRange) // 한 번만 실행되도록 체크
            {
                isPlayerInRange = true;
                // UI 활성화
                interactionUI.SetActive(true);
                Debug.Log("플레이어가 접근했습니다. UI 활성화.");
            }
        }
        else
        {
            // 플레이어가 범위 밖으로 나갔을 때
            if (isPlayerInRange) // 한 번만 실행되도록 체크
            {
                isPlayerInRange = false;
                // UI 비활성화
                interactionUI.SetActive(false);
                Debug.Log("플레이어가 멀어졌습니다. UI 비활성화.");
            }
        }
    }
}