using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebuild_Popup : PopupBase
{
    public GameObject exit_popup;
    public GameObject fadeimage;
    GameObject effect;
    public void Exitpopup_on()
    {
        exit_popup.SetActive(true);
        fadeimage.SetActive(true);
    }
    public void Exitpopup_off()
    {
        exit_popup.SetActive(false);
        fadeimage.SetActive(false);
    }

    public override void HidePopup()
    {
        PaperManager.Instance.Paper_Locked_off();
        base.HidePopup();
    }


    public void LobbyBGM_On() {
        SoundManager.Instance.Lobby_On();
    }

    public void Hero_Rest() //영웅 휴식
    {
        int heal = Mathf.RoundToInt(BarManager.Instance.Hero.GetComponent<Unit>().Max_hp * 0.4f);
        BarManager.Instance.Hero.GetComponent<Unit>().Heal_Unit(heal);

        SoundManager.Instance.Heal_buff_Play();
        CameraManager.Instance.ResultCam_on();

        Rebuild_result_Popup result =  PopupManager.Instance.ShowRebuild_Result_Popup();
        result.SetText("영웅 휴식", "영웅이 충분한 휴식을 취했습니다.\n" + heal + "만큼의 체력을 회복합니다.");
        effect = Instantiate(EffectManager.Instance.Healeffect, BarManager.Instance.Hero.transform.position, BarManager.Instance.Hero.transform.rotation);
        result.SetEffect(effect);

        Destroy(gameObject);
        Destroy(effect, 2f);
    }

    public void Hero_traning()//영웅 훈련
    {
        float training = Random.Range(0f, 1f);
        Rebuild_result_Popup result = PopupManager.Instance.ShowRebuild_Result_Popup();
        int tr_atk, tr_def,tr_hp=0;
       
        if (training < GameManager.Instance.Hero_traning_add[0])// 훈련실패
        {
            tr_atk = Random.Range(-3,1);
            tr_def = Random.Range(-10, 1);
            tr_hp = Random.Range(-10, 1);
            result.SetText("훈련 실패", "훈련을 하다 부상을 입었습니다.\n"+"최대 체력 " + tr_hp + "\n" + "공격력 "+ tr_atk+ "\n" + "방어력 " + tr_def  );

            BarManager.Instance.Hero.GetComponent<Unit>().atk += tr_atk;
            BarManager.Instance.Hero.GetComponent<Unit>().def += tr_def;
            BarManager.Instance.Hero.GetComponent<Unit>().Max_hp += tr_hp;

            SoundManager.Instance.traning(0);
            effect = Instantiate(EffectManager.Instance.traning_debuff, BarManager.Instance.Hero.transform.position, BarManager.Instance.Hero.transform.rotation);
            result.SetEffect(effect);
        }
        else if (GameManager.Instance.Hero_traning_add[0] <= training && training < GameManager.Instance.Hero_traning_add[1]) //훈련성공
        {
            tr_atk = Random.Range(1, 4);
            tr_def = Random.Range(1, 11);
            tr_hp = Random.Range(1, 11);
            result.SetText("훈련 성공", "훈련으로 영웅의 기량이 올랐습니다.\n" + "최대 체력 +" + tr_hp + "\n" + "공격력 +" + tr_atk + "\n" + "방어력 +" + tr_def);

            BarManager.Instance.Hero.GetComponent<Unit>().atk += tr_atk;
            BarManager.Instance.Hero.GetComponent<Unit>().def += tr_def;
            BarManager.Instance.Hero.GetComponent<Unit>().Max_hp += tr_hp;

            SoundManager.Instance.traning(1);
            effect = Instantiate(EffectManager.Instance.traning_buff, BarManager.Instance.Hero.transform.position, BarManager.Instance.Hero.transform.rotation);
            result.SetEffect(effect);
        }
        else //if (GameManager.Instance.Hero_traning_add[1] <= training && training <= GameManager.Instance.Hero_traning_add[2]) 훈련 대성공
        {
            tr_atk = Random.Range(3, 6);
            tr_def = Random.Range(10, 15);
            tr_hp = Random.Range(10, 15);
            result.SetText("훈련 대성공", "훈련으로 깨달음을 얻은 듯 합니다!\n" + "최대 체력 +" + tr_hp + "\n" + "공격력 +" + tr_atk + "\n" + "방어력 +" + tr_def);

            BarManager.Instance.Hero.GetComponent<Unit>().atk += tr_atk;
            BarManager.Instance.Hero.GetComponent<Unit>().def += tr_def;
            BarManager.Instance.Hero.GetComponent<Unit>().Max_hp += tr_hp;

            SoundManager.Instance.traning(2);
            effect = Instantiate(EffectManager.Instance.traning_perfect, BarManager.Instance.Hero.transform.position, BarManager.Instance.Hero.transform.rotation);
            result.SetEffect(effect);
        }

        BarManager.Instance.Hero.GetComponent<Unit>().Heal_Unit(tr_hp);
     
        CameraManager.Instance.ResultCam_on();

        Destroy(gameObject);
        Destroy(effect, 5f);
    }


    public void Pray()
    {

        CameraManager.Instance.ResultCam_on();
        Rebuild_result_Popup praypop = PopupManager.Instance.ShowRebuild_Result_Popup();

        int pray_code = Random.Range(1, 4);
        int pray_turn = Random.Range(3, 31);
        int pray_power;
        switch (pray_code) 
        {
            case 1:
                pray_power = Random.Range(1, 101);
                praypop.SetText("전쟁신의 축복", "신이 전쟁의 가호를 내립니다.\n" + pray_turn + "일 동안 전투 중 아군의 공격력이 " + pray_power +"% 증가합니다.");
                pray_setting(pray_code, pray_turn, pray_power);
                praypop.SetEffect(effect);
                BarManager.Instance.pray_string.text = "공격력 "+ pray_turn+"일 간 "+ pray_power+"% 증가";

                PlayerPrefs.SetInt("pray_code", pray_code);
                PlayerPrefs.SetInt("pray_turn", pray_turn);
                PlayerPrefs.SetInt("pray_power", pray_power);
                break;
            case 2:
                pray_power = Random.Range(1, 101);
                praypop.SetText("풍요신의 축복", "신이 풍요의 가호를 내립니다.\n" + pray_turn + "일 동안 전투 중 영웅의 마나재생이 " + pray_power + "% 증가합니다.");
                pray_setting(pray_code, pray_turn, pray_power);
                BarManager.Instance.pray_string.text = "마나재생 " + pray_turn + "일 간 " + pray_power + "% 증가";

                PlayerPrefs.SetInt("pray_code", pray_code);
                PlayerPrefs.SetInt("pray_turn", pray_turn);
                PlayerPrefs.SetInt("pray_power", pray_power);
                break;
            case 3:
                pray_power = Random.Range(1, 101);
                praypop.SetText("수호신의 축복", "신이 수호의 가호를 내립니다.\n" + pray_turn + "일 동안 전투 중 아군의 방어력이 " + pray_power + "만큼 증가합니다.");
                pray_setting(pray_code, pray_turn, pray_power);
                BarManager.Instance.pray_string.text = "방어력 " + pray_turn + "일 간 " + pray_power + "증가";

                PlayerPrefs.SetInt("pray_code", pray_code);
                PlayerPrefs.SetInt("pray_turn", pray_turn);
                PlayerPrefs.SetInt("pray_power", pray_power);
                break;
        }
        effect = Instantiate(EffectManager.Instance.pray, BarManager.Instance.Hero.transform.position + new Vector3(0,3,0), Quaternion.Euler(0,0,0));
        praypop.SetEffect(effect);
        SoundManager.Instance.pray_Play();
        Destroy(gameObject);
        Destroy(effect, 5f);
    }


    void pray_setting(int code, int turn, int power)
    {
        BarManager.Instance.pray_code = code;
        BarManager.Instance.pray_turn = turn;
        BarManager.Instance.pray_power = power;
        switch (code)
        {
            case 1:
                BarManager.Instance.pray_color.color = Color.red;
                break;
            case 2:
                BarManager.Instance.pray_color.color = Color.blue;
                break;
            case 3:
                BarManager.Instance.pray_color.color = Color.yellow;
                break;
        }
    }

}
