using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Rand : MonoBehaviour
{
    public GameObject turret;
    public GameObject turret_cannon;
    float time;
    Quaternion nturret_q;
    Quaternion nturretc_q;
    Quaternion turret_q;
    Quaternion turretc_q;
    void Start()
    {
        time = 0.0f;
        nturret_q = turret.transform.rotation;
        nturretc_q = turret_cannon.transform.rotation;
        turret_q.eulerAngles = new Vector3(turret.transform.rotation.x, Random.Range(80f, 170f), turret.transform.rotation.z);
        Debug.Log(turret_cannon.transform.rotation.eulerAngles);
        //urretc_q.eulerAngles = new Vector3(Random.Range(-25f, 0f), 0,0);
        turretc_q.eulerAngles =  new Vector3(Random.Range(-25f, 0f), turret_q.eulerAngles.y, 0); 
    }

    // Update is called once per frame
    void Update()
    {
        if (time < 3f)
        {
            time += Time.deltaTime;
            turret.transform.rotation = Quaternion.Lerp(nturret_q, turret_q, time);
            turret_cannon.transform.rotation = Quaternion.Lerp(nturretc_q, turretc_q, time);
        }
        else
        {
            Debug.Log(turret_cannon.transform.rotation.eulerAngles);
            nturret_q = turret_q;
            nturretc_q = turretc_q;
            turret_q.eulerAngles = new Vector3(turret.transform.rotation.x, Random.Range(80f, 170f), turret.transform.rotation.z);
            turretc_q.eulerAngles = new Vector3(Random.Range(-25f, 0f), turret_q.eulerAngles.y, 0);



            time = 0.0f;
        }
    }
}
