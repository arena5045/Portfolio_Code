using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public bool isClick = false;
    public bool isSelected = false;

    public Image outline;
    public Image selectedOutline;
    Vector2 startRect;

    PlayerMove pm;
    Card card;

    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {
        pm = FindAnyObjectByType<PlayerMove>();
        startRect = GetComponent<RectTransform>().anchoredPosition;
        card = GetComponent<Card>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetRefresh()
     {   
        isClick = false;
        isSelected = false;
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = startRect;
        outline.enabled = false;
        selectedOutline.enabled = false;
    }

        public void OnClick() 
    {

        if (BattleManager.Instance.SelectedCard !=null && BattleManager.Instance.SelectedCard != card)
            return;

       if(!isClick) //클릭하면
        {
            if (card.data.isAll) //전체공격 또는 대상지정 하지않는 카드
            {
                print("전체공격");
                Vector3 cp = gameObject.GetComponent<RectTransform>().anchoredPosition;
                cp.y += 20;
                gameObject.GetComponent<RectTransform>().anchoredPosition = cp;
                isClick = true;

                outline.enabled = false;
                selectedOutline.enabled = true;

                BattleManager.Instance.AddCard(gameObject.GetComponent<Card>());
                BattleManager.Instance.AddTarget(-1);
            }
            else//대상지정카드
            {
                Vector3 cp = gameObject.GetComponent<RectTransform>().anchoredPosition;
                cp.y += 20;
                gameObject.GetComponent<RectTransform>().anchoredPosition = cp;
                isClick = true;

                outline.enabled = true;
                selectedOutline.enabled = false;
                pm.MovePos();
                Debug.Log(isClick);

                BattleManager.Instance.AddCard(gameObject.GetComponent<Card>());
            }

        }
       else //취소하면
        {
            if(isSelected) //선택된 상태서 취소시
            {
                Vector3 cp = gameObject.GetComponent<RectTransform>().anchoredPosition;
                cp.y -= 20;
                gameObject.GetComponent<RectTransform>().anchoredPosition = cp;
                isClick = false;

                outline.enabled = false;
                selectedOutline.enabled = false;

                BattleManager.Instance.RemoveTarget(card);
            }
            else //그냥취소시
            {
                Vector3 cp = gameObject.GetComponent<RectTransform>().anchoredPosition;
                cp.y -= 20;
                gameObject.GetComponent<RectTransform>().anchoredPosition = cp;
                isClick = false;

                outline.enabled = false;
                selectedOutline.enabled = false;
                pm.MovePos();
                Debug.Log(isClick);
                BattleManager.Instance.RemoveCard();
            }
        }
    }


    public void Selected()
    {
        outline.enabled = false;
        selectedOutline.enabled = true;
        isSelected = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UiManager.Instance.skill_desc.text= card.data.explanation;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UiManager.Instance.skill_desc.text = "";
    }


}
