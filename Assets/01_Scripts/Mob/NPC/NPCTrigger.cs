using UnityEngine;
using TMPro;

public class NPCTrigger : MonoBehaviour
{
    public Transform player;
    public float triggerDistance = 3f;

    public TextMeshProUGUI dialogueText;
    public string message = "¾È³ç, ¿©ÇàÀÚ. Äù½ºÆ®°¡ ÇÊ¿äÇÏ´Ï F¸¦ ´­·¯º¸°Ô.";

    public GameObject questUI;   // ¡ç Äù½ºÆ® ÆÐ³Î

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

        // FÅ°·Î Äù½ºÆ® Ã¢ ¿­±â
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
        Debug.Log("[NPC] ´ë»ç Ç¥½ÃµÊ");
    }

    void HideDialogue()
    {
        isShown = false;
        dialogueText.gameObject.SetActive(false);
        Debug.Log("[NPC] ´ë»ç ¼û±è");
    }

    void ToggleQuestUI()
    {
        bool newState = !questUI.activeSelf;
        questUI.SetActive(newState);

        if (newState)
            Debug.Log("[NPC] Äù½ºÆ® Ã¢ ¿­¸²");
        else
            Debug.Log("[NPC] Äù½ºÆ® Ã¢ ´ÝÈû");
    }
}
