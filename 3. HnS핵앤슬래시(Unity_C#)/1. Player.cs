using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum PlayerState
    { 
    Idle,
    Run,
    Trace,
    Dash,
    Casting,
    Attack,
    Skill,
    Death
    }
    public PlayerState state;

    private Camera cam;
    private PlayerMovement playermove;
    private PlayerSound playersound;
    [HideInInspector]
    public Animator animator;
    private SkinnedMeshAfterImage Afterglow;
    public Gradient[] Afterglowgradiant;
    private PhotonView pv;

    private Collider col;


    public GameObject target;
    public GameObject next_target;
    public bool isNextTarget;
    public bool canDash = true;

    public AnimationClip Testanim;
    public AnimationClip Testanim2;

    public GameObject nomalhiteffect;

    [HideInInspector]
    public SkillData Resent_Skill;//지금 사용중인 스킬 데이터

    [SerializeField]
    private int lv; //레벨

    [SerializeField]
    private int exp;//경험치

    [SerializeField]
    private int skillpoint;//경험치

    [SerializeField]
    private int max_hp;
    [SerializeField]
    private int max_mp;

    [SerializeField]
    private int cur_hp;
    [SerializeField]
    private int cur_mp;


    [SerializeField]
    private int atk;//공격력

    [SerializeField]
    private int str;//힘
    [SerializeField]
    private int igt;//마법공격력
    [SerializeField]
    private int dex;//민첩
    [SerializeField]
    private int def;//방어력

    [SerializeField]
    float attack_speed;//공격속도
    [SerializeField]
    float move_speed;//이동속도

    [SerializeField]
    float attack_Range;//공격범위(사거리)

    public int Lv { get { return lv; } set { lv = value; } }
    public int Exp { get { return exp; } set { exp = value; } }
    public int SkillPoint { get { return skillpoint; } set { skillpoint = value; } }

    public int Max_Hp {  get { return max_hp; } set { max_hp = value; } }
    public int Max_Mp { get { return max_mp; } set { max_mp = value; } }
    public int Cur_Hp
    {
        get { return cur_hp; }
        set
        {
            if ( value <= 0)
            {
                cur_hp = 0;
                print(cur_hp);
                PlayerDie();
            }
            else
            {
                cur_hp = value;
            }

        }
    }
    public int Cur_Mp { get { return cur_mp; } set { cur_mp = value; } }


    public int Atk { get { return atk; } set { atk = value; } }
    public int Str { get { return str; } set { str = value; } }
    public int Igt { get { return igt; } set { igt = value; } }
    public int Dex { get { return dex; } set { dex = value; } }
    public int Def { get { return def; } set { def = value; } }

    public float Attack_speed { get { return attack_speed; } set { attack_speed = value; } }
    public float Move_Speed { get { return move_speed; } set { move_speed = value; } }

    public float Attack_Range { get { return attack_Range; } set { attack_Range = value; } }


    // 테스트용 변수들
    public AnimatorOverrideController overrideController;

    public void SetState() 
    {
        Max_Hp = 1000;
        Max_Mp = 1000;
        cur_hp = Max_Hp;
        cur_mp = Max_Mp;

        Atk = 10;
        Str = 10;
        Igt = 10;
        Dex = 10;
        Def = 10;

        lv = 1;
        Attack_speed = 1f;
        Move_Speed = 5f;
        Attack_Range = 1.5f;

        SkillPoint = 5;
    }

    private void Awake()
    {
        isNextTarget = false;
        cam = Camera.main;
        playermove = GetComponent<PlayerMovement>();
        playersound = GetComponent<PlayerSound>();
        animator = GetComponentInChildren<Animator>();
        Afterglow = GetComponentInChildren<SkinnedMeshAfterImage>();
        pv = GetComponent<PhotonView>();
        SetState();

        col = GetComponent<Collider>();
    }

    public void PlayerRevival()
    {
        cur_hp = Max_Hp;
        cur_mp = Max_Mp;

        state = PlayerState.Idle;
        animator.Play("Idle");

        MainCanvasManager.Instance.DeadPannelShow();
        enabled = true;
        playermove.enabled = true;
        playersound.enabled = true;
    }


    private void Start()
    {
        state = PlayerState.Idle;
    }



    void Update()
    {
        playermove.LookMoveDirection();

        if (!pv.IsMine)
        {
            return;
        }


        if (canDash) 
        {
            Checkcollider();
        }


        if (Input.GetMouseButton(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject() && !PlayerManager.instance.OnUiInteraction)
            {
                playermove.PlayerMove();
            }
        }

        if (Input.GetMouseButtonUp(0)) // 몬스터 클릭시
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                MouseBtnUpCheck();
            }
        }


        //마우스 클릭했을때 아이템이면 주으러감



        if (Input.GetKeyDown(KeyCode.Z))
        {
            Damaged(150);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Cur_Hp = Max_Hp;
            Cur_Mp = Max_Mp;
        }

        /* 
                if (Input.GetKeyDown(KeyCode.W) && state != Player.PlayerState.Dash)
                {
                    PlayerDash(3f,0.25f);
                }


               if (Input.GetKeyDown(KeyCode.A))
                {
                    Resent_Skill = Resources.Load<SkillData>("_스킬/_공격기/02_검기 발사");
                    Resent_Skill.SkillEvent.Invoke();
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Resent_Skill = Resources.Load<SkillData>("_스킬/_공격기/04_스팅 러시");
                    Resent_Skill.SkillEvent.Invoke();
                }


                if (Input.GetKeyDown(KeyCode.S))
                {
                    Resent_Skill = Resources.Load<SkillData>("_스킬/_공격기/03_소드 레인");
                    Resent_Skill.SkillEvent.Invoke();
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    Resent_Skill = Resources.Load<SkillData>("_스킬/_공격기/01_빠른 참격");
                    Resent_Skill.SkillEvent.Invoke();
                }*/

    }

    private RaycastHit colhit; 

    private void Checkcollider() //전방에 몬스터가 있는지 판별
    {
        if (Physics.Raycast(transform.position+new Vector3(0,1,0), playermove.playerCharacter.transform.forward, out colhit, 0.5f))
        {
            if(colhit.transform.gameObject.layer == 10)
            {
                playermove.agent.speed = 0;
                //Debug.Log("hit point : " + colhit.point + ", distance : " + colhit.distance + ", name : " + colhit.collider.name);
            }
           
            //Debug.DrawRay(transform.position + new Vector3(0, 1, 0), playermove.playerCharacter.transform.forward * colhit.distance, Color.red);
        }
        else
        {
            playermove.agent.speed = Move_Speed ;
            //Debug.DrawRay(transform.position + new Vector3(0, 1, 0), playermove.playerCharacter.transform.forward * 0.5f, Color.red);
        }
    }


    public void MouseBtnUpCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
        {
            // print(hit.transform.gameObject.name + "클릭"); 클릭하는거 체크


            if (hit.transform.gameObject.layer == 10) //만약 클릭한게 몬스터였을경우
            {
                if (!playermove.canMove) // 못움직이면 = 이미 공격 중이면 다음 대기열에 저장
                {
                    if (  hit.transform.gameObject != target) //다른 타겟이면 대기열에 저장
                    {
                        isNextTarget = true;
                        next_target = hit.transform.gameObject;

                        playermove.saveMovePos = Vector3.zero;
                        playermove.isSavePos = false;

                        return;
                    }
                    else
                    { //같은애 클릭했을경우

                        isNextTarget = true;
                        next_target = hit.transform.gameObject;


                        playermove.saveMovePos = Vector3.zero;
                        playermove.isSavePos = false;

                        return;
                    }
                }
                // 공격중이아니라면
                target = hit.transform.gameObject;
                print(hit.transform.gameObject.name + "몬스터 클릭함!");
                playermove.PlayerTargetMove(target); //타겟팅 변경
            }
        }
    }
     
    public void PlayerCasting(float casttime)
    {
        playersound.AttackSound();
        playermove.canMove = false;
        playermove.agent.ResetPath();

        if (state != PlayerState.Casting)
        {
            state = PlayerState.Casting;

            Invoke("Castend", casttime);
        }

    }

    public void PlayerMoveCasting(float casttime)
    {
        playersound.AttackSound();
        playermove.canMove = false;
        //playermove.isMove = true;
        if (state != PlayerState.Casting)
        {
            state = PlayerState.Casting;

            Invoke("MoveCastend", casttime);
        }
    }

    public void MoveCastend()
    {
        playermove.canMove = true;
        playermove.isMove = false;
        //PlayerIdle();
        playermove.CanMove();
    }

    public void Castend()
    {
        playermove.canMove = true;
        playermove.isMove =false;
        //PlayerIdle();
        playermove.CanMove();
    }

    [PunRPC]
    public void PlayerRun()
    {
        if(state != PlayerState.Run ) 
        {
            state = PlayerState.Run;
            if(canDash)
            animator.SetTrigger("DoRun");
            playersound.MoveSound();
        }
    }

    public void PlayerTrace()
    {
        if (state != PlayerState.Trace)
        {
            playersound.AttackSound();
            print(target.name + "트레이스 온");
            state = PlayerState.Trace;
 
            print( "나 아직 길찾는중이야?" + playermove.agent.pathPending);
            //if (Vector3.Distance(playermove.playerCharacter.transform.position,target.transform.position) <= Attack_Range) //적이 평타 사거리 안일경우
            RaycastHit hit;
            Vector3 pos = target.transform.position - playermove.playerCharacter.position;
            int monsterLayer = 1 << 10;
            print("조건 1 : 사거리에 레이걸림?" + Physics.Raycast(playermove.playerCharacter.position, pos, out hit, Attack_Range));
            print("적 거리가 공격사거리야?" + (pos.magnitude <= Attack_Range));
            //문제 => 레이캐스트에 몬스터가 걸려버림

            if(pos.magnitude <= Attack_Range)
            {
                if(!Physics.Raycast(playermove.playerCharacter.position, pos, out hit, Attack_Range, ~monsterLayer))
                    {
                    Debug.Log("내 거리 : " + playermove.playerCharacter.position);
                    Debug.Log("목표거리 : " + playermove.agent.destination);
                    Debug.Log("사거리 : " + Attack_Range);
                    Debug.Log("바로때릴게요");
                }
                else
                {
                    Debug.Log("장애물  : " + hit.transform.gameObject.name);
                    Debug.Log("남은거리 : " + playermove.agent.remainingDistance);
                    Debug.Log("사거리 : " + Attack_Range);
                    Debug.Log("추적합니다");
                    animator.SetTrigger("DoRun");
                }
            }
            else
            {
                Debug.Log("남은거리 : " + playermove.agent.remainingDistance);
                Debug.Log("사거리 : " + Attack_Range);
                Debug.Log("추적합니다");
                animator.SetTrigger("DoRun");
            }


         /*   //문제 => 레이캐스트에 몬스터가 걸려버림
            if (!Physics.Raycast(playermove.playerCharacter.position, pos, out hit, Attack_Range, ~monsterLayer) && pos.magnitude >= Attack_Range) //적이 평타 사거리 안, 아무것도 없는 경우
            {
                Debug.Log("내 거리 : " + playermove.playerCharacter.position);
                Debug.Log("목표거리 : " + playermove.agent.destination);
                Debug.Log("장애물  : " + hit.transform.gameObject.name);
                Debug.Log("사거리 : " + Attack_Range);
                Debug.Log("바로때릴게요");
            }
            else //밖일경우
            {
                Debug.Log("장애물  : " + hit.transform.gameObject.name);
                Debug.Log("남은거리 : " + playermove.agent.remainingDistance);
                Debug.Log("사거리 : " + Attack_Range);
                Debug.Log("추적합니다");
                animator.SetTrigger("DoRun");
            }*/

        }
    }

    public void Damaged(int Damage) // 데미지 받는 함수
    {
        Cur_Hp -= Damage;
    }

    public void PlayerIdle()
    {
        if (state != PlayerState.Idle)
        {
            state = PlayerState.Idle;
            animator.SetTrigger("DoIdle");
        }
    }



    public void PlayerDie()
    {
        if (state != PlayerState.Death)
        {
            playermove.agent.ResetPath();

            state = PlayerState.Death;
            animator.SetTrigger("Die");

            MainCanvasManager.Instance.DeadPannelShow();
            enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            playermove.enabled = false;
            playersound.enabled = false;
        }
    }

    public void PlayerAttack() // 기본공격 구현
    {
        if (state != PlayerState.Attack)
        {
            playersound.AttackSound();
            print("때릴게");
            state = PlayerState.Attack;
            playermove.canMove = false;



            var dir = new Vector3(target.transform.position.x,target.transform.position.y,target.transform.position.z) - transform.position;
            playermove.playerCharacter.transform.forward = dir;

            animator.CrossFade("Attack", 0.1f);
            //animator.Play("Attack");
        }
    }

    public void PlayerDash(float speed , float Holdingtime)
    {
        if (state != PlayerState.Dash && canDash)
        {
            canDash = false;
            print("대쉬 시작");
            Afterglow._afterImageGradient = Afterglowgradiant[0];
            Afterglow.enabled = true;
            state = PlayerState.Dash;
            animator.SetTrigger("DoDash");
            playermove.agent.speed = Move_Speed * speed;
                Invoke("DashEnd", Holdingtime);
        }
    }

    public void SkillDash(float speed, float Holdingtime)
    { 
            canDash = false;
            Afterglow._afterImageGradient = Afterglowgradiant[1];
            Afterglow.enabled = true;
 
            playermove.agent.speed = Move_Speed * speed;
            Invoke("SkillDashEnd", Holdingtime);
    }
    public void SkillDashEnd()
    {
        canDash = true;
        playermove.playerCharacter.localPosition = Vector3.zero;
        Afterglow.enabled = false;
        playermove.agent.speed = Move_Speed;
        //PlayerIdle();
    }


    public void DashEnd()
    {
        canDash = true;
        print("대쉬 종료");
        playermove.playerCharacter.localPosition = Vector3.zero;
        Afterglow.enabled = false;
        playermove.agent.speed = Move_Speed;
        if (playermove.isMove)
        {
            animator.SetTrigger("DoRun");
            PlayerRun();
        }
        else
        {
            PlayerIdle();
        }
    }


}
