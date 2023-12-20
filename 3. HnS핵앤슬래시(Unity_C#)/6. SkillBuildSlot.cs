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
        skill_mana.text = "�Ҹ𸶳� : " + data.ManaRequirement[1];
        skill_con.text = "��ų��� : " + data.Coefficient[0] + "%";
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
        if(PlayerManager.instance.player_s.SkillPoint <=0 && !learnChk)//���� ���µ� �ȹ�� ����
        {
            return; //Ŭ�� �ȴ��~
        }


        if(!learnChk) // ��ų ��ﶧ 
        {
            learnChk = true;
            dispannel.SetActive(false);

            skillChkBtn.GetComponentInChildren<TMP_Text>().text = "�����ϱ�";
            skillChkBtn.GetComponent<Image>().color = new Color(1f,0.5f,0.5f);

            PlayerManager.instance.player_s.SkillPoint--;
            Skill_Build_Pannel sbp = FindObjectOfType<Skill_Build_Pannel>();
            SoundManager.instance.EffectPlay(1); //���� ȿ����
            sbp.Refresh();
        }
        else if(!EquipChk)//�̹� ������� =>�����ϱ�
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
            SoundManager.instance.EffectPlay(2); //�����ϴ� ȿ����
            skillChkBtn.GetComponentInChildren<TMP_Text>().text = "�����ϱ�";
            skillChkBtn.GetComponent<Image>().color = new Color(0.5f, 1f, 0.5f);

        }
        else //��ü�ϴ� ��ư
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
            SoundManager.instance.EffectPlay(3); //��ü�ϴ� ȿ����
            skillChkBtn.GetComponentInChildren<TMP_Text>().text = "�����ϱ�";
            skillChkBtn.GetComponent<Image>().color = new Color(1f, 0.5f, 0.5f);
        }
        
    }
}
