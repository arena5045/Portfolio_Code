using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Intro_Effect : MonoBehaviour
{


    public PostProcessVolume pub; //포스트프로세스 변수
    public Vignette vig; //비네팅 효과
    public Grain grn; //노이즈효과 변수
    bool updown = true; //증감치 확인용 불 변수

    void Start()
    {    //시작할 때 각 변수 할당
        pub = GetComponent<PostProcessVolume>();
        vig = pub.profile.GetSetting<Vignette>();
        grn = pub.profile.GetSetting<Grain>();

    }

    void Update()
    {
        if (updown) {//불변수가 트루면
            //매 프레임마다 비네팅 그레인 값 증감
            vig.intensity.value += 0.003f;
            grn.size.value -= 0.03f;

            if (vig.intensity.value > 0.99f) {//비네팅값이 최대면
                updown = false; //불값 반전
            }
        }else
            {//불변수가 false면
             //매 프레임마다 비네팅 그레인 값 true와 반대로 증감
            vig.intensity.value -= 0.003f;
            grn.size.value += 0.03f;
            if (vig.intensity.value < 0.6f)//비네팅값이 일정수치만큼 내려가면
            {
                updown = true;//불값 반전
            }
        }
       
    }


}
