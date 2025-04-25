using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COb_zone : MonoBehaviour
{
    // 연결된 작업가구
    public Create_Ob c_ob;                
    // 작업 소요시간
    public float takeInterval = 0.2f;
    //아이템 가져가는중?
    private bool isCollecting = false;
    //아이템 수집중 코루틴
    private Coroutine collectRoutine;
    //땅 장판 크기 커졌다 작아졌다 하는 코루틴
    private Coroutine sizeRoutine;
    //스케일 몇배로 커질지
    public float resizeValue;
    //스프라이트 렌더러
    private SpriteRenderer sprite_renderer;


    private void Awake()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollecting)
        {
            if (sizeRoutine != null) StopCoroutine(sizeRoutine);
            //플레이어가 닿으면 코루틴 시작 + 장판 커짐
            sizeRoutine = StartCoroutine(ResizeSprite(new Vector2(resizeValue, resizeValue), 0.2f));
            collectRoutine = StartCoroutine(CollectItemCoroutine(other.GetComponent<Player>()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        { //플레이어가 나가면 코루틴 바로종료
            StopCollecting();
            if (sizeRoutine != null)
                StopCoroutine(sizeRoutine);

            //장판 작아짐
            sizeRoutine = StartCoroutine(ResizeSprite(new Vector2(1f, 1f), 0.2f));
        }
    }

    //코루틴 강종
    private void StopCollecting()
    {
        if (collectRoutine != null)
        {
            StopCoroutine(collectRoutine);
            collectRoutine = null;
        }
        isCollecting = false;
    }

    //플레이어가 아이템 수집하는 코루틴
    private IEnumerator CollectItemCoroutine(Player player )
    {
        isCollecting = true;

        while (true)
        {
            // 조건 아이템이 1개이상 + 플레이어가 손이 꽉차지않음 + 들고있는게 없거나 전부 같은종류
            bool hasItem = c_ob.item_list.Count > 0;
            bool notFull = !player.isMax;
            bool canStackSameItem = player.curitem == null || player.curitem == c_ob.S_itemType;

            if (hasItem && notFull && canStackSameItem)
            {
                // 만약 아직 아무 아이템도 안 들고 있으면, 현재 꺼로 세팅
                if (player.curitem == null)
                {
                    player.curitem = c_ob.S_itemType;
                }

                Debug.Log($"플레이어가 빵을 획득했다!  ");
                GameObject getobs = c_ob.item_list[c_ob.item_list.Count - 1];
                Rigidbody rb = getobs.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;

                getobs.transform.parent = player.stackTrs;
                c_ob.item_list.RemoveAt(c_ob.item_list.Count - 1);
                player.GetItem(getobs.transform, c_ob.S_itemType.size,0.2f,1f);
                player.CurStack++;
            }
            else
            {
                Debug.Log("조건 불충분");
            }

            yield return new WaitForSeconds(takeInterval);
        }
    }

    //바닥 스케일 재조정하는 코루틴
    private IEnumerator ResizeSprite(Vector2 targetSize, float duration)
    {
        Vector2 startSize = sprite_renderer.size;
        float time = 0f;

        while (time < duration)
        {
            //부드럽게 증가
            sprite_renderer.size = Vector2.Lerp(startSize, targetSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        sprite_renderer.size = targetSize;
    }

}
