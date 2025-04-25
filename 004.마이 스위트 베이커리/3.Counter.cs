using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [Header("대기줄 좌표")]
    //싸가는 대기줄 위치
    public Transform waitline_P;
    //먹고가는 대기줄위치
    public Transform waitline_E;

    [Header("불 값")]
    public bool isPurchase = false;
    [Header("돈 쌓이는 곳")]
    public MoneyZone moneyZone;

    public float waitspace = 1f;
    [Header("포장간격")]
    //포장간격
    public float takeInterval = 0.1f;
    [Header("사용되는 오브젝트")]
    public GameObject paperBag;
    [Header("위치값")]
    public Transform spawnPoint;
    public Transform disPoint;
    [Header("싸가는 대기 손님")]
    public List<Customer> waitlineP_Customers;
    public Customer P_Customer;
    [Header("먹고가는 대기손님")]
    public List<Customer> waitlineE_Customers;
    public Customer E_Customer;

    private Coroutine perchaceRoutine;

    //포장시작
    public void StartPacking()
    {
        if(P_Customer != null)
        StartCoroutine(Packing(P_Customer));
    }

    //포장 코루틴
    IEnumerator Packing(Customer customer)
    {
        GameObject bag = Instantiate(paperBag, spawnPoint.position, spawnPoint.rotation);
        //포장된 아이템 지불액 합계
        int bill = 0;

        //손님의 아이템 갯수가 0개 될때까지
        while (customer.stackobs.Count != 0)
        {
            //가장 마지막에 든거 지정
                GameObject getobs = customer.stackobs[customer.stackobs.Count - 1];

                //부모값 변경
                getobs.transform.parent = bag.transform;
                //박스로 넘겨주기
                customer.SetItem(getobs.transform,disPoint, takeInterval, 2f, true);
                //전시된 리스트에 추가
                bill += getobs.GetComponent<S_item>().price;
                //들고있는 리스트 에서 제거
                customer.stackobs.Remove(getobs);


            yield return new WaitForSeconds(takeInterval);
        }

        //상자 포장
        bag.GetComponentInChildren<Animator>().SetTrigger("Close");
        yield return new WaitForSeconds(0.75f);
        //포장지 주기
        bag.transform.SetParent(customer.stackTrs);
        customer.GetItemStack(bag.transform ,new Vector3 (0,90,0),0, takeInterval, 2f);
        yield return new WaitForSeconds(0.3f);

        //여기서 다음손님으로
        Debug.Log("다음손님 이꾸요");
        customer.ExitMove();
        //웃는 이모지
        customer.SetVFX(0);
        P_Customer = null;
        waitlineP_Customers.Remove(customer);
        yield return new WaitForSeconds(0.75f);

        //대기줄 손님들한테 다시 줄세우기
        for(int i=0; i < waitlineP_Customers.Count; i++)
        {
            waitlineP_Customers[i].SetPerchase(false);
        }

        //지불 총액 돈더미에 추가
        moneyZone.PlusMoney(bill);
        //소리실행
        SoundManager.Instance.PlayMoneySound();
        Debug.Log("계산끝!!!" + bill + "원 추가");
        isPurchase = false;
    }


}
