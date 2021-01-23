using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public int Unit_Code;
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
    public int m_spd; // 스텟들

    public AudioClip Attack_sound;
    public AudioClip Deadsound;

    public GameObject Effect;
    public ParticleSystem Attack_Effect;

    public int stack=0;//여러 공격에서 사용되는 스택
    public int end_stack = -1; //쌓아야 하는 스택양
    public int stack_buff=0;//스택이후 추가되는 공격력

    float t = 0;
    float a_speed;

    public GameObject TargetUnit;//공격대상
    public Tile Current_Tile; // 현재 배치된 타일
    public GameObject Current_Location; // 현재 배치된 타일위치
    public string Current_Location_number;// 현재 배치된 타일번호
    public Slider Hp_bar;

    public bool isbattile=false;

    public int rage; //격노 스택
    public bool Berserk=false; //상시 격노상태 유무

    public GameObject atkbuff;

    void Start()
    {


        Code = GetUnitSOInfo.Instance.getUnitCode(Unit_Code);
        Name = GetUnitSOInfo.Instance.getUnitName(Unit_Code);
        //sprite = GetEnemyInfo.Instance.ge
        grade = GetUnitSOInfo.Instance.getUnitGrade(Unit_Code);
        description = GetUnitSOInfo.Instance.getUnitDescript(Unit_Code);
        Max_hp = Mathf.FloorToInt(GetUnitSOInfo.Instance.getUnitHp(Unit_Code));
        hp = Mathf.FloorToInt(GetUnitSOInfo.Instance.getUnitHp(Unit_Code));
        atk = Mathf.FloorToInt(GetUnitSOInfo.Instance.getUnitAtk(Unit_Code));
        def = Mathf.FloorToInt(GetUnitSOInfo.Instance.getUnitDef(Unit_Code));
        a_spd = GetUnitSOInfo.Instance.getUnitAtkSp(Unit_Code);
        m_spd = GetUnitSOInfo.Instance.getUnitMvSp(Code);

        Location_number_Setting();
        Reset();
    }

    public void Location_number_Setting() {
        Current_Location_number = Current_Location.name;
        Current_Location_number = Current_Location_number[Current_Location_number.Length - 1].ToString();
        a_speed = a_spd;
    }


    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Current_Tile.UnitDie(int.Parse(Current_Location_number));
            print(Current_Location_number+"번 " +this.name+"사망");
            this.gameObject.SetActive(false);

            // Destroy(this.gameObject);
        }
        else if (hp != Max_hp)
        {
            Hp_bar.gameObject.SetActive(true);
            Hp_bar.value = (float)hp / (float)Max_hp;

        }else if (hp >= Max_hp)
        {
            Hp_bar.gameObject.SetActive(false);
        }



        if (rage > 0 || Berserk == true || stack == end_stack)
        { atkbuff.gameObject.SetActive(true); }
        else
        { atkbuff.gameObject.SetActive(false);}


        if (isbattile == false)
        {
            t += Time.deltaTime;
        }
        if (t > a_speed / 100.0f) 
        {
            this.transform.position = Current_Location.transform.position;

            if (UnitManager.Instance.isMons(Current_Tile)&&TargetUnit==null) 
            {
            int target = Random.Range(0, 3);
            while (Current_Tile.GetComponent<Tile>().Mons[target] == null)
            {
                target = Random.Range(0, 3);
            }
            TargetUnit = Current_Tile.GetComponent<Tile>().Mons[target];//타겟유닛 할당
                this.GetComponent<Animator>().SetBool("isAttack", false);
                if (isbattile == false) {
                    StartCoroutine(Attack());
                }
               
            }
           // t = 0;
        }
    }

   public void Reset()
    {
        this.transform.position = Current_Location.transform.position;
    }
    public IEnumerator Attack()
    {
        isbattile = true;
          t = 0;
        while (TargetUnit != null) {
            this.GetComponent<Animator>().SetBool("isAttack", true);
            Invoke("Motion_off", 0.1f);
            int dmg;
            SoundManager.Instance.SE_Play(Attack_sound,1f);
            dmg = BattleManager.Instance.Damage_Monster(this.gameObject, TargetUnit);
            TargetUnit.GetComponent<Monster>().hp -= dmg;

            MonsterManager.Instance.DamageFont_produce(dmg, TargetUnit);
            Attack_Effect_on();

                stack++;
            if (UnitManager.Instance.isMons(Current_Tile) && TargetUnit == null)
            {
                int target = Random.Range(0, 3);
                while (Current_Tile.GetComponent<Tile>().Mons[target] == null)
                {
                    target = Random.Range(0, 3);
                }
                TargetUnit = Current_Tile.GetComponent<Tile>().Mons[target];//타겟유닛 할당
            }
            print("적hp  " + TargetUnit.GetComponent<Monster>().hp);
          
            t = 0;
            yield return null;
            //this.GetComponent<Animator>().SetBool("isAttack", false);


                yield return new WaitForSeconds(a_speed / 100.0f);

         
        }
        isbattile = false;



        yield return null;
        t = a_speed / 100.0f;
    }


    public void GetUnit()
    {
        Current_Tile = TileManager.Instance.tiles[11];
        Current_Location = TileManager.Instance.tiles[11].tar_lo;
    }


    void Attack_Effect_on() {
        Effect.transform.position = TargetUnit.transform.position+ new Vector3(0,1,0);
        Attack_Effect.Play();
    }

   public  void Skill1_Motion_on()
    {
        this.GetComponent<Animator>().SetBool("Skill1", true);
        Invoke("Motion_off",0.1f);
    }
    public void Skill2_Motion_on()
    {
        this.GetComponent<Animator>().SetBool("Skill2", true);
        Invoke("Motion_off", 0.1f);
    }
    public void Skill3_Motion_on()
    {
        this.GetComponent<Animator>().SetBool("Skill3", true);
        Invoke("Motion_off", 0.1f);
    }

    void Motion_off()
    {
        this.GetComponent<Animator>().SetBool("isAttack", false);
        this.GetComponent<Animator>().SetBool("Skill1", false);
        this.GetComponent<Animator>().SetBool("Skill2", false);
        this.GetComponent<Animator>().SetBool("Skill3", false);
    }

    public void Heal_Unit(int heal)
    {
        if (heal >= (Max_hp - hp))//힐량이 최대hp보다 높게 힐될경우
        {
            hp = Max_hp;
        }
        else {

            hp += heal;
        }
    }


    public void Disenchant() 
    {
    stack = 0;//여러 공격에서 사용되는 스택
    end_stack = -1; //쌓아야 하는 스택양
    stack_buff = 0;//스택이후 추가되는 공격력

    rage = 0; //격노 스택
    Berserk = false; //상시 격노상태 유무
}
}
