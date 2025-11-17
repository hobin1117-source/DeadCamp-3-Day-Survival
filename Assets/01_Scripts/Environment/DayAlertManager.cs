using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayAlertManager : MonoBehaviour
{
    [Header("Day 시스템 연결")]
    public Day day;

    [Header("알림 UI")]
    public TextMeshProUGUI alerText;
    public float alertDuration = 3f;

    private bool noticed18 = false;
    private bool noticed21 = false;
    private bool noticed06 = false;

    private float previousHour = 0f; //직전 시간 체크 용 변수

    void Start()
    {
        alerText.gameObject.SetActive(false);
    }

    void Update()
    {
        float hour = day.time * 24f;

        if (!noticed18 && previousHour < 18f && hour >= 18f)
        {
            ShowAlert("해가 저물어갑니다....");
            noticed18 = true;
        }

        if (!noticed21 && previousHour < 21f && hour >= 21f)
        {
            ShowAlert("밤이 찾아옵니다 \n습격에 대비하세요");
            noticed21 = true;
        }

        if (!noticed06 && previousHour < 6f && hour >= 6f)
        {
            ShowAlert("아침이 찾아왔습니다");
            noticed06 = true;

            noticed18 = false;
            noticed21 = false;
            noticed06 = false;
        }

        previousHour = hour;
    }

    private void ShowAlert(string message)
    {
        StopAllCoroutines(); //만약 아직 안 꺼진 알림 코르틴이 있을 수 있어서
        StartCoroutine(AlertRoutine(message));
    }

    private IEnumerator AlertRoutine(string message)
    {
        alerText.text = message;
        alerText.gameObject.SetActive(true);

        yield return new WaitForSeconds(alertDuration);

        alerText.gameObject.SetActive(false);
    }
}
