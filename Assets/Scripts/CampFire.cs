using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    //데미지 양과 빈도 설정
    public int damage;
    public float damageRate;

    List<IDamageIble> things = new List<IDamageIble>();

    void Start()
    {//지정된 빈도로 데미지 주기 시작(시작 시간 0초, 빈도 damageRate)
        InvokeRepeating("DealDamage", 0, damageRate);
    }

    void DealDamage()
    {
        for (int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicalDamge(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageIble damageAble))
        {
            things.Add(damageAble);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamageIble damageAble))
        {
            things.Remove(damageAble);
        }
    }
}
