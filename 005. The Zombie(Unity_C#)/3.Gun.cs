using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 총을 구현한다
public class Gun : MonoBehaviour {
    // 총의 상태를 표현하는데 사용할 타입을 선언한다
    public enum State {
        Ready, // 발사 준비됨
        Empty, // 탄창이 빔
        Reloading // 재장전 중
    }

    public State state { get; private set; } // 현재 총의 상태

    public Transform fireTransform; // 총알이 발사될 위치

    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem shellEjectEffect; // 탄피 배출 효과

    private LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러

    private AudioSource gunAudioPlayer; // 총 소리 재생기
    public AudioClip shotClip; // 발사 소리
    public AudioClip reloadClip; // 재장전 소리

    public float damage = 25; // 공격력
    private float fireDistance = 50f; // 사정거리

    public int ammoRemain = 100; // 남은 전체 탄약
    public int magCapacity = 25; // 탄창 용량
    public int magAmmo; // 현재 탄창에 남아있는 탄약


    public float timeBetFire = 0.12f; // 총알 발사 간격
    public float reloadTime = 0.7f; // 재장전 소요 시간
    private float lastFireTime; // 총을 마지막으로 발사한 시점

    public Image ammoui;
    Color cor1, cor2;
    private void Awake() {
        bulletLineRenderer = GetComponent<LineRenderer>();  // 사용할 컴포넌트들의 참조를 가져오기
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;

        magAmmo = magCapacity; //탄창 가득채움
        state = State.Ready;//총의 현재 상태를 총을 쏠 준비가 된 상태로 변경
    }


    private void OnEnable() {
                gunAudioPlayer.PlayOneShot(reloadClip);
        // 총 상태 초기화
        UIManager.instance.UpdateAmmoImage(ammoRemain);//탄약 설정
        //magAmmo = magCapacity; //탄창 가득채움

        lastFireTime = 0;// 마지막으로 총을 쏜 시점 초기화
        state = State.Ready;//총의 현재 상태를 총을 쏠 준비가 된 상태로 변경
        cor1 = Color.white;//색초기값 1 1 1
        cor2 = Color.black;//색마지막값 0 0 0
        ammoui.color = Color.Lerp(cor2, cor1, (float)magAmmo / (float)magCapacity);//총 ui색변경
    }

    // 발사 시도
    public void Fire() {
        if (state == State.Ready && Time.time >= lastFireTime + timeBetFire && magAmmo>0) //탄환0발이면 못쏘는 조건 추가
        {
            lastFireTime = Time.time;
            Shot();
            ammoui.color = Color.Lerp(cor2, cor1, (float)magAmmo / (float)magCapacity); //총쏠때마다 ui색 변경
        }
    }

    // 실제 발사 처리
    private void Shot() {
        RaycastHit hit;//레이캐스트에 의한 충돌 정보를 저장하는 컨테이너
        Vector3 hitPosition = Vector3.zero;//탄알이 맞은 곳을 저장할 변수
        //래이케스트 (시작지점, 방향, 충돌정보 컨테이너, 사정거리)
        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            //충돌한 상대방으로부터 IDamageable 오브젝트 가져오기 시도
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)//가져오기 성공했다면..
            {
                //상대방 OnDamage 함수를 실행시켜 상대방에 데미지 주기
                target.OnDamage(damage, hit.point, hit.normal);
            }
            hitPosition = hit.point;//레이가 충돌한 위치 저장
        }
        else// 레이와 충돌이 아무도 안되었다면
        {
            //탄알이 최대 사정거리까지 날아갔을때의 위치를 충돌위치로 지정
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }
        StartCoroutine(ShotEffect(hitPosition));
        magAmmo--;//현재 탄약 수 감소
        if (magAmmo <= 0) {// 남은 탄약이 없다면
            state = State.Empty;//현재 상태는 empty로 갱신
        }
    }

    // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다
    private IEnumerator ShotEffect(Vector3 hitPosition) {

        muzzleFlashEffect.Play();//총구화염효과 재생
        shellEjectEffect.Play();//탄피배출효과 재생
        gunAudioPlayer.PlayOneShot(shotClip); //총격소리 재생
        bulletLineRenderer.SetPosition(0, fireTransform.position);//라인렌더러 시작점
        bulletLineRenderer.SetPosition(1, hitPosition); //라인렌더러 끝점
        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
        bulletLineRenderer.enabled = false;
    }

    // 재장전 시도
    public bool Reload() {
        //이미 재장전 중이거나 |남은 탄알이 없거나| 탄창에 탄알이 이미 가득찼으면
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= magCapacity)
        {
            return false;
        }
        StartCoroutine(ReloadRoutine());
        return true;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine() {
        // 현재 상태를 재장전 중 상태로 전환
        state = State.Reloading;
        gunAudioPlayer.PlayOneShot(reloadClip);
        
        // 재장전 소요 시간 만큼 처리를 쉬기
        yield return new WaitForSeconds(reloadTime);

        int ammoToFill = magCapacity - magAmmo;//탄창에 채울 탄알 계산
        if(ammoRemain<ammoToFill)// 탄창에 채워야 할 탄알이 남은 탄알보다 많다면 
        {
            ammoToFill = ammoRemain;// 체워야 할 탄알 수를 남은 탄알 수에 맞춰 줄임
        }
        magAmmo += ammoToFill;// 탄창을 채움
        ammoRemain -= ammoToFill;// 남은 탄알에서 탄창에 채운만큼 탄알을 뺌

        UIManager.instance.UpdateAmmoImage(ammoRemain); //탄창 이미지 갱신
        ammoui.color = Color.Lerp(cor2, cor1, (float)magAmmo / (float)magCapacity);

        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;
    }
}