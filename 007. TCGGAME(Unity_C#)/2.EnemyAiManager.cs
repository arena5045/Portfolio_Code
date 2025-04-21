using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiManager : MonoBehaviour
{
    public int cost;
    List<GameObject> Cards;


    int bigcost = 0;
    int mincost = 100;
    
    public static EnemyAiManager Instance { get; private set; }
    public void Awake()
    {
        Instance = this;
    }

    public void EnemyTurn() 
    {
        Main1();
    }

    public void Main1()
    {
        cost = GameManager.Instance.enemypp;
        Cards = CardManager.Instance.EnemyCards;
     

            GameObject UsingCard = null;
            foreach (GameObject card in Cards)
            {
                int cardcost = card.GetComponent<Card>().Cost;
                if (cardcost <= cost && cardcost > bigcost)
                {
                    UsingCard = card;
                }
                if (cardcost <= mincost)
                {
                    mincost = cardcost;
                }
            }

        if (UsingCard != null)
        {
            CardManager.Instance.EnemyCardSummon(UsingCard);
        }
        else
        {
            Main2();
        }
     
    }

    public bool Main1Recycle()//소환을 반복할 것인가 
    {
        cost = GameManager.Instance.enemypp;
        Cards = CardManager.Instance.EnemyCards;

        if (mincost < cost && Cards.Count > 1 &&  !GameManager.Instance.gameover)
            return true;
        else
            return false;
               

    }
    public void Main2()
    {
        StartCoroutine(AttackCor());
    }
    IEnumerator AttackCor()//공격 페이즈
    { float delay = 0.01f;
        Debug.Log("메인2 진입");
        foreach (GameObject enemy in GameManager.Instance.EnemyFields)
        {
            if (enemy.GetComponent<CardField>().OnField)//유닛이 존재하면
            {
                GameObject target = null ;
                bool CheckTarget = false;
                foreach (GameObject filed in GameManager.Instance.MyFields)
                {
                    target = filed;
                    if (target.GetComponent<CardField>().OnField)
                        CheckTarget = true;

                }
                if (CheckTarget == false || GameManager.Instance.gameover) 
                {
                    break;
                }
                do
                {  target = GameManager.Instance.MyFields[Random.Range(0, 9)]; }
                while (target.GetComponent<CardField>().OnField == false);//유닛있을때까지 타겟지정

                if (target != null)
                {
                    CardManager.Instance.EnemyCardAttack(enemy, target);
                    Debug.Log(enemy.name + "가 " + target + "을 공격");
                    delay = 1.3f;
                }   
            }
            else
            {
                delay = 0f;
            }
            yield return new WaitForSeconds(delay);
        }

       
        yield return new WaitForSeconds(1f); ;
        GameManager.Instance.NextTurn();
    }

    }
