using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Enviroment;

public class Day : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
    private float timeRate;
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunlntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;

    [Header("Skybox Blend")]
    [Tooltip("프리셋 바꿀 때 블렌딩에 걸리는 시간(초)")]
    public float skyBlendDuration = 5f;
    private string currentPresetKey = "";
    public float currentHour => time * 24;

    // Start is called before the first frame update
    void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;

        //UpdateSkyboxByTime(true);
    }

    // Update is called once per frame
    void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        UpdateLighting(sun, sunColor, sunlntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);

        //UpdateSkyboxByTime();
    }

    void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;
        lightSource.color = gradient.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }

    }
    string GetPresetKeyByTime(float hour)
    {
        // 예시 시간대 나눔, 원하면 숫자 조정 가능
        // 05~08 : Sunrise
        // 08~17 : Day
        // 17~20 : Sunset
        // 그 외  : Night

        if (hour >= 5f && hour < 8f)
            return "Sunrise";
        else if (hour >= 8f && hour < 17f)
            return "Day";
        else if (hour >= 17f && hour < 20f)
            return "Sunset";
        else
            return "Night";
    }

    // EnviromentManager에게 프리셋 블렌딩 요청
    //void UpdateSkyboxByTime(bool force = false)
    //{
    //    float hour = currentHour;
    //    string targetKey = GetPresetKeyByTime(hour);

    //    // 아직 매니저가 없으면 그냥 리턴 (에러 방지)
    //    if (EnviromentManager.Instance == null)
    //        return;

    //    // 처음 호출(force) 또는 시간대가 바뀌었을 때만 블렌딩
    //    if (force || targetKey != currentPresetKey)
    //    {
    //        currentPresetKey = targetKey;
    //        EnviromentManager.Instance.BlendEnviroment(targetKey, skyBlendDuration);
    //    }
    //}

}
