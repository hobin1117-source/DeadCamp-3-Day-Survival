using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("--- Settings ---")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [Header("--- References ---")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private List<AudioSource> sfxSources;

    private int sfxIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;


            // 저장된 볼륨 불러오기 (없으면 기본값 1)
            LoadVolumeSettings();
        }
        else
        {
           
        }
    }

    private void Start()
    {
        UpdateAllVolumes();
    }

    // --- 재생 기능 (기존과 동일) ---
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip) return;
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        AudioSource source = sfxSources[sfxIndex];
        source.clip = clip;
        source.volume = masterVolume * sfxVolume;
        source.Play();
        sfxIndex = (sfxIndex + 1) % sfxSources.Count;
    }

    // --- 볼륨 조절 및 저장 기능 ---

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        UpdateAllVolumes();
        PlayerPrefs.SetFloat("MasterVol", masterVolume); // 값 저장
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        UpdateAllVolumes();
        PlayerPrefs.SetFloat("BGMVol", bgmVolume); // 값 저장
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        UpdateAllVolumes();
        PlayerPrefs.SetFloat("SFXVol", sfxVolume); // 값 저장
    }

    private void UpdateAllVolumes()
    {
        if (bgmSource != null) bgmSource.volume = masterVolume * bgmVolume;
        foreach (var source in sfxSources) source.volume = masterVolume * sfxVolume;
    }

    private void LoadVolumeSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVol", 1f);
        bgmVolume = PlayerPrefs.GetFloat("BGMVol", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVol", 1f);
    }
}