using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sell_Ob : MonoBehaviour
{
    //선택 화살표
    public GameObject select_Arrow;
    //파는 오브젝트 종류
    public S_itemSO S_itemType;
    //파는 오브젝트의 최대 전시 수
    public int MaxDisplay;
    //파는 오브젝트의 현재 전시된 갯수
    public int item_count=0;

    //장판
    public SOb_zone s_zone;

    //세팅중?
    public bool IsSetting = false;

    //파는 오브젝트의 최대 요구량
    public int maxRquirement=0;
    //파는 오브젝트의 최소 요구량
    public int minRquirement=0;

    //전시된 아이템 리스트
    public List<Transform> waitPos = new();
    //이곳이 목표인 손님 리스트
    public Customer[] targetCustomers;

    //못받은 손님 리스트
    public List<Customer> DelayCustomers;

    //전시된 아이템 리스트
    public List<GameObject> s_items = new();
    //전시될 아이템 위치리스트
    public List<Transform> settingTrs = new();


    private void Awake()
    {
        if(s_zone == null)
        {
            s_zone = GetComponentInChildren<SOb_zone>();
        }
    }

     void Start()
    {
        
    }

    //전시 오브젝트에 손님할당하는 함수 =>잘할당됐으면 true 안됐으면 false
    public bool SetCustomer(Customer customer)
    {
        //손님대기하는곳이 없으면 당연히 x
        if (waitPos.Count <= 0 || targetCustomers.Length != waitPos.Count)
            return false;

        //빈자리가 있나 확인후 할당
        int posnum = CheckWaitPos();
        //빈자리가 있으면
        if (posnum >=0)
        {
            targetCustomers[posnum] = customer;
            customer.SetMove(waitPos[posnum],gameObject.transform);
            customer.orderItem = S_itemType;
            customer.requirements = Random.Range(minRquirement,maxRquirement+1);
            customer.SetBubble_item(S_itemType.icon, customer.requirements);
            customer.ShowBubble();
            return true;
        }
        else
        {        
            //꽉차있으면
            return false;
        }
    }

    //전시 오브젝트에 손님할당하는 함수 =>잘할당됐으면 true 안됐으면 false CheckWaitPos()를 이미 실행했을때
    public bool SetCustomer(Customer customer, int i)
    {
        //손님대기하는곳이 없으면 당연히 x
        if (waitPos.Count <= 0 || targetCustomers.Length != waitPos.Count)
            return false;

        targetCustomers[i] = customer;
        customer.target_ob = this;
        customer.posnum = i;
        customer.S_obMove(waitPos[i], gameObject.transform);
        customer.orderItem = S_itemType;
        customer.requirements = Random.Range(minRquirement, maxRquirement + 1);
        customer.SetBubble_item(S_itemType.icon, customer.requirements);
        customer.ShowBubble();
        return true;
    }

    //빈자리가 있나 확인
    public int CheckWaitPos()
    {
        for (int i = 0; i < waitPos.Count; i++)
        {
            if (targetCustomers[i] == null)
            {
                return i;
            }
        }

        //꽉차있으면
        return -1;
    }

    //기다리는 손님 1번한테 아이템 제출
    public void Re_SettingCustomer()
    {
        if(DelayCustomers.Count >0)
        {
            StartCoroutine(CollectItemCoroutine(DelayCustomers[0]));
        }
    }

    //분배 코루틴 실행용
    public void SetItem(Customer customer)
    {
        StartCoroutine(CollectItemCoroutine(customer));
    }

    //아이템 손님에게 분배하는 코루틴
    private IEnumerator CollectItemCoroutine(Customer customer)
    {
        //요구량 <= 가진량 이거나 매대의 제품이 0이 될때까지 반복
        while (customer.Has_requirements < customer.requirements && s_items.Count > 0)
        {
            GameObject getobs = s_items[s_items.Count - 1];

            //부모값 변경
            getobs.transform.parent = customer.stackTrs;
            customer.GetItemStack(getobs.transform,Vector3.zero ,0.4f,0.1f,1);
            customer.Has_requirements++;
            s_items.RemoveAt(s_items.Count - 1);


            yield return new WaitForSeconds(0.1f);
        }

        // 재고 부족 없이 모든 요구량을 채웠을 때
        if (customer.Has_requirements >= customer.requirements)
        {
            // 손님 계산대로 이동
            DelayCustomers.Remove(customer);
            targetCustomers[customer.posnum] = null;
            customer.SetPerchase(true);
            Re_SettingCustomer();
        }
        else
        {   // 재고가 모자란 상태
            if (!DelayCustomers.Contains(customer))
            { //대기손님으로 지정
                DelayCustomers.Add(customer);
            }
        }
    }

}
