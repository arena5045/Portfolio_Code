using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCtrl : MonoBehaviour
{
    private Transform tr;
    private RaycastHit hit; //Ray가 지면에 맞은 위치를 저장할 변수
    public float rotSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        //메인카메라엣 마우스 커서의 위치로 캐스팅되는 Ray를 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
        {
            //Ray에 맞은 위치를 로컬좌표로 변환
            Vector3 relative = tr.InverseTransformPoint(hit.point);
            // Debug.Log(relative.x + ", " + relative.z);
           //역탄젠트 함수인 Atan2로 두 점 간의 각도를 계산
            float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
            //Atan2(아크탄젠트): 두 점 사이의 절대각도를 재고 절대각도를 라디언값으로 반환
            //Rad2Deg : 라디언값을 Degree값으로 변환
            tr.Rotate(0, angle * Time.deltaTime * rotSpeed, 0);
        }
    }
}
