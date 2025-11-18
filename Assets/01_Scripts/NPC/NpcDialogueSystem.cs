using UnityEngine;
using UnityEngine.UI; // Text 컴포넌트를 사용하기 위해 필요합니다.
using TMPro;

public class NpcDialogueSystem : MonoBehaviour
{
    // === 1. 인스펙터에서 설정할 UI 및 설정 변수 ===
    [Header("UI 및 감지 설정")]
    public GameObject dialoguePanel;     // 대화창 UI 전체 (배경 패널 포함)
    public TextMeshProUGUI dialogueText; 
    public GameObject interactionPrompt; // "E 키를 누르세요" 같은 안내 문구 UI

    public float detectionRange = 5.0f; 
    public KeyCode interactionKey = KeyCode.E; // 대화를 시작/진행할 키


    [Header("대화 내용")]
    [TextArea(3, 10)]
    public string[] dialogueLines; // NPC가 순서대로 할 대화 목록

    
    private Transform playerTransform;
    private bool isPlayerInRange = false;
    private bool isDialogueActive = false;
    private int currentDialogueIndex = 0;

    private float sqrDetectionRange;

    void Start()
    {
       
        sqrDetectionRange = detectionRange * detectionRange;

        
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("씬에 'Player' 태그를 가진 오브젝트를 찾을 수 없습니다! 플레이어 태그를 확인하세요.");
        }

        // 시작 시 모든 UI 비활성화
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerTransform == null) return;


        Vector3 offset = playerTransform.position - transform.position;
        float sqrDistance = offset.sqrMagnitude;
        bool currentlyInRange = sqrDistance <= sqrDetectionRange;

        // 1. 실제 거리(제곱근)를 계산하여 디버그 로그로 출력
        float currentDistance = Mathf.Sqrt(sqrDistance);
      //   Debug.Log($"[NPC 간격] 현재 플레이어와의 거리: {currentDistance:F2} m"); 

        if (currentlyInRange && !isPlayerInRange)
        {
            isPlayerInRange = true;
            if (!isDialogueActive && interactionPrompt != null)
            {
                interactionPrompt.SetActive(true); 
            }
        }
        else if (!currentlyInRange && isPlayerInRange) 
        {
            isPlayerInRange = false;
            EndDialogue(); 
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }

    
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            if (!isDialogueActive)
            {
                StartDialogue(); // 대화 시작
            }
            else
            {
                AdvanceDialogue(); // 다음 대사로 진행
            }
        }
    }

    // 대화 시작 함수
    void StartDialogue()
    {
        if (dialogueLines.Length == 0) return;

        isDialogueActive = true;
        currentDialogueIndex = 0;

        // UI 활성화
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (interactionPrompt != null) interactionPrompt.SetActive(false); // 안내 문구 숨김

        DisplayCurrentLine();
    }

    // 대화 진행 함수
    void AdvanceDialogue()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex < dialogueLines.Length)
        {
            DisplayCurrentLine(); // 다음 대사 표시
        }
        else
        {
            EndDialogue(); // 모든 대화가 끝났을 때
        }
    }

    // 현재 대사를 텍스트 컴포넌트에 표시
    void DisplayCurrentLine()
    {
       
        if (dialogueText != null)
        {
            dialogueText.text = dialogueLines[currentDialogueIndex];
        }
        else
        {
            Debug.LogError("Dialogue Text (TextMeshProUGUI) 컴포넌트가 연결되지 않았습니다.");
        }
    }

    // 대화 종료 함수
    void EndDialogue()
    {
        isDialogueActive = false;

        // UI 비활성화
        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        // 플레이어가 아직 범위 안에 있다면 상호작용 안내를 다시 표시
        if (isPlayerInRange && interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
        }
    }
}