using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    public float speed = 10.0f;
    public float scroll_speed = 10.0f;
    public Transform cameratar;

    private Camera _camera;
    private Vector3 _worldDefaltForward;
    float slerf = 0f;
    float timerun;
    public float duration = 2f;

    public float scrollSpeed = 5f; // 카메라 이동 속도
    public float edgeThreshold = 10f; // 화면 가장자리와 커서 간의 거리


    public float  rotationSpeed = 30f; // 회전 속도
    private float targetRotation = 0f; // 목표 회전값

    private Vector3 targetPosition; // 목표 위치
    private Quaternion targetQuar;
    private Vector3 camPosition; // 카메라 리셋 위치
    private Quaternion camQuar;


    private int fow = 60;

    public GameObject rotob;
    public GameObject moveob;
    Rigidbody rb;

    public LayerMask mask;

    Vector3 RorL;
    Vector3 UorD;



    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _worldDefaltForward = transform.forward;

        targetPosition = moveob.transform.position;
        targetQuar = rotob.transform.rotation;
        camPosition = _camera.transform.localPosition;
        camQuar = _camera.transform.localRotation;

        rb = moveob.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Data_Manager.instance.isPause)
        {
            return;
        }


        float scroll = Input.GetAxis("Mouse ScrollWheel") * scroll_speed;

        if(_camera.fieldOfView <= 20.0f && scroll <0 )
        {
            _camera.fieldOfView = 20.0f;
        }
        else if(_camera.fieldOfView >= 68.0f && scroll > 0)
        {

            _camera.fieldOfView = 68.0f;
        }else
        {


            _camera.fieldOfView += scroll *2;

            _camera.transform.Rotate(scroll * 2 , 0, 0);
            _camera.transform.Translate(0, 0, scroll*1.3f, Space.World);
        }

        Vector3 mousePosition = Input.mousePosition;
        Vector3 moveDirection = Vector3.zero;
 

        // 마우스 커서가 화면 가장자리에 닿으면 이동 방향 설정
        if ((mousePosition.x < edgeThreshold || Input.GetKey(KeyCode.A)))// transform.position.x>-11)
        {   
            RorL = -rotob.transform.right;
        }
        else if ((mousePosition.x > Screen.width - edgeThreshold || Input.GetKey(KeyCode.D))) //&& transform.position.x < 14)
        {
            RorL = rotob.transform.right;
        }
        else
        {
            RorL = Vector3.zero;
        }
 


        if ((mousePosition.y < edgeThreshold || Input.GetKey(KeyCode.S)))  //&& transform.position.z > -22)
        {
            UorD = -rotob.transform.forward;
        }
        else if ((mousePosition.y > Screen.height - edgeThreshold || Input.GetKey(KeyCode.W)))//&& transform.position.z < 5)
        {
            UorD = rotob.transform.forward;
        }
        else
        {
            UorD = Vector3.zero;
        }


        Vector3 pos = (RorL + UorD).normalized;

        rb.velocity = pos* 20f;


        if (Input.GetKey(KeyCode.Q))
        {

            // slerf -= Time.deltaTime*2;
            // _camera.transform.Rotate(0, slerf, 0, Space.World);
            targetRotation -= rotationSpeed * Time.deltaTime;

        }
        else if(Input.GetKey(KeyCode.E))
        {

            //slerf += Time.deltaTime*2;
            //_camera.transform.Rotate(0, slerf, 0, Space.World);
            targetRotation += rotationSpeed * Time.deltaTime;
  
        }
        else
        {
            if (slerf != 0 )
            {

                _camera.transform.Rotate(0, slerf, 0, Space.World);

                timerun += Time.deltaTime;
                slerf = Mathf.Lerp(slerf, 0, timerun / duration);
            }
            else
            {
                timerun = 0;
            }
             
        }



        //transform.position =Vector3.Lerp(transform.position,targetPosition, 0.05f);
        // 카메라를 이동 방향으로 이동
        transform.position = Vector3.Lerp(transform.position, moveob.transform.position, 0.05f);

        float currentRotation = Mathf.LerpAngle(_camera.gameObject.transform.eulerAngles.y, targetRotation, Time.deltaTime *5);
        _camera.gameObject.transform.eulerAngles = new Vector3(_camera.gameObject.transform.eulerAngles.x, currentRotation, 0f);
        rotob.transform.eulerAngles = new Vector3(0, currentRotation, 0f);

        if (Input.GetKeyDown(KeyCode.R))
        {
            CameraReset();
        }

    }

    public void CameraReset()
    {
        rb.velocity = Vector3.zero;

        print(camQuar);
        targetRotation = 0;
        _camera.transform.localPosition = camPosition;
        _camera.transform.localRotation = camQuar;
        moveob.transform.position = targetPosition;
        rotob.transform.rotation = targetQuar;
        _camera.fieldOfView = fow;
  
    }
}




