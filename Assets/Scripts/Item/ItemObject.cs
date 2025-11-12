using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt(); //화면에 띄워줄 프롬프트 관련 함수
    public void OnInteract(); //상호작용했을 때 어떤 효과를 줄지
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManger.Instance.Player.itemData = data;
        CharacterManger.Instance.Player.addItem?.Invoke(); //구독이 되어있으면 실행
        Destroy(gameObject);
    }
}
