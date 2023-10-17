using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


public class CardManager : MonoBehaviour
{

    public static CardManager Instance { get; private set; }

    public GameObject PlaceHolder;
    public GameObject Parent_Canvas;

    public GameObject Hand;
    public GameObject Hand_set;
    public bool showhan= false;
    public bool savehan;
    Vector3 hantr, hantr_on;

    public GameObject SaveZone;

    public bool targeting = false;

    public List<GameObject> Cards;
    public List<GameObject> EnemyCards;

    public GameObject Targettingob, Targetting_Image, Targetting_Lay,Targetting_target;
    GameObject parent;
    int siblingindex;
    public bool isdragged;
    public bool isField_dragged;

    Vector3 savepoint;
    public void Awake()
    {
        Instance = this;
        isdragged = false;
        isField_dragged = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        hantr = Hand_set.transform.position;
       // hantr_on = hantr;
        hantr_on = Hand_set.transform.position + new Vector3(0, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FieldPointerDown(GameObject Field) //필드에서 카드가 눌렸을때 호출되는 함수
    {
        isField_dragged = false;

        //Targettingob.SetActive(true);
        Targetting_Image.SetActive(true);
        Targetting_Lay.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
        Targetting_Lay.SetActive(false);
        Targetting_target.SetActive(false);
        Targetting_Image.transform.position = Field.transform.position;
    }

    public void FieldPointerUp(GameObject Field) //필드에서 카드를 뗏을때 호출되는 함수
    {


        Targetting_Image.SetActive(false);
        Targetting_Image.transform.position = Field.transform.position;

        GraphicRaycaster gr = Parent_Canvas.GetComponent<GraphicRaycaster>();
        var ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> result = new List<RaycastResult>();
        gr.Raycast(ped, result);



        if (result[0].gameObject.tag == "Field" && result[0].gameObject.GetComponent<CardField>().OnField == true && Field != result[0].gameObject) //몬스터 필드에 몬스터가 있는 상태로 떨어뜨린 경우(공격시)
        {

          
            GameObject TARGET_FIELD = result[0].gameObject;
            CardField target_filed = TARGET_FIELD.GetComponent<CardField>();
            Transform reset_tr = Field.GetComponent<Transform>();
            // Sequence Attack_sq = DOTween.Sequence();
            // Attack_sq.Append(Field.GetComponent<CardField>().CardZone.transform.DOMove(TARGET_FIELD.transform.position, 0.5f).SetEase(Ease.InQuint));

            if (!BattleCheck(Field.GetComponent<CardField>(), target_filed))
            {
                return;
            }
            Field.GetComponent<CardField>().SaveCard.GetComponent<Card>().OnBattle();
            Field.GetComponent<CardField>().CanAttack = false;
            Field.GetComponent<CardField>().CardZone.transform.SetParent(SaveZone.transform);


            Field.GetComponent<CardField>().CardZone.transform.DOMove(TARGET_FIELD.transform.position, 0.5f).SetEase(Ease.InQuint).OnComplete(() => {
                SoundManager.Instance.AttackSound_Play();

                if (Battle(Field.GetComponent<CardField>(), target_filed)) //공격
                {//안죽었으면 =>반격
                    target_filed.CardZone.transform.SetParent(SaveZone.transform);

                    Transform savetr = TARGET_FIELD.transform;
                    target_filed.CardZone.transform.DOMove(savetr.position + new Vector3(1,0,0) , 0.35f).OnComplete(() => 
                        {
                            target_filed.CardZone.transform.DOMove(savetr.position,0.15f).OnComplete(()=> {
                                SoundManager.Instance.AttackSound_Play();
                                Battle(target_filed, Field.GetComponent<CardField>());
                                target_filed.CardZone.transform.SetParent(TARGET_FIELD.transform);
                                Field.GetComponent<CardField>().CardZone.transform.DOMove(reset_tr.position, 0.5f).OnComplete(() => {
                                    Field.GetComponent<CardField>().CardZone.transform.SetParent(Field.transform);
                                    Field.GetComponent<CardField>().EndAttack();
                                });
                            });
                        }
                    );

                }
                else
                {//죽었으면 끝
                    Field.GetComponent<CardField>().CardZone.transform.DOMove(reset_tr.position, 0.5f).OnComplete(() => {
                        Field.GetComponent<CardField>().CardZone.transform.SetParent(Field.transform);
                        Field.GetComponent<CardField>().EndAttack();
                    });
                }



            });
            target_filed.OffTargetting();

        }
        if (result[0].gameObject.tag == "Field" && result[0].gameObject.GetComponent<CardField>().OnField == true && isField_dragged == false)
        {
            Debug.Log("이러면 실행이지");
            result[0].gameObject.GetComponent<CardField>().SaveCard.GetComponent<Card>().info_popup_open();
            //Card card = result[0].gameObject.GetComponent<CardField>().SaveCard.GetComponent<Card>();
            //PopupManager.Instance.Show_InfoPopup(card.Card_data, card.Rarity); 
        }
            isField_dragged = false;
            targeting = false;
    }
    public bool Battle(CardField Attack, CardField Defend)
    {

        int atk = int.Parse(Attack.Atk_text.text);
        int def = int.Parse(Defend.Hp_text.text);
       
        //int damage = def - atk;
        int newhp = def - atk;
        if (newhp > 0)
        {
            Defend.Hp_text.text = newhp.ToString();
            Defend.SaveCard.GetComponent<Card>().c_Hp = newhp;
            //Battle(Defend, Attack);
            return true;
        }
        else
        {
            if (Defend.SaveCard.GetComponent<Card>().Species == Card.species.Leader)
            {
                Defend.Hp_text.text = "0";
                Defend.SaveCard.GetComponent<Card>().c_Hp = 0;
                return false; 
            }

            Defend.CardZone.SetActive(false);
            Defend.GetComponent<Outline>().enabled = false;
            Defend.CanAttack = false;
            Destroy(Defend.SaveCard);
            Defend.OnField = false;
            return false;
        }

    }

    public bool BattleCheck(CardField Attack, CardField Defend)
    {
        if (Attack.friendly && !Defend.friendly)
        {
            return true;
        }
        return false;
    
    }
    public void FieldPointerDrag(GameObject Field) //필드에서 카드를 드래그될때 호출되는 함수
    {
        isField_dragged = true;
        targeting = true;

        Targetting_Lay.SetActive(true);
        Targetting_target.SetActive(true);

        Vector3 vector3;
        vector3 = (Input.mousePosition);
        var screenPoint = vector3;
        screenPoint.z = 100.0f; //distance of the plane from the camera

        Vector3 layvec = Targetting_target.transform.localPosition - Targetting_Lay.transform.localPosition ;// - Input.mousePosition;
        //Debug.Log(Targetting_Image.transform.localPosition);
        //Debug.Log(Targetting_Lay.transform.localPosition);
        var laypoint = layvec;
        laypoint.z = 0f;
        // Vector3 pos = Camera.main.ScreenToWorldPoint(screenPoint) - Field.transform.position;
        // Vector3 pos = Camera.main.ScreenToWorldPoint(screenPoint)
        //pos *= 4.65f;
        // Targetting_Lay.GetComponent<LineRenderer>().SetPosition(0, vector3);
        Targetting_target.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        Targetting_Lay.GetComponent<LineRenderer>().SetPosition(1, laypoint);

        //Targetting_Image.transform.position = Field.transform.position;
    }

    public void CardPointerDown(GameObject Card) //카드가 눌렸을때 호출되는 함수
    {
        isdragged = false;

        parent = Card.transform.parent.gameObject; //카드의 부모 가져오기
        siblingindex = Card.transform.GetSiblingIndex();//카드의 하이어라키 순서를 가져옴
        PlaceHolder.transform.SetParent(parent.transform); //플레이스홀더의 부모지정
        PlaceHolder.transform.SetSiblingIndex(siblingindex); //플레이스홀더의 부모지정
        Card.transform.SetParent(Parent_Canvas.transform); //카드의 부모 지정


        Vector3 pos = Card.transform.position;

        PlaceHolder.SetActive(true);

        savepoint = Card.transform.position;
        RaycastOff();
    }

    public void CardPointerDrag(GameObject Card) //카드가 드래그 될 때 호출되는 함수
    {
        Hand_off();
        isdragged = true;

        Vector3 vector3;
        vector3 = (Input.mousePosition);
        var screenPoint = vector3;
        screenPoint.z = 100.0f; //distance of the plane from the camera
        //Card.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        Card.transform.DOMove(Camera.main.ScreenToWorldPoint(screenPoint), 0.1f);

        //Card.GetComponent<Image>().raycastTarget = false;

    }

    public void CardPointerUp(GameObject Card) //카드에서 커서를 뗄 때 호출되는 함수
    {
        Hand_on();

        if (isdragged == true) //드래그 했으면
        {
            isdragged = false;
           

            GraphicRaycaster gr = Parent_Canvas.GetComponent<GraphicRaycaster>();
            var ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> result = new List<RaycastResult>();
            gr.Raycast(ped,result);


            if (result[0].gameObject.tag == "Field" && result[0].gameObject.GetComponent<CardField>().OnField == false && result[0].gameObject.GetComponent<CardField>().friendly) //몬스터 필드에 떨어뜨린 경우
            {  
                GameManager.Instance.PPupdate(true,Card.GetComponent<Card>().Cost);
                GameObject TARGET_FIELD = result[0].gameObject;
                CardField target_filed = TARGET_FIELD.GetComponent<CardField>();
                Card.transform.SetParent(TARGET_FIELD.transform);
                target_filed.SaveCard = Card;
                target_filed.OnField = true;
                target_filed.CanAttack = true;
                target_filed.Attack_re();
                Card.GetComponent<Card>().OnField();

                target_filed.GetCardStatus(Card);
                if (Card.GetComponent<Card>().illust != null)
                {
                    target_filed.CardImage.GetComponent<Image>().sprite = Card.GetComponent<Card>().illust;
                }
                Cards.Remove(Card);

             

                Card.SetActive(false);
                PlaceHolder.SetActive(false);
                RaycastON();

            }
            else
            {//다시원래대로 가져다 두기
                Card.transform.DOMove(savepoint, 0.3f).SetEase(Ease.OutQuart).OnComplete(() => {
                    Card.transform.SetParent(parent.transform);
                    int phindex = PlaceHolder.transform.GetSiblingIndex();
                    Card.transform.SetSiblingIndex(phindex);
                    PlaceHolder.SetActive(false);


                    //Card.GetComponent<Image>().raycastTarget = true;

                    RaycastON();
                });
            }
        }
        else
        {
            Card.transform.SetParent(parent.transform);
            int phindex = PlaceHolder.transform.GetSiblingIndex();
            Card.transform.SetSiblingIndex(phindex);
            PlaceHolder.SetActive(false);

            RaycastON();
        }




       
    }


    public void CardListAdd(GameObject card) 
    {
        Cards.Add(card);
    }

    void RaycastOff() 
    {
        int count = Cards.Count;
        for (int i = 0; i < count; i++) 
        {
               Cards[i].GetComponent<Image>().raycastTarget = false;
        }
    }

    void RaycastON()
    {
        int count = Cards.Count;
        for (int i = 0; i < count; i++)
        {
            Cards[i].GetComponent<Image>().raycastTarget = true;
        }
    }


    public void Show_Hand() 
    {
        if (!showhan)
        {
            Hand_set.transform.DOMove( hantr_on , 0.5f, false);
            showhan = true;
        }
        else
        {
            Hand_set.transform.DOMove(hantr, 0.5f, false);
            showhan = false;
        }

    }


    public void Hand_on()
    {
       
        if (savehan) 
        { 
            Hand_set.transform.DOMove(hantr_on, 0.5f, false);
            showhan = true;
        }
       
    }

    public void Hand_off()
    {
        if(!isdragged)
        savehan = showhan;
        Hand_set.transform.DOMove(hantr, 0.5f, false);
        showhan = false;
    }


    public void EnemyCardSummon(GameObject Card)//적이 카드 소환 
    {
            GameManager.Instance.PPupdate(false, Card.GetComponent<Card>().Cost); //적 코스트 감소
            GameObject TARGET_FIELD;
            do
            { TARGET_FIELD = GameManager.Instance.EnemyFields[Random.Range(0, 9)].gameObject; }
            while (TARGET_FIELD.GetComponent<CardField>().OnField == true);
            CardField target_filed = TARGET_FIELD.GetComponent<CardField>();


            Card.transform.SetParent(Parent_Canvas.transform);
            Card.transform.DOMove(TARGET_FIELD.transform.position,1f).OnComplete(()=> {
                target_filed.GetCardStatus(Card);
                target_filed.OnField = true;
                target_filed.CanAttack = true;
                EnemyCards.Remove(Card);

                Card.transform.SetParent(TARGET_FIELD.transform);
                target_filed.SaveCard = Card;
                Card.SetActive(false);
                //PlaceHolder.SetActive(false);
                //RaycastON();

                if (EnemyAiManager.Instance.Main1Recycle())
                {
                    EnemyAiManager.Instance.Main1();
                    Debug.Log("한번더");
                }
                else
                {
                    EnemyAiManager.Instance.Main2();
                }
                });


     }

    public void EnemyCardAttack(GameObject Card, GameObject Target) //적이 공격하는 함수
    {


  
            Card.GetComponent<CardField>().CanAttack = false;
            Card.GetComponent<CardField>().CardZone.transform.SetParent(SaveZone.transform);

            CardField target_filed = Target.GetComponent<CardField>();
            Transform reset_tr = Card.GetComponent<Transform>();
            // Sequence Attack_sq = DOTween.Sequence();
            // Attack_sq.Append(Field.GetComponent<CardField>().CardZone.transform.DOMove(TARGET_FIELD.transform.position, 0.5f).SetEase(Ease.InQuint));

            Card.GetComponent<CardField>().CardZone.transform.DOMove(Target.transform.position, 0.5f).SetEase(Ease.InQuint).OnComplete(() => {
                SoundManager.Instance.AttackSound_Play();

                if (Battle(Card.GetComponent<CardField>(), target_filed)) //공격
                {//안죽었으면 =>반격
                    target_filed.CardZone.transform.SetParent(SaveZone.transform);

                    Transform savetr = Target.transform;
                    target_filed.CardZone.transform.DOMove(savetr.position + new Vector3(-1, 0, 0), 0.35f).OnComplete(() =>
                    {
                        target_filed.CardZone.transform.DOMove(savetr.position, 0.15f).OnComplete(() => {
                            SoundManager.Instance.AttackSound_Play();
                            Battle(target_filed, Card.GetComponent<CardField>());
                            target_filed.CardZone.transform.SetParent(Target.transform);
                            Card.GetComponent<CardField>().CardZone.transform.DOMove(reset_tr.position, 0.5f).OnComplete(() => {
                                Card.GetComponent<CardField>().CardZone.transform.SetParent(Card.transform);
                                Card.GetComponent<CardField>().EndAttack();
                            });
                        });
                    }
                    );

                }
                else
                {//죽었으면 끝
                    Card.GetComponent<CardField>().CardZone.transform.DOMove(reset_tr.position, 0.5f).OnComplete(() => {
                        Card.GetComponent<CardField>().CardZone.transform.SetParent(Card.transform);
                        Card.GetComponent<CardField>().EndAttack();
                    });
                }



            });
            //target_filed.OffTargetting();

     

        //targeting = false;
    }
}



