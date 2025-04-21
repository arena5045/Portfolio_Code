using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickSystem : MonoBehaviour
{

    public static ClickSystem instance { get; private set; }

    public Camera cam;
    public GameObject test;
    private RaycastHit[] hit;

    public GameObject SummonEffect;
    public AudioClip SummonSound;

    public bool isSuperBuild=false;

    public int towercode = 500;

    public enum PlayerMode
    {
        Nomal,
        TowerBuild,
        TowerMix,
        TowerSell
    }

    public PlayerMode playerMode = PlayerMode.Nomal;

    public GameObject[] towerZone;



    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        towerZone = null;
        towerZone = GameObject.FindGameObjectsWithTag("TowerZone");
        foreach (GameObject zone in towerZone)
        {
            zone.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                switch (playerMode)
                {
                    case PlayerMode.TowerBuild://Ÿ���Ǽ���忡�� Ŭ�� ��
                        bool canbuild=false;
                        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                        hit = Physics.RaycastAll(ray, 100f);
                        if (hit != null)
                        {
                            foreach (RaycastHit hitob in hit)
                            {
                                GameObject hitObject = hitob.transform.gameObject;
                                if (hitObject.tag == "TowerZone")
                                {
                                    if(isSuperBuild)
                                    {
                                        int rand = Random.Range(22, 25);
                                        Sound_Manager.instance.NarPlay(rand);
                                        Tower_Manager.instance.TowerInstance(hitObject, Random.Range(1,7));
                                        TowerSuperBuildBtn();
                                        Data_Manager.instance.money1 -= 100;
                                        Data_Manager.instance.money3 -= 1;
                                        Ui_Manager.instance.UiRefresh();
                                        isSuperBuild = false;
                                        canbuild = true;

                                    }
                                    else
                                    {
                                        if(Data_Manager.instance.money1 < 100)
                                        {
                                            int rands = Random.Range(12, 16);
                                            Sound_Manager.instance.NarPlay(rands);
                                            Ui_Manager.instance.state.text = "��ȭ ����!";
                                            return;
                                        }

                                        if(towercode <10)
                                        {
                                            int rand = Random.Range(22, 25);
                                            Sound_Manager.instance.NarPlay(rand);
                                            Tower_Manager.instance.TowerInstance(hitObject, 6 , towercode);
                                            Data_Manager.instance.money1 -= 100;
                                            Ui_Manager.instance.UiRefresh();
                                            canbuild = true;
                                        }
                                        else
                                        {
                                            int rand = Random.Range(22, 25);
                                            Sound_Manager.instance.NarPlay(rand);
                                            Tower_Manager.instance.TowerInstance(hitObject, 1);
                                            Data_Manager.instance.money1 -= 100;
                                            Ui_Manager.instance.UiRefresh();
                                            canbuild = true;
                                        }    
                                    }
                                   
                                }
                                else
                                {
                                    print("������ ����");
                                }
                            }
                            if (!canbuild)
                            {
                                int rand = Random.Range(19, 22);
                                Sound_Manager.instance.NarPlay(rand);
                            }
                        }
                        break;

                    case PlayerMode.TowerSell:


                        break;

                    case PlayerMode.TowerMix:

                        RaycastHit hit1;
                        Ray ray1 = cam.ScreenPointToRay(Input.mousePosition);


                        if (Physics.Raycast(ray1, out hit1))
                        {
                            GameObject clickedTower = hit1.collider.gameObject;
                            if (clickedTower != null)
                            {
                                Component[] components = clickedTower.GetComponents<Component>();
                                foreach (Component component in components)
                                {
                                    if (component is Twr_0Base yourComponent)
                                    {
                                        // ������Ʈ �ȿ� int towerRank ���� �ִ��� Ȯ��
                                        if (yourComponent.GetType().GetField("towerRank") != null)
                                        {
                                            // int towerRank ���� �ִ� ���, ���ϴ� �۾� ����
                                            int towerRank = (int)yourComponent.GetType().GetField("towerRank").GetValue(yourComponent);

                                            GameObject towerZone = (GameObject)yourComponent.GetType().GetField("TowerZone").GetValue(yourComponent);



                                            string listName = "Tower" + (towerRank + 1);

                                            List<GameObject> towerList = (List<GameObject>)typeof(Tower_Manager).GetField(listName).GetValue(Tower_Manager.instance);
                                            GameObject towerObject = towerList.Find(GameObject => GameObject != clickedTower && GameObject.name == clickedTower.name);
                                            if (towerRank < 5 && towerRank >= 0)
                                            {
                                                //GameObject towerzone = (GameObject)targetObject.GetType().GetField("TowerZone").GetValue(targetObject);
                                                //���� �Ŵ����� ���� ����Ʈ���� ���� �̸��� ���� �ٸ� Ÿ���� ã�ƾ��Ѵ�.

                                                towerRank += 2;
                                                if (towerObject != null)
                                                {
                                                    int rand = Random.Range(9, 11);
                                                    Sound_Manager.instance.NarPlay(rand);

                                                    Tower_Manager.instance.TowerSell(towerObject, false);
                                                    Tower_Manager.instance.TowerSell(clickedTower, false);
                              
                                                    Tower_Manager.instance.TowerInstance(towerZone, towerRank);
                                                    towerZone.SetActive(true);
                                                }
                                                else
                                                {
                                                    print("�ռ��� ������ Ÿ���� �����ϴ�.");
                                                }
                                            }
                                            else
                                            {
                                                print("�ռ� �� �� ���� Ÿ�� �Դϴ�.");
                                            }
                                        }
                                    }
                                }
                                break; // ���ϴ� ������Ʈ�� ã������ ���� ����
                            }
                        }

                        break;


                    case PlayerMode.Nomal:

                        Ray rays = cam.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hitinfo = new RaycastHit();
                        if (Physics.Raycast(rays, out hitinfo))
                        {
                            if (!(hitinfo.transform.tag == "TowerZone" || hitinfo.transform.tag == "Tower"))
                            {
                                print(hitinfo.transform.gameObject.name);
                                Ui_Manager.instance.InfoPannelActive = false;
                                Ui_Manager.instance.towerInfoPannel.SetActive(false);
                            }
                        }

                        break;
                }

               
            }
        }





        if(playerMode == PlayerMode.TowerSell)
        {


        }

    }


   

    public void TowerBuildBtn()
    {
        if(playerMode!=PlayerMode.TowerBuild)
        {
            towercode = 500;
            playerMode = PlayerMode.TowerBuild;
            Ui_Manager.instance.state.text = "�Ǽ� ���";
            BtnColorReset();
            BuildBtnDark();

            foreach (GameObject zone in towerZone)
            {
                if(!zone.GetComponent<TowerZone>().towerOn)
                { 
                    zone.SetActive(true); 
                }
                else
                {
                    zone.SetActive(false);
                }
                
            }
        }
        else
        {    towercode = 500;
            playerMode = PlayerMode.Nomal;
            BtnColorReset();
            Ui_Manager.instance.state.text = "";
            Image[] imgs = Input_Manager.instance.towerBuildBtn.GetComponentsInChildren<Image>();

            foreach (GameObject zone in towerZone)
            {
                if (!zone.GetComponent<TowerZone>().towerOn)
                {
                    zone.SetActive(false); 
                }
                else
                {
                    zone.SetActive(true);
                }
            }

        }
        
    }


    public void TestTowerBuildBtn(int code)
    {
        if (playerMode != PlayerMode.TowerBuild)
        {
            towercode = code;
            playerMode = PlayerMode.TowerBuild;
            Ui_Manager.instance.state.text = "ġƮ ���";
            BtnColorReset();
            BuildBtnDark();

            foreach (GameObject zone in towerZone)
            {
                if (!zone.GetComponent<TowerZone>().towerOn)
                {
                    zone.SetActive(true);
                }
                else
                {
                    zone.SetActive(false);
                }

            }
        }
        else
        {
            towercode = 500;
            playerMode = PlayerMode.Nomal;
            BtnColorReset();
            Ui_Manager.instance.state.text = "";
            Image[] imgs = Input_Manager.instance.towerBuildBtn.GetComponentsInChildren<Image>();

            foreach (GameObject zone in towerZone)
            {
                if (!zone.GetComponent<TowerZone>().towerOn)
                {
                    zone.SetActive(false);
                }
                else
                {
                    zone.SetActive(true);
                }
            }

        }

    }

    public void TowerSuperBuildBtn()
    {
        if (Data_Manager.instance.money3 < 1)
        {
            int rand = Random.Range(12, 16);
            Sound_Manager.instance.NarPlay(rand);
            Ui_Manager.instance.state.text = "��ȭ ����!";
            return;
        }


        if (playerMode != PlayerMode.TowerBuild)
        {

            playerMode = PlayerMode.TowerBuild;
            Ui_Manager.instance.state.text = "��ȭ�Ǽ� ���";
            isSuperBuild = true;
            BtnColorReset();
            foreach (GameObject zone in towerZone)
            {
                if (!zone.GetComponent<TowerZone>().towerOn)
                {
                    zone.SetActive(true);
                }
                else
                {
                    zone.SetActive(false);
                }

            }
        }
        else
        {
            playerMode = PlayerMode.Nomal;
            BtnColorReset();
            Ui_Manager.instance.state.text = "";
 
            foreach (GameObject zone in towerZone)
            {
                if (!zone.GetComponent<TowerZone>().towerOn)
                {
                    zone.SetActive(false);
                }
                else
                {
                    zone.SetActive(true);
                }
            }

        }

    }

    public void TowerMixBtn()
    {
        if (playerMode != PlayerMode.TowerMix)
        {
            playerMode = PlayerMode.TowerMix;
            Ui_Manager.instance.state.text = "���� ���";
            BtnColorReset();
            MixBtnDark();

            foreach (GameObject zone in towerZone)
            {
                if (!zone.GetComponent<TowerZone>().towerOn)
                {
                    zone.SetActive(false);
                }
                else
                {
                    zone.SetActive(true);
                }
            }
        }
        else
        {
            playerMode = PlayerMode.Nomal;
            Ui_Manager.instance.state.text = "";
            BtnColorReset();
            foreach (GameObject zone in towerZone)
            {
                if (!zone.GetComponent<TowerZone>().towerOn)
                {
                    zone.SetActive(false);
                }
                else
                {
                    zone.SetActive(true);
                }
            }

        }
    }
    public void TowerSellBtn() 
    {
        if (playerMode != PlayerMode.TowerSell)
        {
            playerMode = PlayerMode.TowerSell;
            Ui_Manager.instance.state.text = "�Ǹ� ���";
            BtnColorReset();
            SellBtnDark();

            foreach (GameObject zone in towerZone)
            {
                if (!zone.GetComponent<TowerZone>().towerOn)
                {
                    zone.SetActive(false);
                }
                else
                {
                    zone.SetActive(true);
                }
            }
        }
        else
        {
            playerMode = PlayerMode.Nomal;
            Ui_Manager.instance.state.text = "";
            BtnColorReset();
            foreach (GameObject zone in towerZone)
            {
                if (!zone.GetComponent<TowerZone>().towerOn)
                {
                    zone.SetActive(false);
                }
                else
                {
                    zone.SetActive(true);
                }
            }

        }
    }

    public void BtnColorReset()
    {
        Image[] imgs = Input_Manager.instance.towerBuildBtn.GetComponentsInChildren<Image>();
        for(int i = 0; i < imgs.Length; i++) 
        {
            imgs[i].color = Input_Manager.instance.SaveColor[i];
        }
          
        

        imgs = Input_Manager.instance.towerSellBtn.GetComponentsInChildren<Image>();
        for (int i = 0; i < imgs.Length; i++)
        {
            imgs[i].color = Input_Manager.instance.SaveColor[i];
        }



        imgs = Input_Manager.instance.towerMixBtn.GetComponentsInChildren<Image>();
        for (int i = 0; i < imgs.Length; i++)
        {
            imgs[i].color = Input_Manager.instance.SaveColor[i];
        }


    }

    public void BuildBtnDark()
    {
        Image[] imgs = Input_Manager.instance.towerBuildBtn.GetComponentsInChildren<Image>();
        foreach (Image img in imgs)
        {
            img.color -= new Color(0.5f, 0.5f, 0.5f, 0f);
        }
    }
    public void SellBtnDark()
    {
        Image[] imgs = Input_Manager.instance.towerSellBtn.GetComponentsInChildren<Image>();
        foreach (Image img in imgs)
        {
            img.color -= new Color(0.5f, 0.5f, 0.5f, 0f);
        }
    }
    public void MixBtnDark()
    {
        Image[] imgs = Input_Manager.instance.towerMixBtn.GetComponentsInChildren<Image>();
        foreach (Image img in imgs)
        {
            img.color -= new Color(0.5f, 0.5f, 0.5f, 0f);
        }
    }


}
