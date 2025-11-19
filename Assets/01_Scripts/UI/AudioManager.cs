using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixer")]
    public AudioMixer masterMixer; // 에디터에 할당

    // 노출한 파라미터 이름 (Inspector에서 Mixer에서 Expose한 이름과 동일해야 함)
    private const string MASTER_KEY = "MasterVolume";
    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";

    // PlayerPrefs에 저장하는 키들 (같은 이름 사용)
    private float saveDelay = 1.0f; // 마지막 변경 후 저장까지의 지연시간
    private Coroutine saveCoroutine;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }

        // 시작 시 PlayerPrefs에서 불러와 믹서에 적용
        float master = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float music = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfx = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        ApplyToMixer(MASTER_KEY, master);
        ApplyToMixer(MUSIC_KEY, music);
        ApplyToMixer(SFX_KEY, sfx);
    }

    // 앱이 포커스 잃거나 종료될 때 확실히 저장
    private void OnApplicationPause(bool pause)
    {
        if (pause) ForceSave();
    }

    private void OnApplicationQuit()
    {
        ForceSave();
    }

    // 선형(0..1) -> 데시벨 변환
    private float LinearToDb(float linear)
    {
        linear = Mathf.Clamp01(linear);
        if (linear <= 0.0001f) return -80f; // 무음 처리
        return Mathf.Log10(linear) * 20f;
    }

    // 믹서에 적용
    private void ApplyToMixer(string key, float linearValue)
    {
        if (masterMixer == null) return;
        float db = LinearToDb(linearValue);
        masterMixer.SetFloat(key, db);
    }

    // 외부 호출용 (슬라이더 이벤트에서 호출)
    public void SetMasterVolume(float linear)
    {
        ApplyToMixer(MASTER_KEY, linear);
        PlayerPrefs.SetFloat(MASTER_KEY, Mathf.Clamp01(linear));
        DebouncedSave();
    }

    public void SetMusicVolume(float linear)
    {
        ApplyToMixer(MUSIC_KEY, linear);
        PlayerPrefs.SetFloat(MUSIC_KEY, Mathf.Clamp01(linear));
        DebouncedSave();
    }

    public void SetSFXVolume(float linear)
    {
        ApplyToMixer(SFX_KEY, linear);
        PlayerPrefs.SetFloat(SFX_KEY, Mathf.Clamp01(linear));
        DebouncedSave();
    }

    // 디바운스: 마지막 변경 후 saveDelay 초 동안 변경이 없으면 저장
    private void DebouncedSave()
    {
        if (saveCoroutine != null) StopCoroutine(saveCoroutine);
        saveCoroutine = StartCoroutine(DelayedSaveCoroutine());
    }

    private IEnumerator DelayedSaveCoroutine()
    {
        yield return new WaitForSeconds(saveDelay);
        PlayerPrefs.Save();
        saveCoroutine = null;
        Debug.Log("Audio settings saved (debounced).");
    }

    // 즉시 저장 강제 호출
    public void ForceSave()
    {
        if (saveCoroutine != null) { StopCoroutine(saveCoroutine); saveCoroutine = null; }
        PlayerPrefs.Save();
        Debug.Log("Audio settings saved (forced).");
    }

    // UI 초기화나 리셋용
    public void ResetToDefaults()
    {
        SetMasterVolume(1f);
        SetMusicVolume(1f);
        SetSFXVolume(1f);
        ForceSave();
    }

    // 믹서에서 읽어 0..1로 반환 (UI 초기화 시 사용)
    public float GetLinearValue(string key)
    {
        if (masterMixer == null) return 1f;
        float db;
        if (masterMixer.GetFloat(key, out db))
        {
            if (db <= -79.999f) return 0f;
            return Mathf.Pow(10f, db / 20f);
        }
        return PlayerPrefs.GetFloat(key, 1f);
    }
   
}
