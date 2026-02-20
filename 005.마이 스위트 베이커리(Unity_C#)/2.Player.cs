using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Player : MonoBehaviour
{
    //플레이어 오브젝트
    public GameObject Model;
    //플레이어 애니메이터
    public Animator PlayerAnimator;
    //물건 집어들 좌표
    public Transform stackTrs;

    //플레이어가 가질수있는 맥스스택
    public S_itemSO curitem = null;

    //플레이어가 가질수있는 맥스스택
    public int maxStack = 8;
    //플레이어 현재 들고있는 스택
    private int curStack = 0;
    //꽉찼음?
    public bool isMax = false;
    //꽉찼을때 띄워줄 월드스페이스 캔버스
    public GameObject maxCanvas ;

    //지금들고있는 것들
    public List<GameObject> stackobs = new List<GameObject>();

    public int CurStack//현재 들고있는 스택수
    {
        get => curStack;
        set
        {
            curStack = Mathf.Clamp(value, 0, maxStack);
            Debug.Log($"현재 스택: {curStack}");

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
        //애니메이터가 없으면 받아온다
        if (PlayerAnimator == null)
        {
            PlayerAnimator = GetComponentInChildren<Animator>();
        }
    }

    //아이템 들기
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
    //아이템 던지기
    public void SetItemSO(Sell_Ob sell_ob, Transform item, Transform targetPos, float duration, float arcHeight)
    {
        stackobs.Remove(item.gameObject);
        StartCoroutine(SetItemWithArc(item, targetPos, duration, arcHeight, sell_ob));
    }

    //플레이어가 아이템 얹기 = 받기
    public IEnumerator GetItemWithArc(Transform item, float size, float duration, float arcHeight)
    {
        Vector3 start = item.position;
        Quaternion startRot = item.rotation;
        int stack = CurStack;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            // 수평 위치 보간
            Vector3 flatPos = Vector3.Lerp(start, stackTrs.position + new Vector3(0, size * stack, 0), t);

            // 포물선 높이 계산 (중간에 높고, 시작/끝은 낮음)
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight;

            // 최종 위치 = 수평 위치 + 위로 올린 만큼
            item.position = flatPos + Vector3.up * height;

            // 회전 부드럽게 따라가기
            item.rotation = Quaternion.Slerp(startRot, stackTrs.transform.rotation,t);


            time += Time.deltaTime;
            yield return null;
        }
        //소리실행
        SoundManager.Instance.PlayGetSound();
        item.position = stackTrs.position + new Vector3(0, size * stack, 0);
    }

   
    //플레이어가 아이템 던지기 = 놓기
    public IEnumerator SetItemWithArc(Transform item, Transform targetPos, float duration, float arcHeight, Sell_Ob sell_Ob = null)
    {
        Vector3 start = item.position;
        Quaternion startRot = item.rotation;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            // 수평 위치 보간
            Vector3 flatPos = Vector3.Lerp(start, targetPos.position, t);

            // 포물선 높이 계산 (중간에 높고, 시작/끝은 낮음)
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight;

            // 최종 위치 = 수평 위치 + 위로 올린 만큼
            item.position = flatPos + Vector3.up * height;

            // 회전 부드럽게 따라가기
            item.rotation = Quaternion.Slerp(startRot, targetPos.transform.rotation, t);


            time += Time.deltaTime;
            yield return null;
        }
        //소리실행
        SoundManager.Instance.PlaySetSound();
        item.position = targetPos.position;

        //여기에 add추가하면될거같음
        if (sell_Ob != null)
        {
            sell_Ob.s_items.Add(item.gameObject);
        }

    }
}
