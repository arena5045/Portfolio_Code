using DG.Tweening;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PaperManager : MonoBehaviour
{
    public int[,] MonthPapers = new int[30, 11];//한달 paper배열 생성
    int[] ClickPaper = new int[30];//지나간 paper배열
    public int Last_Click;//마지막 클릭 값
    public int today = 0;//현재 날짜 초기화

    public AudioClip Page_click;


    public Image fade;
    float fadetime = 1;
    public Color Now;
    Color oriC;



    public GameObject accpet;
    public GameObject[] EmptyPapers = new GameObject[15];
    public GameObject[] TodayPaper = new GameObject[3];
    GameObject[] TomorrowPaper = new GameObject[5];
    GameObject[] TTomorrowPaper = new GameObject[7];

    public GameObject[] Papers = new GameObject[8];

    public static PaperManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

       int prefs_continue = PlayerPrefs.GetInt("CONTINUE", 0);
        if (prefs_continue == 0)
        {
            //CONTINUE = false;
            N_NewMonth();
            PaperSetting();
        }
        else
        {
            //CONTINUE = true;
            N_LOADMonth();
            PaperSetting();
        }

    }

    void Start()
    {

    }


    void Update()
    {

    }

    public void N_NewMonth() { 
    
        for(int day=0; day < 30; day++)//한달 동안 반복
        {
            for(int id = 0; id < 11; id++)//커맨드 id 할당
            {
                if (day == 29)//마지막 날이면
                { MonthPapers[day, id] = 0 ;}//무조건 보스커맨드
                else if (day == 28)//마지막 전날이면
                { MonthPapers[day, id] = 5; }//무조건 정비 커맨드 
                else//그외의 날에는
                { 
                    MonthPapers[day, id] = UnityEngine.Random.Range(1, 30);//종이id할당
                    if (MonthPapers[day, id] > 7) //7이상이면
                    { MonthPapers[day, id] = 7; }//일반전투 id인 7할당
                    //print("[" + day + "," + id + "]" + "="+MonthPapers[day, id]); 
                }
                PlayerPrefs.SetInt("Month" + day + "/" + id, MonthPapers[day, id]);
            }
            ClickPaper[day] = -1;
        }
        today = 0;
        PlayerPrefs.SetInt("today", today);

    }

    public void N_LOADMonth()
    {

        for (int day = 0; day < 30; day++)//한달 동안 반복
        {
            for (int id = 0; id < 11; id++)//커맨드 id 할당
            {
                if (day == 29)//마지막 날이면
                { MonthPapers[day, id] = 0; }//무조건 보스커맨드
                else if (day == 28)//마지막 전날이면
                { MonthPapers[day, id] = 5; }//무조건 정비 커맨드 
                else//그외의 날에는
                {
                    MonthPapers[day, id] = PlayerPrefs.GetInt("Month" + day + "/" + id, 7);
                }
             
            }
            ClickPaper[day] = -1;
        }
        today = PlayerPrefs.GetInt("today", 0);
        Last_Click = PlayerPrefs.GetInt("Last_Click", 5);
    }
    public void PaperSetting() {




        for (int i = 0; i < 3; i++)
        {
            Destroy(TodayPaper[i]);
        }
        for (int i = 0; i < 5; i++)
        {
            Destroy(TomorrowPaper[i]);
        }
        for (int i = 0; i < 7; i++)
        {
            Destroy(TTomorrowPaper[i]);
        }

        PlayerPrefs.SetInt("Last_Click", Last_Click);
        switch (Last_Click)
        {
           case 0 : Last0();break;
           case 1 : Last1();break;
           case 2:  Last2();break;
           case 3: Last3_7(); break;
           case 4: Last3_7(); break;
           case 5: Last3_7(); break;
           case 6: Last3_7(); break;
           case 7: Last3_7(); break;
           case 8: Last8(); break;
           case 9: Last9(); break;
           case 10: Last10(); break;
        }

    }

    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    private void Last0()
    {


        for (int i = 1; i < 3; i++)
        {

            TodayPaper[i] = Instantiate(Papers[MonthPapers[today, (Last_Click - 1 + i)]], EmptyPapers[i].transform.position, Quaternion.identity);
            TodayPaper[i].transform.parent = EmptyPapers[i].transform;
            TodayPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TodayPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            switch (i)
            {
                case 0: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click - 1; break;
                case 1: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click; break;
                case 2: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click + 1; break;
            }
            TodayPaper[i].GetComponent<TodayPaper>().sizeUD();
        }
        for (int i = 2; i < 5; i++)
        {

            int tp = i + 3;
            TomorrowPaper[i] = Instantiate(Papers[MonthPapers[today + 1, (Last_Click - 2 + i)]], EmptyPapers[tp].transform.position, Quaternion.identity);
            TomorrowPaper[i].transform.parent = EmptyPapers[tp].transform;
            TomorrowPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TomorrowPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            TomorrowPaper[i].GetComponent<EventTrigger>().enabled = false;
            TomorrowPaper[i].GetComponent<Image>().color = TomorrowPaper[i].GetComponent<Image>().color - new Color(0.3f, 0.3f, 0.3f, 0);
        }
        for (int i = 3; i < 7; i++)
        {
            int ttp = i + 8;
            TTomorrowPaper[i] = Instantiate(Papers[MonthPapers[today + 2, (Last_Click - 3 + i)]], EmptyPapers[ttp].transform.position, Quaternion.identity);
            TTomorrowPaper[i].transform.parent = EmptyPapers[ttp].transform;
            TTomorrowPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TTomorrowPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            TTomorrowPaper[i].GetComponent<EventTrigger>().enabled = false;
            TTomorrowPaper[i].GetComponent<Image>().color = TTomorrowPaper[i].GetComponent<Image>().color - new Color(0.3f, 0.3f, 0.3f, 0);
        }
    }

    private void Last1()
    {
        for (int i = 0; i < 3; i++)
        {

            TodayPaper[i] = Instantiate(Papers[MonthPapers[today, (Last_Click - 1 + i)]], EmptyPapers[i].transform.position, Quaternion.identity);
            TodayPaper[i].transform.parent = EmptyPapers[i].transform;
            TodayPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TodayPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            switch (i)
            {
                case 0: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click - 1; break;
                case 1: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click; break;
                case 2: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click + 1; break;
            }
            TodayPaper[i].GetComponent<TodayPaper>().sizeUD();
        }
        for (int i = 1; i < 5; i++)
        {

            int tp = i + 3;
            TomorrowPaper[i] = Instantiate(Papers[MonthPapers[today + 1, (Last_Click - 2 + i)]], EmptyPapers[tp].transform.position, Quaternion.identity);
            TomorrowPaper[i].transform.parent = EmptyPapers[tp].transform;
            TomorrowPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TomorrowPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            TomorrowPaper[i].GetComponent<EventTrigger>().enabled = false;
            TomorrowPaper[i].GetComponent<Image>().color = TomorrowPaper[i].GetComponent<Image>().color - new Color(0.3f, 0.3f, 0.3f, 0);
        }
        for (int i = 2; i < 7; i++)
        {
            int ttp = i + 8;
            TTomorrowPaper[i] = Instantiate(Papers[MonthPapers[today + 2, (Last_Click - 3 + i)]], EmptyPapers[ttp].transform.position, Quaternion.identity);
            TTomorrowPaper[i].transform.parent = EmptyPapers[ttp].transform;
            TTomorrowPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TTomorrowPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            TTomorrowPaper[i].GetComponent<EventTrigger>().enabled = false;
            TTomorrowPaper[i].GetComponent<Image>().color = TTomorrowPaper[i].GetComponent<Image>().color - new Color(0.3f, 0.3f, 0.3f, 0);
        }
    }

    private void Last2()
    {
        for (int i = 0; i < 3; i++)
        {

            TodayPaper[i] = Instantiate(Papers[MonthPapers[today, (Last_Click - 1 + i)]], EmptyPapers[i].transform.position, Quaternion.identity);
            TodayPaper[i].transform.parent = EmptyPapers[i].transform;
            TodayPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TodayPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            switch (i)
            {
                case 0: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click - 1; break;
                case 1: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click; break;
                case 2: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click + 1; break;
            }
            TodayPaper[i].GetComponent<TodayPaper>().sizeUD();

        }
        for (int i = 0; i < 5; i++)
        {

            int tp = i + 3;
            TomorrowPaper[i] = Instantiate(Papers[MonthPapers[today + 1, (Last_Click - 2 + i)]], EmptyPapers[tp].transform.position, Quaternion.identity);
            TomorrowPaper[i].transform.parent = EmptyPapers[tp].transform;
            TomorrowPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TomorrowPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            TomorrowPaper[i].GetComponent<EventTrigger>().enabled = false;
            TomorrowPaper[i].GetComponent<Image>().color = TomorrowPaper[i].GetComponent<Image>().color - new Color(0.3f, 0.3f, 0.3f, 0);

        }
        for (int i = 1; i < 7; i++)
        {
            int ttp = i + 8;
            TTomorrowPaper[i] = Instantiate(Papers[MonthPapers[today + 2, (Last_Click - 3 + i)]], EmptyPapers[ttp].transform.position, Quaternion.identity);
            TTomorrowPaper[i].transform.parent = EmptyPapers[ttp].transform;
            TTomorrowPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TTomorrowPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            TTomorrowPaper[i].GetComponent<EventTrigger>().enabled = false;
            TTomorrowPaper[i].GetComponent<Image>().color = TTomorrowPaper[i].GetComponent<Image>().color - new Color(0.3f, 0.3f, 0.3f, 0);
        }
    }

    private void Last3_7()
    {

        for (int i = 0; i < 3; i++)
        {//today_paper 설정

            TodayPaper[i] = Instantiate(Papers[MonthPapers[today, (Last_Click - 1 + i)]], EmptyPapers[i].transform.position, Quaternion.identity);//종이 객체 생성
            //TodayPaper[i].transform.parent = EmptyPapers[i].transform; // 미리 지정한 부모 위치에 생성
            TodayPaper[i].transform.SetParent(EmptyPapers[i].transform);
            TodayPaper[i].transform.localScale = new Vector3(1, 1, 1); // 크기는 1,1,1배로 생성
            TodayPaper[i].transform.rotation = EmptyPapers[i].transform.rotation;// 회전도값 부모값 받아옴
            switch (i) {//선택한 위치값을 받아옴
                case 0: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click-1;break;
                    //1번째 종이 클릭하면 현재 클릭값은 마지막 클릭값-1
                case 1: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click; break;
                    //2번째 종이 클릭하면 현재 클릭값은 마지막 클릭값
                case 2: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click+1; break;
                    //3번째 종이 클릭하면 현재 클릭값은 마지막 클릭값+1
            }
            TodayPaper[i].GetComponent<TodayPaper>().sizeUD();
        }
        for (int i = 0; i < 5; i++)
        {//TomorrowPaper 설정

            int tp = i + 3;
            TomorrowPaper[i] = Instantiate(Papers[MonthPapers[today + 1, (Last_Click - 2 + i)]], EmptyPapers[tp].transform.position, Quaternion.identity);
            //TomorrowPaper[i].transform.parent = EmptyPapers[tp].transform;
            TomorrowPaper[i].transform.SetParent(EmptyPapers[tp].transform);

            TomorrowPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TomorrowPaper[i].transform.rotation = EmptyPapers[i].transform.rotation;
            TomorrowPaper[i].GetComponent<EventTrigger>().enabled = false;
            TomorrowPaper[i].GetComponent<Image>().color = TomorrowPaper[i].GetComponent<Image>().color - new Color(0.3f, 0.3f, 0.3f, 0);

        }
        for (int i = 0; i < 7; i++)
        {// Tomorrow_afer_Paper 설정
            int ttp = i + 8;
            TTomorrowPaper[i] = Instantiate(Papers[MonthPapers[today + 2, (Last_Click - 3 + i)]], EmptyPapers[ttp].transform.position, Quaternion.identity);
            // TTomorrowPaper[i].transform.parent = EmptyPapers[ttp].transform;
            TTomorrowPaper[i].transform.SetParent(EmptyPapers[ttp].transform);
            TTomorrowPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TTomorrowPaper[i].transform.rotation = EmptyPapers[i].transform.rotation;
            TTomorrowPaper[i].GetComponent<EventTrigger>().enabled = false;
            TTomorrowPaper[i].GetComponent<Image>().color =TTomorrowPaper[i].GetComponent<Image>().color - new Color(0.3f, 0.3f, 0.3f, 0);
        }

    }

    private void Last8()
    {

        for (int i = 0; i < 3; i++)
        {

            TodayPaper[i] = Instantiate(Papers[MonthPapers[today, (Last_Click - 1 + i)]], EmptyPapers[i].transform.position, Quaternion.identity);
            TodayPaper[i].transform.parent = EmptyPapers[i].transform;
            TodayPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TodayPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            switch (i)
            {
                case 0: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click - 1; break;
                case 1: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click; break;
                case 2: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click + 1; break;
            }
            TodayPaper[i].GetComponent<TodayPaper>().sizeUD();
        }
        for (int i = 0; i < 5; i++)
        {

            int tp = i + 3;
            TomorrowPaper[i] = Instantiate(Papers[MonthPapers[today + 1, (Last_Click - 2 + i)]], EmptyPapers[tp].transform.position, Quaternion.identity);
            TomorrowPaper[i].transform.parent = EmptyPapers[tp].transform;
            TomorrowPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TomorrowPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            TomorrowPaper[i].GetComponent<EventTrigger>().enabled = false;
            TomorrowPaper[i].GetComponent<Image>().color = TomorrowPaper[i].GetComponent<Image>().color - new Color(0.3f, 0.3f, 0.3f, 0);

        }
        for (int i = 0; i < 6; i++)
        {
            int ttp = i + 8;
            TTomorrowPaper[i] = Instantiate(Papers[MonthPapers[today + 2, (Last_Click - 3 + i)]], EmptyPapers[ttp].transform.position, Quaternion.identity);
            TTomorrowPaper[i].transform.parent = EmptyPapers[ttp].transform;
            TTomorrowPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TTomorrowPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            TTomorrowPaper[i].GetComponent<EventTrigger>().enabled = false;
            TTomorrowPaper[i].GetComponent<Image>().color = TTomorrowPaper[i].GetComponent<Image>().color - new Color(0.3f, 0.3f, 0.3f, 0);
        }

    }

    private void Last9()
    {

        for (int i = 0; i < 3; i++)
        {

            TodayPaper[i] = Instantiate(Papers[MonthPapers[today, (Last_Click - 1 + i)]], EmptyPapers[i].transform.position, Quaternion.identity);
            TodayPaper[i].transform.parent = EmptyPapers[i].transform;
            TodayPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TodayPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            switch (i)
            {
                case 0: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click - 1; break;
                case 1: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click; break;
                case 2: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click + 1; break;
            }
            TodayPaper[i].GetComponent<TodayPaper>().sizeUD();
        }
        for (int i = 0; i < 4; i++)
        {

            int tp = i + 3;
            TomorrowPaper[i] = Instantiate(Papers[MonthPapers[today + 1, (Last_Click - 2 + i)]], EmptyPapers[tp].transform.position, Quaternion.identity);
            TomorrowPaper[i].transform.parent = EmptyPapers[tp].transform;
            TomorrowPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TomorrowPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            TomorrowPaper[i].GetComponent<EventTrigger>().enabled = false;
            TomorrowPaper[i].GetComponent<Image>().color = TomorrowPaper[i].GetComponent<Image>().color - new Color(0.3f, 0.3f, 0.3f, 0);

        }
        for (int i = 0; i < 5; i++)
        {
            int ttp = i + 8;
            TTomorrowPaper[i] = Instantiate(Papers[MonthPapers[today + 2, (Last_Click - 3 + i)]], EmptyPapers[ttp].transform.position, Quaternion.identity);
            TTomorrowPaper[i].transform.parent = EmptyPapers[ttp].transform;
            TTomorrowPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TTomorrowPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            TTomorrowPaper[i].GetComponent<EventTrigger>().enabled = false;
            TTomorrowPaper[i].GetComponent<Image>().color = TTomorrowPaper[i].GetComponent<Image>().color - new Color(0.3f, 0.3f, 0.3f, 0);
        }

    }

    private void Last10()
    {

        for (int i = 0; i < 2; i++)
        {

            TodayPaper[i] = Instantiate(Papers[MonthPapers[today, (Last_Click - 1 + i)]], EmptyPapers[i].transform.position, Quaternion.identity);
            TodayPaper[i].transform.parent = EmptyPapers[i].transform;
            TodayPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TodayPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            switch (i)
            {
                case 0: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click - 1; break;
                case 1: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click; break;
                case 2: TodayPaper[i].GetComponent<TodayPaper>().curruntClick = Last_Click + 1; break;
            }
            TodayPaper[i].GetComponent<TodayPaper>().sizeUD();
        }
        for (int i = 0; i < 3; i++)
        {

            int tp = i + 3;
            TomorrowPaper[i] = Instantiate(Papers[MonthPapers[today + 1, (Last_Click - 2 + i)]], EmptyPapers[tp].transform.position, Quaternion.identity);
            TomorrowPaper[i].transform.parent = EmptyPapers[tp].transform;
            TomorrowPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TomorrowPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            TomorrowPaper[i].GetComponent<EventTrigger>().enabled = false;
            TomorrowPaper[i].GetComponent<Image>().color = TomorrowPaper[i].GetComponent<Image>().color - new Color(0.3f, 0.3f, 0.3f, 0);

        }
        for (int i = 0; i < 4; i++)
        {
            int ttp = i + 8;
            TTomorrowPaper[i] = Instantiate(Papers[MonthPapers[today + 2, (Last_Click - 3 + i)]], EmptyPapers[ttp].transform.position, Quaternion.identity);
            TTomorrowPaper[i].transform.parent = EmptyPapers[ttp].transform;
            TTomorrowPaper[i].transform.localScale = new Vector3(1, 1, 1);
            TTomorrowPaper[i].transform.rotation = EmptyPapers[1].transform.rotation;
            TTomorrowPaper[i].GetComponent<EventTrigger>().enabled = false;
            TTomorrowPaper[i].GetComponent<Image>().color = TTomorrowPaper[i].GetComponent<Image>().color - new Color(0.3f, 0.3f, 0.3f, 0);
        }

    }

    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 

    public IEnumerator Fadein_Next(String tag)
    {

        float curT = 0; //현재시간 초기화
        fade.gameObject.SetActive(true); //오브젝트 활성화
        while (curT < fadetime)
        {
            curT += Time.deltaTime*2; //현재시간 ++
            fade.color = Color.Lerp(oriC, Now, curT); //현재시간 값 만큼 러프

            yield return null;

        }


        if (curT >= 1)
        {
            yield return new WaitForSeconds(0.2F);
            fade.color = new Color(0, 0, 0, 0);
            fade.gameObject.SetActive(false);


        }

        Paper_action(tag);
        BarManager.Instance.Update_Pray();
        yield return null;
        yield return null;
        fade.gameObject.SetActive(false);
        yield return null;
    }

    public void Paper_action(String tag) {

        switch (tag)
        {
            case "BattlePaper":
                {
                    GameManager.Instance.Battle = GameManager.battleState.nomal;
                    BattleManager.Instance.DoBattle(); 
                    CameraManager.Instance.BattleCam_on();
                    SoundManager.Instance.NomalBattle_On();
                    break;
                    
                }
            case "EBattlePaper":
                {
                    GameManager.Instance.Battle = GameManager.battleState.elite;
                    SoundManager.Instance.EliteBattle_On();
                    BattleManager.Instance.DoBattle(); CameraManager.Instance.BattleCam_on();
                    break;
                }
            case "BossPaper":
                {
                    GameManager.Instance.Battle = GameManager.battleState.boss;
                    SoundManager.Instance.BossBattle_On();
                    BattleManager.Instance.DoBattle(); CameraManager.Instance.BattleCam_on();
                    break;
                }
            case "UShopPaper":
                {
                    PopupManager.Instance.ShowUnitShop_Popup();
                    SoundManager.Instance.Popup_On();
                    break;
                }
            case "EventPaper":
                {
                    EventManager.Instance.Event();
                    SoundManager.Instance.Popup_On();
                    break;
                }
            case "RebuildPaper":
                {
                    PopupManager.Instance.ShowRebuild_Popup();
                    SoundManager.Instance.Popup_On();
                    break;
                }

        }


    }

    public void Paper_action_END() {


        Paper_Locked_off();


    }
    public void Paper_Locked() {
        if (Last_Click == 0)
        {

            TodayPaper[1].GetComponentInChildren<EventTrigger>().enabled = false;
            TodayPaper[2].GetComponentInChildren<EventTrigger>().enabled = false;

        }
        else if (Last_Click == 10)
        {
            TodayPaper[0].GetComponentInChildren<EventTrigger>().enabled = false;
            TodayPaper[1].GetComponentInChildren<EventTrigger>().enabled = false;
        }
        else
        {
            TodayPaper[0].GetComponentInChildren<EventTrigger>().enabled = false;
            TodayPaper[1].GetComponentInChildren<EventTrigger>().enabled = false;
            TodayPaper[2].GetComponentInChildren<EventTrigger>().enabled = false;
        }
            
      

    }

    public void Paper_Locked_off() {
        if (Last_Click == 0)
        {

            TodayPaper[1].GetComponentInChildren<EventTrigger>().enabled = true;
            TodayPaper[2].GetComponentInChildren<EventTrigger>().enabled = true;

        }
        else if (Last_Click == 10)
        {
            TodayPaper[0].GetComponentInChildren<EventTrigger>().enabled = true;
            TodayPaper[1].GetComponentInChildren<EventTrigger>().enabled = true;
        }
        else
        {
            TodayPaper[0].GetComponentInChildren<EventTrigger>().enabled = true;
            TodayPaper[1].GetComponentInChildren<EventTrigger>().enabled = true;
            TodayPaper[2].GetComponentInChildren<EventTrigger>().enabled = true;
        }

    }

}
