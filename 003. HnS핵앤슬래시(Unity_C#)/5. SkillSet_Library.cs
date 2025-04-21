using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using static Player;
using static Unity.Burst.Intrinsics.X86;
using static UnityEngine.GraphicsBuffer;

public class SkillSet_Library : MonoBehaviour
{
   
    public static SkillSet_Library instance = null;

    public GameObject player;
    public Player player_s;
    public PlayerMovement player_m;

    Dictionary<string, Skill_LibraryData> Skill_CoolDowns = new Dictionary<string, Skill_LibraryData>();

    private void Awake()
    {
            if (null == instance)
            {
                //이 클래스 인스턴스가 탄생했을 때 전역변수 instance에 게임매니저 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
                instance = this;
                if (player != null)
                {
                player = FindObjectOfType<Player>().gameObject;
                player_s = player.GetComponent<Player>();
                player_m = player.GetComponent<PlayerMovement>();
                }
                else
                {
                //print("찾을게");
                StartCoroutine(PlayerFind());
                }

            //씬 전환이 되더라도 파괴되지 않게 한다.
            //gameObject만으로도 이 스크립트가 컴포넌트로서 붙어있는 Hierarchy상의 게임오브젝트라는 뜻이지만, 
            //나는 헷갈림 방지를 위해 this를 붙여주기도 한다.
            DontDestroyOnLoad(this.gameObject);
            }
            else
            {
            Debug.Log("중복파괴됨");
                //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
                //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
                //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 GameMgr)을 삭제해준다.
                Destroy(this.gameObject);
            }

    }

    IEnumerator PlayerFind()
    {
        while (PlayerManager.instance.player == null)
        {
            yield return null;
        }
        player = PlayerManager.instance.player;
        player_s = PlayerManager.instance.player_s;
        player_m = PlayerManager.instance.player_m;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (player != null)
        {
            player = FindObjectOfType<Player>().gameObject;
            player_s = player.GetComponent<Player>();
            player_m = player.GetComponent<PlayerMovement>();
        }
        else
        {
            //print("찾을게");
            StartCoroutine(PlayerFind());
        }

    }

    bool SkillCostCheck(int cost) //마나코스트
    {
        if(instance.player.GetComponent<Player>().Cur_Mp < cost)
        {
            return false;
        }
        else 
        {
            instance.player.GetComponent<Player>().Cur_Mp -= cost;
            return true;
        }
    }

    int SkillDamageSet(int Damage, float Coefficient)
    {
        print("플레이어공격력 = " + instance.player_s.Atk);

        int result_Damage = (int)((Damage * Coefficient)/100);

        return result_Damage;
    }

    public void SkillTest(GameObject player , GameObject target)
    {

        Debug.Log(player.name + target.name);

    }

    public void SkillTest2(SkillData s_data)
    {
        Debug.Log(instance.player.GetComponent<Player>().Cur_Hp);
       

        Debug.Log(s_data.skilltype);

        Debug.Log(s_data.SkillName);

    }   

    public void Active_Slash(SkillData s_data)
    {
      
        //instance.player_m.canMove = false;
        Debug.Log(name);
        if (!Skill_CoolDowns.ContainsKey(s_data.SkillName)) // 스킬이 라이브러리에 없으면 추가
        {
            Skill_LibraryData data = new Skill_LibraryData(s_data);
            Skill_CoolDowns.Add(s_data.SkillName, data);
        }

        Skill_LibraryData c_data = Skill_CoolDowns[s_data.SkillName];

        if (!c_data.CanSkill) //스킬쿨 체크
        {
            return; // 스킬 사용불가능하면 리턴
        }
        if (!SkillCostCheck(s_data.ManaRequirement[s_data.SkillLV]))
            return; //마나 딸리면 취소

        Player _plyaer = instance.player.GetComponent<Player>();
        instance.StartCoroutine(CoolTime(c_data));//여기서부터 쿨돔
        SoundManager.instance.EffectPlay(s_data.SkillSound);//스킬 사용 효과음
        SkillRot(s_data);//플레이어 회전

        GameObject effect = Instantiate(s_data.Effect,instance.player.transform.position,instance.player_m.playerCharacter.rotation);
        SkillCollider skillCollider = effect.GetComponentInChildren<SkillCollider>();
        if (s_data.HitEffect != null)
        {
            skillCollider.HitEffect = s_data.HitEffect;
        }

        print("계수 = " + s_data.Coefficient[0]);
        print("예상 데미지 = " + SkillDamageSet(instance.player_s.Atk, s_data.Coefficient[0]));
        skillCollider.damage = SkillDamageSet(instance.player_s.Atk, s_data.Coefficient[0]);
        instance.player_s.PlayerCasting(s_data.After_Delay);

        Destroy(effect, s_data.Effect_maintenance_time);


        _plyaer.overrideController = new AnimatorOverrideController(_plyaer.animator.runtimeAnimatorController);
        _plyaer.overrideController["_"] = _plyaer.Resent_Skill.SkillMotion;
        _plyaer.animator.SetFloat("SkillSpeed", 5f);
        _plyaer.animator.runtimeAnimatorController = _plyaer.overrideController;


        _plyaer.animator.CrossFade("Skill_1", 0.05f);
    }

    public void Dash(SkillData s_data)
    {
        if (!Skill_CoolDowns.ContainsKey(s_data.SkillName)) // 스킬이 라이브러리에 없으면 추가
        {
            Skill_LibraryData data = new Skill_LibraryData(s_data);
            Skill_CoolDowns.Add(s_data.SkillName, data);
        }
        Skill_LibraryData c_data = Skill_CoolDowns[s_data.SkillName];
        if (!c_data.CanSkill) //스킬쿨 체크
        {
            return; // 스킬 사용불가능하면 리턴
        }
        if (!SkillCostCheck(s_data.ManaRequirement[s_data.SkillLV]))
            return; //마나 딸리면 취소

        instance.StartCoroutine(CoolTime(c_data));//여기서부터 쿨돔
        SoundManager.instance.EffectPlay(s_data.SkillSound);//스킬 사용 이펙트

        if (instance.player_s.state != Player.PlayerState.Dash)
        {
            instance.player_s.PlayerDash(3f, 0.25f);
        }

    }
    public void Flooring_Skill(SkillData s_data)
    {
        /////////////여기부터 스킬 기본 세팅

                    //instance.player_m.canMove = false;
        if (!Skill_CoolDowns.ContainsKey(s_data.SkillName)) // 스킬이 라이브러리에 없으면 추가
        {
            Skill_LibraryData data = new Skill_LibraryData(s_data);
            Skill_CoolDowns.Add(s_data.SkillName, data);
        }
        Skill_LibraryData c_data = Skill_CoolDowns[s_data.SkillName];
        if (!c_data.CanSkill) //스킬쿨 체크
        {
            return; // 스킬 사용불가능하면 리턴
        }
        if (!SkillCostCheck(s_data.ManaRequirement[s_data.SkillLV]))
            return; //마나 딸리면 취소

        Player _plyaer = instance.player.GetComponent<Player>();
        instance.StartCoroutine(CoolTime(c_data));//여기서부터 쿨돔
        SoundManager.instance.EffectPlay(s_data.SkillSound);//스킬 사용 효과음
        SkillRot(s_data);//플레이어 회전
        /////////////여기까지


        SkillDispose(s_data); //장판 생성
        print("계수 = " + s_data.Coefficient[0]);
        print("예상 데미지 = " + SkillDamageSet(instance.player_s.Atk, s_data.Coefficient[0]));



        _plyaer.overrideController = new AnimatorOverrideController(_plyaer.animator.runtimeAnimatorController);
        _plyaer.overrideController["_"] = _plyaer.Resent_Skill.SkillMotion;
        _plyaer.animator.SetFloat("SkillSpeed", 2f);
        _plyaer.animator.runtimeAnimatorController = _plyaer.overrideController;


        _plyaer.animator.CrossFade("Skill_1", 0.05f);
    }

    public void TargetRushSkill(SkillData s_data) //타겟팅후 돌진하는 스킬
    {

        //instance.player_m.canMove = false;
        Debug.Log(name);
        if (!Skill_CoolDowns.ContainsKey(s_data.SkillName)) // 스킬이 라이브러리에 없으면 추가
        {
            Skill_LibraryData data = new Skill_LibraryData(s_data);
            Skill_CoolDowns.Add(s_data.SkillName, data);
        }
        Skill_LibraryData c_data = Skill_CoolDowns[s_data.SkillName];

        GameObject target=null;
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, instance.player_m.checkLand))
            {
                if (hit.transform.gameObject.layer == 10) //맞은게 몬스터일 경우
                {
                    target = hit.transform.gameObject; //타겟에 저장
                }
                else
                {
                    return;
                }

            }
        }

        if (!c_data.CanSkill || target == null) //스킬쿨,타겟쿨 체크
        {
            return; // 스킬 사용불가능하면 리턴
        }

        if (!SkillCostCheck(s_data.ManaRequirement[s_data.SkillLV]))
            return; //마나 딸리면 취소


        Player _plyaer = instance.player.GetComponent<Player>();
        instance.StartCoroutine(CoolTime(c_data));//여기서부터 쿨돔
        SoundManager.instance.EffectPlay(s_data.SkillSound);//스킬 사용 효과음
        SkillRot(s_data);//플레이어 회전

        GameObject effect = Instantiate(s_data.Effect, instance.player.transform.position, instance.player_m.playerCharacter.rotation, instance.player.transform);//자신에 이펙트 생성
        SkillCollider skillCollider = effect.GetComponentInChildren<SkillCollider>();

        if(s_data.HitEffect != null)
        {
            skillCollider.HitEffect = s_data.HitEffect;
        }


        Targetting(skillCollider as TargetSkillCollider, target);//타겟팅하고!!
        Rush(target,5f,1f);//플레이어 발싸!!!!

        skillCollider.StartCoroutine(skillCollider.ColliderOn(0.35F));

        print("계수 = " + s_data.Coefficient[0]);
        print("예상 데미지 = " + SkillDamageSet(instance.player_s.Atk, s_data.Coefficient[0]));
        skillCollider.damage = SkillDamageSet(instance.player_s.Atk, s_data.Coefficient[0]);
        instance.player_s.PlayerMoveCasting(s_data.After_Delay);

        Destroy(effect, s_data.Effect_maintenance_time);


        _plyaer.overrideController = new AnimatorOverrideController(_plyaer.animator.runtimeAnimatorController);
        _plyaer.overrideController["_"] = _plyaer.Resent_Skill.SkillMotion;
        _plyaer.animator.SetFloat("SkillSpeed", 2f);
        _plyaer.animator.runtimeAnimatorController = _plyaer.overrideController;


        _plyaer.animator.CrossFade("Skill_1", 0.05f);
    }


    void SkillRot(SkillData s_data) //스킬쓸때 스마캐마냥 자동회전
    {

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Vector3 hitpos = hit.point;
                hitpos.y = instance.player.transform.position.y;

                instance.player_m.playerCharacter.transform.LookAt(hitpos);

            }
        }
    }

    void Targetting(TargetSkillCollider tcol, GameObject target) 
    {
                   tcol.target = target; //타겟에 저장
    }

    void Rush(GameObject target, float dashspeed, float maintain)
    {
        instance.player_m.agent.SetDestination(target.transform.position);
        instance.player_m.isMove = true;
        instance.player_s.SkillDash(dashspeed, maintain);//플레이어 발싸!

    }


    void SkillDispose(SkillData s_data)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, instance.player_m.checkLand))
        {
            if (hit.transform.gameObject.layer == 7) // 땅인지 체크
            {
                Vector3 hitpos = hit.point;
                GameObject effect = Instantiate(s_data.Effect, hitpos, instance.player_m.playerCharacter.rotation);
                SkillCollider skillCollider = effect.GetComponentInChildren<SkillCollider>();
                if (s_data.HitEffect != null)
                {
                    skillCollider.HitEffect = s_data.HitEffect;
                }

                if (s_data.activetype == SkillData.TargetType.Flooring)
                {
                    FlooringDamage f_Collider = skillCollider as FlooringDamage;
                    f_Collider.tick_time = s_data.Tick_time;
                }

                skillCollider.damage = SkillDamageSet(instance.player_s.Atk, s_data.Coefficient[0]);
                instance.player_s.PlayerCasting(s_data.After_Delay);

                Destroy(effect, s_data.Effect_maintenance_time);
            }
            else if (hit.transform.gameObject.layer == 10)
            {
                Vector3 hitpos = hit.transform.gameObject.transform.position;
                GameObject effect = Instantiate(s_data.Effect, hitpos, instance.player_m.playerCharacter.rotation);
                SkillCollider skillCollider = effect.GetComponentInChildren<SkillCollider>();
                if (s_data.HitEffect != null)
                {
                    skillCollider.HitEffect = s_data.HitEffect;
                }

                if (s_data.activetype == SkillData.TargetType.Flooring)
                {
                    FlooringDamage f_Collider = skillCollider as FlooringDamage;
                    f_Collider.tick_time = s_data.Tick_time;
                }

                skillCollider.damage = SkillDamageSet(instance.player_s.Atk, s_data.Coefficient[0]);
                instance.player_s.PlayerCasting(s_data.After_Delay);

                Destroy(effect, s_data.Effect_maintenance_time);
            }

        }
    }
    IEnumerator CoolTime(Skill_LibraryData skill_Data)
    {
        Image coolimg = KeyInputManager.instance.ResentCheckSkill.skill_Colimg;
        KeyInputManager.instance.ResentCheckSkill = null;
        skill_Data.CoolDown = 0; //쿨타임 값 초기화
        skill_Data.CanSkill = false;
        while (skill_Data.CoolDown <= skill_Data.s_data.SkillCoolDown) 
        {
            skill_Data.CoolDown += Time.deltaTime;
            coolimg.fillAmount = 1 - (skill_Data.CoolDown / skill_Data.s_data.SkillCoolDown);
            yield return null;
        }

        skill_Data.CanSkill = true;

    }
}

class Skill_LibraryData
{
    public SkillData s_data;
    public float CoolDown =0;
    public bool CanSkill = true;

    public Skill_LibraryData(SkillData data)
    {
        s_data = data;
        CoolDown = data.SkillCoolDown;
        CanSkill = true;
    }
}