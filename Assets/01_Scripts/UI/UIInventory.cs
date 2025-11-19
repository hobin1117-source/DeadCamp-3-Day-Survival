using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton;
    public GameObject craftButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;
    public Button useBtn;
    public Button craftBtn;
    public Button dropBtn;
    public Button equipBtn;
    public Button unEquipBtn;

    private int curEquipIndex;

    private PlayerController controller;
    private PlayerCondition condition;
    private ObjectPlacer objectPlacer;

    private void OnEnable()
    {
        useBtn.onClick.AddListener(OnUseButton);
        craftBtn.onClick.AddListener(OnCraftButton);
        dropBtn.onClick.AddListener(OnDropButton);
        equipBtn.onClick.AddListener(OnEquipButton);
        unEquipBtn.onClick.AddListener(OnUnEquipButton);
    }

    private void OnDisable()
    {
        useBtn.onClick.RemoveAllListeners();
        craftBtn.onClick.RemoveAllListeners();
        dropBtn.onClick.RemoveAllListeners();
        equipBtn.onClick.RemoveAllListeners();
        unEquipBtn.onClick.RemoveAllListeners();
    }

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;
        objectPlacer = CharacterManager.Instance.Player.objectPlacer;

        controller.inventory += Toggle;
        objectPlacer.OnObjectPlaced += RemoveSelctedItem;
        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }

        ClearSelectedItemWindow();
    }

    void ClearSelectedItemWindow()
    {
        selectedItem = null;

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        craftButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
        
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
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

    // Player 스크립트 먼저 수정
    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefabs, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }


    // ItemSlot 스크립트 먼저 수정
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.item.cosumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.cosumables[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.cosumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        craftButton.SetActive(selectedItem.item.type == ItemType.Craft);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !slots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if (selectedItem.item.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.item.cosumables.Length; i++)
            {
                switch (selectedItem.item.cosumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.item.cosumables[i].value); break;
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.item.cosumables[i].value); break;
                }
            }
            RemoveSelctedItem();
        }
    }

    public void OnCraftButton()
    {
        objectPlacer.BuildObject(selectedItem.item);
        controller.inventory?.Invoke();
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);
        RemoveSelctedItem();
    }

    void RemoveSelctedItem()
    {
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0)
        {
            if (slots[selectedItemIndex].equipped)
            {
                UnEquip(selectedItemIndex);
            }

            selectedItem.item = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    public bool HasItem(ItemData item, int quantity)
    {
        int total = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
            {
                total += slots[i].quantity;
            }
        }

        return total >= quantity;
    }

    public void RemoveItem(ItemData item, int quantity)
    {
        // 인벤토리 슬롯을 돌며 아이템을 제거
        for (int i = 0; i < slots.Length && quantity > 0; i++)
        {
            if (slots[i].item == item)
            {
                int amountToRemove = Mathf.Min(slots[i].quantity, quantity);

                slots[i].quantity -= amountToRemove;
                quantity -= amountToRemove;

                // quantity가 0이 되면 슬롯을 비움
                if (slots[i].quantity <= 0)
                {
                    slots[i].item = null;
                    slots[i].Clear();
                }
            }
        }

        UpdateUI();
    }

    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }

        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equip.EquipNew(selectedItem.item);
        UpdateUI();

        SelectItem(selectedItemIndex);
    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        UpdateUI();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }
}