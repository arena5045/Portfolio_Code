using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class Customer : MonoBehaviour
{
    //�⺻���� �մ� ���� ��ũ��Ʈ

    public enum OrderState // �԰��� �ΰ���
    { none, packaging, inStore }
    public float inStoreProbability = 0.2f; //Ȯ��
    public enum CustomerState //�մ� ���¸ӽſ�
    { wait, walk, purchase, packaging, eat }
    //����ִ� ������ ������
    [HideInInspector]
    public S_itemSO orderItem;

    //���� ��ȣ�ۿ����� ������
    [HideInInspector]
    public Sell_Ob target_ob;

    //���� �����忡�� ������ ��ġ ������ -1
    [HideInInspector]
    public int posnum = -1;

    //����� ��ȣ ������ -1
    [HideInInspector]
    public int waitnum = -1;

    //����ǥ�� ����� ��ġ
    public Transform vfxTrs;

    public OrderState orderState = OrderState.none;

    private CustomerState customerState = CustomerState.wait;
    //�䱸��
    public int requirements = 0;
    //���� ������ �ִ� ��
    private int has_requirements = 0;

    public int Has_requirements // �������ִ� �� ������Ƽ
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

    //����ִ� �����۵�
    public List<GameObject> stackobs = new List<GameObject>();


    // ȸ������ �����ų ��
    public GameObject model;
    // ��ǳ��
    public WishBubble wishBubble;

    //�ִϸ�����
    [HideInInspector]
    public Animator customerAnimator;

    public float customerSpeed = 1f;

    private void Awake()
    {
        //�ִϸ����Ͱ� ������ �޾ƿ´�
        if(customerAnimator == null)
        {
            customerAnimator = GetComponentInChildren<Animator>();
        }

        if (wishBubble.gameObject.activeSelf)
        {
            wishBubble.gameObject.SetActive(false);
        }

        float rand = Random.value; // 0.0f ~ 1.0f ���� ������

        if (rand <= inStoreProbability) // 20% Ȯ��
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

    //����ǥ��
    public void SetVFX(int num)
    {
        GameObject vfx = Instantiate(CustomerManager.Instance.customerVFX[num], vfxTrs.transform);
    }

    //�մ� �̵���ų��
    public void SetMove(Transform targetPos,Transform Lookpos)
    {
        ChangeState(CustomerState.walk);
        StartCoroutine(MoveWithSpeed(gameObject.transform, targetPos.position, customerSpeed, Lookpos));
    }

    //�մ� �����̵���Ű�� ��
    public void SetMove(Transform targetPos)
    {
        ChangeState(CustomerState.walk);
        StartCoroutine(MoveWithSpeed(gameObject.transform, targetPos.position, customerSpeed, false));
    }

    //�մ��� ī���ͷ� �̵���ų ��
    public void SetPerchase(bool isfirst)
    {
        StartCoroutine(PerchaseMove(isfirst));
        CustomerManager.Instance.customers.Remove(this);
    }

    //�մ��� �Ŵ�� �̵���ų ��
    public void S_obMove(Transform targetPos, Transform Lookpos)
    {
        ChangeState(CustomerState.walk);
        StartCoroutine(MoveS_ob(gameObject.transform, targetPos.position, customerSpeed, Lookpos));
    }

    //�մ� ����
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

    //����ǳ�� ����
    public void SetBubble_item(Sprite icon, int wishCount)
    {
        wishBubble.wishItem.sprite = icon;
        wishBubble.wishText.text = wishCount.ToString();

        wishBubble.wishGoal.enabled = false;
        wishBubble.wishItem.enabled = true;
        wishBubble.wishText.enabled = true;
    }

    //����ǳ�� ��ǥ�̵���
    public void SetBubble_go(Sprite icon)
    {
        wishBubble.wishGoal.sprite = icon;

        wishBubble.wishGoal.enabled = true;
        wishBubble.wishItem.enabled = false;
        wishBubble.wishText.enabled = false;
    }

    //����ǳ�� ����
    public void ShowBubble()
    {
        if (!wishBubble.gameObject.activeSelf)
        {
            wishBubble.gameObject.SetActive(true);
        } 
    }

    //����ǳ�� ����
    public void HideBubble()
    {
        if (wishBubble.gameObject.activeSelf)
        {
            wishBubble.gameObject.SetActive(false);
        }
    }

    //�����Ҵ� �߾����� �̵��ڷ�ƾ
    IEnumerator StartMove()
    {
        ChangeState(CustomerState.walk);
        yield return  StartCoroutine(MoveWithSpeed(gameObject.transform, CustomerManager.Instance.mainTrs.position, customerSpeed,false));
        CustomerManager.Instance.SetCustomerWishItem(this);
    }

    //���� ��ǥ ���������� �̵� �ڷ�ƾ
    IEnumerator StartListMove(List<Transform> targetPos)
    {
        ChangeState(CustomerState.walk);
        yield return StartCoroutine(MoveWithSpeed(gameObject.transform,targetPos, customerSpeed, false));
    }

    //����� �̵�
    IEnumerator PerchaseMove(bool isFirst)
    {


        Counter counter = CustomerManager.Instance.counter;
        float wait_value;
        List<Customer> waitcus = counter.waitlineP_Customers;

        //���µ��� �ټ��°� ����
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

        //���µ��� �ټ��°� ����
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
        Debug.Log($"{gameObject.name}�� ������ �Ÿ� : {distance}");
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
        //��⿭ 1���̸� �ڱ��Ҵ�
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
                    // ī�䰡 �رݵǾ������� ī���
                    if(UnlockManager.Instance.unlockCafe)
                    {
                        UnlockManager.Instance.Cafe.EnterCafe();
                    }
                }
                break;
        }

        ChangeState(CustomerState.wait);
    }

    //�մ� ���� ���� + �ִϸ����� ����
    public void ChangeState(CustomerState newState)
    {
        //���� ���¸� �ǵ�����
        if (customerState == newState) return;

        customerState = newState;

        // ���� ��ȯ �� ������ �۾�
        switch (customerState)
        {
            case CustomerState.wait:
                customerAnimator.SetTrigger("Idle");
                customerAnimator.SetBool("Sit", false);
                Debug.Log("��� ���·� ��ȯ");
                break;
            case CustomerState.walk:
                customerAnimator.SetTrigger("Walk");
                customerAnimator.SetBool("Sit", false);
                Debug.Log("�ȱ� ����");
                break;
            case CustomerState.eat:
                customerAnimator.SetBool("Sit",true);
                Debug.Log("�Ա� ����");
                break;
        }
    }

    //��ǥ�������� n�ʵ��� �̵�
    IEnumerator MoveOverTime(Transform a, Vector3 targetPos, float duration)
    {
        Vector3 start = a.position;
        Quaternion targetRot = Quaternion.LookRotation((targetPos - start).normalized);
        a.rotation = targetRot;

        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            // ��ġ �̵�
            a.position = Vector3.Lerp(start, targetPos, t);

            time += Time.deltaTime;
            yield return null;
        }

        a.position = targetPos;

        ChangeState(CustomerState.wait);
    }

    //�ܼ��� �̵� //������ �ٸ��� �ӵ���
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

    //�̵��ε� ��ġ�� �迭
    IEnumerator MoveWithSpeed(Transform a, List<Transform> targetPos, float speed, bool iswait)
    {

        for(int i =0; i < targetPos.Count; i++) 
        {
            yield return StartCoroutine(MoveWithSpeed(gameObject.transform, targetPos[i].position, customerSpeed, false));
        }

    }


    //������ �ٶ󺸴� �̵�
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

    //�Ŵ�� �̵��ϴ� �̵� = �̵��� ������ ���𰡸� ��û�Ҷ�
    IEnumerator MoveS_ob(Transform a, Vector3 targetPos, float speed, Transform Lookpos)
    {
        yield return StartCoroutine(MoveWithSpeed( a,  targetPos,  speed,  Lookpos));
        //�̵��� ������ �ϴ� �䱸
        RequestItem();
    }

    //������ ��� ȣ��� �Լ�
    public void GetItemStack(Transform item, Vector3 rotvalue, float size,  float duration, float arcHeight)
    {
        stackobs.Add(item.gameObject);
        StartCoroutine(GetItemWithArc(item, rotvalue,size, duration, arcHeight));
    }

    //�մ��� ������ ���
    public IEnumerator GetItemWithArc(Transform item,Vector3 rotvalue ,float size, float duration, float arcHeight)
    {
        Vector3 start = item.position;
        Quaternion startRot = item.rotation;
        int stack = Has_requirements;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            // ���� ��ġ ����
            Vector3 flatPos = Vector3.Lerp(start, stackTrs.position + new Vector3(0, size * stack, 0), t);

            // ������ ���� ��� 
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight;

            // ���� ��ġ = ���� ��ġ + ���� �ø� ��ŭ
            item.position = flatPos + Vector3.up * height;

            // ȸ�� �ε巴�� ���󰡱�
            Quaternion targetRot = stackTrs.transform.rotation * Quaternion.Euler(rotvalue);
            item.rotation = Quaternion.Slerp(startRot, targetRot, t);


            time += Time.deltaTime;
            yield return null;
        }
        //�Ҹ�����
        SoundManager.Instance.PlayGetSound();
        item.position = stackTrs.position + new Vector3(0, size * stack, 0);
    }

    //�մ��� ������ ������(����������, ������ġ, �ð� ,����, ������ ������ )
    public IEnumerator SetItemWithArc(Transform item, Transform targetPos, float duration, float arcHeight, bool isDelete)
    {
        Vector3 start = item.position;
        Quaternion startRot = item.rotation;

        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            // ���� ��ġ ����
            Vector3 flatPos = Vector3.Lerp(start, targetPos.position, t);

            // ������ ���� ���
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight;

            // ���� ��ġ = ���� ��ġ + ���� �ø� ��ŭ
            item.position = flatPos + Vector3.up * height;

            // ȸ�� �ε巴��
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

    //�����ϸ� ������ �䱸
    public void RequestItem()
    {
        // �������� �ƴϸ�
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
