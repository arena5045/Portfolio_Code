using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class Customer : MonoBehaviour
{
    //기본적인 손님 로직 스크립트

    public enum OrderState // 먹고갈지 싸갈지
    { none, packaging, inStore }
    public float inStoreProbability = 0.2f; //확률
    public enum CustomerState //손님 상태머신용
    { wait, walk, purchase, packaging, eat }
    //들고있는 아이템 데이터
    [HideInInspector]
    public S_itemSO orderItem;

    //현재 상호작용중인 진열장
    [HideInInspector]
    public Sell_Ob target_ob;

    //현재 진열장에서 배정된 위치 없으면 -1
    [HideInInspector]
    public int posnum = -1;

    //대기줄 번호 없으면 -1
    [HideInInspector]
    public int waitnum = -1;

    //감정표현 띄워줄 위치
    public Transform vfxTrs;

    public OrderState orderState = OrderState.none;

    private CustomerState customerState = CustomerState.wait;
    //요구량
    public int requirements = 0;
    //현재 가지고 있는 양
    private int has_requirements = 0;

    public int Has_requirements // 가지고있는 양 프로퍼티
    {
        get => has_requirements;
        set
        {
            has_requirements = value;
            customerAnimator.SetBool("Stack", has_requirements > 0);
            wishBubble.Refresh(requirements - has_requirements);
        }
    }

    public Transform stackTrs;

    //들고있는 아이템들
    public List<GameObject> stackobs = new List<GameObject>();


    // 회전값을 적용시킬 모델
    public GameObject model;
    // 말풍선
    public WishBubble wishBubble;

    //애니메이터
    [HideInInspector]
    public Animator customerAnimator;

    public float customerSpeed = 1f;

    private void Awake()
    {
        //애니메이터가 없으면 받아온다
        if(customerAnimator == null)
        {
            customerAnimator = GetComponentInChildren<Animator>();
        }

        if (wishBubble.gameObject.activeSelf)
        {
            wishBubble.gameObject.SetActive(false);
        }

        float rand = Random.value; // 0.0f ~ 1.0f 사이 랜덤값

        if (rand <= inStoreProbability) // 20% 확률
        {
            orderState = OrderState.inStore;
        }
        else
        {
            orderState = OrderState.packaging;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartMove());
    }

    //감정표현
    public void SetVFX(int num)
    {
        GameObject vfx = Instantiate(CustomerManager.Instance.customerVFX[num], vfxTrs.transform);
    }

    //손님 이동실킬때
    public void SetMove(Transform targetPos,Transform Lookpos)
    {
        ChangeState(CustomerState.walk);
        StartCoroutine(MoveWithSpeed(gameObject.transform, targetPos.position, customerSpeed, Lookpos));
    }

    //손님 연속이동시키는 용
    public void SetMove(Transform targetPos)
    {
        ChangeState(CustomerState.walk);
        StartCoroutine(MoveWithSpeed(gameObject.transform, targetPos.position, customerSpeed, false));
    }

    //손님을 카운터로 이동시킬 때
    public void SetPerchase(bool isfirst)
    {
        StartCoroutine(PerchaseMove(isfirst));
        CustomerManager.Instance.customers.Remove(this);
    }

    //손님을 매대로 이동실킬 때
    public void S_obMove(Transform targetPos, Transform Lookpos)
    {
        ChangeState(CustomerState.walk);
        StartCoroutine(MoveS_ob(gameObject.transform, targetPos.position, customerSpeed, Lookpos));
    }

    //손님 퇴장
    public void ExitMove()
    {
        StartCoroutine(ExitMoveRoutine());
    }

    IEnumerator ExitMoveRoutine()
    {
        ChangeState(CustomerState.walk);
        yield return StartCoroutine(StartListMove(CustomerManager.Instance.exitTrs));
        Destroy(gameObject);
    }

    //생각풍선 세팅
    public void SetBubble_item(Sprite icon, int wishCount)
    {
        wishBubble.wishItem.sprite = icon;
        wishBubble.wishText.text = wishCount.ToString();

        wishBubble.wishGoal.enabled = false;
        wishBubble.wishItem.enabled = true;
        wishBubble.wishText.enabled = true;
    }

    //생각풍선 목표이동용
    public void SetBubble_go(Sprite icon)
    {
        wishBubble.wishGoal.sprite = icon;

        wishBubble.wishGoal.enabled = true;
        wishBubble.wishItem.enabled = false;
        wishBubble.wishText.enabled = false;
    }

    //생각풍선 띄우기
    public void ShowBubble()
    {
        if (!wishBubble.gameObject.activeSelf)
        {
            wishBubble.gameObject.SetActive(true);
        } 
    }

    //생각풍선 끄기
    public void HideBubble()
    {
        if (wishBubble.gameObject.activeSelf)
        {
            wishBubble.gameObject.SetActive(false);
        }
    }

    //시작할대 중앙으로 이동코루틴
    IEnumerator StartMove()
    {
        ChangeState(CustomerState.walk);
        yield return  StartCoroutine(MoveWithSpeed(gameObject.transform, CustomerManager.Instance.mainTrs.position, customerSpeed,false));
        CustomerManager.Instance.SetCustomerWishItem(this);
    }

    //여러 좌표 연속적으로 이동 코루틴
    IEnumerator StartListMove(List<Transform> targetPos)
    {
        ChangeState(CustomerState.walk);
        yield return StartCoroutine(MoveWithSpeed(gameObject.transform,targetPos, customerSpeed, false));
    }

    //대기줄 이동
    IEnumerator PerchaseMove(bool isFirst)
    {


        Counter counter = CustomerManager.Instance.counter;
        float wait_value;
        List<Customer> waitcus = counter.waitlineP_Customers;

        //상태따라 줄서는곳 변경
        switch (orderState)
        {
            case OrderState.packaging:
                waitcus = counter.waitlineP_Customers;
                break;
            case OrderState.inStore: 
                waitcus = counter.waitlineE_Customers;
                break;
        }

        if (isFirst)
        {
            ChangeState(CustomerState.walk);
            yield return StartCoroutine(MoveWithSpeed(gameObject.transform, CustomerManager.Instance.mainTrs.position, customerSpeed, false));
            wait_value = counter.waitspace * waitcus.Count;
        } 
        else
        {
            wait_value = waitcus.IndexOf(this);
        }
        Vector3 waitpos = Vector3.zero;

        //상태따라 줄서는곳 변경
        switch (orderState)
        {
            case OrderState.packaging : waitpos = counter.waitline_P.position + new Vector3(0, 0, wait_value); break;
            case OrderState.inStore : waitpos = counter.waitline_E.position + new Vector3(0, 0, wait_value); break;
        }


        if(!waitcus.Contains(this))
        {
            waitcus.Add(this);
        }

        float distance = Vector3.Distance(transform.position, waitpos);
        Debug.Log($"{gameObject.name}의 다음줄 거리 : {distance}");
        if (distance > 0.05f) 
        {
            ChangeState(CustomerState.walk);
            yield return StartCoroutine(MoveWithSpeed(transform, waitpos, customerSpeed, true));
            wishBubble.SetGaol(orderState);
        }
        else
        {
            transform.position = waitpos;
        }

        /*
        //대기열 1번이면 자기할당
        if (counter.waitlineP_Customers[0] == this)
        {
            counter.P_Customer = this;
        }
        */
        switch (orderState)
        {
            case OrderState.packaging:
                if (counter.waitlineP_Customers[0] == this)
                {
                    counter.P_Customer = this;
                }
                break;
            case OrderState.inStore:
                if (counter.waitlineE_Customers[0] == this)
                {
                    counter.E_Customer = this;
                    // 카페가 해금되어있으면 카페로
                    if(UnlockManager.Instance.unlockCafe)
                    {
                        UnlockManager.Instance.Cafe.EnterCafe();
                    }
                }
                break;
        }

        ChangeState(CustomerState.wait);
    }

    //손님 상태 변경 + 애니메이터 관련
    public void ChangeState(CustomerState newState)
    {
        //같은 상태면 되돌리기
        if (customerState == newState) return;

        customerState = newState;

        // 상태 전환 시 실행할 작업
        switch (customerState)
        {
            case CustomerState.wait:
                customerAnimator.SetTrigger("Idle");
                customerAnimator.SetBool("Sit", false);
                Debug.Log("대기 상태로 전환");
                break;
            case CustomerState.walk:
                customerAnimator.SetTrigger("Walk");
                customerAnimator.SetBool("Sit", false);
                Debug.Log("걷기 시작");
                break;
            case CustomerState.eat:
                customerAnimator.SetBool("Sit",true);
                Debug.Log("먹기 시작");
                break;
        }
    }

    //목표지점까지 n초동안 이동
    IEnumerator MoveOverTime(Transform a, Vector3 targetPos, float duration)
    {
        Vector3 start = a.position;
        Quaternion targetRot = Quaternion.LookRotation((targetPos - start).normalized);
        a.rotation = targetRot;

        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            // 위치 이동
            a.position = Vector3.Lerp(start, targetPos, t);

            time += Time.deltaTime;
            yield return null;
        }

        a.position = targetPos;

        ChangeState(CustomerState.wait);
    }

    //단순한 이동 //위에랑 다르게 속도로
    public IEnumerator MoveWithSpeed(Transform a, Vector3 targetPos, float speed , bool iswait)
    {
        Vector3 start = a.position;
        float distance = Vector3.Distance(start, targetPos);
        float duration = distance / speed;

        Quaternion targetRot = Quaternion.LookRotation((targetPos - start).normalized);
        //a.rotation = targetRot;
        model.transform.rotation = targetRot;

        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            a.position = Vector3.Lerp(start, targetPos, t);

            time += Time.deltaTime;
            yield return null;
        }

        a.position = targetPos;

        if(iswait)
        {
            ChangeState(CustomerState.wait);
        }
       
        //transform.LookAt(LookTest);
    }

    //이동인데 위치가 배열
    IEnumerator MoveWithSpeed(Transform a, List<Transform> targetPos, float speed, bool iswait)
    {

        for(int i =0; i < targetPos.Count; i++) 
        {
            yield return StartCoroutine(MoveWithSpeed(gameObject.transform, targetPos[i].position, customerSpeed, false));
        }

    }


    //끝나고 바라보는 이동
    IEnumerator MoveWithSpeed(Transform a, Vector3 targetPos, float speed, Transform Lookpos)
    {
        Vector3 start = a.position;
        float distance = Vector3.Distance(start, targetPos);
        float duration = distance / speed;

        Quaternion targetRot = Quaternion.LookRotation((targetPos - start).normalized);
        //a.rotation = targetRot;
        model.transform.rotation = targetRot;

        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            a.position = Vector3.Lerp(start, targetPos, t);

            time += Time.deltaTime;
            yield return null;
        }

        a.position = targetPos;

        ChangeState(CustomerState.wait);
        model.transform.LookAt(Lookpos);
    }

    //매대로 이동하는 이동 = 이동이 끝나고 무언가를 요청할때
    IEnumerator MoveS_ob(Transform a, Vector3 targetPos, float speed, Transform Lookpos)
    {
        yield return StartCoroutine(MoveWithSpeed( a,  targetPos,  speed,  Lookpos));
        //이동이 끝나면 일단 요구
        RequestItem();
    }

    //아이템 얹기 호출용 함수
    public void GetItemStack(Transform item, Vector3 rotvalue, float size,  float duration, float arcHeight)
    {
        stackobs.Add(item.gameObject);
        StartCoroutine(GetItemWithArc(item, rotvalue,size, duration, arcHeight));
    }

    //손님이 아이템 얹기
    public IEnumerator GetItemWithArc(Transform item,Vector3 rotvalue ,float size, float duration, float arcHeight)
    {
        Vector3 start = item.position;
        Quaternion startRot = item.rotation;
        int stack = Has_requirements;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            // 수평 위치 보간
            Vector3 flatPos = Vector3.Lerp(start, stackTrs.position + new Vector3(0, size * stack, 0), t);

            // 포물선 높이 계산 
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight;

            // 최종 위치 = 수평 위치 + 위로 올린 만큼
            item.position = flatPos + Vector3.up * height;

            // 회전 부드럽게 따라가기
            Quaternion targetRot = stackTrs.transform.rotation * Quaternion.Euler(rotvalue);
            item.rotation = Quaternion.Slerp(startRot, targetRot, t);


            time += Time.deltaTime;
            yield return null;
        }
        //소리실행
        SoundManager.Instance.PlayGetSound();
        item.position = stackTrs.position + new Vector3(0, size * stack, 0);
    }

    //손님이 아이템 던지기(던질아이템, 던질위치, 시간 ,높이, 끝나고 지울지 )
    public IEnumerator SetItemWithArc(Transform item, Transform targetPos, float duration, float arcHeight, bool isDelete)
    {
        Vector3 start = item.position;
        Quaternion startRot = item.rotation;

        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            // 수평 위치 보간
            Vector3 flatPos = Vector3.Lerp(start, targetPos.position, t);

            // 포물선 높이 계산
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight;

            // 최종 위치 = 수평 위치 + 위로 올린 만큼
            item.position = flatPos + Vector3.up * height;

            // 회전 부드럽게
            item.rotation = Quaternion.Slerp(startRot, targetPos.transform.rotation, t);


            time += Time.deltaTime;
            yield return null;
        }

        item.position = targetPos.position;
        if (isDelete) 
        {
        Destroy(item.gameObject);
        }
    }
    public void SetItem(Transform item, Transform targetPos, float duration, float arcHeight,bool isDelete)
    {
        stackobs.Remove(item.gameObject);
        StartCoroutine(SetItemWithArc(item, targetPos, duration, arcHeight,isDelete));
    }

    //도착하면 아이템 요구
    public void RequestItem()
    {
        // 세팅중이 아니면
        if (!target_ob.IsSetting && !target_ob.s_zone.isdisplaying)
        {
            target_ob.SetItem(this);
        }
        else
        {
            target_ob.DelayCustomers.Add(this);
        }
        
    }
}
