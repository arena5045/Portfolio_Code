using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [Header("����� ��ǥ")]
    //�ΰ��� ����� ��ġ
    public Transform waitline_P;
    //�԰��� �������ġ
    public Transform waitline_E;

    [Header("�� ��")]
    public bool isPurchase = false;
    [Header("�� ���̴� ��")]
    public MoneyZone moneyZone;

    public float waitspace = 1f;
    [Header("���尣��")]
    //���尣��
    public float takeInterval = 0.1f;
    [Header("���Ǵ� ������Ʈ")]
    public GameObject paperBag;
    [Header("��ġ��")]
    public Transform spawnPoint;
    public Transform disPoint;
    [Header("�ΰ��� ��� �մ�")]
    public List<Customer> waitlineP_Customers;
    public Customer P_Customer;
    [Header("�԰��� ���մ�")]
    public List<Customer> waitlineE_Customers;
    public Customer E_Customer;

    private Coroutine perchaceRoutine;

    //�������
    public void StartPacking()
    {
        if(P_Customer != null)
        StartCoroutine(Packing(P_Customer));
    }

    //���� �ڷ�ƾ
    IEnumerator Packing(Customer customer)
    {
        GameObject bag = Instantiate(paperBag, spawnPoint.position, spawnPoint.rotation);
        //����� ������ ���Ҿ� �հ�
        int bill = 0;

        //�մ��� ������ ������ 0�� �ɶ�����
        while (customer.stackobs.Count != 0)
        {
            //���� �������� ��� ����
                GameObject getobs = customer.stackobs[customer.stackobs.Count - 1];

                //�θ� ����
                getobs.transform.parent = bag.transform;
                //�ڽ��� �Ѱ��ֱ�
                customer.SetItem(getobs.transform,disPoint, takeInterval, 2f, true);
                //���õ� ����Ʈ�� �߰�
                bill += getobs.GetComponent<S_item>().price;
                //����ִ� ����Ʈ ���� ����
                customer.stackobs.Remove(getobs);


            yield return new WaitForSeconds(takeInterval);
        }

        //���� ����
        bag.GetComponentInChildren<Animator>().SetTrigger("Close");
        yield return new WaitForSeconds(0.75f);
        //������ �ֱ�
        bag.transform.SetParent(customer.stackTrs);
        customer.GetItemStack(bag.transform ,new Vector3 (0,90,0),0, takeInterval, 2f);
        yield return new WaitForSeconds(0.3f);

        //���⼭ �����մ�����
        Debug.Log("�����մ� �̲ٿ�");
        customer.ExitMove();
        //���� �̸���
        customer.SetVFX(0);
        P_Customer = null;
        waitlineP_Customers.Remove(customer);
        yield return new WaitForSeconds(0.75f);

        //����� �մԵ����� �ٽ� �ټ����
        for(int i=0; i < waitlineP_Customers.Count; i++)
        {
            waitlineP_Customers[i].SetPerchase(false);
        }

        //���� �Ѿ� �����̿� �߰�
        moneyZone.PlusMoney(bill);
        //�Ҹ�����
        SoundManager.Instance.PlayMoneySound();
        Debug.Log("��곡!!!" + bill + "�� �߰�");
        isPurchase = false;
    }


}
