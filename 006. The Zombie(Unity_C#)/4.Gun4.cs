using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun4 : MonoBehaviour
{// 총의 상태를 표현하는데 사용할 타입을 선언한다
    public enum State
    {
        Ready, // 발사 준비됨
        Empty, // 탄창이 빔
        Reloading // 재장전 중
    }
    public State state { get; private set; } // 현재 총의 상태

    public Transform fireTransform; // 총알이 발사될 위치
    public GameObject Bullet;//총알

    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem shellEjectEffect; // 탄피 배출 효과

    private LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러

    private AudioSource gunAudioPlayer; // 총 소리 재생기
    public AudioClip shotClip; // 발사 소리
    public AudioClip reloadClip; // 재장전 소리

    public int damage = 200; // 공격력

    public int ammoRemain = 36; // 남은 전체 탄약
    public int magCapacity = 6; // 탄창 용량
    public int magAmmo; // 현재 탄창에 남아있는 탄약

    public float timeBetFire = 1.0f; // 총알 발사 간격
    public float reloadTime = 1.8f; // 재장전 소요 시간
    private float lastFireTime; // 총을 마지막으로 발사한 시점

    public Image ammoui;
    Color cor1, cor2;
    private void Awake()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();  // 사용할 컴포넌트들의 참조를 가져오기
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
        magAmmo = magCapacity; //탄창 가득채움
        state = State.Ready;//총의 현재 상태를 총을 쏠 준비가 된 상태로 변경
    }


    private void OnEnable()
    {
        gunAudioPlayer.PlayOneShot(reloadClip);
        // 총 상태 초기화
        UIManager.instance.UpdateAmmoImage(ammoRemain);//탄약 설정
        //magAmmo = magCapacity; //탄창 가득채움
        state = State.Ready;//총의 현재 상태를 총을 쏠 준비가 된 상태로 변경
        lastFireTime = 0;// 마지막으로 총을 쏜 시점 초기화


        cor1 = Color.white;
        cor2 = Color.black;

        ammoui.color = Color.Lerp(cor2, cor1, (float)magAmmo / (float)magCapacity);
    }

    // 발사 시도
    public void Fire()
    {
        if (state == State.Ready && Time.time >= lastFireTime + timeBetFire && magAmmo >0)
        {
            lastFireTime = Time.time;
            Shot();
            ammoui.color = Color.Lerp(cor2, cor1, (float)magAmmo / (float)magCapacity);
        }
    }

    // 실제 발사 처리
    private void Shot()
    {
        Bullet.GetComponent<Missile_bullet>().damage = damage;
        Instantiate(Bullet, fireTransform.position, fireTransform.rotation);
        muzzleFlashEffect.Play();//총구화염효과 재생
        shellEjectEffect.Play();//탄피배출효과 재생
        gunAudioPlayer.PlayOneShot(shotClip); //총격소리 재생

        //StartCoroutine(ShotEffect(hitPosition));
        magAmmo--;//현재 탄약 수 감소
        if (magAmmo <= 0)
        {// 남은 탄약이 없다면
            state = State.Empty;//현재 상태는 empty로 갱신
        }
    }


    // 재장전 시도
    public bool Reload()
    {
        //이미 재장전 중이거나 |남은 탄알이 없거나| 탄창에 탄알이 이미 가득찼으면
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= magCapacity)
        {
            return false;
        }
        StartCoroutine(ReloadRoutine());
        return true;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine()
    {
        // 현재 상태를 재장전 중 상태로 전환
        state = State.Reloading;
        gunAudioPlayer.PlayOneShot(reloadClip);

        // 재장전 소요 시간 만큼 처리를 쉬기
        yield return new WaitForSeconds(reloadTime);

        int ammoToFill = magCapacity - magAmmo;//탄창에 채울 탄알 계산
        if (ammoRemain < ammoToFill)// 탄창에 채워야 할 탄알이 남은 탄알보다 많다면 
        {
            ammoToFill = ammoRemain;// 체워야 할 탄알 수를 남은 탄알 수에 맞춰 줄임
        }
        magAmmo += ammoToFill;// 탄창을 채움
        ammoRemain -= ammoToFill;// 남은 탄알에서 탄창에 채운만큼 탄알을 뺌

        UIManager.instance.UpdateAmmoImage(ammoRemain);
        ammoui.color = Color.Lerp(cor2, cor1, (float)magAmmo / (float)magCapacity);

        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;
    }
}