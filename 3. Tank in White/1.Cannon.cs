using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public float speed = 6000.0f;
    public int damage = 20;
    public GameObject expEffect;
    private CapsuleCollider _collider;
    private Rigidbody _rigidbody;
    public string tagname;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        StartCoroutine(this.ExplosionCannon(3.0f));//3초가 지난 후 자동 폭발하는 코루틴 함수 실행
    }
    private void OnTriggerEnter(Collider other)
    {
        //지면 또는 적 탱크에 충돌한 경우 즉시 폭발하도록 코루틴 함수 실행
        StartCoroutine(this.ExplosionCannon(0.0f));
    }
    IEnumerator ExplosionCannon(float tm)
    {
        yield return new WaitForSeconds(tm);
        _collider.enabled = false;//충돌 콜백함수가 발생하지 않도록 Collider를 비활성화
        _rigidbody.isKinematic = true; // 물리엔진 영향 안받게 변경
        GameObject obj = (GameObject)Instantiate(expEffect, transform.position,
            Quaternion.identity);
        Destroy(obj, 1.0f); // expEffect 1초뒤 파괴
        Destroy(this.gameObject, 1.0f); // Cannon 오브젝트 1초뒤 파괴
    }
}
