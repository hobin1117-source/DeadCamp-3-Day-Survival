using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent;

    private PlayerController controller;
    private PlayerCondition condition;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

    public void EquipNew(ItemData data)
    {
        UnEquip();
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();

        // >>> 추가 : 이번에 장착한 장비 안에 Gun 컴포넌트가 있으면 GunController에 등록
        if (curEquip != null)
        {
            Gun gun = curEquip.GetComponent<Gun>();
            if (gun != null)
            {
                GunController gunController = FindObjectOfType<GunController>();
                if (gunController != null)
                {
                    gunController.SetGun(gun);
                }
            }
        }
    }

    public void UnEquip()
    {
        if (curEquip != null)
        {
            // >>> 추가 : 해제하려는 장비가 총이면 GunController에서 제거
            Gun gun = curEquip.GetComponent<Gun>();
            if (gun != null)
            {
                GunController gunController = FindObjectOfType<GunController>();
                if (gunController != null)
                {
                    gunController.ClearGun();
                }
            }

            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook)
        {
            curEquip.OnAttackInput();
        }
    }
}
