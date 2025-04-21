using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalAttack : StateMachineBehaviour
{
    float delay = 0;
    bool check = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("공격모션 실행");

        check = false;
        delay = 0;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
   
        if (!check)
        {
            delay += Time.deltaTime;
        }
        else
        {
            return;
        }

        if(delay>0.2f)
        {
            check = true;
            GameObject player = animator.gameObject;
            GameObject target = player.GetComponentInParent<Player>().target;

            target.GetComponent<Monster>().Damaged(player.GetComponentInParent<Player>().Atk);
            Instantiate(player.GetComponentInParent<Player>().nomalhiteffect, target.transform.position +new Vector3(0,1f,0), target.transform.rotation);
            player.GetComponentInParent<PlayerSound>().PlayerNomalAttackSound();
            Debug.Log(target.GetComponent<Monster>().Hp);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject mainob = animator.gameObject;
        PlayerMovement pm = mainob.GetComponentInParent<PlayerMovement>();
        Player player = mainob.GetComponentInParent<Player>();
        player.target = null;
        Debug.Log("공격모션 종료");

        // mainob.transform.LookAt(player.target.transform);
        if(player.state != Player.PlayerState.Casting)
        {
            pm.CanMove();

        }

        //animator.gameObject
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
