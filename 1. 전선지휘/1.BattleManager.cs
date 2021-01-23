using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public GameObject commandcanvas;
    public GameObject Unit;
    public GameObject[] Unit_inGame;
    public GameObject Hero;

    public Button[] Skills = new Button[3];

    public AudioClip Battle_win1, Battle_win2;
    public bool isBattle;

    public float Hero_Mana;

    public void Awake()
    {
        Instance = this;
        isBattle = false;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    
        if (isBattle == true && MonsterManager.Instance.Clear_Count <= 0) 
        {
            isBattle = false;
            Invoke("ExitBattle", 1f);
        }
    }

    public void DoBattle()//전투시작 함수
    {

        Unit_Setting();
        MonsterManager.Instance.Regen();//리젠 시작
        isBattle = true;//전투중인지 알려주는 bool값 

        foreach (Button btn in Skills) //스킬버튼 활성화
        {
            btn.gameObject.SetActive(true);
        }
    }

    public void Unit_Setting()//전투 시작전 유닛 세팅하는 함수
    {

        int unit_count = Unit.transform.childCount;//유닛 카운트 생성

        Unit_inGame = new GameObject[unit_count];//유닛 배열 생성

        for (int i = 0; i < unit_count-1; i++)
        {//유닛 개수만큼 반복
            
            Unit_inGame[i] = Unit.transform.GetChild(i).gameObject;//유닛 배열에 유닛할당
            print(Unit_inGame[i].name);
            Unit_inGame[i].GetComponent<Unit>().hp = Unit_inGame[i].GetComponent<Unit>().Max_hp;
            Unit_inGame[i].GetComponent<Unit>().Reset();
            Unit_inGame[i].gameObject.SetActive(true);
            Unit_inGame[i].GetComponent<Unit>().Current_Tile.GetComponent<Tile>().Unit[int.Parse(Unit_inGame[i].GetComponent<Unit>().Current_Location_number) - 1] = Unit_inGame[i];
            Unit_inGame[i].GetComponent<Unit>().Disenchant();
            Unit_inGame[i].GetComponent<Unit>().isbattile = false;
            //전투시작전에 영웅 제외 모든 유닛 체력 최대로 회복
        }
        Unit_inGame[unit_count-1] = Unit.transform.GetChild(unit_count-1).gameObject;
        print(Unit_inGame[unit_count - 1].name);
        Unit_inGame[unit_count-1].GetComponent<Unit>().Disenchant();
    }

    public void ExitBattle()//전투종료함수 
    {

        Hero_Mana = 0f;
        BarManager.Instance.mp_bar.fillAmount = Hero_Mana / 100;
        BarManager.Instance.mp_string.text = Mathf.FloorToInt(Hero_Mana) + "/ 100";

        SoundManager.Instance.SE_Play(Battle_win1, 3f);
        SoundManager.Instance.BgmAudio.clip = Battle_win2;
        SoundManager.Instance.BgmAudio.Play();
        PopupManager.Instance.ShowBattle_Win_Popup();
        CameraManager.Instance.MainCam_on();
        Unit_Setting();
        PaperManager.Instance.Paper_Locked_off();
        foreach (Button btn in Skills) //스킬버튼 비활성화
        {
            btn.gameObject.SetActive(false);
        }


    }


    public int Damage_Monster(GameObject UNIT, GameObject  MONSTER)//유닛이 몬스터 공격
    {
        Unit unit = UNIT.GetComponent<Unit>();
        Monster monster = MONSTER.GetComponent<Monster>();

        int damage;//초기 데미지 0

        damage = unit.atk;//유닛의 공격력
        damage = (damage /((100 + monster.def)/100));//몬스터 방어력 계산


        //격노 스탯의 존재 | 존재할경우 데미지 1.5배
        if (unit.rage > 0 )
        {
            damage = Mathf.RoundToInt(damage * 1.5f);
            unit.rage--;
            print("데미지 증가! " + damage+"의 피해!");
        }

        //버서커 상태 | 존재할경우 데미지 1.5배
        if (unit.Berserk == true)
        {
            damage = Mathf.RoundToInt(damage * 1.5f);
            print("데미지 증가! " + damage + "의 피해!");
        }

        //스택형 공격의 존재 | 스택을 터뜨려 추가 효과 적용
        if (unit.end_stack == unit.stack)
        {
            damage = Mathf.RoundToInt(damage * unit.stack_buff);
            print("스택 폭발! " + damage + "의 피해!");
        }
        //공격력의 축복을 받고있나
        if (BarManager.Instance.pray_code == 1) {
            damage = Mathf.RoundToInt(damage * (1+(BarManager.Instance.pray_power/100f)));
        }


        return damage;
    }
    public int Damage_Skill(GameObject UNIT, GameObject MONSTER, float coefficient)//유닛이 스킬로 몬스터 공격
    {
        Unit unit = UNIT.GetComponent<Unit>();
        Monster monster = MONSTER.GetComponent<Monster>();

        int damage;//초기 데미지 0

        damage = Mathf.FloorToInt(unit.atk * coefficient);//유닛의 공격력
        damage = (damage / ((100 + monster.def) / 100));//몬스터 방어력 계산

        if (BarManager.Instance.pray_code == 1)
        {
            damage = Mathf.RoundToInt(damage * (1 + (BarManager.Instance.pray_power / 100f)));
        }


        MonsterManager.Instance.DamageFont_produce(damage, MONSTER);
        return damage;
    }

    public int Damage_Unit(GameObject Monster, GameObject Unit)//몬스터가 유닛공격
    {

        Monster monster = Monster.GetComponent<Monster>();
        Unit unit = Unit.GetComponent<Unit>();
        int def = unit.def;
        int damage;//초기 데미지 0
        if (BarManager.Instance.pray_code == 3)
        {
          def += BarManager.Instance.pray_power;
        }
        damage = monster.atk;//유닛의 공격력
        damage = (damage / ((100 + def) / 100));//몬스터 방어력 계산


        return damage;
    }



}
