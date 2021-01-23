using System.Collections;
using UnityEngine;
using UnityEngine.AI; // AI, 내비게이션 시스템 관련 코드를 가져오기

// 적 AI를 구현한다
public class Enemy : LivingEntity {
    public LayerMask whatIsTarget; // 추적 대상 레이어
    //특정레이어를 가진 게임 오브젝트에 물리 또는 그래픽 처리등을 적용시킬 때 사용

    private LivingEntity targetEntity; // 추적할 대상
    private NavMeshAgent pathFinder; // 경로계산 AI 에이전트

    public AudioClip deathSound; // 사망시 재생할 소리

    public GameObject turret;
    public GameObject turret_c;
    private GameObject player;

    private Animator enemyAnimator; // 애니메이터 컴포넌트
    private AudioSource enemyAudioPlayer; // 오디오 소스 컴포넌트
    private Renderer enemyRenderer; // 렌더러 컴포넌트

    public int score;

    public float damage = 20f; // 공격력
    public float timeBetAttack = 0.5f; // 공격 간격
    public float AttackTime; // 마지막 공격 시점

    // 추적할 대상이 존재하는지 알려주는 프로퍼티
    private bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            // 그렇지 않다면 false
            return false;
        }
    }

    private void Awake() {
        // 초기화
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAudioPlayer = GetComponent<AudioSource>();
        enemyRenderer = GetComponentInChildren<Renderer>();

    }

    // 적 AI의 초기 스펙을 결정하는 셋업 메서드
    public void Setup(int wave) {
        float upper = 1 + ((wave-1) * 0.1f); //웨이브에 따라 증가치

        startingHealth = Mathf.FloorToInt(startingHealth * upper);
        health = Mathf.FloorToInt(startingHealth * upper);
        pathFinder.speed *= upper;

    }

    private void Start() {
        player = GameObject.FindWithTag("player");
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        pathFinder.enabled = false; pathFinder.enabled = true;
        StartCoroutine(UpdatePath());
    }

    private void Update() {
        if(player != null)//플레이어 존재 확인
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y + 3, player.transform.position.z);
            turret.transform.LookAt(targetPosition); //플레이어 바라봄
            targetPosition = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
            turret_c.transform.LookAt(player.transform.position + new Vector3(0, 5, 0)); //포신은 살짝 더 높게
        }

    }

    // 주기적으로 추적할 대상의 위치를 찾아 경로를 갱신
    private IEnumerator UpdatePath() {
        // 살아있는 동안 무한 루프
        pathFinder.enabled = false; pathFinder.enabled = true;
        while (!dead)
        {
           
                pathFinder.isStopped = false; // 경로를 갱신하고 ai이동을 계속 진행
                pathFinder.destination = player.transform.position;

            float dist = Vector3.Distance(player.transform.position, gameObject.transform.position);
            if (dist < pathFinder.stoppingDistance) //발사 사거리보다 가까워지면
            {
                Vector3 targetPosition = new Vector3(player.transform.position.x+ Random.Range(-10, 11), transform.position.y + 3, player.transform.position.z+ Random.Range(-10, 11));
                turret.transform.LookAt(targetPosition);//포신의 각도를 랜덤하게 비튼뒤 발사
                GetComponent<FireCannon>().Fire();
            }

            yield return new WaitForSeconds(AttackTime); //공격 쿨타임 동안 대기
        }
    }

    // 데미지를 입었을때 실행할 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal) {
        // LivingEntity의 OnDamage()를 실행하여 데미지 적용
        if (!dead)
        {
           // enemyAudioPlayer.PlayOneShot(hitSound);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // 사망 처리
    public override void Die() {
        Debug.Log("다이 실행");
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        Collider[] enamyColliders = GetComponents<Collider>();
        for (int i = 0; i< enamyColliders.Length; i++)
        {
            enamyColliders[i].enabled = false;
        }
        pathFinder.isStopped = true;
        pathFinder.enabled = false;
        GameManager.instance.AddScore(Mathf.FloorToInt(score*Random.Range(0.5f,1.5f)));
        base.Die();
    }

}