using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPositionl;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private PlayerController controller;
    private PlayerCondition condition;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    int curEquipIndex;

    void Start()
    {
        controller = CharacterManger.Instance.Player.controller;
        condition = CharacterManger.Instance.Player.condition;
        dropPositionl = CharacterManger.Instance.Player.dropPosition;

        controller.inventrory += Toggle;
        CharacterManger.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount]; //슬롯판넬 하위 오브젝트 개수만큼 슬롯배열 생성

        for (int i = 0; i < slots.Length; i++) //각각의 슬롯 초기화
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        ClearSelectedItemWindow();
    }

    void Update()
    {
        
    }

    void ClearSelectedItemWindow() //각각의 선택된 아이템 창 초기화
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle() //키 눌렀을 때 인벤토리 창 켜지도록
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

    public bool IsOpen() //인벤토리 창 열려있는지 확인
    {
        return inventoryWindow.activeInHierarchy; //하이라키 상에서 활성화 되어있는지 반환
    }

    void AddItem()
    {
        ItemData data = CharacterManger.Instance.Player.itemData;

        //아이템이 중복 가능한지 canStack
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManger.Instance.Player.itemData = null;
            }
        }

        // 비어있는 슬롯 가져온다
        ItemSlot emptySlot = GetEmptySlot();

        //있다면
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManger.Instance.Player.itemData = null;
            return;
        }

        //없다면(슬롯 꽉 차있는 경우) 버려야 함
        ThrowItem(data);

        CharacterManger.Instance.Player.itemData = null; //데이터 초기화
    }

    void UpdateUI()
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
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
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

    void ThrowItem(ItemData data)
    {
        //360도 랜덤 회전
        Instantiate(data.dropPrefab, dropPositionl.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;
        
        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        //for (int i = 0; i < selectedItem.consumbales.Length; i++)
        //{
        //    selectedStatName.text += selectedItem.consumbales[i].type.ToString() + "\n";
        //    selectedStatValue.text += selectedItem.consumbales[i].value.ToString() + "\n";
        //}
        for (int i = 0; i < selectedItem.consumbales.Length; i++)
        {
            ItemDataConsumbale consumable = selectedItem.consumbales[i];
            selectedStatName.text += consumable.type.ToString() + "\n";

            if (consumable.type == ConsumableType.SpeedUP)
            {
                // 버프의 경우, 증가량과 지속 시간을 함께 표시
                selectedStatValue.text += $"+{consumable.value} ({consumable.duration}s)\n";
            }
            else
            {
                // 체력, 배고픔의 경우, 회복량만 표시
                selectedStatValue.text += $"+{consumable.value}\n";
            }
        }

        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    //사용하기
    public void OnUseButton()
    {
        if (selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumbales.Length; i++)
            {
                switch(selectedItem.consumbales[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.consumbales[i].value);
                        break;
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.consumbales[i].value);
                        break;
                    case ConsumableType.SpeedUP:
                        // value = 속도 증가량, duration = 지속 시간
                        controller.ApplySpeedBuff(
                            selectedItem.consumbales[i].value,
                            selectedItem.consumbales[i].duration
                        );
                        break;
                }
            }
            RemoveSelectedItem(); //정보 제거
        }
    }

    //버리기
    public void OnDropButton()
    {
        ThrowItem(selectedItem); //눈앞에서 아이템이 던져짐
        RemoveSelectedItem(); //정보 제거
    }

    void RemoveSelectedItem() //버려졌다면 UI 정보 변경해줘야함
    {
        slots[selectedItemIndex].quantity--;
        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
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
        CharacterManger.Instance.Player.equip.EquipNew(selectedItem);
        UpdateUI();

        SelectItem(selectedItemIndex);
    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManger.Instance.Player.equip.UnEquip();
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
