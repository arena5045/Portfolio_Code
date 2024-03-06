using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireCannon : MonoBehaviour
{
    public GameObject cannon = null;
    public Transform firePos;
    private AudioClip fireSfx = null;
    private AudioSource sfx = null;


    private PhotonView pv = null;

    void Awake()
    {
        cannon = (GameObject)Resources.Load("cannon");
        fireSfx = Resources.Load<AudioClip>("CannonFire");
        sfx = GetComponent<AudioSource>();
        pv = GetComponent<PhotonView>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)&& pv.IsMine)
        {
            Fire();
            pv.RPC("Fire", RpcTarget.Others, null);
        }
    }

    [PunRPC]
    void Fire() {
        sfx.PlayOneShot(fireSfx, 1.0f);
        Instantiate(cannon, firePos.position, firePos.rotation);
    
    }
}
