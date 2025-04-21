using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] footstep;
    [SerializeField]
    private AudioClip[] nomalAttackSound;
    [SerializeField]
    private AudioClip[] VoiceSound;
    [SerializeField]
    private AudioClip[] MoveVoices;
    [SerializeField]
    private AudioClip[] AttackVoices;
    [SerializeField]
    private float footstepInterval=0.35f;

    float chktime;//��� ��Ÿ��
    float cooltime =3f;//��� ��Ÿ��

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayFootstepSound());
        chktime = cooltime;
    }

    private void Update()
    {
        chktime += Time.deltaTime;
    }

    private IEnumerator PlayFootstepSound()
    {
        while (PlayerManager.instance.player_s == null)
        {
            yield return null;
        }

        while (true)
        {
            yield return new WaitUntil(() => PlayerManager.instance.player_s.state == Player.PlayerState.Run);
            // ���°� Run�� ���� ����
            if (PlayerManager.instance.player_s.state == Player.PlayerState.Run)
            {
                SoundManager.instance.EffectPlay(footstep[Random.Range(0,footstep.Length)]);
            }
            yield return new WaitForSeconds(footstepInterval);
        }
    }


    public void PlayerNomalAttackSound()
    {
        SoundManager.instance.EffectPlay(nomalAttackSound[Random.Range(0, nomalAttackSound.Length)]);
    }


    public void MoveSound()
    {
        if(chktime >= cooltime && !SoundManager.instance.narSoundPlayer.isPlaying)
        {
            SoundManager.instance.NarPlay(MoveVoices[Random.Range(0, MoveVoices.Length)]);
            chktime = 0;
        }
    }


    public void AttackSound()
    {
        if (chktime >= cooltime && !SoundManager.instance.narSoundPlayer.isPlaying)
        {
            SoundManager.instance.NarPlay(AttackVoices[Random.Range(0, AttackVoices.Length)]);
            chktime = 0;
        }
    }


}
