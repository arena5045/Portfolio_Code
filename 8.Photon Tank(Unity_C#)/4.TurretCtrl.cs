using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCtrl : MonoBehaviourPun ,IPunObservable
{
    private Transform tr;
    private RaycastHit hit;
    public float rotSpeed = 5.0f;

    private PhotonView pv = null;
    private Quaternion currRot = Quaternion.identity;
    // Start is called before the first frame update
    void Awake()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;
        pv.Synchronization = ViewSynchronization.UnreliableOnChange;
        currRot = tr.localRotation;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tr.localRotation);
        }
        else
        {
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
    // Update is called once per frame
    void Update()
    {if(pv.IsMine)
        {
            //메인 카메라에서 마우스 커서의 위치로 캐스팅되는 ray를 생성
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
            {
                //ray에 맞은 위치를 로컬좌표로 변환
                Vector3 relative = tr.InverseTransformDirection(hit.point);
                //역탄젠트 함수인 atan2로 두 점 간의 각도를 계산
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                //아크탄젠트 : 두 점 사이의 절대각도를 재고 절대각도를 라디언 값으로 반환
                //rag2deg : 라디언 값을 degree 값으로 변환
                tr.Rotate(0, angle * Time.deltaTime * rotSpeed, 0);


            }

        }
    else
        {
            tr.localRotation = Quaternion.Slerp(tr.localRotation, currRot, Time.deltaTime * 3.0f);
        }

    }
}
