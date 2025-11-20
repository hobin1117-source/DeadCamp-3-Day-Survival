using UnityEngine;

public class Skybox4Blender : MonoBehaviour
{
    [Header("Day 스크립트 연결")]
    public Day day;   // DayNightCycle.cs (time 0~1 사용하는 스크립트)

    [Header("시간대별 Cubemap")]
    public Cubemap night;     // 0시 ~ 새벽
    public Cubemap sunrise;   // 아침
    public Cubemap daySky;    // 낮
    public Cubemap sunset;    // 저녁

    [Header("설정")]
    public float blendSharpness = 1f; // 1이면 선형, 2 이상이면 중앙이 더 부드럽게
    [Header("회전 설정")]
    public float rotationSpeed = 1f; // 초당 몇 도 회전할지 (아주 천천히면 0.1~0.3 정도)
    Material skyMat;

    void Start()
    {
        // Lighting에서 지정한 Skybox_Blend 머티리얼 가져오기
        skyMat = RenderSettings.skybox;
    }

    void Update()
    {
        if (day == null || skyMat == null) return;

        // Day 스크립트의 time (0~1)을 4구간으로 나누기
        float t = Mathf.Repeat(day.time, 1f); // 0~1
        float phase = t * 4f;                 // 0~4
        int index = Mathf.FloorToInt(phase);  // 0,1,2,3
        float lerp = phase - index;           // 각 구간 안에서 0~1

        Cubemap from = night;
        Cubemap to = sunrise;

        // 어떤 두 장을 섞을지 결정
        switch (index)
        {
            case 0: // Night -> Sunrise
                from = night;
                to = sunrise;
                break;
            case 1: // Sunrise -> Day
                from = sunrise;
                to = daySky;
                break;
            case 2: // Day -> Sunset
                from = daySky;
                to = sunset;
                break;
            case 3: // Sunset -> Night
            default:
                from = sunset;
                to = night;
                break;
        }

        // 블렌딩 곡선 조금 더 부드럽게 하고 싶으면 pow 사용
        float k = Mathf.Clamp01(lerp);
        if (blendSharpness != 1f)
            k = Mathf.Pow(k, blendSharpness);

        // 셰이더에 값 전달
        skyMat.SetTexture("_Tex1", from);
        skyMat.SetTexture("_Tex2", to);
        skyMat.SetFloat("_Blend", k);
        // ---- 여기서부터 회전 코드 ----
        float rot = skyMat.GetFloat("_Rotation");
        rot += rotationSpeed * Time.deltaTime;  // 시간에 따라 증가
        if (rot > 360f) rot -= 360f;           // 숫자 너무 커지는 거 방지
        skyMat.SetFloat("_Rotation", rot);
    }
}