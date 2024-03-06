using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class TankMove : MonoBehaviourPun,IPunObservable
{
    // Start is called before the first frame update
    public float moveSpeed = 20.0f;
    public float rotSpeed = 50.0f;
    private Rigidbody rbody;
    private Transform tr;
    private float h, v;

    private PhotonView pv = null;
    public Transform camPivot;//

    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

    void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
       
        pv = GetComponent<PhotonView>();
        pv.Synchronization = ViewSynchronization.UnreliableOnChange;
        pv.ObservedComponents[0] = this;
        if(pv.IsMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = camPivot;
            rbody.centerOfMass = new Vector3(0.0f, -0.5f, 0.0f);//무게중심좌표 낮게설정
        }
        else
        {
            rbody.isKinematic = true;
        }
            
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);

        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
    // Update is called once per frame
    void Update()
    {   //if (!pv.IsMine) return;
        if (pv.IsMine)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            tr.Rotate(Vector3.up * rotSpeed * h * Time.deltaTime);
            tr.Translate(Vector3.forward * v * moveSpeed * Time.deltaTime);
        }
        else//원격이면 이동과 회전을 수신받은 위치로 부드럽게 이동
        {
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 3.0f);
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 3.0f);

        }

    }


}
