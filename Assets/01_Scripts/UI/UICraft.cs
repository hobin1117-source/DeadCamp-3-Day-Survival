using TMPro;
using UnityEngine;

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

    private PlayerController controller;
    private UIInventory inventory;

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        inventory = FindAnyObjectByType<UIInventory>();

        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        craftWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].craft = this;
            slots[i].Clear();
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

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
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

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmout)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefabs, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
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
            selectedItemResourceName.text += selectedItem.item.cosumables[i].type.ToString() + "\n";
            selectedItemResourceValue.text += selectedItem.item.cosumables[i].value.ToString() + "\n";
        }

        craftButton.SetActive(true);
    }

    public void OnCraftButton()
    {
        // 인벤토리에 재료 있는지 확인
        // 없으면 return
        inventory.AddItem();// 있으면 재료 소모하고 인벤토리에 아이템 추가

        UpdateUI();
    }
}
