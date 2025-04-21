using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class UnitBuy_Popup : PopupBase
{
    public GameObject exit_popup;
    public GameObject fadeimage;

    public Image[] unit_face = new Image[3];
    public Image[] unit1_star = new Image[5];
    public Image[] unit2_star = new Image[5];
    public Image[] unit3_star = new Image[5];
    public Text[] unit_name = new Text[3];
    public Text[] unit_hp = new Text[3];
    public Text[] unit_atk = new Text[3];
    public Text[] unit_spd = new Text[3];
    public Text[] unit_def = new Text[3];
    public Text[] unit1_skill = new Text[3];
    public Text[] unit2_skill = new Text[3];
    public Text[] unit3_skill = new Text[3];

    public int[] unit_code = new int[3];
    public string[] save_unit_name = new string[3];
    public int[] save_unit_grade = new int[3];
    public int[] save_unit_atk = new int[3];
    public int[] save_unit_def = new int[3];
    public int[] save_unit_spd = new int[3];
    public int[] save_unit_hp = new int[3];
    public int[] save_unit_skill1 = new int[3];
    public int[] save_unit_skill2 = new int[3];
    public int[] save_unit_skill3 = new int[3];
    public int unit_lenth;
    public void Awake()
    {
        unit_lenth = GetUnitSOInfo.Instance.unit.Length;
    }
    // Start is called before the first frame update
    void Start()
    {
        RandomUnit(0);
        RandomUnit(1);
        RandomUnit(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Exitpopup_on()
    {
        exit_popup.SetActive(true);
        fadeimage.SetActive(true);
    }
    public void Exitpopup_off()
    {
        exit_popup.SetActive(false);
        fadeimage.SetActive(false);
    }

    public override void HidePopup()
    {

        base.HidePopup();

    }


    public void End_Popup() 
    {

        if (GameManager.Instance.SavePopup != null)
        {
            GameManager.Instance.SavePopup.SetActive(true);
            GameManager.Instance.SavePopup = null;
        }
        else
        {
            SoundManager.Instance.Lobby_On();
        }

        HidePopup();
    }

    public void LobbyBGM_On()
    {
        //if (GameManager.Instance.SavePopup == null)
        {
            // SoundManager.Instance.Lobby_On();

        }
    }

        void RandomUnit(int num) {
        float unitpercentage = Random.Range(0f,1f);
        int unitgrade,unitcode;

        if (unitpercentage < GameManager.Instance.unitper_add[1])//1성뜸
        {
            do
            {
                unitcode = Random.Range(0, unit_lenth);
                unitgrade = GetUnitSOInfo.Instance.getUnitGrade(unitcode);
            } while (unitgrade != 1);
            unit_code[num] = unitcode;
            UpdateBuyPopup(num, unitcode,1);

        }
        else if (GameManager.Instance.unitper_add[1] <= unitpercentage && unitpercentage < GameManager.Instance.unitper_add[2]) //2성뜸
        {
            do
            {
                unitcode = Random.Range(0, unit_lenth);
                unitgrade = GetUnitSOInfo.Instance.getUnitGrade(unitcode);
            } while (unitgrade != 2);
            unit_code[num] = unitcode;
            UpdateBuyPopup(num, unitcode,2);


        }
        else if (GameManager.Instance.unitper_add[2] <= unitpercentage && unitpercentage < GameManager.Instance.unitper_add[3])//3성뜸
        {
            do
            {
                unitcode = Random.Range(0, unit_lenth);
                unitgrade = GetUnitSOInfo.Instance.getUnitGrade(unitcode);
            } while (unitgrade != 3);
            unit_code[num] = unitcode;
            UpdateBuyPopup(num, unitcode,3);

        }
        else if (GameManager.Instance.unitper_add[3] <= unitpercentage && unitpercentage < GameManager.Instance.unitper_add[4])//4성뜸
        {
            do
            {
                unitcode = Random.Range(0, unit_lenth);
                unitgrade = GetUnitSOInfo.Instance.getUnitGrade(unitcode);
            } while (unitgrade != 4);
            unit_code[num] = unitcode;
            UpdateBuyPopup(num, unitcode,4);

        }
        else if (GameManager.Instance.unitper_add[4] <= unitpercentage && unitpercentage <= GameManager.Instance.unitper_add[5])//5성뜸
        {
            do
            {
                unitcode = Random.Range(0, unit_lenth);
                unitgrade = GetUnitSOInfo.Instance.getUnitGrade(unitcode);
            } while (unitgrade != 5);
            unit_code[num] = unitcode;
            UpdateBuyPopup(num, unitcode,5);

        }

    }


    void UpdateBuyPopup(int num, int code,int grade) {
        unit_face[num].sprite = GetUnitSOInfo.Instance.unitface[code];
        switch(num){
            case 0: { for (int i = 0; i < grade; i++) {
                        unit1_star[i].gameObject.SetActive(true);
                    }
                } break;
            case 1: {
                    for (int i = 0; i < grade; i++)
                    {
                        unit2_star[i].gameObject.SetActive(true);
                    }
                }break;
            case 2: {
                    for (int i = 0; i < grade; i++)
                    {
                        unit3_star[i].gameObject.SetActive(true);
                    }
                } break;
        }


        save_unit_name[num] = GetUnitSOInfo.Instance.getUnitName(code);
        save_unit_hp[num] = GetUnitSOInfo.Instance.getUnitHp(code);
        save_unit_atk[num] = GetUnitSOInfo.Instance.getUnitAtk(code);
        save_unit_def[num] = GetUnitSOInfo.Instance.getUnitDef(code);
        save_unit_spd[num] = GetUnitSOInfo.Instance.getUnitAtkSp(code);
        save_unit_grade[num] = GetUnitSOInfo.Instance.getUnitGrade(code);

        unit_name[num].text = "[" + save_unit_name[num] + "]";
        unit_hp[num].text = "HP : " + save_unit_hp[num];
        unit_atk[num].text = "ATK : " + save_unit_atk[num];
        unit_spd[num].text = "SPD : " + save_unit_spd[num];
        unit_def[num].text = "DEF : " + save_unit_def[num];

        int unitskill_count = GetUnitSOInfo.Instance.getUnitSkillLength(code);

    }


   public void test() {

        for (int i = 0; i < 5; i++)
        {
            unit1_star[i].gameObject.SetActive(false);
            unit2_star[i].gameObject.SetActive(false);
            unit3_star[i].gameObject.SetActive(false);
        }
        RandomUnit(0);
        RandomUnit(1);
        RandomUnit(2);
    }




    public void Unitbuy(int num) {
        int i=1;
        string name = save_unit_name[num];
        while (PlayerPrefs.HasKey("unit_" + name + "_code_" + i.ToString())) 
        {
            print("unit_" + name + "_code_" + i.ToString() + "라는 키 존재!");
            i++;
        }
        //////////////////////////////////////////////////////////////////////////////
        PlayerPrefs.SetInt("unit_" + name + "_code_" + i.ToString(), unit_code[num]);


        PlayerPrefs.SetInt("unit_" + name + "_grade_" + i.ToString(), save_unit_grade[num]);


        PlayerPrefs.SetInt("unit_" + name + "_hp_" + i.ToString(), save_unit_hp[num]);


        PlayerPrefs.SetInt("unit_" + name + "_maxhp_" + i.ToString(), save_unit_hp[num]);


        PlayerPrefs.SetInt("unit_" + name + "_atk_" + i.ToString(), save_unit_atk[num]);


        PlayerPrefs.SetInt("unit_" + name + "_def_" + i.ToString(), save_unit_def[num]);


        PlayerPrefs.SetInt("unit_" + name + "_atkSp_" + i.ToString(), save_unit_spd[num]);


        PlayerPrefs.SetInt("unit_" + name + "_location_" + i.ToString(),-1);


        PlayerPrefs.SetInt("unit_" + name + "_tile_" + i.ToString(),-1);

        //////////////////////////////////////////////////////////////////////////////
        GameObject bought_unit = Instantiate(GetUnitSOInfo.Instance.unit[unit_code[num]], TileManager.Instance.tiles[11].tar_lo.transform.position, Quaternion.Euler(new Vector3(0, 98, 0)));
        bought_unit.name = name + i;
        bought_unit.transform.SetParent(GameManager.Instance.inventory.transform);
        bought_unit.GetComponent<Unit>().GetUnit();

        if (PlacementManager.Instance.root == PlacementManager.Root._none)
        { PlacementManager.Instance.root = PlacementManager.Root._shop; }
        PlacementManager.Instance.Open_Placement();

        Hero_Skill_Popup information = PopupManager.Instance.ShowHero_Skill_Popup();
        information.SetText("유닛 획득","유닛을 확인하고 배치하세요");

        HidePopup();


    }
}
