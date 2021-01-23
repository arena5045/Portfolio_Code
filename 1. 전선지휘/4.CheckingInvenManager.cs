using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AllEnum;

public class CheckingInvenManager : MonoBehaviour
{
    // 현재 스크립트를 싱글톤 방식으로 변경하는 스크립트
    // Debug.log(InvenMN.instance.Player_HP); 이런식으로 싱글톤에 접근하는듯 (클래스이름.클래스이름으로 생성한 변수.사용할 내부변수)
    private static CheckingInvenManager _instance;
    //지금 만들어놓은 싱글톤은 여러가지 싱글톤 예제들과 이해한것을 바탕으로 조합해놓은 형식입니다.
    public void Awake() // start 보다 빠르게 작동
    {
        if (_instance == null) //인스턴스가 비어있으면 이 대상을 그 인스턴스 안에 넣는다
            _instance = this;
        else // 인스턴스가 이 대상이 아니라면 그걸 부순다
            if (this != _instance)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject); // 다른 씬으로 넘어가도 남아있도록 만든다
    }
    public static CheckingInvenManager instance
    {
        get // get은 대상이 이 스크립트에 접근했을때 일어나는 일; get 접근자는 필드 값을 반환하거나 필드 값을 계산하여 그 결과를 반환하는 데 사용됩니다. 
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<CheckingInvenManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    // 이 스크립트는 이미 PlayerPrefs에 값이 존재한다는 것을 가정하에 작동합니다.
    // 테스트용으로 Awake에 PlayerPrefs를 생성하는 내용을 넣을 수 있습니다. (어디까지나 테스트용, 사용하지 않아도 괜찮, 할 수 있다는 의미.)
    public struct UnitDataIdentification
    {
        public bool isEmpty; // 구조체가 비어있는지 판단. 삭제 된다면 없다는 것을 알려줘야 하니까 만듦.
        // Load한 유닛들의 모든 정보를 변수로 저장하기 위해서 필요한 구조체
        public int code; // 유닛의 종류를 구분(식별)하기 위한 ID
        public int count; // 유닛의 고유 개체를 구분(식별)하기 위한 Int 값
        public string name; // 유닛의 이름
        public Sprite[] sprite; // 유닛의 이미지
        public int grade; // 유닛의 등급
        public string descript; // 유닛의 설명
        public int maxHp; // 유닛의 최대 생명력
        public int hp; //유닛의 생명력
        public int atk; //유닛의 공격력
        public int def; //유닛의 방어력
        public int atkSp; //유닛의 공격 속도
        public int tile; // 유닛의 배치된 타일번호
        public int location; //유닛의 타일에서의 추가 위치값
        public int price; // 유닛의 가격
    }

    public UnitInfo[] allUnitSO; // 게임 내에서 사용하는 모든 Unit 의 Scriptable Object 변수
    public GameObject[] inventory = new GameObject[10]; // 게임 내의 인벤토리 (10칸)
    public GameObject showingData;  // 데이터를 보여주는 UI 및 상호 작용 Button.
    public GameObject pageText; // 현재 페이지를 알려주는 텍스트 오브젝트;
    public int selectedButtonNumber = 0; // 현재 선택된 버튼이 몇번째 버튼인지 알려주는 변수. (해당 변수를 통해서, 선택한 데이터의 정보를 확인할 수 있음.)
    public int pageNumber = 1;  // 현재 Page Number를 알려주는 변수

    public Image[] stars = new Image[5];

    // 주의사항! option 변수들은 처음 상태로 되돌리는 행위(정의된 Default 값으로 돌려줘야 한다는 의미)가 매우 중요함.
    public bool checkFirstUDID = false, checkSecondUDID = false, checkReinforceUDID = false; // Option을 사용하면서 버튼에 데이터가 올바르게 들어갔다면? 각각의 변수가 True로 바뀔 것이다. (기본 Default는 false)
    public UnitDataIdentification firstFusionUDID, secondFusionUDID, reinforceUDID; // Option을 사용하면서, 현재 선택된 유닛 데이터 값이 들어갈 변수. (기본 Default는 null)
    public UnitDataIdentification selectedUDID; // 현재 선택된 UDID(Unit Data Identification) 값을 저장하는 변수. (Show Invenotry 메서드에서 할당됨)
    public GameObject firstFusionUDBtn, secondFusionUDBtn, reinforceUDBtn; // 선택된 UDID를 확인할 수 있는 UI 변수(GameObject로 생성함).
    public int selectedInvenNum = -1, firstFusionInvenNum = -1, secondFusionInvenNum = -1, reinforceInvenNum = -1; // 고유값이라고 부를 수 있는 인벤토리 넘버값.
    public GameObject fusionUI, reinforceUI;
    // 주의사항! option 창을 끄거나 키면, reset 과정이 필요하다. 정의된 Default 값으로 돌려줘야 함.

    // 1_2에서 새롭게 추가되는 변수
    public GameObject exitBtn;  // 종료 버튼을 담을 변수, 현재 옵션(Fusion 혹은 Reinforce)과 상관없이 종료버튼을 호출하기 위해서 필요.
    public GameObject showingImg, showingText; // 인벤토리 UI에서 Showing Image 오브젝트랑 ShowingText 오브젝트를 켜주는 용도로 사용.
    public GameObject prevBtn, nextBtn; // 이전 페이지로 넘어가거나 다음 페이지로 넘어가는 버튼
    private int maxUnitCount; // 현재 생성된 모든 유닛의 숫자를 확인하는 변수

    private const int MAX_NAME_ENUM = 15;
    private const int MAX_ATTRIBUTE_ENUM = 9;

    public GameObject exit_popup;
    public GameObject fadeimage;
    //Inventory Local Data Manager를 담당함.
    #region
    private static UnitDataIdentification[] in_UDID = new UnitDataIdentification[100]; // 유닛 100개 데이터
    /// <summary>
    /// 유닛 100개, 10개당 1Page. [10][10]과 동일함.
    /// </summary>
    public UnitDataIdentification[] out_UDID
    {
        get
        {
            return in_UDID;
        }
        set
        {
            in_UDID = value;
        }
    }

    #endregion


    public void OnEnable()
    {
        pageNumber = 1;
        maxUnitCount = 0;
        // 상단 변수는, 하단 메서드 사용에 지대한 영향을 미치기에 순서를 중요하게 여겨야 합니다. 그래서 메서드보다 높은 순서로 실행되어야 함 ㅇ.ㅇ
        LoadUnitDataIdentification();
        InventoryRefresh();
        MovingPage(false);
        showingImg.GetComponent<Image>().sprite = null;
        showingImg.SetActive(false);
        showingText.GetComponent<Text>().text = "";
        showingText.SetActive(false);
        selectedUDID = new UnitDataIdentification(); // 인벤토리를 키면, 이전에 선택했던 UDID를 리셋시켜줌

        stars.SetActiveAll(false);
    }

    // 인벤토리는 한 페이지당 10개로 한정.
    // 처음 게임 시작하고 난 다음에, 딱 1번만 실행된다.
    // 인벤토리 데이터가 있다면 불러오고, 없다면 0 값으로 새롭게 생성한다.
    // 100개의 인벤토리 데이터를 한 번에 불러들인다.

    // 쪼개진 유닛의 데이터를 읽어들여서, 하나의 유닛 데이터로 합성시켜서 로컬 데이터(out_UDID)로 저장
    public void LoadUnitDataIdentification()
    {
        // 인벤토리 1~100까지 작동 (만약에 값이 비어있다면, 멈춤. 100이전에 멈춘다는 의미)
        // 해당 반복문 작동 방식
        // 1. 최대 100개의 인벤토리 데이터를 읽어들인다.
        // 2. 시작값을 지정한다. (nameCount = 0) (unitEachCount = 1) (attributeCount = 0)
        // 3. 내부의 반복문은 attribute 값을 전부 읽어들이는 반복문이다. (attributeCount는 0~8 까지)
        // 4. 한번 attribute를 전부 읽어들이면, 해당 name은 그대로 유지한 상태로 unitEachCount만 늘린다. (nameCount는 0~4까지)
        // 5. 해당 유닛의 unitEachCount가 끝(100)에 도달하면, nameCount를 1 올리고 unitEachCount를 0으로 만든다.
        // 6. nameCount가 끝에 도달하면, 더이상 조사할 대상이 없다는 뜻이 된다.
        int nameCount = 0; // 현재 검색하고 있는 대상의 이름 (AllEnum.Name 을 뜻함)
        int unitEachCount = 1; // 유닛1 유닛2 라는 이름이 있을 때, 그 뒤의 숫자 (1, 2 ...)를 뜻함
        for (int invenCount = 0; invenCount < 100;)
        {
            // attributeCount는 현재 검색하고 있는 대상의 속성 (AllEnum.Attribute 를 뜻함)
            for (int attributeCount = 0; attributeCount < MAX_ATTRIBUTE_ENUM; attributeCount++)
            {
                // 해당 조건문을 읽으면 다음과 같다.
                // Unit_(UnitName의 nameCount에 있는 이름)_(UnitAttribute의 attributeCount에 있는 이름)_(해당 유닛의 동명이인을 구분하는 unitEachCount 값)
                // ex)) Unit_용병전사_code_1 <- 이런식으로 나올 수 있음.
                // willSearchString은 앞으로 검색하게 될 Key의 이름(PlayerPrefs 기준, Key를 뜻함)
                string willSearchString = "unit_" + Enum.GetName(typeof(UnitName), nameCount) + "_" + Enum.GetName(typeof(UnitAttribute), attributeCount) + "_" + unitEachCount;
                if (PlayerPrefs.HasKey(willSearchString))
                {
                    // attributeCount 값에 따라서, 어떤 속성 값을 대입해야 되는지 알 수 있다.
                    switch (attributeCount)
                    {
                        case 0:
                            in_UDID[invenCount].code = PlayerPrefs.GetInt(willSearchString);
                            in_UDID[invenCount].sprite = allUnitSO[nameCount].BasedSprite; // 한번만 넣는 경우인데, 어디에 넣어야 할지 몰라서 일단 0번 코드를 넣을 때에 같이 들어가게끔 해놓음.
                            in_UDID[invenCount].name = Enum.GetName(typeof(UnitName), nameCount);
                            in_UDID[invenCount].count = unitEachCount;
                            in_UDID[invenCount].isEmpty = false; // 내부 데이터가 비어있지 않다고 표현함.
                            maxUnitCount++;
                            break;
                        case 1:
                            in_UDID[invenCount].grade = PlayerPrefs.GetInt(willSearchString);
                            break;
                        case 2:
                            in_UDID[invenCount].hp = PlayerPrefs.GetInt(willSearchString);
                            break;
                        case 3:
                            in_UDID[invenCount].maxHp = PlayerPrefs.GetInt(willSearchString);
                            break;
                        case 4:
                            in_UDID[invenCount].atk = PlayerPrefs.GetInt(willSearchString);
                            break;
                        case 5:
                            in_UDID[invenCount].def = PlayerPrefs.GetInt(willSearchString);
                            break;
                        case 6:
                            in_UDID[invenCount].atkSp = PlayerPrefs.GetInt(willSearchString);
                            break;
                        case 7:
                            in_UDID[invenCount].tile = PlayerPrefs.GetInt(willSearchString);
                            break;
                        case 8:
                            in_UDID[invenCount].location = PlayerPrefs.GetInt(willSearchString);
                            invenCount++; // 해당 데이터가 성공적으로 저장되었다는 것을 의미하며, 다음 인벤토리 창에 값을 넣으라는 의미이기도 함. 마지막 값을 넣는 타이밍.
                            break;
                    }
                }
                else
                {
                    break;  // Unit_유닛이름_속성_숫자 의 값이 존재하지 않는다면, 바로 break 넘어감.
                }
            }
            unitEachCount++; // 이전 유닛은 이미 데이터를 읽어들였으니까. 새로운 데이터를 읽어들이기 위해서 unitEachCount를 먼저 올린다. (무조건 100까지 검사)
            // 만약에, unit100까지 도달했다면? 이제 nameCount를 올려서 다음 유닛을 검색하라는 의미가 됨.
            if (unitEachCount == 100)
            {
                nameCount++; // AllEnum.Name의 0번째 대상이 끝났으면 ++해줘서 다음 대상을 검색하게끔 만듦.
                unitEachCount = 1; // 유닛1 부터 다시 검색하라는 의미라서, 1로 지정. (1~100 -> 다시 1~ 100 .. 총 MAX_NAME_ENUM번 반복하게 됨)
                // 유닛 Enum의 최대 종류까지 확인했다면, 반복문을 그만두도록 하는 조건문
                if (nameCount == MAX_NAME_ENUM)
                {
                    for (int insertEmptyUDID = invenCount; insertEmptyUDID < 100; insertEmptyUDID++)
                    {
                        in_UDID[insertEmptyUDID].isEmpty = true;
                    }
                    break; // break는 모든 조건문을 뚫고, 가장 가까운 반복문을 벗어난다.
                }
            }
        }
    }

    // Page 버튼을 눌렀거나, 데이터 변경이 있었을 때 Refresh 해주는 메서드
    public void InventoryRefresh()
    {
        for (int countOrder = 0; countOrder < 10; countOrder++)
        {
            //inventory로 지정된 버튼의 이미지와 이름을 변경해주는 내용.
            if (in_UDID[(pageNumber - 1) * 10 + countOrder].isEmpty == false)
            {
                // in_UDID의 isEmpty가 False 라면, 값이 있다는 뜻이니까? 해당 조건문을 수행할 수 있다.
                // pageNumber는 1부터 시작하니까, (pageNumber - 1)을 사용함
                inventory[countOrder].GetComponentsInChildren<Image>()[2].sprite = in_UDID[(pageNumber - 1) * 10 + countOrder].sprite[0];
                //inventory[countOrder].GetComponentInChildren<Image>().sprite = null;
                inventory[countOrder].GetComponentInChildren<Text>().text = in_UDID[(pageNumber - 1) * 10 + countOrder].name;
                switch (in_UDID[(pageNumber - 1) * 10 + countOrder].grade)
                {
                    case 2:
                        inventory[countOrder].GetComponentInChildren<Text>().color = new Color(0.25f, 1.0f, 1.0f, 1.0f);
                        break;
                    case 3:
                        inventory[countOrder].GetComponentInChildren<Text>().color = new Color(1.0f, 1.0f, 0.25f, 1.0f);
                        break;
                    case 4:
                        inventory[countOrder].GetComponentInChildren<Text>().color = new Color(0.05f, 1.0f, 0.0f, 1.0f);
                        break;
                    case 5:
                        inventory[countOrder].GetComponentInChildren<Text>().color = new Color(1.0f, 0.22f, 0.98f, 1.0f);
                        break;
                    default:
                        inventory[countOrder].GetComponentInChildren<Text>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                        break;
                }
                inventory[countOrder].SetActive(true);
            }
            else
            {
                inventory[countOrder].GetComponentsInChildren<Image>()[2].sprite = null;
                inventory[countOrder].GetComponentInChildren<Text>().text = "NoData";
                inventory[countOrder].SetActive(false);
            }
        }
    }

    // 인벤토리 버튼을 누르면, 대상 버튼이 갖고 있는 내용을 가지고 옴.
    // 내용을 가지고 오는 것뿐만이 아니라, 할당도 할 수 있음.
    public void ShowInventory(int inventoryOrder)
    {
        // 현재 몇 페이지인지는 이미 내부에 Static 변수로 작성되어 있어서 동일하게 사용 가능.
        // inventoryType이 0이면 유닛, 1이면 건축물을 뜻함. (추가 가능)
        // 외부에서 해당 메서드 실행하면, 항상 0 혹은 1의 값을 넣어줘야 함 (그 외에 값에 따라서 인벤토리 표현도 가능.
        selectedButtonNumber = inventoryOrder; // 지금 누른 버튼의 값이 inventoryOrder라서, public 변수에 할당해준다.
        UnitDataIdentification tmpUDID = in_UDID[(pageNumber - 1) * 10 + inventoryOrder - 1]; // 인벤토리 오더는 1~10칸이니까. 0부터 시작하려면 1을 빼줘야 함.
        if (tmpUDID.isEmpty == true)
        {
            //만약에 누른 버튼이 isEmpty == true 라면, 데이터가 없다며 표시한다.
            showingImg.SetActive(false); // 선택한 데이터가 없다면, ShowingImg를 꺼버리는 부분
            showingText.SetActive(false); // 선택한 데이터가 없다면, ShowingText를 꺼버리는 부분
            showingImg.GetComponent<Image>().sprite = null;
            showingData.GetComponentInChildren<Text>().text = "No Data";
            return;
        }
        showingImg.SetActive(true); // 선택한 데이터가 있다면, ShowingImg를 켜는 부분
        showingText.SetActive(true); // 선태한 데이터가 있다면, ShowingText를 켜는 부분
        showingImg.GetComponent<Image>().sprite = tmpUDID.sprite[0];
        //showingData.GetComponentsInChildren<Image>()[1].sprite = null;
        showingData.GetComponentInChildren<Text>().text = "Name : " + tmpUDID.name + "\n";
        showingData.GetComponentInChildren<Text>().text += "Grade : " + tmpUDID.grade + "성\n";
        showingData.GetComponentInChildren<Text>().text += "Hp : " + tmpUDID.hp + "\n";

        showingData.GetComponentInChildren<Text>().text += "Atk : " + tmpUDID.atk + "\n";
        showingData.GetComponentInChildren<Text>().text += "Def : " + tmpUDID.def + "\n";
        showingData.GetComponentInChildren<Text>().text += "AtkSp : " + tmpUDID.atkSp + "\n";

        stars.SetActiveAll(false);
        for (int i = 0; i < tmpUDID.grade; i++)
        {
            stars[i].gameObject.SetActive(true);
        }

        selectedUDID = tmpUDID; // 일단, 구조체가 값 형식인건 아는데 가끔 작동 안 되기도 함. 혹시나 작동 안 되면 다시 확인할 것.
        selectedInvenNum = (pageNumber - 1) * 10 + inventoryOrder - 1; // 고유값으로 작동하게끔 설계한 인벤토리 넘버, 만약에 문제가 생기면 name + eachCount 방식 사용
        maxUnitCount = 0; // 하단의 메서드가 실행되면서 maxUnitCount 값의 변화를 주기 때문에? 값을 리셋시켜줘야 함.
        LoadUnitDataIdentification();
    }

    public void MovingPage(bool isUping)
    {
        int maxPageCount = (int)(maxUnitCount / 10);
        if (maxUnitCount % 10 > 0)
        {
            // 만약에, 34개의 유닛이라면 3page 가 나올 거고? + 1 해서 4page까지 만들어야 나머지 4개도 보여줄 수 있음.
            maxPageCount += 1;
        }
        else if (maxUnitCount == 0)
        {
            maxPageCount = 1;
        }
        
        pageText.GetComponent<Text>().text = "현재 쪽 : " + pageNumber + " / " + maxPageCount; // 초기 상태(변화 전)의 값을 내포하는 텍스트 UI
        // isUping 페이지를 Next 하시겠습니까? is~upPage? 랑 같은 맥락
        if (maxPageCount == 1)  //seung, 전체 페이지가 한 쪽일때
        {
            nextBtn.GetComponent<Image>().color = new Color(255, 255, 255, 0.2f);
            prevBtn.GetComponent<Image>().color = new Color(255, 255, 255, 0.2f);
            return;
        }
        if (isUping)
        {
            if (pageNumber >= maxPageCount || maxPageCount >= 10)       //seung, 마지막 페이지에 도달했을때
            {
                nextBtn.GetComponent<Image>().color = new Color(255, 255, 255, 0.2f);
                prevBtn.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);   //seung
                Debug.Log("페이지 안 올라간다!");
                return;   // 10쪽에서 안 올라감
            }
            else
            {
                prevBtn.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);   //seung
                nextBtn.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
            }
            Debug.Log("페이지 올라간다!");
            pageNumber++;
        }
        else
        {
            // Prev 버튼 눌렀을 때 작동
            if (pageNumber <= 1)    //첫 페이지 도달
            {
                prevBtn.GetComponent<Image>().color = new Color(255, 255, 255, 0.2f);
                nextBtn.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);   //seung
                Debug.Log("페이지 안 내려간다!");
                return; // 1쪽에서 안 내려감
            }
            else
            {
                prevBtn.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                nextBtn.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);   //seung
            }
            Debug.Log("페이지 내려간다!");
            pageNumber--;
        }
        if (pageNumber == 1)
        {
            prevBtn.GetComponent<Image>().color = new Color(255, 255, 255, 0.2f);
        }
        else if(pageNumber == maxPageCount || maxPageCount >= 10)
        {
            nextBtn.GetComponent<Image>().color = new Color(255, 255, 255, 0.2f);
        }
        else
        {
            prevBtn.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
            nextBtn.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
        }
        pageText.GetComponent<Text>().text = "현재 쪽 : " + pageNumber + " / " + maxPageCount; // 페이지 상태가 변화되고 난 다음의 텍스트 UI
        InventoryRefresh();
    }

    public void OrganizingInventoryData()
    {
        int dataIndex = 0;
        for (int excuteCount = 0; excuteCount < 100; excuteCount++)
        {
            if (in_UDID[dataIndex].isEmpty == true)
            {
                // in_UDID의 현재 index 값이 비어 있다면, 정렬을 시작한다.
                if (in_UDID[dataIndex + 1].isEmpty == true)
                {
                    // 현재 값과 다음 값이 연속으로 비어있다는 의미는 "마지막 데이터를 지웠기에" 정렬할 필요가 없다거나 "할당된 데이터가 없다"는 뜻이다.
                    break; // break를 하게 되면, 이제 for문을 벗어나서 InventoryRefresh를 하게 된다.
                }
                else
                {
                    in_UDID[dataIndex] = in_UDID[dataIndex + 1]; // 앞에 있는 데이터를 현재 위치로 옮긴다. isEmpty 라서 값이 비어있으니 가능하다.
                    UnitDataIdentification emptyUDID = new UnitDataIdentification();
                    in_UDID[dataIndex + 1] = emptyUDID;// 앞에 있는 데이터를 현재 위치로 옮겼으니, 앞에 있는 데이터를 지워버린다.
                    in_UDID[dataIndex + 1].isEmpty = true; // 지웠기에, 값은 없으니? 기본 초기화만 해준다. isEmpty를 True 해주면 기본 초기화가 끝난다.
                    //다음 반복에는 in_UDID[dataIndex + 1]이 지워졌고 기본 초기화만 되어 있기 때문에, 없다고 뜰 것이고? 데이터 정리를 다시 할 것이다.
                }
                dataIndex++; // dataIndex의 값을 올려줘서, 다음 반복 검사를 가능하게 한다.
            }
        }
        InventoryRefresh(); // 정렬이 끝나고 인벤토리 다시 보여주기
    }

    public void DeletingInventoryData()
    {
        if (selectedButtonNumber == 0)
        {
            //Debug.Log("선택된 버튼이 없음."); //버튼을 눌러서 데이터를 삭제하면, selectedButtonNumber가 0이 된다. 그럼, 삭제할 수 없으니까 return을 만난다.
            // 또한, 아무 버튼도 선택하지 않았다면 초기값이 0이라서 삭제할 수 없으니까 return을 만난다.
            return;
        }
        UnitDataIdentification tmpUDID = in_UDID[(pageNumber - 1) * 10 + selectedButtonNumber - 1];
        if (tmpUDID.isEmpty == true)
        {
            //Debug.Log("해당 UDID가 없음."); // 유닛 데이터(UDID)가 없다는 뜻. 비어있다는 뜻은 없다는 뜻입니다..
            return;
        }
        tmpUDID = new UnitDataIdentification(); // 해당 코드가 작동하는 지 확실하지 않음.
        tmpUDID.isEmpty = true; // UDID의 기본 값은 isEmpty가 True여야 한다.
        in_UDID[(pageNumber - 1) * 10 + selectedButtonNumber - 1] = tmpUDID; //새롭게 만든 tmpUDID를 기존 in_UDID 값에 덮어씌운다.
        selectedButtonNumber = 0;
        OrganizingInventoryData();
    }

    // Selector 버튼에 들어가는 메서드, 버튼을 누르면 지금 선택된 유닛 데이터가 순차적(first -> second)으로 비어있는 유닛 데이터 값(first, second)에 들어감.
    public void SelectorInput(bool isFusionOption)
    {
        // FusionOption을 이용하는지 ReinforceOption을 이용하는지 판단한다.
        if(isFusionOption)
        {

            // 유닛 데이터를 순차적으로 넣는 조건식
            // checkFirstUDID가 false 라면, 왼쪽에 넣는다.
            // checkSecondUDID가 false 라면, 오른쪽에 넣는다.
            // 만약에 양쪽 다 True라면, 넣지 않는다.
            if (!checkFirstUDID)
            {
                // 조건문으로, 선택된 두 대상(first와 second)이 같은지를 확인하는 부분. 
                if(selectedInvenNum == secondFusionInvenNum && selectedInvenNum < 0 || selectedInvenNum > 100)
                {
                    Debug.Log("중복되는 대상을 선택했습니다.");
                    return;
                }
                firstFusionUDID = selectedUDID;
                firstFusionInvenNum = selectedInvenNum;
                checkFirstUDID = true;
                RefreshShowOptionUI();
            }
            else if (!checkSecondUDID)
            {
                // 조건문으로, 선택된 두 대상(first와 second)이 같은지를 확인하는 부분.
                if (selectedInvenNum == firstFusionInvenNum && selectedInvenNum != -1)
                {
                    Debug.Log("중복되는 대상을 선택했습니다.");
                    return;
                }
                secondFusionUDID = selectedUDID;
                secondFusionInvenNum = selectedInvenNum;
                checkSecondUDID = true;
                RefreshShowOptionUI();
            }
            else
            {
                // 둘 다 참이라는 이야기가 된다. 넣어주지 않아도 되니까 아무런 행동도 안 하고 스킵함.
                // 둘 다 값이 들어있다면, 합성이나 강화를 해주면 된다.
                return;
            }
        }
        else
        {
            if(selectedInvenNum == -1)
            {
                Debug.Log("대상을 선택해주세요.");
                return;
            }
            reinforceUDID = selectedUDID;
            checkReinforceUDID = true;
            reinforceInvenNum = selectedInvenNum;
            RefreshShowOptionUI();
        }
    }

    // 합성 옵션에서 선택을 취소하는 메서드 (취소하는 대상이 첫번째 버튼인지 두번째 버튼인지를 구분하기 위해서 매개변수를 사용했음)
    public void SelectorFusionCancle(bool isFirstUDBtn)
    {
        // 할당되어 있는 유닛 데이터 값을 취소하는 메서드, 내가 누른 대상이 FirstUD를 가지고 있는 Btn인지 확인하는 값을 매개변수로 전달한다.
        // isLeftPlusBtn이 참이라면 왼쪽의 값이 취소되고, 거짓이라면 오른쪽의 값이 취소된다.
        if(isFirstUDBtn)
        {
            firstFusionUDID = new UnitDataIdentification();
            checkFirstUDID = false;
            firstFusionInvenNum = -1;
            firstFusionUDBtn.GetComponentsInChildren<Image>()[1].sprite = null; // 버튼 UI의 이미지 변경.
            firstFusionUDBtn.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 0); // 하얀 배경 없애드렸습니다^.^
            firstFusionUDBtn.GetComponentInChildren<Text>().text = ""; // 버튼 UI의 텍스트 변경
        }
        else
        {
            secondFusionUDID = new UnitDataIdentification();
            checkSecondUDID = false;
            secondFusionInvenNum = -1;
            secondFusionUDBtn.GetComponentsInChildren<Image>()[1].sprite = null; // 버튼 UI의 이미지 변경.
            secondFusionUDBtn.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 0); // 하얀 배경 없애드렸습니다^.^
            secondFusionUDBtn.GetComponentInChildren<Text>().text = ""; // 버튼 UI의 텍스트 변경
        }
    }

    // 강화 옵션에서 선택을 취소하는 메서드 (취소하는 대상을 구분할 필요가 없어서 매개변수를 넣지 않았음)
    public void SelectorReinforceCancle()
    {
        reinforceUDID = new UnitDataIdentification();
        checkReinforceUDID = false;
        reinforceInvenNum = -1;
        reinforceUDBtn.GetComponentsInChildren<Image>()[1].sprite = null; // 버튼 UI의 이미지 변경.
        reinforceUDBtn.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 0); // 하얀 배경 없애드렸습니다^.^
        reinforceUDBtn.GetComponentInChildren<Text>().text = ""; // 버튼 UI의 텍스트 변경
    }

    // Option UI를 새로고침 해주는 메서드 
    public void RefreshShowOptionUI()
    {
        if(fusionUI.activeSelf)
        {
            if(firstFusionInvenNum == selectedInvenNum && selectedInvenNum != -1)
            {
                firstFusionUDBtn.GetComponentsInChildren<Image>()[1].sprite = firstFusionUDID.sprite[0]; // 버튼 UI의 이미지 변경.
                firstFusionUDBtn.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 255); // 하얀 배경 넣어드렸습니다^.^
                firstFusionUDBtn.GetComponentInChildren<Text>().text = firstFusionUDID.name; // 버튼 UI의 텍스트 변경
            }
            else if(secondFusionInvenNum == selectedInvenNum && selectedInvenNum != -1)
            {
                secondFusionUDBtn.GetComponentsInChildren<Image>()[1].sprite = secondFusionUDID.sprite[0]; // 버튼 UI의 이미지 변경.
                secondFusionUDBtn.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 255); // 하얀 배경 넣어드렸습니다^.^
                secondFusionUDBtn.GetComponentInChildren<Text>().text = secondFusionUDID.name; // 버튼 UI의 텍스트 변경

            }
        }
        if(reinforceUI.activeSelf)
        {
            reinforceUDBtn.GetComponentsInChildren<Image>()[1].sprite = reinforceUDID.sprite[0]; // 버튼 UI의 이미지 변경.
            reinforceUDBtn.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 255); // 하얀 배경 넣어드렸습니다^.^
            reinforceUDBtn.GetComponentInChildren<Text>().text = reinforceUDID.name; // 버튼 UI의 텍스트 변경
        }
    }

    // Fusion Option UI를 끄거나 키는 메서드. 꺼져있으면 키고, 켜져있으면 끈다..
    public void ShowFusionOptionUI(bool isActiveFusionUI)
    {
        showingImg.GetComponent<Image>().sprite = null;
        showingImg.SetActive(false);
        showingText.GetComponent<Text>().text = "";
        showingText.SetActive(false);
        if (isActiveFusionUI)
        {
            // 합성이 끝나고 난 후에는, 기존의 값들은 그대로 유지하지만? 합성 UI는 꺼준다.
            firstFusionUDBtn.GetComponentsInChildren<Image>()[1].sprite = null;
            firstFusionUDBtn.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 0);
            firstFusionUDBtn.GetComponentInChildren<Text>().text = "";
            secondFusionUDBtn.GetComponentsInChildren<Image>()[1].sprite = null;
            secondFusionUDBtn.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 0);
            secondFusionUDBtn.GetComponentInChildren<Text>().text = "";
            exitBtn.SetActive(true);
            fusionUI.SetActive(false);
            exitBtn.GetComponent<Button>().onClick.Invoke(); // exitBtn은 인벤토리를 끄는 Event가 Onclick에 들어있다. Invoke를 통해서 실행시키면, 인벤토리가 꺼진다.
            // 순서가 중요함. (옵션 UI 끄기 -> 인벤토리 UI 끄기)  인벤토리 UI를 먼저 끄면, 옵션 UI가 꺼지지 않고 값이 그대로 유지될 위험이 있음.
        }
        else
        {
            // 합성 옵션 창이 켜지면, 기존에 합성으로 사용했던 변수들을 리셋(초기 값으로 되돌려주는)해주는 과정.
            selectedInvenNum = -1;
            firstFusionInvenNum = -1;
            secondFusionInvenNum = -1;
            reinforceInvenNum = -1;
            firstFusionUDID = new UnitDataIdentification();
            secondFusionUDID = new UnitDataIdentification();
            reinforceUDID = new UnitDataIdentification();
            selectedUDID = new UnitDataIdentification();
            checkFirstUDID = false;
            checkSecondUDID = false;
            exitBtn.SetActive(false);
            fusionUI.SetActive(true);
        }
    }

    // Reinforce Option UI를 끄거나 키는 메서드. 꺼져있으면 키고, 켜져있으면 끈다..
    public void ShowReinforceOptionUI(bool isActiveReinforceUI)
    {
        showingImg.GetComponent<Image>().sprite = null;
        showingImg.SetActive(false);
        showingText.GetComponent<Text>().text = "";
        showingText.SetActive(false);
        if (isActiveReinforceUI)
        {
            // 강화가 끝나고 난 후에는, 기존의 값들은 그대로 유지하지만? 강화 UI는 꺼준다.
            reinforceUDBtn.GetComponentsInChildren<Image>()[1].sprite = null;
            reinforceUDBtn.GetComponentsInChildren<Image>()[1].color = new Color(255, 255, 255, 0);
            reinforceUDBtn.GetComponentInChildren<Text>().text = "";
            exitBtn.SetActive(true);
            reinforceUI.SetActive(false);
            exitBtn.GetComponent<Button>().onClick.Invoke(); // exitBtn은 인벤토리를 끄는 Event가 Onclick에 들어있다. Invoke를 통해서 실행시키면, 인벤토리가 꺼진다.
            // 순서가 중요함. (옵션 UI 끄기 -> 인벤토리 UI 끄기)  인벤토리 UI를 먼저 끄면, 옵션 UI가 꺼지지 않고 값이 그대로 유지될 위험이 있음.
        }
        else
        {
            // 강화 옵션 창이 켜지면, 기존에 강화로 사용했던 변수들을 리셋(초기 값으로 되돌려주는)해주는 과정.
            selectedInvenNum = -1;
            firstFusionInvenNum = -1;
            secondFusionInvenNum = -1;
            reinforceInvenNum = -1;
            firstFusionUDID = new UnitDataIdentification();
            secondFusionUDID = new UnitDataIdentification();
            reinforceUDID = new UnitDataIdentification();
            selectedUDID = new UnitDataIdentification();
            checkFirstUDID = false;
            checkSecondUDID = false;
            exitBtn.SetActive(false);
            reinforceUI.SetActive(true);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //*
    //강화랑 조합을 하는 부분

    // 버튼의 양쪽에 데이터(유닛 데이터)가 할당되었는지 확인하는 메서드. (람다식을 늘려놓은 람다 함수이다.)
    // 조건문 안의 변수는 외부에서 True 혹은 False를 해준다.
    public bool WaitConditionOption()
    {
        if (checkFirstUDID && checkSecondUDID)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void FusionOption()
    {
        if (WaitConditionOption())
        {
            int result_grade = BIG_grade(firstFusionUDID.grade, secondFusionUDID.grade);//큰 별확인
            int add_grade = SMALL_grade(firstFusionUDID.grade, secondFusionUDID.grade);//작은별확인
            int final_grade = Funsion_grade_add(result_grade, add_grade);//별 상승률 계산

            Fusion_Delete();// 조합삭제
            UnitSummon.CheckFusion = true;
            Fusion_result(final_grade);// 합성결과 생성

            if (PlacementManager.Instance.root == PlacementManager.Root._none)
            { PlacementManager.Instance.root = PlacementManager.Root._shop; }
            
            PlacementManager.Instance.Open_Placement(); //배치창 열기

            InventoryManager.Instance.Close_Fusion(); //조합창 닫기

        }
       // StartCoroutine(FusionCoroutine());
    }

    void Fusion_result(int final_grade)
    {
        int unitcode;
        int unitgrade;
        do
        {
            unitcode = UnityEngine.Random.Range(0, GetUnitSOInfo.Instance.unit.Length);
            unitgrade = GetUnitSOInfo.Instance.getUnitGrade(unitcode);
        } while (unitgrade != final_grade);


        int i = 1;
        string name = GetUnitSOInfo.Instance.getUnitName(unitcode);
        while (PlayerPrefs.HasKey("unit_" + name + "_code_" + i.ToString()))
        {
            print("unit_" + name + "_code_" + i.ToString() + "라는 키 존재!");
            i++;
        }
        //////////////////////////////////////////////////////////////////////////////
        PlayerPrefs.SetInt("unit_" + name + "_code_" + i.ToString(), GetUnitSOInfo.Instance.getUnitCode(unitcode));
        PlayerPrefs.SetInt("unit_" + name + "_grade_" + i.ToString(), GetUnitSOInfo.Instance.getUnitGrade(unitcode));
        PlayerPrefs.SetInt("unit_" + name + "_hp_" + i.ToString(), GetUnitSOInfo.Instance.getUnitHp(unitcode));
        PlayerPrefs.SetInt("unit_" + name + "_maxhp_" + i.ToString(), GetUnitSOInfo.Instance.getUnitHp(unitcode));
        PlayerPrefs.SetInt("unit_" + name + "_atk_" + i.ToString(), GetUnitSOInfo.Instance.getUnitAtk(unitcode));
        PlayerPrefs.SetInt("unit_" + name + "_def_" + i.ToString(), GetUnitSOInfo.Instance.getUnitDef(unitcode));
        PlayerPrefs.SetInt("unit_" + name + "_atkSp_" + i.ToString(), GetUnitSOInfo.Instance.getUnitAtkSp(unitcode));
        PlayerPrefs.SetInt("unit_" + name + "_location_" + i.ToString(), -1);
        PlayerPrefs.SetInt("unit_" + name + "_tile_" + i.ToString(), -1);
        //////////////////////////////////////////////////////////////////////////////
        GameObject bought_unit = Instantiate(GetUnitSOInfo.Instance.unit[unitcode], TileManager.Instance.tiles[11].tar_lo.transform.position, Quaternion.Euler(new Vector3(0, 98, 0)));
        bought_unit.name = name + i;
        bought_unit.transform.SetParent(GameManager.Instance.inventory.transform);
        bought_unit.GetComponent<Unit>().GetUnit();
        //Debug.Log(GetUnitSOInfo.Instance.getUnitName(unitcode) + "/" + GetUnitSOInfo.Instance.getUnitGrade(unitcode) + "성");


        U_result_Popup resultpop = PopupManager.Instance.ShowU_Result_Popup();
        resultpop.Fusion_popup(unitcode);
    }

    void Fusion_Delete()
    {

        for (int attributeCount = 0; attributeCount < MAX_ATTRIBUTE_ENUM; attributeCount++)
        {
            string DELETE_KEY = "unit_" + firstFusionUDID.name + "_" + Enum.GetName(typeof(UnitAttribute), attributeCount) + "_" + firstFusionUDID.count;
            PlayerPrefs.DeleteKey(DELETE_KEY);
            Destroy(GameObject.Find(firstFusionUDID.name + firstFusionUDID.count));
            //Debug.Log(DELETE_KEY);

        }
        for (int attributeCount = 0; attributeCount < MAX_ATTRIBUTE_ENUM; attributeCount++)
        {
            string DELETE_KEY = "unit_" + secondFusionUDID.name + "_" + Enum.GetName(typeof(UnitAttribute), attributeCount) + "_" + secondFusionUDID.count;
            PlayerPrefs.DeleteKey(DELETE_KEY);
            Destroy(GameObject.Find(secondFusionUDID.name + secondFusionUDID.count));
           // Debug.Log(DELETE_KEY);
        }
    }

        int BIG_grade(int first, int second) {
        if (first > second)
        {
            return first;
        }
        else {
            return second;
        } 
    }
    int SMALL_grade(int first, int second)
    {
        if (first > second)
        {
            return second;
        }
        else
        {
            return first;
        }
    }

    int Funsion_grade_add(int result, int add) {
        if (result == 5)
        {
            return result;
        }
        else if (result == add)
        {
            if ((UnityEngine.Random.Range(0f, 1f) <= 0.5f))
            {
                return result + 1;
            }
            else
            {
                return result;
            }
        } else
        {
            float add_per = 0.425f - ((result - add)*0.075f);
            if ((UnityEngine.Random.Range(0f, 1f)) <= add_per)
            {
                return result + 1;
            }
            else
            {
                return result;
            }
        }

    }


    // 강화를 하기 위해서 사용하는 메서드
    public void ReinforceOption()
    {
        if (checkReinforceUDID)
        {
            string name = reinforceUDID.name;
            int code = reinforceUDID.code;
            int count = reinforceUDID.count;
            int unit_atk = reinforceUDID.atk + UnityEngine.Random.Range(0,6);
            int unit_maxhp = reinforceUDID.maxHp + UnityEngine.Random.Range(0, 21);
            int unit_hp = unit_maxhp;
            int unit_def = reinforceUDID.def + UnityEngine.Random.Range(0, 21); ;
            int unit_atkspd = reinforceUDID.atkSp - (UnityEngine.Random.Range(0, 1)*10); ;

            //////////////////////////////////////////////////////////////////////////////
            PlayerPrefs.SetInt("unit_" + name + "_hp_" + count, unit_hp);
            PlayerPrefs.SetInt("unit_" + name + "_maxhp_" +count, unit_maxhp);
            PlayerPrefs.SetInt("unit_" + name + "_atk_" + count, unit_atk);
            PlayerPrefs.SetInt("unit_" + name + "_def_" + count, unit_def);
            PlayerPrefs.SetInt("unit_" + name + "_atkSp_" + count, unit_atkspd);
            //////////////////////////////////////////////////////////////////////////////
            GameObject target = GameObject.Find(name + count);
            target.GetComponent<Unit>().hp = unit_hp;
            target.GetComponent<Unit>().Max_hp = unit_maxhp;
            target.GetComponent<Unit>().atk = unit_atk;
            target.GetComponent<Unit>().def = unit_def;
            target.GetComponent<Unit>().a_spd = unit_atkspd;

            U_result_Popup resultpop = PopupManager.Instance.ShowU_Result_Popup();
            resultpop.Reinforce_Popup(code,unit_maxhp, unit_atk ,unit_atkspd, unit_def);


            InventoryManager.Instance.Close_Reinforce(); //조합창 닫기
        }
    }

    //*/
    public void Exitpopup_on()
    {
        SoundManager.Instance.menu_ok_Play();
        exit_popup.SetActive(true);
        fadeimage.SetActive(true);
    }
    public void Exitpopup_off()
    {
        SoundManager.Instance.cancle_menu();
        exit_popup.SetActive(false);
        fadeimage.SetActive(false);
    }



}
