using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICraft : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject craftWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemResourceName;
    public TextMeshProUGUI selectedItemResourceValue;
    public GameObject craftButton;
    public Button craftBtn;

    private PlayerController controller;
    private UIInventory inventory;

    private void OnEnable()
    {
        craftBtn.onClick.AddListener(OnCraftButton);
    }

    private void OnDisable()
    {
        craftBtn.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        inventory = FindAnyObjectByType<UIInventory>();

        controller.inventory += Toggle;
        //CharacterManager.Instance.Player.addItem += AddItem;

        craftWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].craft = this;
            //slots[i].Clear();
        }

        ClearSelectedItemWindow();
    }

    void ClearSelectedItemWindow()
    {
        selectedItem = null;

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemResourceName.text = string.Empty;
        selectedItemResourceValue.text = string.Empty;

        craftButton.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            craftWindow.SetActive(false);
        }
        else
        {
            craftWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return craftWindow.activeInHierarchy;
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemResourceName.text = string.Empty;
        selectedItemResourceValue.text = string.Empty;

        for (int i = 0; i < selectedItem.item.crafts.Length; i++)
        {
            selectedItemResourceName.text += selectedItem.item.crafts[i].requiredItem.displayName + "\n";
            selectedItemResourceValue.text += selectedItem.item.crafts[i].value.ToString() + "\n";
        }

        craftButton.SetActive(true);
    }

    public void OnCraftButton()
    {
        if (selectedItem == null || selectedItem.item == null)
            return;

        ItemData itemToCraft = selectedItem.item;

        bool hasAllResources = true;

        // 재료가 충분한지 확인
        for (int i = 0; i < itemToCraft.crafts.Length; i++)
        {
            ItemData resourceItem = itemToCraft.crafts[i].requiredItem;
            int resourceValue = itemToCraft.crafts[i].value;         

            if (resourceItem == null || !inventory.HasItem(resourceItem, resourceValue))
            {
                hasAllResources = false;
                break;
            }
        }

        if (!hasAllResources)
        {
            Debug.Log("제작에 필요한 재료가 부족합니다: " + itemToCraft.displayName);
            return;
        }

        // 재료 소모
        for (int i = 0; i < itemToCraft.crafts.Length; i++)
        {
            ItemData resourceItem = itemToCraft.crafts[i].requiredItem;
            int resourceValue = itemToCraft.crafts[i].value;

            inventory.RemoveItem(resourceItem, resourceValue);
        }

        // 인벤토리에 아이템 추가
        CharacterManager.Instance.Player.itemData = itemToCraft;
        inventory.AddItem();

        UpdateUI();
        ClearSelectedItemWindow();
    }
}
