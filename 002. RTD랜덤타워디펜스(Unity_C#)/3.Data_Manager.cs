using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Data_Manager : MonoBehaviour
{
    public static Data_Manager instance { get; private set; }



    public int CurHp
    {
        get
        { 
            return curHp;
        }
        set 
        {
            int lastHp = curHp;
            curHp = value;
            if(curHp <= 0)
            {
                Sound_Manager.instance.NarPlay(25);

                return;
            }
            if( curHp < lastHp)
            {
                int rand = Random.Range(2, 5);
                Sound_Manager.instance.NarPlay(rand);
            }
        }
    }


    public int maxRound = 50;
    public int curRound;

    public int maxHp = 5;
    private int curHp;

    public int money1 = 0;
    public int money2 = 0;
    public int money3 = 0;

    public int killcount=0;

    public bool isPause = false;

    public float playtime;
    public void DataReset()
    {
        curRound = 1;

        maxHp = 5;
        CurHp = 5;

        money1 = 500;
        money2 = 50;
        money3 = 0;
    }



    public void ChangeBlueGreen()
    {
        if(money1 >=100)
        {
            Sound_Manager.instance.EffectPlay(0);
            money1 -= 100;

            money2 += Random.Range(40, 161);
        }
        else
        {
            int rand = Random.Range(12, 16);
            Sound_Manager.instance.NarPlay(rand);
            Ui_Manager.instance.state.text = "재화 부족!";

        }
        Ui_Manager.instance.UiRefresh();
    }

    public void ChangePGreen()
    {
        if (money3 >= 1)
        {
            Sound_Manager.instance.EffectPlay(2);
            money3 -= 1;

            money2 += 500;
        }
        else
        {
            int rand = Random.Range(12, 16);
            Sound_Manager.instance.NarPlay(rand);
            Ui_Manager.instance.state.text = "재화 부족!";

        }
        Ui_Manager.instance.UiRefresh();
    }
    public void ChangePBlue()
    {
        if (money3 >= 1)
        {
            Sound_Manager.instance.EffectPlay(1);
            money3 -= 1;

            money1 += 500;
        }
        else
        {
            int rand = Random.Range(12, 16);
            Sound_Manager.instance.NarPlay(rand);
            Ui_Manager.instance.state.text = "재화 부족!";

        }
        Ui_Manager.instance.UiRefresh();
    }
    public void ChangePHeart()
    {
        if (money3 >= 1)
        {
            Sound_Manager.instance.EffectPlay(3);
            money3 -= 1;
            maxHp += 10;
            CurHp += 10;
        }
        else
        {
            int rand = Random.Range(12, 16);
            Sound_Manager.instance.NarPlay(rand);
            Ui_Manager.instance.state.text = "재화 부족!";

        }
        Ui_Manager.instance.UiRefresh();
    }


    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator StartTimer()
    {
        int min=0;
        int hour=0;

        while(playtime<=10f) 
        {
            playtime += Time.deltaTime;
            Ui_Manager.instance.Timer.text = "0:"+ (10-Mathf.Floor(playtime)).ToString() ;
            //print(playtime);
            if(playtime > 4.9f && playtime < 5.1f)
            {
                Sound_Manager.instance.NarPlay(17);
            }
            yield return null;
        }
        Sound_Manager.instance.NarPlay(0);
        playtime = 0;
        while (true)
        {
            playtime += Time.deltaTime;
            if(playtime>=60)
            {
                playtime = 0; min++;
            }
            if (min >= 60)
            {
                min = 0; hour++;
            }
            Ui_Manager.instance.Timer.text = hour.ToString() + ":" + min.ToString()+ ":" + Mathf.Floor(playtime).ToString();
            yield return null;
        }
    }
}
