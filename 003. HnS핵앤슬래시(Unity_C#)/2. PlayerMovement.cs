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

    public Transform playerCharacter;//�÷��̾� ĳ����

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


        if (player.state == Player.PlayerState.Trace) // �� �������ϰ��
        {

            if (agent.remainingDistance > player.Attack_Range) //���� ��Ÿ ��Ÿ� ���ϰ��
            {
                SetDest(player.target.transform.position);
            }
            else // ��Ÿ� ������ ��������
            {
                print("���ݽ���");
                agent.SetDestination(transform.position);
                player.PlayerAttack();
                isMove = false;
            }
        }
    }


    public void PlayerMove() //��Ŭ�������� ȣ��Ǵ� �Լ�
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, checkLand))
        {
            if (hit.transform.gameObject.layer == 7) // ������ üũ
            {
                if (!canMove) //�����ϼ� �ִ� �� �Ǵ� �������̸� ��⿭�� ����
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
                //player.PlayerRun(); // �̵� ��Ŵ
            }

            else if (hit.transform.gameObject.layer == 10) // ������̸�
            {
                return;
            }

        }
    }

    public void PlayerTargetMove(GameObject target) //�� Ŭ��������
    {

        SetDest(target.transform.position);
        player.Attack_Range = target.GetComponent<CapsuleCollider>().radius + 1;
        Debug.Log("�����Ÿ� : " + agent.remainingDistance);
        print("��ǥ�����Ϸ�");
        player.PlayerTrace(); // �̵� ��Ŵ
    }

    public void CanMove() //��Ÿ ������ Ǯ��
    {

        Debug.Log("���ݳ�!" + "���̺����� : " + isSavePos + " �̽��ؽ�Ʈ Ÿ��" + player.isNextTarget);


        canMove = true;



        if (isSavePos && !player.isNextTarget)
        {

            Debug.Log("�̵��մϴ�!");
            SetDest(saveMovePos);
            isSavePos = false;
            agent.velocity = agent.desiredVelocity.normalized * agent.speed;
            player.PlayerRun(); // �̵� ��Ŵ
        }
        else if (player.isNextTarget && player.next_target)
        {
            Debug.Log("�����մϴ�!");

            player.isNextTarget = false;
            player.target = player.next_target;
            player.next_target = null;


            PlayerTargetMove(player.target); //Ÿ���� ����

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

    public void LookMoveDirection() // �ش���ġ�� �ٶ󺸰� �ϴ� �Լ� => �� �����϶���
    {
        if (isMove)
        {

            if (agent.remainingDistance == 0.0f && agent.velocity.sqrMagnitude < 0.1f * 0.1f && player.state == Player.PlayerState.Run)  // �����̴� ���°� �ƴҶ�
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
        var targetRotation = Quaternion.LookRotation(dir); // ��ǥ ȸ���� ���

        // �ε巴�� ȸ���ϱ� ���� Quaternion.Slerp ���
        playerCharacter.transform.rotation = Quaternion.Slerp(playerCharacter.transform.rotation, targetRotation, 10 * Time.deltaTime);
    }
}
