using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile_bullet : MonoBehaviour
{

    public int damage = 100;
    public float speed = 1000.0f;
    public GameObject Target;

    private float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Delete", 2f);//2초뒤 자동삭제
        GameObject[] mons = GameObject.FindGameObjectsWithTag("Monster");//몬스터태그 전부불러옴

        foreach (GameObject tars in mons)//몬스터수만큼 반복
        {
            if (tars.GetComponent<LivingEntity>().dead == false)//죽지않은 몬스터면
            {
                Target = tars; return;//타겟으로 지정
            }
        }
        if (Target == null)//타겟이없으면
        {
            //전방으로만 전진
            GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable target = collision.collider.GetComponent<IDamageable>();

        if (collision.collider.tag == "Player")
            return;

        if (target != null)//가져오기 성공했다면..
        {

            //상대방 OnDamage 함수를 실행시켜 상대방에 데미지 주기

            target.OnDamage(damage, gameObject.transform.position, transform.position - collision.transform.position);
            Destroy(gameObject);
        }


    }


    // Update is called once per frame
    void Update()
    {

        if (Target == null)//타겟이 중간에 죽으면
        { Destroy(gameObject);}//탄환 삭제


        waitTime += Time.deltaTime;//시간 변수

        if (waitTime < 0.5f)        //0.5초 동안 천천히 forward 방향으로 전진
        {
           speed = 0.1f;
            transform.Translate(transform.forward * speed, Space.World);
        }
        else//0.5초뒤
        {
            speed += 0.01f; //속도 가속
            //점점 다가감
            transform.position = Vector3.Lerp(transform.position, Target.transform.position+new Vector3(0,1,0), speed);

            //타겟 바라보기
            Vector3 vec = Target.transform.position - transform.position + new Vector3(0, 1, 0);
            //바라보는 백터값 생성
            Quaternion q = Quaternion.LookRotation(vec);
            //해당 백터값으로 로테이션 지정
            transform.rotation = q;
        }


    }


    void Delete()
    {
        Destroy(gameObject);
    }
}
