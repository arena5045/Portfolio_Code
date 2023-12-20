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
                //�� Ŭ���� �ν��Ͻ��� ź������ �� �������� instance�� ���ӸŴ��� �ν��Ͻ��� ������� �ʴٸ�, �ڽ��� �־��ش�.
                instance = this;
                if (player != null)
                {
                player = FindObjectOfType<Player>().gameObject;
                player_s = player.GetComponent<Player>();
                player_m = player.GetComponent<PlayerMovement>();
                }
                else
                {
                //print("ã����");
                StartCoroutine(PlayerFind());
                }

            //�� ��ȯ�� �Ǵ��� �ı����� �ʰ� �Ѵ�.
            //gameObject�����ε� �� ��ũ��Ʈ�� ������Ʈ�μ� �پ��ִ� Hierarchy���� ���ӿ�����Ʈ��� ��������, 
            //���� �򰥸� ������ ���� this�� �ٿ��ֱ⵵ �Ѵ�.
            DontDestroyOnLoad(this.gameObject);
            }
            else
            {
            Debug.Log("�ߺ��ı���");
                //���� �� �̵��� �Ǿ��µ� �� ������ Hierarchy�� GameMgr�� ������ ���� �ִ�.
                //�׷� ��쿣 ���� ������ ����ϴ� �ν��Ͻ��� ��� ������ִ� ��찡 ���� �� ����.
                //�׷��� �̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ�(���ο� ���� GameMgr)�� �������ش�.
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
            //print("ã����");
            StartCoroutine(PlayerFind());
        }

    }

    bool SkillCostCheck(int cost) //�����ڽ�Ʈ
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
        print("�÷��̾���ݷ� = " + instance.player_s.Atk);

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
        if (!Skill_CoolDowns.ContainsKey(s_data.SkillName)) // ��ų�� ���̺귯���� ������ �߰�
        {
            Skill_LibraryData data = new Skill_LibraryData(s_data);
            Skill_CoolDowns.Add(s_data.SkillName, data);
        }

        Skill_LibraryData c_data = Skill_CoolDowns[s_data.SkillName];

        if (!c_data.CanSkill) //��ų�� üũ
        {
            return; // ��ų ���Ұ����ϸ� ����
        }
        if (!SkillCostCheck(s_data.ManaRequirement[s_data.SkillLV]))
            return; //���� ������ ���

        Player _plyaer = instance.player.GetComponent<Player>();
        instance.StartCoroutine(CoolTime(c_data));//���⼭���� ��
        SoundManager.instance.EffectPlay(s_data.SkillSound);//��ų ��� ȿ����
        SkillRot(s_data);//�÷��̾� ȸ��

        GameObject effect = Instantiate(s_data.Effect,instance.player.transform.position,instance.player_m.playerCharacter.rotation);
        SkillCollider skillCollider = effect.GetComponentInChildren<SkillCollider>();
        if (s_data.HitEffect != null)
        {
            skillCollider.HitEffect = s_data.HitEffect;
        }

        print("��� = " + s_data.Coefficient[0]);
        print("���� ������ = " + SkillDamageSet(instance.player_s.Atk, s_data.Coefficient[0]));
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
        if (!Skill_CoolDowns.ContainsKey(s_data.SkillName)) // ��ų�� ���̺귯���� ������ �߰�
        {
            Skill_LibraryData data = new Skill_LibraryData(s_data);
            Skill_CoolDowns.Add(s_data.SkillName, data);
        }
        Skill_LibraryData c_data = Skill_CoolDowns[s_data.SkillName];
        if (!c_data.CanSkill) //��ų�� üũ
        {
            return; // ��ų ���Ұ����ϸ� ����
        }
        if (!SkillCostCheck(s_data.ManaRequirement[s_data.SkillLV]))
            return; //���� ������ ���

        instance.StartCoroutine(CoolTime(c_data));//���⼭���� ��
        SoundManager.instance.EffectPlay(s_data.SkillSound);//��ų ��� ����Ʈ

        if (instance.player_s.state != Player.PlayerState.Dash)
        {
            instance.player_s.PlayerDash(3f, 0.25f);
        }

    }
    public void Flooring_Skill(SkillData s_data)
    {
        /////////////������� ��ų �⺻ ����

                    //instance.player_m.canMove = false;
        if (!Skill_CoolDowns.ContainsKey(s_data.SkillName)) // ��ų�� ���̺귯���� ������ �߰�
        {
            Skill_LibraryData data = new Skill_LibraryData(s_data);
            Skill_CoolDowns.Add(s_data.SkillName, data);
        }
        Skill_LibraryData c_data = Skill_CoolDowns[s_data.SkillName];
        if (!c_data.CanSkill) //��ų�� üũ
        {
            return; // ��ų ���Ұ����ϸ� ����
        }
        if (!SkillCostCheck(s_data.ManaRequirement[s_data.SkillLV]))
            return; //���� ������ ���

        Player _plyaer = instance.player.GetComponent<Player>();
        instance.StartCoroutine(CoolTime(c_data));//���⼭���� ��
        SoundManager.instance.EffectPlay(s_data.SkillSound);//��ų ��� ȿ����
        SkillRot(s_data);//�÷��̾� ȸ��
        /////////////�������


        SkillDispose(s_data); //���� ����
        print("��� = " + s_data.Coefficient[0]);
        print("���� ������ = " + SkillDamageSet(instance.player_s.Atk, s_data.Coefficient[0]));



        _plyaer.overrideController = new AnimatorOverrideController(_plyaer.animator.runtimeAnimatorController);
        _plyaer.overrideController["_"] = _plyaer.Resent_Skill.SkillMotion;
        _plyaer.animator.SetFloat("SkillSpeed", 2f);
        _plyaer.animator.runtimeAnimatorController = _plyaer.overrideController;


        _plyaer.animator.CrossFade("Skill_1", 0.05f);
    }

    public void TargetRushSkill(SkillData s_data) //Ÿ������ �����ϴ� ��ų
    {

        //instance.player_m.canMove = false;
        Debug.Log(name);
        if (!Skill_CoolDowns.ContainsKey(s_data.SkillName)) // ��ų�� ���̺귯���� ������ �߰�
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
                if (hit.transform.gameObject.layer == 10) //������ ������ ���
                {
                    target = hit.transform.gameObject; //Ÿ�ٿ� ����
                }
                else
                {
                    return;
                }

            }
        }

        if (!c_data.CanSkill || target == null) //��ų��,Ÿ���� üũ
        {
            return; // ��ų ���Ұ����ϸ� ����
        }

        if (!SkillCostCheck(s_data.ManaRequirement[s_data.SkillLV]))
            return; //���� ������ ���


        Player _plyaer = instance.player.GetComponent<Player>();
        instance.StartCoroutine(CoolTime(c_data));//���⼭���� ��
        SoundManager.instance.EffectPlay(s_data.SkillSound);//��ų ��� ȿ����
        SkillRot(s_data);//�÷��̾� ȸ��

        GameObject effect = Instantiate(s_data.Effect, instance.player.transform.position, instance.player_m.playerCharacter.rotation, instance.player.transform);//�ڽſ� ����Ʈ ����
        SkillCollider skillCollider = effect.GetComponentInChildren<SkillCollider>();

        if(s_data.HitEffect != null)
        {
            skillCollider.HitEffect = s_data.HitEffect;
        }


        Targetting(skillCollider as TargetSkillCollider, target);//Ÿ�����ϰ�!!
        Rush(target,5f,1f);//�÷��̾� �߽�!!!!

        skillCollider.StartCoroutine(skillCollider.ColliderOn(0.35F));

        print("��� = " + s_data.Coefficient[0]);
        print("���� ������ = " + SkillDamageSet(instance.player_s.Atk, s_data.Coefficient[0]));
        skillCollider.damage = SkillDamageSet(instance.player_s.Atk, s_data.Coefficient[0]);
        instance.player_s.PlayerMoveCasting(s_data.After_Delay);

        Destroy(effect, s_data.Effect_maintenance_time);


        _plyaer.overrideController = new AnimatorOverrideController(_plyaer.animator.runtimeAnimatorController);
        _plyaer.overrideController["_"] = _plyaer.Resent_Skill.SkillMotion;
        _plyaer.animator.SetFloat("SkillSpeed", 2f);
        _plyaer.animator.runtimeAnimatorController = _plyaer.overrideController;


        _plyaer.animator.CrossFade("Skill_1", 0.05f);
    }


    void SkillRot(SkillData s_data) //��ų���� ����ĳ���� �ڵ�ȸ��
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
                   tcol.target = target; //Ÿ�ٿ� ����
    }

    void Rush(GameObject target, float dashspeed, float maintain)
    {
        instance.player_m.agent.SetDestination(target.transform.position);
        instance.player_m.isMove = true;
        instance.player_s.SkillDash(dashspeed, maintain);//�÷��̾� �߽�!

    }


    void SkillDispose(SkillData s_data)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, instance.player_m.checkLand))
        {
            if (hit.transform.gameObject.layer == 7) // ������ üũ
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
        skill_Data.CoolDown = 0; //��Ÿ�� �� �ʱ�ȭ
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