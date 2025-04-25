using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Player : MonoBehaviour
{
    //�÷��̾� ������Ʈ
    public GameObject Model;
    //�÷��̾� �ִϸ�����
    public Animator PlayerAnimator;
    //���� ����� ��ǥ
    public Transform stackTrs;

    //�÷��̾ �������ִ� �ƽ�����
    public S_itemSO curitem = null;

    //�÷��̾ �������ִ� �ƽ�����
    public int maxStack = 8;
    //�÷��̾� ���� ����ִ� ����
    private int curStack = 0;
    //��á��?
    public bool isMax = false;
    //��á���� ����� ���彺���̽� ĵ����
    public GameObject maxCanvas ;

    //���ݵ���ִ� �͵�
    public List<GameObject> stackobs = new List<GameObject>();

    public int CurStack//���� ����ִ� ���ü�
    {
        get => curStack;
        set
        {
            curStack = Mathf.Clamp(value, 0, maxStack);
            Debug.Log($"���� ����: {curStack}");

            isMax = (curStack >= maxStack);
            PlayerAnimator.SetBool("Stack", curStack > 0);
            maxCanvas.SetActive(isMax);
        }
    }

    public enum PlayerState
    { wait, walk }

    public PlayerState playerState = PlayerState.wait;

    private void Awake()
    {
        //�ִϸ����Ͱ� ������ �޾ƿ´�
        if (PlayerAnimator == null)
        {
            PlayerAnimator = GetComponentInChildren<Animator>();
        }
    }

    //������ ���
    public void GetItem(Transform item, float size, float duration, float arcHeight)
    {
        stackobs.Add(item.gameObject);
        StartCoroutine(GetItemWithArc(item, size, duration, arcHeight));
    }

    public void SetItem(Transform item, Transform targetPos, float duration, float arcHeight)
    {
        stackobs.Remove(item.gameObject);
        StartCoroutine(SetItemWithArc(item, targetPos, duration, arcHeight));
    }
    //������ ������
    public void SetItemSO(Sell_Ob sell_ob, Transform item, Transform targetPos, float duration, float arcHeight)
    {
        stackobs.Remove(item.gameObject);
        StartCoroutine(SetItemWithArc(item, targetPos, duration, arcHeight, sell_ob));
    }

    //�÷��̾ ������ ��� = �ޱ�
    public IEnumerator GetItemWithArc(Transform item, float size, float duration, float arcHeight)
    {
        Vector3 start = item.position;
        Quaternion startRot = item.rotation;
        int stack = CurStack;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            // ���� ��ġ ����
            Vector3 flatPos = Vector3.Lerp(start, stackTrs.position + new Vector3(0, size * stack, 0), t);

            // ������ ���� ��� (�߰��� ����, ����/���� ����)
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight;

            // ���� ��ġ = ���� ��ġ + ���� �ø� ��ŭ
            item.position = flatPos + Vector3.up * height;

            // ȸ�� �ε巴�� ���󰡱�
            item.rotation = Quaternion.Slerp(startRot, stackTrs.transform.rotation,t);


            time += Time.deltaTime;
            yield return null;
        }
        //�Ҹ�����
        SoundManager.Instance.PlayGetSound();
        item.position = stackTrs.position + new Vector3(0, size * stack, 0);
    }

   
    //�÷��̾ ������ ������ = ����
    public IEnumerator SetItemWithArc(Transform item, Transform targetPos, float duration, float arcHeight, Sell_Ob sell_Ob = null)
    {
        Vector3 start = item.position;
        Quaternion startRot = item.rotation;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            // ���� ��ġ ����
            Vector3 flatPos = Vector3.Lerp(start, targetPos.position, t);

            // ������ ���� ��� (�߰��� ����, ����/���� ����)
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight;

            // ���� ��ġ = ���� ��ġ + ���� �ø� ��ŭ
            item.position = flatPos + Vector3.up * height;

            // ȸ�� �ε巴�� ���󰡱�
            item.rotation = Quaternion.Slerp(startRot, targetPos.transform.rotation, t);


            time += Time.deltaTime;
            yield return null;
        }
        //�Ҹ�����
        SoundManager.Instance.PlaySetSound();
        item.position = targetPos.position;

        //���⿡ add�߰��ϸ�ɰŰ���
        if (sell_Ob != null)
        {
            sell_Ob.s_items.Add(item.gameObject);
        }

    }
}
