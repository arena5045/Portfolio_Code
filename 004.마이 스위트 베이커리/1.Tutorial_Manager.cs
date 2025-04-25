using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tutorial_Manager : MonoBehaviour
{
    //�����Ҵ� ���̵� ȭ��ǥ ���� �̱��� Ʃ�丮�� �ٳ����� �����ص� ������?

    public bool isDontDeastroyOnLoad = false;

    //��� ȭ��ǥ
    public GameObject topArow;

    //�ϴ�ȭ��ǥ�� ȭ��ǥ �ٶ󺸴� ��ǥ
    public Vector3 ArrowTarget;
    public GameObject bottomArow;

    //Ʃ�丮�� Ŭ����� ������Ʈ��
    public Sell_Ob sellobs;
    public MoneyZone mzone;
    public MoneyZone mzone2;


    //���ȭ�� ��ġ ����
    public List<Transform> arrowTrs;
    //ī�޶� �̵���ġ
    public List<Transform> camTrans;
    //���� �ܰ�
    private int currentStep = 0;
    private bool stepComplete = false;

    private Action[] stepActions; // �� �ܰ躰 ���� üũ�� �ݹ�
    public static Tutorial_Manager Instance
    {
        get; private set;
    }

    void Awake()
    {
        Instance = this;

        if (isDontDeastroyOnLoad)
        {
            DontDestroyOnLoad(Instance);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //�׼ǵ� �Ҵ� ��
        InitActions();
        //0�ܰ� ���ͽ���
        StartStep(0);
    }

    // Update is called once per frame
    void Update()
    {
        //�ϴ� ȭ��ǥ�� �������� ����Ű��
        bottomArow.transform.LookAt(ArrowTarget);

        // ���� �ܰ��� ���� üũ (�Ϸ���� �ʾҴٸ�)
        if (!stepComplete && currentStep < stepActions.Length)
        {
            stepActions[currentStep]?.Invoke();
        }
    }

    //ķ �̵�
    public void CamTutorial(int i,float startDelay)
    {
        StartCoroutine(MoveCameraPositionToAndBack((camTrans[i]),1f,1f, startDelay));
    }

    //�䱸���Ǿ׼ǵ�
    void InitActions()
    {
        stepActions = new Action[]
        {
            Step_1_GetItem,
            Step_2_SetItem,
            Step_3_SellItem,
            Step_4_GetMoney,
            Step_5_UseMoney,
            Step_6_EnterCafe,
            Step_7_CleanCafe,
            Step_8_GetMoney2,
            Step_9_Final
        };
    }
    //Ʃ�丮�� �ܰ躰 ���� -> ȭ��ǥ ��ġ ������ ���
    void StartStep(int step)
    {
        Debug.Log($"Ʃ�� {step + 1} ����");
        stepComplete = false;
        //��� ȭ��ǥ ��ǥ ������
        topArow.transform.position = arrowTrs[step].position;
        topArow.GetComponent<GuideArrow>().Refresh();
        //�ϴ� ȭ��ǥ ��ǥ ������
        float save_y = bottomArow.transform.position.y;
        ArrowTarget = new Vector3(arrowTrs[step].transform.position.x, save_y, arrowTrs[step].transform.position.z);
    }

    //�ܰ� Ŭ����
    public void CompleteStep()
    {
        stepComplete = true;
        currentStep++;
        //�������� �غ�
        if (currentStep < arrowTrs.Count)
        {
            StartStep(currentStep);
        }
        else
        {
            //�ٳ����� ȭ��ǥ ��Ȱ��ȭ
            topArow.SetActive(false);
            bottomArow.SetActive(false);
        }


    }

    //ī�޶� ��ġ����
    public IEnumerator MoveCameraPositionToAndBack(Transform target, float moveTime , float holdTime , float startDelay)
    {
        yield return new WaitForSeconds(startDelay);

        GameManager.Instance.Player_move.canMove = false;

        Transform cam = Camera.main.transform;
        Vector3 originalPos =  cam.position;
        Vector3 targetPos = target.position;

        float t = 0f;

        // ���� ��ġ����  ��ǥ ��ġ��
        while (t < moveTime)
        {
            float percent = t / moveTime;
            // Ease-In ���� �����ϴ� ��������
            float eased = Mathf.Pow(percent, 2f);
            cam.position = Vector3.Lerp(originalPos, targetPos, eased);
            t += Time.deltaTime;
            yield return null;
        }

        cam.position = targetPos; // ������ ��ǥ ����

        yield return new WaitForSeconds(holdTime);

        t = 0f;

        // ��ǥ ��ġ���� ���� ��ġ��
        while (t < moveTime)
        {
            float percent = t / moveTime;
            // ���ƿ� ���� ����
            float eased = Mathf.Pow(percent, 2f); 
            cam.position = Vector3.Lerp(targetPos, originalPos, eased);
            t += Time.deltaTime;
            yield return null;
        }

        cam.position = originalPos; // ����

        GameManager.Instance.Player_move.canMove = true;
    }


    // �� �ܰ躰 ���� ó�� ------------------------------
    void Step_1_GetItem()
    {//1�ܰ� ������ �̻� ����
        if (GameManager.Instance.Player.stackobs.Count>2)
        {
            CompleteStep();
        }
    }

    void Step_2_SetItem()
    {// 2�ܰ� �� 3���̻� ��ġ�ϱ�
        if (sellobs.s_items .Count > 2)
        {
            CompleteStep();
        }
    }

    void Step_3_SellItem()
    {// 3�ܰ� ���뿡 �������
        if (mzone.MoneyCounts>0)
        {
            CompleteStep();
            CamTutorial(0, 3f);
        }
    }

    void Step_4_GetMoney()
    {//4�ܰ� ���ݱ�
        if (GameManager.Instance.Money > 0)
        {
            CompleteStep();
        }
    }

    void Step_5_UseMoney()
    { //5�ܰ� ī�� �ر�
        if (UnlockManager.Instance.unlockCafe)
        {
            CompleteStep();
            bottomArow.SetActive(false);
            topArow.SetActive(false);
        }
    }
    void Step_6_EnterCafe()
    { //7�ܰ� ī�� ������ ����
        if (UnlockManager.Instance.Cafe.hasTrash)
        {
            CompleteStep();
            bottomArow.SetActive(true);
            topArow.SetActive(true);
        }
    }

    void Step_7_CleanCafe()
    { //7�ܰ� ī�� ġ���
        if (!UnlockManager.Instance.Cafe.hasTrash)
        {
            CompleteStep();
        }
    }

    void Step_8_GetMoney2()
    { //8�ܰ� ���� �ٽ� �б�
        if (mzone2.MoneyCounts == 0)
        {
            CompleteStep();
        }
    }

    void Step_9_Final()
    { //9�ܰ� ��


    }

}
