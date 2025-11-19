using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


public class PauseMenuManager : MonoBehaviour
{

    [Header("UI")]
    public GameObject pauseMenuPanel; // 인스펙터에 할당 권장
    public GameObject settingsPanel; // Settings 창 (Panel)
    public Toggle musicToggle;       // 예: 설정 항목들
    public Slider volumeSlider;

    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu"; // 인스펙터에서 메인 메뉴 씬 이름 지정


    private bool isPaused = false;

    void Start()
    {
        // 초기값 불러오기
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (musicToggle != null) musicToggle.isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        if (volumeSlider != null) volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);

        // 시작 시 UI 상태 초기화 (null 체크 포함)
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                BackToMain();
            }
            else if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

    }

    public void PauseGame()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenSettings()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void BackToMain()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

    // 재시작 버튼 연결
    public void Restart()
    {
        // mainMenuSceneName이 빌드 세팅에 등록되어 있는지 확인
        if (IsSceneInBuildSettings(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {

            // 안전장치: 메인 메뉴가 없으면 현재 씬 재시작
            Debug.LogWarning($"Main menu scene '{mainMenuSceneName}' is not in Build Settings. Reloading current scene instead.");
            Scene current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(current.name);
        }
    ;
    }

    // 새 카메라의 AudioListener를 활성화하고, 이전 것은 비활성화
    void SwitchAudioListener(Camera newCam)
    {
        // 이전 활성 리스너 모두 비활성화
        foreach (var l in FindObjectsOfType<AudioListener>())
        {
            l.enabled = false;
        }
        var listener = newCam.GetComponent<AudioListener>();
        if (listener != null) listener.enabled = true;

    }

    // 설정 열기 버튼 연결
    public void Settings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }



    // 나가기 버튼 연결
    public void Exit()
    {

        // 에디터에서 테스트 시 재생 중지
        UnityEditor.EditorApplication.isPlaying = false;

        // 빌드된 앱에서는 종료
        Application.Quit();

    }
    bool IsSceneInBuildSettings(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return false;
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName) return true;
        }
        return false;
    }
}