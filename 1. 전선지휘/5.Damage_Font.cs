using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Font : MonoBehaviour
{
    public GameObject[] Damage = new GameObject[10];
    public Sprite[] Fonts= new Sprite[10];
    public int Damage_int;
    void Start()
    {
        SettingSprite(Damage_int);
        Destroy(gameObject, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(0, 1f * Time.deltaTime, 0);
    }


    void SettingSprite(int dmg) {

        int digit = (int)Mathf.Floor(Mathf.Log10(dmg))+1;

        for (int i=0; i < digit; i++)
        {
            int num = (dmg / (int)Mathf.Pow(10, i)) % 10;
            Damage[i].GetComponent<SpriteRenderer>().sprite = Fonts[num];
            Damage[i].SetActive(true);
        }
    
    
    }
}
