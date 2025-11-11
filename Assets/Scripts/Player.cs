using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    private void Awake()
    {
        CharacterManger.Instance.Player = this;
        controller = GetComponent<PlayerController>();
    } // 외부에서 이 스크립트를 통해서 플레이어 정보에 접근 가능
}
