using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public GameObject settingWindow;

    bool isOpened = false;
    public bool IsSettingOpen => isOpened;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 시작할 때 설정창은 꺼두기
        if (settingWindow != null)
            settingWindow.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ESC 키 감지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingWindow();
        }
    }

    public void ToggleSettingWindow()
    {
        if (settingWindow == null) return;

        isOpened = !settingWindow.activeSelf;
        settingWindow.SetActive(isOpened);

        // 설정창 열리면 게임 멈춤 / 닫히면 다시 진행
        Time.timeScale = isOpened ? 0f : 1f;

        if (isOpened)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void CloseSettingWindow()
    {
        if (settingWindow == null) return;

        settingWindow.SetActive(false);
        isOpened = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GoToMainScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    // 게임 종료
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();

#if UNITY_EDITOR
        // 에디터에서 플레이 모드 종료
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}