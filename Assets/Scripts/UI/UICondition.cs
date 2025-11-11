using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    void Start()
    {
        //(플레이어컨디션을 불러 온)플레이어 스크립트의 컨디션에 자기 자신을 넣기
        CharacterManger.Instance.Player.condition.uiCondition = this; //즉 내가 플레이컨디션 대체
    }
}
