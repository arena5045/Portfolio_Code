using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackAnim : MonoBehaviour
{
    private float scrollSpeed = 1.0f;
    private Renderer _renderer;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
          float offset = Time.time * scrollSpeed * Input.GetAxisRaw("Vertical");
       // float offset = Time.time * scrollSpeed * Input.GetAxis("Vertical");
        _renderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
        _renderer.material.SetTextureOffset("_BumpMap", new Vector2(0, offset));
        //propertyName : 오프셋값을 수정할 텍스처의 이름을 지정함
        //_"Maintex" - Diffuse : 물체의 깊이감과 입체감
        //_"BumpMap" - Nornal Map : 평면상의 높이값이 있는것처럼 빛반사를 바꾸어 줌
        //"_Cube" - CubeMap
    }
}
