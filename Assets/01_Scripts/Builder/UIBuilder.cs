using UnityEngine;

public class UIBuilder : MonoBehaviour
{
    [Header("UI")]
    public GameObject buildWindow;
    public Transform slotPanel;
    public UIBuildSlot[] slots;

    [Header("Data")]
    public BuildableData[] buildables;       // 설치 가능한 건물 리스트
    public ItemData woodItemData;            // 목재 아이템 (UIInventory에서 사용했던 그거)

    private PlayerController controller;
    private UIInventory inventory;
    private ObjectPlacer objectPlacer;

    private int selectedIndex = -1;

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        inventory = FindObjectOfType<UIInventory>();
        objectPlacer = FindObjectOfType<ObjectPlacer>();

        // PlayerController 쪽에 builder 이벤트를 하나 만들었다고 가정
        controller.builder += Toggle;

        buildWindow.SetActive(false);

        // 슬롯 배열 세팅
        slots = new UIBuildSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<UIBuildSlot>();
            slots[i].index = i;
            slots[i].builder = this;

            if (i < buildables.Length && buildables[i] != null)
            {
                slots[i].data = buildables[i];
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }

        RefreshSlotsInteractable();
    }

    public void Toggle()
    {
        bool open = !buildWindow.activeInHierarchy;
        buildWindow.SetActive(open);

        if (open)
        {
            RefreshSlotsInteractable();
        }
    }

    // 인벤토리의 목재 개수를 기준으로 슬롯 활성화/비활성화
    public void RefreshSlotsInteractable()
    {
        if (inventory == null) return;

        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            if (slot.data == null) continue;

            bool canBuild = inventory.HasItem(woodItemData, slot.data.requiredWood);
            slot.SetInteractable(canBuild);
        }
    }

    public void SelectBuild(int index)
    {
        if (index < 0 || index >= buildables.Length) return;

        var data = buildables[index];
        if (data == null) return;

        // 자원 체크
        if (!inventory.HasItem(woodItemData, data.requiredWood))
        {
            Debug.Log("자원이 부족합니다.");
            RefreshSlotsInteractable();
            return;
        }

        // 여기서 ObjectPlacer에게 현재 프리팹 전달 + 배치 모드 진입
        if (objectPlacer != null)
        {
            //objectPlacer.StartPlacementFromUI(
            //    data.placeablePrefab,
            //    data.previewPrefab
            //);

            // 선택된 것 저장하고, 창 닫기
            selectedIndex = index;
            buildWindow.SetActive(false);
        }
    }
}