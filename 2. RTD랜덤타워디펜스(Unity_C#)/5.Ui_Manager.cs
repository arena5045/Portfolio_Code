using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Manager : MonoBehaviour
{
    public static Ui_Manager instance { get; private set; }

    public GameObject roundZone;
    public GameObject moneyZone;

    public TMP_Text roundTxt;
    public TMP_Text lifeTxt;
    public TMP_Text money1Txt;
    public TMP_Text money2Txt;
    public TMP_Text money3Txt;

    public TMP_Text killScore;
    public TMP_Text mission1CoolTime;
    public TMP_Text mission2CoolTime;
    public TMP_Text mission3CoolTime;


    public TMP_Text s_upgrade_plus;
    public TMP_Text m_upgrade_plus;
    public TMP_Text a_upgrade_plus;
    public TMP_Text s_upgrade_price;
    public TMP_Text m_upgrade_price;
    public TMP_Text a_upgrade_price;

    public TMP_Text state;

    public GameObject towerInfoPannel;

    public GameObject btnMenu;

    public GameObject InfoZone;
    public Image tower_portrait;
    public TMP_Text tower_Name;
    public TMP_Text tower_Info;
    public TMP_Text tower_rank;

    private bool drawerRound = false;
    private Vector2 roundrec;
    private Vector2 moneyrec;
    private Vector2 end_pos;
    private bool drawerMoney = false;

    public GameObject OptionPannel;
    public GameObject pMoneyPannel;
    GameObject lastinfoOb;

    public TMP_Text Timer;

    [HideInInspector]
    public bool InfoPannelActive= false;

    private void Awake()
    {
        instance = this;

    }


    private void Start()
    {
        UIreset();
    }



    public void UiRefresh()
    {
        roundTxt.text = Data_Manager.instance.curRound + "/" + Data_Manager.instance.maxRound;
        lifeTxt.text = Data_Manager.instance.CurHp.ToString();


        money1Txt.text = Data_Manager.instance.money1.ToString();
        money2Txt.text = Data_Manager.instance.money2.ToString();
        money3Txt.text = Data_Manager.instance.money3.ToString();

        s_upgrade_plus.text = "+" + UpGrade_Manager.instance.warrior_upgrade_rank;
        a_upgrade_plus.text = "+" + UpGrade_Manager.instance.archer_upgrade_rank;
        m_upgrade_plus.text = "+" + UpGrade_Manager.instance.mage_upgrade_rank;
        s_upgrade_price.text = "-" + UpGrade_Manager.instance.warrior_upgrade_price;
        a_upgrade_price.text = "-" + UpGrade_Manager.instance.archer_upgrade_price;
        m_upgrade_price.text = "-" + UpGrade_Manager.instance.mage_upgrade_price;
        
        
        killScore.text = "킬 스코어 : " + Data_Manager.instance.killcount;
        mission1CoolTime.text = "미션1 쿨타임 : " + (20- StageManager.instance.mission1CoolTime);
        mission2CoolTime.text = "미션2 쿨타임 : " + (30- StageManager.instance.mission2CoolTime);
        mission3CoolTime.text = "미션3 쿨타임 : " + (40- StageManager.instance.mission3CoolTime);
        if (lastinfoOb != null)
            InfoPannelRefresh(lastinfoOb);
    }


    void UIreset() // ui 초기값으로 배치
    {
        Data_Manager.instance.DataReset();

        roundTxt.text = Data_Manager.instance.curRound + "/" + Data_Manager.instance.maxRound;
        lifeTxt.text = Data_Manager.instance.CurHp.ToString();


        money1Txt.text = Data_Manager.instance.money1.ToString();
        money2Txt.text = Data_Manager.instance.money2.ToString();
        money3Txt.text = Data_Manager.instance.money3.ToString();

        roundrec = roundZone.GetComponent<RectTransform>().anchoredPosition;
        moneyrec = moneyZone.GetComponent<RectTransform>().anchoredPosition;

        state.text = "";
        s_upgrade_plus.text = "+0";
        m_upgrade_plus.text = "+0";
        a_upgrade_plus.text = "+0";
        s_upgrade_price.text = "-20";
        m_upgrade_price.text = "-20";
        a_upgrade_price.text = "-20";
        killScore.text = "킬 스코어 : " + Data_Manager.instance.killcount;
        mission1CoolTime.text = "미션1 쿨타임 : 0 ";
        mission2CoolTime.text = "미션2 쿨타임 : 0 ";
        mission3CoolTime.text = "미션3 쿨타임 : 0 ";
        tower_portrait.color = Color.clear;
    }


    public void RoundUidrawer() 
    { 
        RectTransform rect = roundZone.GetComponent<RectTransform>();
        Vector2 start_pos = rect.anchoredPosition;
        Vector2 end_pos;
        if (!drawerRound) // 열려있을때
        {
             end_pos = start_pos + new Vector2(0, 140);
             drawerRound = true;
        }
        else // 닫혀있을때
        {
             end_pos = roundrec;
            drawerRound = false;
        }
       

        StartCoroutine(LerfUI(rect, start_pos, end_pos,0.2f));
    }

    public void MoneydUidrawer()
    {
        RectTransform rect = moneyZone.GetComponent<RectTransform>();
        Vector2 start_pos = rect.anchoredPosition;
        Vector2 end_pos;
        if (!drawerMoney)
        {
            end_pos = start_pos + new Vector2(0, 140);
            drawerMoney = true;
        }
        else
        {
            end_pos = moneyrec;
            drawerMoney = false;
        }


        StartCoroutine(LerfUI(rect, start_pos, end_pos, 0.2f));
    }

    public void OptionPannelOpen()
    {
        Sound_Manager.instance.EffectPlay(8);
        if (OptionPannel.activeSelf) 
        {
            OptionPannel.SetActive(false);
        }
        else
        {
            OptionPannel.SetActive(true);
        }
    }

    public void PmoneyPannelOpen()
    {
        Sound_Manager.instance.EffectPlay(4);
        if (pMoneyPannel.activeSelf)
        {
            pMoneyPannel.SetActive(false);
        }
        else
        {
            pMoneyPannel.SetActive(true);
        }
    }


    IEnumerator LerfUI(RectTransform target,Vector2 start_pos,Vector2 end_pos,float mtime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < mtime)
        {
            float t = elapsedTime / mtime;
            target.anchoredPosition = Vector2.Lerp(start_pos, end_pos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.anchoredPosition = end_pos;
        //isMoving = false;

    }



    public void InfoPannelRefresh(GameObject infoOb)
    {
        towerInfoPannel.SetActive(InfoPannelActive);

        lastinfoOb = infoOb;
        Twr_0Base towerinfo = infoOb.gameObject.GetComponent<Twr_0Base>();

        float up_per=0;
        string _towerType="";
        string _towerAttackType="";

        switch(towerinfo.towerType)
        {
            case Twr_0Base.TowerType.Archer:
                _towerType = "궁수";
                up_per = UpGrade_Manager.instance.archerUpgrade;
                break;
            case Twr_0Base.TowerType.Mage:
                _towerType = "마법사";
                up_per = UpGrade_Manager.instance.mageUpgrade;
                break;
            case Twr_0Base.TowerType.Warrior:
                _towerType = "전사";
                up_per = UpGrade_Manager.instance.warriorUpgrade;
                break;
        }

        switch (towerinfo.towerAttackType)
        {
            case Twr_0Base.TowerAttackType.Area:
                _towerAttackType = "범위형";
                break;
            case Twr_0Base.TowerAttackType.Shooter:
                _towerAttackType = "즉발형";
                break;
            case Twr_0Base.TowerAttackType.InstanceThorwer:
                _towerAttackType = "투척형";
                break;
        }
        tower_portrait.sprite = towerinfo.portrait;
        tower_portrait.color = Color.white;
        tower_Name.text = towerinfo.towerName;
        int rank = towerinfo.towerRank + 1;
        tower_rank.text = "★" + rank;
        tower_Info.text = "타입 : " + _towerType + "/"+_towerAttackType+
          "\n공격력 : " + towerinfo.towerAttackDamage[towerinfo.towerRank] +"(+"+ up_per.ToString() + "%)"+
          "\n공격속도 : " + towerinfo.towerAttackSpeed[towerinfo.towerRank] +
          "\n사정거리 : " + towerinfo.towerAttackRange[towerinfo.towerRank];

    
    }

}
