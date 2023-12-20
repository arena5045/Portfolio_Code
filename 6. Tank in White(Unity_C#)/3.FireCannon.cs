using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireCannon : MonoBehaviour
{
    private GameObject cannon = null;
    public Transform firePos;
    public string number;
    public int shootingdamage;
    private AudioClip fireSfx = null;
    private AudioSource sfx = null;

    public float time;
    public Text timet;
    public int ammo=0;

    private LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러
    public float fireDistance = 170f; // 사정거리
    // Start is called before the first frame update
    void Awake()
    {
        cannon = (GameObject)Resources.Load("cannon"+ number);
        fireSfx = Resources.Load<AudioClip>("CannonFire");
        sfx = GetComponent<AudioSource>();

        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
        //   cannon = (GameObject)Resources.Load("Enemy/cannon"); 리소스폴더안에 Enemy폴더가 있는 경우
    }
    // Update is called once per frame

    public void Cannonchange() 
    
    {
        cannon = (GameObject)Resources.Load("cannon" + number);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)&& gameObject.tag=="player")
        {
            Fire();
        }


        if (time > 0f && gameObject.tag == "player")
        {
            timet.enabled = true;
            timet.text = "Time : "+Mathf.Round(time).ToString();
            time -= Time.deltaTime;
        }
        else if(time <= 0f && gameObject.tag == "player")
        {
            timet.enabled = false;
            time = 0;
            number = "1";
            Cannonchange();
        }
    }
    public void Fire()
    {
        if (number == "0")
        {
            Shot();
            return;
        }

        sfx.PlayOneShot(fireSfx, 1.0f);
       GameObject firecannon =  Instantiate(cannon, firePos.position, firePos.rotation);
        firecannon.GetComponent<Cannon>().tagname = gameObject.tag;
    }
    public void Shot()
    {
        RaycastHit hit;//레이캐스트에 의한 충돌 정보를 저장하는 컨테이너
        Vector3 hitPosition = Vector3.zero;//탄알이 맞은 곳을 저장할 변수
                                           //래이케스트 (시작지점, 방향, 충돌정보 컨테이너, 사정거리)
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, fireDistance))
        {
            //충돌한 상대방으로부터 IDamageable 오브젝트 가져오기 시도
            TankDamage target = hit.collider.GetComponent<TankDamage>();
            if (target != null)//가져오기 성공했다면..
            {
                Debug.Log("발사확인");
                //상대방 OnDamage 함수를 실행시켜 상대방에 데미지 주기
                target.OnLaserEnter(shootingdamage);
                target.hpbarchange();
            }
            hitPosition = hit.point;//레이가 충돌한 위치 저장
        }
        else// 레이와 충돌이 아무도 안되었다면
        {
            Debug.Log("아무것도없음");
            //탄알이 최대 사정거리까지 날아갔을때의 위치를 충돌위치로 지정
            hitPosition = firePos.position + firePos.forward * fireDistance;
        }
        StartCoroutine(ShotEffect(hitPosition));
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {

        sfx.PlayOneShot(fireSfx, 1.0f);//총격소리 재생
        bulletLineRenderer.SetPosition(0, firePos.position);//라인렌더러 시작점
        bulletLineRenderer.SetPosition(1, hitPosition); //라인렌더러 끝점
                                                        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
        bulletLineRenderer.enabled = false;
    }


}
