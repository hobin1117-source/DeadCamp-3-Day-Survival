using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    // 시작하기 버튼
    public void OnClickStart()
    {
        Time.timeScale = 1f;  // 혹시 멈춰있을 수 있으니 초기화
        SceneManager.LoadScene("PlayScene");   // ▶ 실제 플레이씬 이름으로 바꿔줘
    }

    // 종료하기 버튼
    public void OnClickExit()
    {
        Application.Quit();

#if UNITY_EDITOR
        // 에디터에서 테스트할 땐 플레이 모드만 종료
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}