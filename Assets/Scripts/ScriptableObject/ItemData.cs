using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable, //장착 가능한 아이템
    Consumable, //소비 아이템
    Resource //재료 아이템
}

public enum ConsumableType
{
    Health, // 체력회복
    Hunger, // 배고픔회복
    SpeedUP // 속도버프
}

[Serializable]
public class ItemDataConsumbale
{
    public ConsumableType type;
    public float value; //회복량, 버프량 등의 값
    public float duration; //지속시간 (버프 아이템일 경우)
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack; //여러개 가질 수 있는 아이템인지
    public int maxStackAmount; //최대 몇개까지 쌓을 수 있는지

    [Header("Consumable")]
    public ItemDataConsumbale[] consumbales; //체력과 배고픔 등 여러 효과

    [Header("Equip")]
    public GameObject equipPrefab;

    [Header("EquipBuff")] // Buff 관련 정보 추가
    public bool isBuffItem = false;
    public float speedBuffAmount; // 추가할 이동 속도
    public float jumpBuffAmount; // 추가할 점프 힘
}
