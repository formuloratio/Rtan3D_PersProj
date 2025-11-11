using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue; //바의 현재 상태 값
    public float startValue; // 시작 값
    public float maxValue; // 최대 값
    public float passiveValue; // 시간이 지남에 따라 변화하는 값
    public Image uiBar;


    void Start()
    {
        curValue = startValue; // 저장된 값을 불러와도 됨
    }

    void Update()
    {
        //ui업데이트
        uiBar.fillAmount = GetPercentage();
    }

    float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue); // 최대값을 안 넘게 예외처리
    }

    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0); // 최소값을 안 넘게 예외처리
    }
}
