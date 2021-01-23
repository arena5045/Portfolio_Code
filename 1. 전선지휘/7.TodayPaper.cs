using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TodayPaper : PaperBase
{
    object tweenId = new object();

    public int curruntClick;
    bool plus = true;
    GameObject accpetion;
    public override void ClickPaper()
    {
        base.ClickPaper();

        print(curruntClick);
    }


    public void OnClick()
    {
        SoundManager.Instance.SE_Play(PaperManager.Instance.Page_click, 7f);
        accpetion =Instantiate(PaperManager.Instance.accpet,this.transform.position, Quaternion.identity);
        accpetion.transform.position += new Vector3(0, 0, 0);
        accpetion.transform.parent = PanelManager.Instance._commandPanel.transform;
        accpetion.transform.localScale = new Vector3(1, 1, 1);
        accpetion.transform.rotation = this.transform.rotation;

        StartCoroutine(Accpet());
        StartCoroutine(PaperManager.Instance.Fadein_Next(this.tag));
        //PaperManager.Instance.Paper_action(;

    }


    public void sizeUD()
    {
        StartCoroutine(clickable());
    }

    IEnumerator clickable() //클릭 가능한 종이깜빡거림
    {
        while (true){

            yield return null;
            if (plus == true)
            {
                for (float f = 0.9f; f < 1.1; f += 0.002f)
                {
                    this.transform.localScale = new Vector3(f, f, f);

                    if (f >= 1.09f)
                    {
                        plus = false;
                        break;
                    }
                    yield return null;
                }
            } else
            {
                for (float f = 1.1f; f > 0.9; f -= 0.002f)
                {
                    this.transform.localScale = new Vector3(f, f, f);
                    if (f <= 0.91f)
                    {
                        plus = true;
                        break;
                    }
                    yield return null;
                }

            }
            yield return null;
        }




    }



    IEnumerator Accpet()//클릭하며 승인도장 띄움
    { 
        yield return new WaitForSeconds(1.0f);//1초뒤에 지속

        Destroy(accpetion);//도장삭제

        PaperManager.Instance.Last_Click = curruntClick;//마지막클릭=현재클릭
        BarManager.Instance.date++;//날짜 증가
        PaperManager.Instance.today++;//이번달 날짜 증가
        BarManager.Instance._SetDate();//날짜 할당


        if (PaperManager.Instance.today == 30)
        {//만약 오늘이 30일이면
            PaperManager.Instance.N_NewMonth();//새로운 달 시작 배열 생성
        }

        PaperManager.Instance.PaperSetting();//종이 오브젝트 생성,배열
        if (gameObject.tag == "BattlePaper")
        {
            PaperManager.Instance.Paper_Locked();//종이클릭 잠금
        }


    }




}
