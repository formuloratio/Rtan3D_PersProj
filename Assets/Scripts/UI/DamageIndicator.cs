using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;
    public float flashSpeed;
    private Coroutine coroutine;

    void Start()
    {
        CharacterManger.Instance.Player.condition.onTakeDamage += Flash;
    }
    // 데미지를 입었을 때 화면이 잠시 빨갛게 깜빡이도록 함
    public void Flash()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        image.enabled = true;
        image.color = new Color(1f, 100f/255f, 100f/255f, 0.3f);
        coroutine = StartCoroutine(FadeAway());
    }
    // 서서히 빨간색이 사라지도록 함
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0f)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 100f/255f, 100f/255f, a);
            yield return null;
        }
        image.enabled = false;
    }
}
