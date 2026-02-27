using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public CanvasGroup canvasGroup; //캔버스 그룹
    public RectTransform rect; //HUD RectTransform

    public Transform target; //따라다닐 타겟
    public Vector3 offset = new Vector3(0, 2.0f, 0); //HUD 오프셋

    private Camera cam; //카메라

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main; //메인 카메라 저장
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        //타겟이 없다면
        if(target == null)
        {
            canvasGroup.alpha = 0; //캔버스 그룹의 알파값 0으로 변경
            return;
        }

        //월드 좌표를 스크린 좌표로 변환
        Vector3 worldPos = target.position + offset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        //UI가 0보다 크거나 같고 스크린의 가로길이와 세로길이보다는 작을 때 보이게 함
        bool isVisible = screenPos.z > 0 &&
            screenPos.x >= 0 && screenPos.x <= Screen.width &&
            screenPos.y >= 0 && screenPos.y <= Screen.height;

        if (isVisible)
        {
            rect.position = screenPos; //HUD의 위치를 screenPos로 대입한다.
            canvasGroup.alpha = 1;//HUD가 눈에 보이게 알파값을 1로 변경
        }
        else
        {
            canvasGroup.alpha = 0;
        }
    }
}
