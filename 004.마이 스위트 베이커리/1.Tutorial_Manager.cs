using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tutorial_Manager : MonoBehaviour
{
    //시작할대 가이드 화살표 관련 싱글턴 튜토리얼 다끝나면 삭제해도 될지도?

    public bool isDontDeastroyOnLoad = false;

    //상단 화살표
    public GameObject topArow;

    //하단화살표와 화살표 바라보는 좌표
    public Vector3 ArrowTarget;
    public GameObject bottomArow;

    //튜토리얼 클리어용 오브젝트들
    public Sell_Ob sellobs;
    public MoneyZone mzone;
    public MoneyZone mzone2;


    //상단화살 위치 저장
    public List<Transform> arrowTrs;
    //카메라 이동위치
    public List<Transform> camTrans;
    //현재 단계
    private int currentStep = 0;
    private bool stepComplete = false;

    private Action[] stepActions; // 각 단계별 조건 체크용 콜백
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
        //액션들 할당 후
        InitActions();
        //0단계 부터시작
        StartStep(0);
    }

    // Update is called once per frame
    void Update()
    {
        //하단 화살표는 지정방향 가리키게
        bottomArow.transform.LookAt(ArrowTarget);

        // 현재 단계의 조건 체크 (완료되지 않았다면)
        if (!stepComplete && currentStep < stepActions.Length)
        {
            stepActions[currentStep]?.Invoke();
        }
    }

    //캠 이동
    public void CamTutorial(int i,float startDelay)
    {
        StartCoroutine(MoveCameraPositionToAndBack((camTrans[i]),1f,1f, startDelay));
    }

    //요구조건액션들
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
    //튜토리얼 단계별 시작 -> 화살표 위치 재지정 등등
    void StartStep(int step)
    {
        Debug.Log($"튜토 {step + 1} 시작");
        stepComplete = false;
        //상단 화살표 좌표 재지정
        topArow.transform.position = arrowTrs[step].position;
        topArow.GetComponent<GuideArrow>().Refresh();
        //하단 화살표 목표 재지정
        float save_y = bottomArow.transform.position.y;
        ArrowTarget = new Vector3(arrowTrs[step].transform.position.x, save_y, arrowTrs[step].transform.position.z);
    }

    //단계 클리어
    public void CompleteStep()
    {
        stepComplete = true;
        currentStep++;
        //다음스텝 준비
        if (currentStep < arrowTrs.Count)
        {
            StartStep(currentStep);
        }
        else
        {
            //다끝나면 화살표 비활성화
            topArow.SetActive(false);
            bottomArow.SetActive(false);
        }


    }

    //카메라 위치조정
    public IEnumerator MoveCameraPositionToAndBack(Transform target, float moveTime , float holdTime , float startDelay)
    {
        yield return new WaitForSeconds(startDelay);

        GameManager.Instance.Player_move.canMove = false;

        Transform cam = Camera.main.transform;
        Vector3 originalPos =  cam.position;
        Vector3 targetPos = target.position;

        float t = 0f;

        // 원래 위치에서  목표 위치로
        while (t < moveTime)
        {
            float percent = t / moveTime;
            // Ease-In 점점 가속하는 느낌으로
            float eased = Mathf.Pow(percent, 2f);
            cam.position = Vector3.Lerp(originalPos, targetPos, eased);
            t += Time.deltaTime;
            yield return null;
        }

        cam.position = targetPos; // 끝나고 좌표 고정

        yield return new WaitForSeconds(holdTime);

        t = 0f;

        // 목표 위치에서 원래 위치로
        while (t < moveTime)
        {
            float percent = t / moveTime;
            // 돌아올 때도 가속
            float eased = Mathf.Pow(percent, 2f); 
            cam.position = Vector3.Lerp(targetPos, originalPos, eased);
            t += Time.deltaTime;
            yield return null;
        }

        cam.position = originalPos; // 보정

        GameManager.Instance.Player_move.canMove = true;
    }


    // 각 단계별 조건 처리 ------------------------------
    void Step_1_GetItem()
    {//1단계 빵세개 이상 집기
        if (GameManager.Instance.Player.stackobs.Count>2)
        {
            CompleteStep();
        }
    }

    void Step_2_SetItem()
    {// 2단계 빵 3개이상 배치하기
        if (sellobs.s_items .Count > 2)
        {
            CompleteStep();
        }
    }

    void Step_3_SellItem()
    {// 3단계 돈통에 돈만들기
        if (mzone.MoneyCounts>0)
        {
            CompleteStep();
            CamTutorial(0, 3f);
        }
    }

    void Step_4_GetMoney()
    {//4단계 돈줍기
        if (GameManager.Instance.Money > 0)
        {
            CompleteStep();
        }
    }

    void Step_5_UseMoney()
    { //5단계 카페 해금
        if (UnlockManager.Instance.unlockCafe)
        {
            CompleteStep();
            bottomArow.SetActive(false);
            topArow.SetActive(false);
        }
    }
    void Step_6_EnterCafe()
    { //7단계 카페 쓰레기 생성
        if (UnlockManager.Instance.Cafe.hasTrash)
        {
            CompleteStep();
            bottomArow.SetActive(true);
            topArow.SetActive(true);
        }
    }

    void Step_7_CleanCafe()
    { //7단계 카페 치우기
        if (!UnlockManager.Instance.Cafe.hasTrash)
        {
            CompleteStep();
        }
    }

    void Step_8_GetMoney2()
    { //8단계 돈통 다시 털기
        if (mzone2.MoneyCounts == 0)
        {
            CompleteStep();
        }
    }

    void Step_9_Final()
    { //9단계 끝


    }

}
