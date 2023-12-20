using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBuildSlot : MonoBehaviour
{
    public SkillData data;
    public Image skill_icon;
    public TMP_Text skill_name;
    public TMP_Text skill_mana;
    public TMP_Text skill_con;
    public TMP_Text skill_des;
    public Button skillChkBtn;
    public GameObject dispannel;
    public bool learnChk = false;
    public bool EquipChk = false;

    private void Awake()
    {
        skill_icon.sprite = data.skill_Icon;
        skill_name.text = data.SkillName;
        skill_mana.text = "소모마나 : " + data.ManaRequirement[1];
        skill_con.text = "스킬계수 : " + data.Coefficient[0] + "%";
        skill_des.text = data.Skill_Explanation;
}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void BtnClick()
    {
        if(PlayerManager.instance.player_s.SkillPoint <=0 && !learnChk)//스포 없는데 안배운 상태
        {
            return; //클릭 안대용~
        }


        if(!learnChk) // 스킬 배울때 
        {
            learnChk = true;
            dispannel.SetActive(false);

            skillChkBtn.GetComponentInChildren<TMP_Text>().text = "장착하기";
            skillChkBtn.GetComponent<Image>().color = new Color(1f,0.5f,0.5f);

            PlayerManager.instance.player_s.SkillPoint--;
            Skill_Build_Pannel sbp = FindObjectOfType<Skill_Build_Pannel>();
            SoundManager.instance.EffectPlay(1); //배우는 효과음
            sbp.Refresh();
        }
        else if(!EquipChk)//이미 배웠으면 =>장착하기
        {
            foreach(SkillSlot skillSlot in KeyInputManager.instance.ssp.skillSlots)
            {
                if(skillSlot._skillData ==null)
                {
                    skillSlot._skillData = data;
                    skillSlot.Refresh();
                    break;
                }
            }

            EquipChk = true;
            SoundManager.instance.EffectPlay(2); //장착하는 효과음
            skillChkBtn.GetComponentInChildren<TMP_Text>().text = "헤제하기";
            skillChkBtn.GetComponent<Image>().color = new Color(0.5f, 1f, 0.5f);

        }
        else //헤체하는 버튼
        {
            foreach (SkillSlot skillSlot in KeyInputManager.instance.ssp.skillSlots)
            {
                if (skillSlot._skillData == data)
                {
                    skillSlot._skillData = null;
                    skillSlot.Refresh();
                    break;
                }
            }

            EquipChk = false;
            SoundManager.instance.EffectPlay(3); //헤체하는 효과음
            skillChkBtn.GetComponentInChildren<TMP_Text>().text = "장착하기";
            skillChkBtn.GetComponent<Image>().color = new Color(1f, 0.5f, 0.5f);
        }
        
    }
}
