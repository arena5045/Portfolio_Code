using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler

    {
    //뒷배경
    public RectTransform joystickBG;
    //조이스틱 핸들
    public RectTransform joystickHandle;
    //해당 캔버스
    public Canvas canvas;

    public Vector2 InputDir { get; private set; }
    private bool isDragging = false;

    void Start()
    { 
        // 시작 시 안보이게
        joystickBG.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 2D 좌표 변수 생성
        Vector2 localPoint;
        //스크린좌표를 캔버스 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out localPoint
        );
        // 배경을 터치한 위치로 이동
        joystickBG.anchoredPosition = localPoint;
        // 조이스틱 핸들 중앙으로 초기화 
        joystickHandle.anchoredPosition = Vector2.zero;
        // 입력 방향 초기화
        InputDir = Vector2.zero;
        // UI 활성화
        joystickBG.gameObject.SetActive(true);
        // 드래그 상태 ON
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 현재 터치 위치를 배경 기준 로컬 좌표로 변환
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBG,
            eventData.position,
            canvas.worldCamera,
            out localPoint
        );

        // 최대 반지름 내에서 위치 제한
        Vector2 clamped = Vector2.ClampMagnitude(localPoint, joystickBG.sizeDelta.x * 0.5f);

        // 조이스틱 핸들 이동
        joystickHandle.anchoredPosition = clamped;

        // 정규화된 방향값 저장
        InputDir = clamped.normalized;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 조이스틱 핸들 초기화
        joystickHandle.anchoredPosition = Vector2.zero;

        // 입력 방향 초기화
        InputDir = Vector2.zero;

        // UI 비활성화
        joystickBG.gameObject.SetActive(false);

        // 드래그 상태 OFF
        isDragging = false;
    }


    //혹시 나중에 조작 중 값 판별할 때 용
    public bool IsDragging()
    {
        // 드래그 중 여부 반환
        return isDragging;
    }
}

