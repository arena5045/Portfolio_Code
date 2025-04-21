using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputManager : MonoBehaviour
{
    public static KeyInputManager instance = null;

    //[HideInInspector]
    public SkillSlotPannel ssp;
    [HideInInspector]
    public SkillSlot ResentCheckSkill;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    private void Start()
    {
        ssp = MainCanvasManager.Instance.SkillSlotPannel.GetComponent<SkillSlotPannel>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (ssp.skillSlots[0]._skillData == null) 
            {
                return;
            }
            ResentCheckSkill = ssp.skillSlots[0];
            PlayerManager.instance.player_s.Resent_Skill = ssp.skillSlots[0]._skillData;
            ssp.skillSlots[0]._skillData.SkillEvent.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (ssp.skillSlots[1]._skillData == null)
            {
                return;
            }
            ResentCheckSkill = ssp.skillSlots[1];
            PlayerManager.instance.player_s.Resent_Skill = ssp.skillSlots[1]._skillData;
            ssp.skillSlots[1]._skillData.SkillEvent.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (ssp.skillSlots[2]._skillData == null)
            {
                return;
            }
            ResentCheckSkill = ssp.skillSlots[2];
            PlayerManager.instance.player_s.Resent_Skill = ssp.skillSlots[2]._skillData;
            ssp.skillSlots[2]._skillData.SkillEvent.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (ssp.skillSlots[3]._skillData == null)
            {
                return;
            }
            ResentCheckSkill = ssp.skillSlots[3];
            PlayerManager.instance.player_s.Resent_Skill = ssp.skillSlots[3]._skillData;
            ssp.skillSlots[3]._skillData.SkillEvent.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (ssp.skillSlots[4]._skillData == null)
            {
                return;
            }
            ResentCheckSkill = ssp.skillSlots[4];
            PlayerManager.instance.player_s.Resent_Skill = ssp.skillSlots[4]._skillData;
            ssp.skillSlots[4]._skillData.SkillEvent.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (ssp.skillSlots[5]._skillData == null)
            {
                return;
            }
            ResentCheckSkill = ssp.skillSlots[5];
            PlayerManager.instance.player_s.Resent_Skill = ssp.skillSlots[5]._skillData;
            ssp.skillSlots[5]._skillData.SkillEvent.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (ssp.skillSlots[6]._skillData == null)
            {
                return;
            }
            ResentCheckSkill = ssp.skillSlots[6];
            PlayerManager.instance.player_s.Resent_Skill = ssp.skillSlots[6]._skillData;
            ssp.skillSlots[6]._skillData.SkillEvent.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (ssp.skillSlots[7]._skillData == null)
            {
                return;
            }
            ResentCheckSkill = ssp.skillSlots[7];
            PlayerManager.instance.player_s.Resent_Skill = ssp.skillSlots[7]._skillData;
            ssp.skillSlots[7]._skillData.SkillEvent.Invoke();
        }
    }
}
