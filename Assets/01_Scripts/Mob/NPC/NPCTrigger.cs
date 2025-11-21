using UnityEngine;
using TMPro;

public class NPCTrigger : MonoBehaviour
{
    public Transform player;
    public float triggerDistance = 3f;

    public TextMeshProUGUI dialogueText;
    public string message = "안녕, 여행자. 퀘스트가 필요하니 F를 눌러보게.";

    public GameObject questUI;   // ← 퀘스트 패널

    bool isShown = false;
    bool isPlayerNear = false;

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        if (dialogueText != null)
            dialogueText.gameObject.SetActive(false);

        if (questUI != null)
            questUI.SetActive(false);
    }



    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        isPlayerNear = dist <= triggerDistance;

        if (isPlayerNear && !isShown)
            ShowDialogue();
        else if (!isPlayerNear && isShown)
            HideDialogue();

        // F키로 퀘스트 창 열기
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            ToggleQuestUI();
        }
    }

    void ShowDialogue()
    {
        isShown = true;
        dialogueText.text = message;
        dialogueText.gameObject.SetActive(true);
        Debug.Log("[NPC] 대사 표시됨");
    }

    void HideDialogue()
    {
        isShown = false;
        dialogueText.gameObject.SetActive(false);
        Debug.Log("[NPC] 대사 숨김");
    }

    void ToggleQuestUI()
    {
        bool newState = !questUI.activeSelf;
        questUI.SetActive(newState);

        if (newState)
            Debug.Log("[NPC] 퀘스트 창 열림");
        else
            Debug.Log("[NPC] 퀘스트 창 닫힘");
    }
}
