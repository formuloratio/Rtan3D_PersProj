using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public Equipment equip;

    public ItemData itemData;
    public Action addItem; //델리게이트 -> 구독이 되어있으면 실행시키도록

    public Transform dropPosition;

    private void Awake()
    {
        CharacterManger.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        equip = GetComponent<Equipment>();
    } // 외부에서 이 스크립트를 통해서 플레이어 정보에 접근 가능
}
