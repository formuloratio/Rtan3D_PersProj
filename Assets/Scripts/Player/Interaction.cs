using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // 초당 검사 횟수
    private float lastCheckTime; // 마지막 검사 시간
    public float maxCheckDistance; // 최대 상호작용 거리
    public LayerMask layerMask; // 상호작용 레이어 마스크

    public GameObject curInteractGameObject; // 현재 상호작용 중인 게임 오브젝트
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText; //나중에 UI분리해서 드래그앤드롭 없이 연결해보기
    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        //매프레임마다 검사하지 않게(checkRate의 시간마다 호출)
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time; //현재시간 넣기

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); //화면 중앙에서 레이 발사
                                                                                                 //위는 오리진이고, 방향도 필요한데 카메라가 찍고 있는 방향이 있기 때문에 ray가 나가는 시작점만 잡아주면 된다.
            RaycastHit hit; // 부딪친 정보를 담을 변수

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else //아무것도 안맞음(빈공간에 쐈을 경우)
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
