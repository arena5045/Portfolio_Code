using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sell_Ob : MonoBehaviour
{
    //���� ȭ��ǥ
    public GameObject select_Arrow;
    //�Ĵ� ������Ʈ ����
    public S_itemSO S_itemType;
    //�Ĵ� ������Ʈ�� �ִ� ���� ��
    public int MaxDisplay;
    //�Ĵ� ������Ʈ�� ���� ���õ� ����
    public int item_count=0;

    //����
    public SOb_zone s_zone;

    //������?
    public bool IsSetting = false;

    //�Ĵ� ������Ʈ�� �ִ� �䱸��
    public int maxRquirement=0;
    //�Ĵ� ������Ʈ�� �ּ� �䱸��
    public int minRquirement=0;

    //���õ� ������ ����Ʈ
    public List<Transform> waitPos = new();
    //�̰��� ��ǥ�� �մ� ����Ʈ
    public Customer[] targetCustomers;

    //������ �մ� ����Ʈ
    public List<Customer> DelayCustomers;

    //���õ� ������ ����Ʈ
    public List<GameObject> s_items = new();
    //���õ� ������ ��ġ����Ʈ
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

    //���� ������Ʈ�� �մ��Ҵ��ϴ� �Լ� =>���Ҵ������ true �ȵ����� false
    public bool SetCustomer(Customer customer)
    {
        //�մԴ���ϴ°��� ������ �翬�� x
        if (waitPos.Count <= 0 || targetCustomers.Length != waitPos.Count)
            return false;

        //���ڸ��� �ֳ� Ȯ���� �Ҵ�
        int posnum = CheckWaitPos();
        //���ڸ��� ������
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
            //����������
            return false;
        }
    }

    //���� ������Ʈ�� �մ��Ҵ��ϴ� �Լ� =>���Ҵ������ true �ȵ����� false CheckWaitPos()�� �̹� ����������
    public bool SetCustomer(Customer customer, int i)
    {
        //�մԴ���ϴ°��� ������ �翬�� x
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

    //���ڸ��� �ֳ� Ȯ��
    public int CheckWaitPos()
    {
        for (int i = 0; i < waitPos.Count; i++)
        {
            if (targetCustomers[i] == null)
            {
                return i;
            }
        }

        //����������
        return -1;
    }

    //��ٸ��� �մ� 1������ ������ ����
    public void Re_SettingCustomer()
    {
        if(DelayCustomers.Count >0)
        {
            StartCoroutine(CollectItemCoroutine(DelayCustomers[0]));
        }
    }

    //�й� �ڷ�ƾ �����
    public void SetItem(Customer customer)
    {
        StartCoroutine(CollectItemCoroutine(customer));
    }

    //������ �մԿ��� �й��ϴ� �ڷ�ƾ
    private IEnumerator CollectItemCoroutine(Customer customer)
    {
        //�䱸�� <= ������ �̰ų� �Ŵ��� ��ǰ�� 0�� �ɶ����� �ݺ�
        while (customer.Has_requirements < customer.requirements && s_items.Count > 0)
        {
            GameObject getobs = s_items[s_items.Count - 1];

            //�θ� ����
            getobs.transform.parent = customer.stackTrs;
            customer.GetItemStack(getobs.transform,Vector3.zero ,0.4f,0.1f,1);
            customer.Has_requirements++;
            s_items.RemoveAt(s_items.Count - 1);


            yield return new WaitForSeconds(0.1f);
        }

        // ��� ���� ���� ��� �䱸���� ä���� ��
        if (customer.Has_requirements >= customer.requirements)
        {
            // �մ� ����� �̵�
            DelayCustomers.Remove(customer);
            targetCustomers[customer.posnum] = null;
            customer.SetPerchase(true);
            Re_SettingCustomer();
        }
        else
        {   // ��� ���ڶ� ����
            if (!DelayCustomers.Contains(customer))
            { //���մ����� ����
                DelayCustomers.Add(customer);
            }
        }
    }

}
