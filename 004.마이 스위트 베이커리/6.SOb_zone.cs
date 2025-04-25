using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOb_zone : MonoBehaviour
{
    // 연결된 판매가구
    public Sell_Ob s_ob;               
    // 작업 소요시간
    public float takeInterval = 0.2f;
    public bool isdisplaying = false;

    //작업 코루틴들
    private Coroutine collectRoutine;
    private Coroutine disableSetRoutine;
    private Coroutine sizeRoutine;
    //장판 크기조절 밸류
    public float resizeValue;
    private SpriteRenderer sprite_renderer;

    private void Awake()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isdisplaying)
        {
            Debug.Log($"{other.name}");
            collectRoutine = StartCoroutine(CollectItemCoroutine(other.GetComponent<Player>()));
            s_ob.IsSetting = true;

            if (sizeRoutine != null) StopCoroutine(sizeRoutine);
            sizeRoutine = StartCoroutine(ResizeSprite(new Vector2(resizeValue, resizeValue), 0.2f));

            // 예약된 꺼짐 코루틴이 있으면 취소
            if (disableSetRoutine != null)
            {
                StopCoroutine(disableSetRoutine);
                disableSetRoutine = null;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCollecting();

            if (sizeRoutine != null) 
                StopCoroutine(sizeRoutine);

            sizeRoutine = StartCoroutine(ResizeSprite(new Vector2(1f, 1f), 0.2f));
            // 0.3초 뒤 isTrigger 끄는 코루틴 시작
            disableSetRoutine = StartCoroutine(DisableTriggerAfterDelay(0.3f));
        }
    }

    private void StopCollecting()
    {
        
        if (collectRoutine != null)
        {
            StopCoroutine(collectRoutine);
            collectRoutine = null;
        }
        isdisplaying = false;
    }

    //이부분 다시볼것
    private IEnumerator CollectItemCoroutine(Player player)
    {
        isdisplaying = true;

        while (true)
        {
            //  매대가 꽉차지않음 + 들고있는 아이템이 1개이상 + 들고있는게 매대용이랑 같은종류
            bool hasItem = player.CurStack > 0;
            bool notFull = s_ob.s_items.Count < s_ob.MaxDisplay;
            bool canStackSameItem =  player.curitem == s_ob.S_itemType;

            if (hasItem && notFull && canStackSameItem)
            {
                Debug.Log($"플레이어가 아이템을 전시했다!");
                GameObject getobs = player.stackobs[player.stackobs.Count - 1];

                //부모값 변경
                getobs.transform.parent = s_ob.settingTrs[s_ob.s_items.Count];
                //플레이어가 넘겨주기
                player.SetItemSO(s_ob, getobs.transform, s_ob.settingTrs[s_ob.s_items.Count], 0.2f, 1f);

                //플레이어 스택 제거
                player.CurStack--;
            }
            else
            {
                Debug.Log("조건 불충분");
            }
            yield return new WaitForSeconds(takeInterval);

        }
    }
    //매대 나가고 바로끝나게하지말고 딜레이 이후 꺼짐
    IEnumerator DisableTriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        s_ob.IsSetting = false;
        disableSetRoutine = null;

        //재시작 
        s_ob.Re_SettingCustomer();
    }

    //장판 크기조절
    private IEnumerator ResizeSprite(Vector2 targetSize, float duration)
    {
        Vector2 startSize = sprite_renderer.size;
        float time = 0f;

        while (time < duration)
        {
            sprite_renderer.size = Vector2.Lerp(startSize, targetSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        sprite_renderer.size = targetSize;
    }
}
