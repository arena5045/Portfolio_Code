using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;

public class Monster : MonoBehaviour
{
    [Serializable]
    public struct monsterPart
    {
        public bool isMain;
        public string PartName;
        public int maxhp;
        public int hp;
        public bool Active;


        public string NomalAttackName;
        public int NomalAttackDamage;


        public string SupperAttackName;
        public int SuperAttackDamage;

        public GameObject targettingUi;
        public GameObject hpUi;
        public TMP_Text Hptxt;
        public GameObject transRoot;

        public void Dead()
        {
             Active = false;
             print(Active);
             targettingUi.SetActive(false);
             hpUi.SetActive(false);
        }
    }

    public Animator animator;
    public bool isDead;
    public List<monsterPart> parts;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < parts.Count; i++) //각 파츠에 이름이랑 번호 할당
        {
            TargettingCanvas tc = parts[i].targettingUi.GetComponentInChildren<TargettingCanvas>();
            if (tc)
            {
                tc.partname = parts[i].PartName;
                tc.partnum = i;
            }
        }



    }


    private void Update()
    {
        foreach (monsterPart part in parts) 
        {
          part.hpUi.GetComponentInChildren<Slider>().value = (float)part.hp/part.maxhp;
            part.Hptxt.text = part.hp + " / " + part.maxhp;
        }
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        foreach(monsterPart part in parts)
        {
            part.Dead();
        }
        isDead = true;
        PlayerManager.Instance.GameClear();
    }

    public void AttackAnim(int code)
    {
        string trigger = "Attack" + code.ToString();
        animator.SetTrigger(trigger);
    }

    public void DamagedAnim(int code)
    {
        string trigger = "Damage" + code.ToString();
        animator.SetTrigger(trigger);
    }

    public void Damaged(int partsnum, Card card, bool effective)
    {
            monsterPart part = parts[partsnum];
            if (part.hp <= card.Value)//죽는 피면
            {
                card.Value = part.hp;
                part.hp -= card.Value;
                part.Dead();
                if(part.isMain)
               {
                Die();
               }
            }
            else
            {
                part.hp -= card.Value;
            }
            parts[partsnum] = part;

            if(!isDead && effective)
        {
            DamagedAnim(UnityEngine.Random.Range(1, 3));
            }
          
            print(part.PartName + "에 " + card.Value + "만큼의 데미지!");


        if(card.data != null && effective)
        { 
                GameObject hiteffect = Instantiate(card.data.effect, parts[partsnum].transRoot.transform);
                Destroy(hiteffect, card.data.casttime);
        }

        if(partsnum != 0) //본체친게아니면 본체도 데미지
        {
            Damaged(0, card, false);
        }
    }

    public void DamagedAll(Card card, bool effective)
    {

            for(int i =0; i< parts.Count; i++)
            {
                monsterPart part = parts[i];
                if (part.hp <= card.Value)//죽는 피면
                {
                    card.Value = part.hp;
                    part.hp -= card.Value;
                    part.Dead();
                if (part.isMain)
                {
                    Die();
                }
            }
                else
                {
                    part.hp -= card.Value;
                }
                parts[i] = part;
                print(part.PartName + "에 " + card.Value + "만큼의 데미지!");

            if (i != 0) //본체친게아니면 본체도 데미지
            {
                Damaged(0, card, false);
            }
        }
        if (!isDead)
        {
            DamagedAnim(UnityEngine.Random.Range(1, 3));
        }
        GameObject hiteffect = Instantiate(card.data.effect, gameObject.transform);
        hiteffect.transform.localPosition = new Vector3(0, 3, 0);
        Destroy(hiteffect, card.data.casttime);

    }

}
