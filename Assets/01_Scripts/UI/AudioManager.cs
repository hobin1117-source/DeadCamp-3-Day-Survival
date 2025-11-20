using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Main Mixer")]
    public AudioMixer mainMixer;   // MainAudioMixer 에셋 연결

    const string MASTER_PARAM = "MasterVolume";  // Mixer exposed 이름

    void Awake()
    {
        // 싱글톤 유지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 0~1 → dB 변환 함수 (AudioMixer를 위해 필요)
    float ToDecibel(float value)
    {
        if (value <= 0.0001f)
            return -80f; // 거의 무음

        return Mathf.Log10(value) * 20f;
    }

    public void SetMasterVolume(float value)
    {
        mainMixer.SetFloat(MASTER_PARAM, ToDecibel(value));
    }
}
