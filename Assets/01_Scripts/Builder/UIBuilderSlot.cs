using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildSlot : MonoBehaviour
{
    public BuildableData data;
    public UIBuilder builder;

    [Header("UI")]
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI woodCostText;
    public Button button;

    public int index;

    public void Set()
    {
        if (data == null) return;

        icon.gameObject.SetActive(true);
        icon.sprite = data.icon;

        nameText.text = data.displayName;
        woodCostText.text = $"Wood : {data.requiredWood}";
    }

    public void Clear()
    {
        icon.gameObject.SetActive(false);
        nameText.text = string.Empty;
        woodCostText.text = string.Empty;
        data = null;
    }

    public void OnClickButton()
    {
        builder.SelectBuild(index);
    }

    // 자원 부족하면 버튼 비활성화
    public void SetInteractable(bool canBuild)
    {
        if (button != null)
            button.interactable = canBuild;
    }
}