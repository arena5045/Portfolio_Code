
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public enum CardType
    {
        Active,
        Buff,
        DeBuff,
        Heal
    }

    public CardData data;

    public string cardName;
    public CardType type;
    public int cost;
    public int Value;
    public Sprite[] typeillust;

    public TMP_Text nametext;
    public Image typeimg;
    public Image cardImg;
    public TMP_Text costtext;
    public TMP_Text valuetext;
    public bool val_up;

    public GameObject orderImage;
    public TMP_Text orderText;

    public GameObject DisalbeImage;
    public CardBtn cardbtn;


    private void Awake()
    {
        cardbtn = GetComponent<CardBtn>();
       
    }
    private void Start()
    {
        SettingUi();
    }

    public void SettingUi()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        if (data == null)
        {
            data = BattleManager.Instance.Deck[Random.Range(0, BattleManager.Instance.Deck.Count)];
        }


        if (data != null)
        {
            cardName = data.cardName;
            nametext.text = cardName;
            cost = data.cost;
            costtext.text = cost.ToString();
            switch(data.type)
            {
                case CardData.Type.Active: type = CardType.Active; break;
                case CardData.Type.Buff: type = CardType.Buff; break;
                case CardData.Type.heal: type = CardType.Heal; break;
                case CardData.Type.Debuff: type = CardType.DeBuff; break;

            }
            
       switch(type)
      {
          case CardType.Active:
                    typeimg.sprite = typeillust[0];
              break;
          case CardType.Buff:
                    typeimg.sprite = typeillust[1];
                    break;

          case CardType.Heal:
                    typeimg.sprite = typeillust[2];
                    break;
                case CardType.DeBuff:

                    typeimg.sprite = typeillust[3];
                    break;
            }
     

            ValueSetting();

            cardImg.sprite = data.cardImg;

        }
        else
        {
            nametext.text = cardName;
            costtext.text = cost.ToString();
            valuetext.text = Value.ToString();
        }

        orderImage.SetActive(false);
        SetAble(true);
        cardbtn.SetRefresh();

        if (!cardbtn.isSelected && cost >BattleManager.Instance.CurCost)
        {
            SetAble(false);
        }
        else
        {
            SetAble(true);
        }


    }

    public void ValueSetting()
    {
        if (data == null)
            return;


        Value = data.value;

        if (BattleManager.Instance.BuffList.Count > 0)
        {
            foreach (int[] value in BattleManager.Instance.BuffList)
            {
                if (data.type == CardData.Type.Active)
                {
                    Value += (value[1] * cost);
                }
            }
        }
        if(Value != data.value)
        {
            valuetext.color = Color.green;
        }
        else
        {
            valuetext.color = Color.white;
        }
        valuetext.text = Value.ToString();

    }
    public void OrderSetting(bool isOn, int ordercount)
    {
        orderImage.SetActive(isOn);

        if (isOn)
        { orderText.text = ordercount.ToString(); }
    }


    public void SetAble(bool isAble)
    {
            gameObject.GetComponent<Button>().enabled = isAble;
            DisalbeImage.SetActive(!isAble);
    }


}
