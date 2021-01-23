using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public int Monster_Code;
    public int Code;
    public string Name;
    public Sprite sprite;
    public int grade;
    public string description;
    public int Max_hp;
    public int hp;
    public int atk;
    public int def;
    public int a_spd;
    public int a_del;
    public int m_spd; // 스텟들

    public GameObject TargetUnit;//공격대상

    public GameObject Currentlocation;

    public AudioClip Attack_sound;
    public AudioClip Deadsound;

    public GameObject Attack_Effect_ob;
    public ParticleSystem Attack_Effect;

    public GameObject Targetlocation;
    public Tile Targettile;
    public Tile CurrentTile;
    public Animator animator;


    public Slider Hp_bar;
    public bool Do_Battle;
    public bool isBoss = false;
    float add_state;
    void Start()
    {
        switch (GameManager.Instance.Battle) {
            case GameManager.battleState.nomal:
                {
                    add_state = 1.0f;
                    break;
                }
            case GameManager.battleState.elite:
                {
                    add_state = 1.2f;
                    break;
                }
            case GameManager.battleState.boss:
                {
                    add_state = 1.4f;
                    break;
                }

        }
        if (isBoss)
        {
            add_state *= 1.5f;
        }

        add_state += 0.01f * BarManager.Instance.date;

        Code = GetEnemyInfo.Instance.getEnemyCode(Monster_Code);
        Name = GetEnemyInfo.Instance.getEnemyName(Monster_Code);
        //sprite = GetEnemyInfo.Instance.ge
        grade = GetEnemyInfo.Instance.getEnemyGrade(Monster_Code);
        description = GetEnemyInfo.Instance.getEnemyDescript(Monster_Code);
        Max_hp = Mathf.FloorToInt(GetEnemyInfo.Instance.getEnemyHp(Monster_Code)*add_state);
        hp = Mathf.FloorToInt(GetEnemyInfo.Instance.getEnemyHp(Monster_Code) * add_state);
        atk = Mathf.FloorToInt(GetEnemyInfo.Instance.getEnemyAtk(Monster_Code) * add_state);
        def = Mathf.FloorToInt(GetEnemyInfo.Instance.getEnemyDef(Monster_Code) * add_state);
        a_spd = GetEnemyInfo.Instance.getEnemyAtkSp(Monster_Code);
        m_spd = GetEnemyInfo.Instance.getEnemyMvSp(Monster_Code);
        //스텟들 할당
        animator = this.GetComponent<Animator>();

        MonsterManager.Instance.Move(this.gameObject);//시작하면 이동
}

    // Update is called once per frame
    void Update()
    {
        if (this.hp <= 0)
        {
            Destroy(this.gameObject);
            MonsterManager.Instance.Clear_Count--;
        }
        else if (hp != Max_hp && Do_Battle)
        {
            Hp_bar.gameObject.SetActive(true);
            Hp_bar.value = (float)hp / (float)Max_hp;

        }
        else
        {
            Hp_bar.gameObject.SetActive(false);
        }
    }

    public IEnumerator GotoTarget() //타일 위치로 이동하는 코루틴
    {
        Do_Battle = false; //전투중인가를 확인하는 변수
        while (Vector3.Distance(transform.position, Targetlocation.GetComponent<Transform>().position)>=0.01f) //목표위치에 도달할때까지 반복
        {
            Vector3 dir = Targetlocation.transform.position - this.transform.position; //바라보게 하기위한 벡터 생성
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 3); //바라보게 각도 조절

            transform.position = Vector3.MoveTowards(transform.position,Targetlocation.GetComponent<Transform>().position, 3 * Time.deltaTime);
            yield return null;
        }

        Currentlocation = Targetlocation; // 현재위치 할당
        CurrentTile = Targettile; //현재 타일 할당
        MonsterManager.Instance.InTile(this.gameObject); //적이 있나 검사
    }

    public IEnumerator Battle_Monster() //
    {
        while (MonsterManager.Instance.isUnit(CurrentTile))//적유닛이 없을 때까지 반복
        {
            Do_Battle = true;
            yield return new WaitForSeconds(a_spd / 100); //공격 속도 이후에
            StartCoroutine(Attack());//공격코루틴 실행


        yield return new WaitForSeconds(a_spd /100);
        }
        Do_Battle = false;
        this.GetComponent<Animator>().SetBool("isIdle", false);//이동애니를위한 변수조정
        this.GetComponent<Animator>().SetBool("isAttack", false);//이동애니를위한 변수조정
        MonsterManager.Instance.Move(this.gameObject);//이동시작

        yield return null;
    }

    public IEnumerator Attack() {
       // print("전투중");

        if (TargetUnit == null && MonsterManager.Instance.isUnit(CurrentTile)) //타일에 유닛은 있으나 타겟유닛이없으면
        {
            int target = Random.Range(0, 3); //랜덤한 번호부여
            while (CurrentTile.GetComponent<Tile>().Unit[target] == null) //만약 해당번호의 타일유닛이 없으면
            { 
                target = Random.Range(0, 3); //있을 때까지 반복
            }
            TargetUnit = CurrentTile.GetComponent<Tile>().Unit[target];//타겟유닛 할당

        }


        if (TargetUnit != null) {//타겟 유닛이 있으면
            this.GetComponent<Animator>().SetBool("isAttack", true);
            if (TargetUnit != null)
            {
                SoundManager.Instance.SE_Play(Attack_sound, 1f); //공격 사운드 실행
                int dmg;
                dmg = BattleManager.Instance.Damage_Unit(this.gameObject, TargetUnit);//데미지 계산식 실행
                TargetUnit.GetComponent<Unit>().hp -= dmg; //데미지 계산
                MonsterManager.Instance.DamageFont_produce(dmg, TargetUnit); //데미지 폰트 생성
                Attack_Effect_on(); //이펙트 실행

            }

            if (TargetUnit.GetComponent<Unit>().hp < 0) //타겟유닛의 체력이 0이하면
            {
                TargetUnit = null; //타겟유닛 초기화
            }

        }
        
        yield return null;
        this.GetComponent<Animator>().SetBool("isAttack", false); //애니메이터 변수 초기화
    }

    void Attack_Effect_on()
    {
        //GameObject atkeffect = Instantiate(Attack_Effect_ob, TargetUnit.transform.position + new Vector3(0, 1, 0), gameObject.transform.rotation);
        //atkeffect.GetComponent<ParticleSystem>().Play();
        Attack_Effect.transform.position = TargetUnit.transform.position + new Vector3(0, 1, 0);
        //Attack_Effect.transform.SetParent(TargetUnit.transform);
        Attack_Effect.transform.SetParent(null);
        Attack_Effect.Play();
        print("몬스터 이펙트");
    }

}
