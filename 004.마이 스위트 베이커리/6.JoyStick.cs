using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class JoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler

    {
    //�޹��
    public RectTransform joystickBG;
    //���̽�ƽ �ڵ�
    public RectTransform joystickHandle;
    //�ش� ĵ����
    public Canvas canvas;

    public Vector2 InputDir { get; private set; }
    private bool isDragging = false;

    void Start()
    { 
        // ���� �� �Ⱥ��̰�
        joystickBG.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 2D ��ǥ ���� ����
        Vector2 localPoint;
        //��ũ����ǥ�� ĵ���� ��ǥ�� ��ȯ
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out localPoint
        );
        // ����� ��ġ�� ��ġ�� �̵�
        joystickBG.anchoredPosition = localPoint;
        // ���̽�ƽ �ڵ� �߾����� �ʱ�ȭ 
        joystickHandle.anchoredPosition = Vector2.zero;
        // �Է� ���� �ʱ�ȭ
        InputDir = Vector2.zero;
        // UI Ȱ��ȭ
        joystickBG.gameObject.SetActive(true);
        // �巡�� ���� ON
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ���� ��ġ ��ġ�� ��� ���� ���� ��ǥ�� ��ȯ
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBG,
            eventData.position,
            canvas.worldCamera,
            out localPoint
        );

        // �ִ� ������ ������ ��ġ ����
        Vector2 clamped = Vector2.ClampMagnitude(localPoint, joystickBG.sizeDelta.x * 0.5f);

        // ���̽�ƽ �ڵ� �̵�
        joystickHandle.anchoredPosition = clamped;

        // ����ȭ�� ���Ⱚ ����
        InputDir = clamped.normalized;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // ���̽�ƽ �ڵ� �ʱ�ȭ
        joystickHandle.anchoredPosition = Vector2.zero;

        // �Է� ���� �ʱ�ȭ
        InputDir = Vector2.zero;

        // UI ��Ȱ��ȭ
        joystickBG.gameObject.SetActive(false);

        // �巡�� ���� OFF
        isDragging = false;
    }


    //Ȥ�� ���߿� ���� �� �� �Ǻ��� �� ��
    public bool IsDragging()
    {
        // �巡�� �� ���� ��ȯ
        return isDragging;
    }
}

