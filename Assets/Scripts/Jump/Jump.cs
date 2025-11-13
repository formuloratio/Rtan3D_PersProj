using System.Diagnostics;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Header("JumpPower")]
    public float jumpPower;

    private void OnCollisionEnter(Collision collision)
    {
        UnityEngine.Debug.Log("점프!");
        collision.rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
}
