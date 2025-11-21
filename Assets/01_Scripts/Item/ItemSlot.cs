using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SlotType
{
    Inventory,
    Craft
}

public class ItemSlot : MonoBehaviour
{
    public SlotType slotType;

    public ItemData item;

    public UIInventory inventory;
    public UICraft craft;
    public Button button;
    public Image icon;
    public TextMeshProUGUI quatityText;
    private Outline outline;

    public int index;
    public bool equipped;
    public int quantity;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        switch (slotType)
        {
            case SlotType.Inventory:
                button.onClick.AddListener(OnClickButton);
                break;
            case SlotType.Craft:
                button.onClick.AddListener(OnClickCraftButton);
                break;
        }
        outline.enabled = equipped;
    }

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quatityText.text = string.Empty;
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }

    public void OnClickCraftButton()
    {
        craft.SelectItem(index);
    }
}