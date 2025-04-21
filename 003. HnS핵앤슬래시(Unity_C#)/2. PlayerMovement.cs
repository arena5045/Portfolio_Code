using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    private Camera cam;
    [HideInInspector]
    public NavMeshAgent agent;

    public bool isMove;

    public bool canMove;

    public LayerMask checkLand;
    public Vector3 saveMovePos;
    public bool isSavePos;

    public Transform playerCharacter;//플레이어 캐릭터

    Player player;
    PlayerSound playersound;
    PhotonView pv;
    private void Awake()
    {
        cam = Camera.main;
        canMove = true;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        player = GetComponent<Player>();
        playersound = GetComponent<PlayerSound>();
        pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!pv.IsMine)
        {
            return;
        }


        if (player.state == Player.PlayerState.Trace) // 적 추적중일경우
        {

            if (agent.remainingDistance > player.Attack_Range) //적이 평타 사거리 밖일경우
            {
                SetDest(player.target.transform.position);
            }
            else // 사거리 안으로 들어왔으면
            {
                print("공격실행");
                agent.SetDestination(transform.position);
                player.PlayerAttack();
                isMove = false;
            }
        }
    }


    public void PlayerMove() //땅클릭했을때 호출되는 함수
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, checkLand))
        {
            if (hit.transform.gameObject.layer == 7) // 땅인지 체크
            {
                if (!canMove) //움직일수 있는 지 판단 못움직이면 대기열에 저장
                {
                    saveMovePos = hit.point;
                    isSavePos = true;

                    player.next_target = null;
                    player.isNextTarget = false;

                    return;
                }
                
                player.target = null;

                pv.RPC(nameof(SetDest),RpcTarget.All, hit.point);
                //SetDest(hit.point);


                agent.velocity = agent.desiredVelocity.normalized * agent.speed;

                pv.RPC(nameof(player.PlayerRun), RpcTarget.All,null);
                //player.PlayerRun(); // 이동 시킴
            }

            else if (hit.transform.gameObject.layer == 10) // 만약몹이면
            {
                return;
            }

        }
    }

    public void PlayerTargetMove(GameObject target) //적 클릭했을때
    {

        SetDest(target.transform.position);
        player.Attack_Range = target.GetComponent<CapsuleCollider>().radius + 1;
        Debug.Log("남은거리 : " + agent.remainingDistance);
        print("목표지정완료");
        player.PlayerTrace(); // 이동 시킴
    }

    public void CanMove() //평타 움직임 풀때
    {

        Debug.Log("공격끝!" + "세이브포스 : " + isSavePos + " 이스넥스트 타겟" + player.isNextTarget);


        canMove = true;



        if (isSavePos && !player.isNextTarget)
        {

            Debug.Log("이동합니다!");
            SetDest(saveMovePos);
            isSavePos = false;
            agent.velocity = agent.desiredVelocity.normalized * agent.speed;
            player.PlayerRun(); // 이동 시킴
        }
        else if (player.isNextTarget && player.next_target)
        {
            Debug.Log("공격합니다!");

            player.isNextTarget = false;
            player.target = player.next_target;
            player.next_target = null;


            PlayerTargetMove(player.target); //타겟팅 변경

        }
        else if (player.state == Player.PlayerState.Casting)
        {
            player.PlayerIdle();
        }
    }



    [PunRPC]
    public void SetDest(Vector3 dest)
    {
        agent.SetDestination(dest);
        isMove = true;
    }

    public void LookMoveDirection() // 해당위치를 바라보게 하는 함수 => 단 움직일때만
    {
        if (isMove)
        {

            if (agent.remainingDistance == 0.0f && agent.velocity.sqrMagnitude < 0.1f * 0.1f && player.state == Player.PlayerState.Run)  // 움직이는 상태가 아닐때
            {
                player.PlayerIdle();
                isMove = false;

                return;
            }

            if (agent.remainingDistance != 0.0f)
            {
                var dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;
                LerfRot(dir);
            }

        }
    }


    public void LerfRot(Vector3 dir)
    {
        var targetRotation = Quaternion.LookRotation(dir); // 목표 회전값 계산

        // 부드럽게 회전하기 위해 Quaternion.Slerp 사용
        playerCharacter.transform.rotation = Quaternion.Slerp(playerCharacter.transform.rotation, targetRotation, 10 * Time.deltaTime);
    }
}
