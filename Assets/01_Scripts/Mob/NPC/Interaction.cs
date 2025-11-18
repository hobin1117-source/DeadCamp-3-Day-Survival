using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    //public LayerMask layerMask;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera camera;

    [SerializeField] private float hideDelay = 0.15f; // 프롬프트를 끄기까지 기다릴 시간
    private float lastHitInteractTime;                // 마지막으로 상호작용 가능한 걸 본 시간

    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance))
            {
                Debug.DrawRay(ray.origin, ray.direction * maxCheckDistance, Color.red);
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    // 상호작용 가능한 걸 본 시각 저장
                    lastHitInteractTime = Time.time;

                    if (hit.collider.gameObject != curInteractGameObject)
                    {
                        curInteractGameObject = hit.collider.gameObject;
                        curInteractable = interactable;
                        SetPromptText();
                    }
                }
                else
                {
                    // 일정 시간 동안은 그냥 그대로 두고, 그 이후에만 끔
                    if (Time.time - lastHitInteractTime > hideDelay)
                    {
                        ClearPrompt();
                    }
                }
            }
            else
            {
                if (Time.time - lastHitInteractTime > hideDelay)
                {
                    ClearPrompt();
                }
            }
        }
    }
    private void ClearPrompt()
    {
        curInteractGameObject = null;
        curInteractable = null;

        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }
    private void SetPromptText()
    {
        // 안전장치
        if (promptText == null || curInteractable == null)
            return;
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}