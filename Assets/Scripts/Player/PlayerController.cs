using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask; //플레이어는 감지되지 않도록 따로 설정 필요

    [Header("Look")] //캐릭터 회전
    public Transform cameraContainer;
    //회전 범위 최대 최소 값
    public float minXLook;
    public float maxXLook;
    private float camCurXRot; //inputAction에서 받아온 마우스 델타 값 저장
    public float lookSensitivity; //민감도 조절
    private Vector2 mouseDelta;
    public bool canLook = true; //인벤토리 열었을 때 커서 보이게 컨트롤

    public Action inventrory;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // 마우스 커서 숨기기 (CursorLockMode -> 숨기기 싫으면 None)
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    void Move()
    {   //forward -> 앞뒤, right -> 좌우
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed; // 속도 곱해주기
        dir.y = _rigidbody.velocity.y; // 점프 시에만 위아래로 움직임이 유지되도록

        _rigidbody.velocity = dir; // 방향 값 정해짐
    }

    void CameraLook()
    {
        //마우스 움직임에 따라 카메라 회전 (당장 캐릭터 리소스가 없으니 캐릭터 회전 대신으로)
        camCurXRot += mouseDelta.y * lookSensitivity; //마우스 y축 움직임에 따라 카메라 x축 회전
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); //회전 범위 제한
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0f, 0f); //카메라 회전 적용

        transform.eulerAngles += new Vector3(0f, mouseDelta.x * lookSensitivity, 0f); //캐릭터 y축 회전 적용
    }

    public void OnMove(InputAction.CallbackContext context)
    { // CallbackContext -> 현재 상태 받아올 수 있음
        if (context.phase == InputActionPhase.Performed)
        { //InputActionPhase.Performed 눌리고 내부 로직이 실행 됐을 때, InputActionPhase.Canceled 취소됐을 때
          //InputActionPhase.Started 키가 눌렸을 때
            curMovementInput = context.ReadValue<Vector2>(); // ReadValue -> 값을 읽어오겠다
        }
        else if (context.phase == InputActionPhase.Canceled) // 키가 떨어졌을 때
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>(); //마우스는 값만 읽어오면 됨 -> 상태 변경이 없으니
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        //플레이어의 바닥에 닿아있는지 확인
        Ray[] rays = new Ray[4] //책상다리 4개
        {   //          시작점                                         그라운드 미인식 예방 ,    방향
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        // 각 레이마다 바닥에 닿아있는지 확인
        //foreach (Ray ray in rays)
        //{
        //    if (Physics.Raycast(ray, 0.1f, groundLayerMask))
        //    {
        //        return true;
        //    }
        //}

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventrory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked; //인벤토리 안 열려져 있으면
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
