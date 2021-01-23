using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{

    public GameObject Monsters;
    public int Clear_Count;
    public GameObject regentr;
    int[] mons_list;
    public GameObject[] mons;
    public int go; // 앞으로 갈 확률
    public int back;// 뒤로 갈 확률
    public GameObject Damage;
    public static MonsterManager Instance { get; private set; }
    //public 

    private void Awake()
    {
        Instance = this;
    }


    public void Regen( ) {
        int date = BarManager.Instance.date; //몬스터 id 계수
        int range;
        if (date < 15)
        {
            range = 2;
        }else if(date < 30)
        {
            range = 4;
        }
        else if (date < 45)
        {
            range = 6;
        }
        else if (date < 60)
        {
            range = 8;
        }
        else if (date < 75)
        {
            range = 10;
        }
        else 
        {
            range = 10;
        }


        int mons_cont = Mathf.FloorToInt((10 + (BarManager.Instance.date / 3))*Add_difficulty());
        //몬스터 배열 생성 /생성값은 현재 날짜에 비례해서 증가한다.

        mons_list = new int[mons_cont];//새로운 몬스터 id배열생성
        mons = new GameObject[mons_cont];//몬스터 오브젝트 배열생성

        for (int i = 0; i < mons_cont; i++)//몬스터 젠 수만큼 반복
        { 
            
            mons_list[i] = Random.Range(0, range);//몬스터id 부여
        }
        Clear_Count = mons_cont;// 몬스터 숫자만큼 카운트 할당
        StartCoroutine(RegenStart(mons_cont));//몬스터 리젠 코루틴 발동
    }

    float Add_difficulty() {
        switch (GameManager.Instance.Battle)
        {
            case GameManager.battleState.nomal:
                {
                    return 1.0f;
                }
            case GameManager.battleState.elite:
                {
                    return 1.3f;
                }
            case GameManager.battleState.boss:
                {
                    return 1.5f;
                }

        }
        return 1.0f;
    }


    public IEnumerator RegenStart(int mons_cont)//리젠되는 숫자를 매개변수로 받음
    {
        for (int i = 0; i < mons_cont; i++)//리젠숫자만큼 반복
        {
            //보스몬스터 젠 되는 코드
            if ((i == mons_cont - 1 && GameManager.Instance.Battle == GameManager.battleState.boss))
            {
                Hero_Skill_Popup information = PopupManager.Instance.ShowHero_Skill_Popup();
                information.SetText("보스 등장", "강력한 적이 등장했습니다!");
                mons[i] = Instantiate(GetEnemyInfo.Instance.enemy[mons_list[i]], regentr.transform.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                //몬스터 오브젝트 생성

                mons[i].transform.localScale *= 2f;

                mons[i].name = GetEnemyInfo.Instance.getEnemyName(mons_list[i]) + i;
                //오브젝트 이름은 해당하는 코드+ 숫자로 할당
                mons[i].GetComponent<Monster>().Currentlocation = regentr;
                //리젠되는 위치와 현재위치값 할당
                mons[i].GetComponent<Monster>().Targettile = TileManager.Instance.tiles[0];
                //처음 리젠되고 목적지 할당
                mons[i].transform.SetParent(Monsters.transform);

                mons[i].GetComponent<Monster>().isBoss = true;
                GameObject par = Instantiate(EffectManager.Instance.BossEffect, mons[i].transform.position, mons[i].transform.rotation);
                par.transform.SetParent(mons[i].transform);
                par.transform.localScale = Vector3.one;
                SoundManager.Instance.Boss_encount();
                break;
            }
                 mons[i] = Instantiate(GetEnemyInfo.Instance.enemy[mons_list[i]], regentr.transform.position, Quaternion.Euler(new Vector3(0, -90, 0)));
                //몬스터 오브젝트 생성
                mons[i].name = GetEnemyInfo.Instance.getEnemyName(mons_list[i]) + i;
                //오브젝트 이름은 해당하는 코드+ 숫자로 할당
                mons[i].GetComponent<Monster>().Currentlocation = regentr;
                //리젠되는 위치와 현재위치값 할당
                mons[i].GetComponent<Monster>().Targettile = TileManager.Instance.tiles[0];
                //처음 리젠되고 목적지 할당
                mons[i].transform.SetParent(Monsters.transform);
                yield return new WaitForSeconds(Random.Range(0.1f, 0.7f));//리젠 간격은 01~0.7초 랜덤

        }


    }


    public void Move(GameObject mon)
    {
         switch( mon.GetComponent<Monster>().Currentlocation.name)
        {
            case "regen":mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[0].GetComponent<Tile>().tar_lo;
              
                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());
               
                break;
            case "TargetLocation_0" : /////////////////0번타일
                if (Random.Range(1, 100) >= back)//앞으로 감
                {
                    switch (Random.Range(0, 3)){
                        case 0:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[1].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[1];
                               StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 1:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[2].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[2];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 2:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[3].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[3];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                    }

                }//뒤로감
                else {
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[1].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[1];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 1:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[2].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[2];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 2:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[3].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[3];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                    }

                }
                break;

            case "TargetLocation_1"://///////////////1번타일
                if (Random.Range(1, 100) >= back)//앞으로 감
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[2].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[2];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 1:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[4].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[4];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                    }

                }//뒤로감
                else
                {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[0].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[0];
                    StartCoroutine(mon.GetComponent<Monster>().GotoTarget());


                }
                break;

            case "TargetLocation_2"://///////////////2번타일
                if (Random.Range(1, 100) >= back)//앞으로 감
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[4].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[4];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 1:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[5].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[5];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 2:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[6].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[6];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                    }

                }//뒤로감
                else
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[0].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[0];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 1:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[1].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[1];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 2:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[3].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[3];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                    }

                }
                break;

            case "TargetLocation_3"://///////////////3번타일
                if (Random.Range(1, 100) >= back)//앞으로 감
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[2].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[2];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 1:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[6].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[6];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }

                    }

                }//뒤로감
                else
                {

                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[0].GetComponent<Tile>().tar_lo;
                    mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[0];
                    StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

            

                }
                break;


            case "TargetLocation_4"://///////////////4번타일
                if (Random.Range(1, 100) >= back)//앞으로 감
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[5].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[5];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 1:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[7].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[7];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                    }

                }//뒤로감
                else
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[1].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[1];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 1:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[2].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[2];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }

                    }

                }
                break;


            case "TargetLocation_5"://///////////////5번타일
                if (Random.Range(1, 100) >= back)//앞으로 감
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[7].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[7];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 1:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[8].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[8];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 2:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[9].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[9];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                    }

                }//뒤로감
                else
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[2].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[2];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 1:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[4].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[4];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 2:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[6].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[6];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                    }

                }
                break;

            case "TargetLocation_6"://///////////////6번타일
                if (Random.Range(1, 100) >= back)//앞으로 감
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[5].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[5];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 1:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[9].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[9];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                    }

                }//뒤로감
                else
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[2].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[2];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 1:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[3].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[3];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }

                    }

                }
                break;

            case "TargetLocation_7"://///////////////7번타일
                if (Random.Range(1, 100) >= back)//앞으로 감
                {
                    mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[8].GetComponent<Tile>().tar_lo;
                    mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[8];
                    StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                }//뒤로감
                else
                {
                    

                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[4].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[4];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 1:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[5].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[5];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                    }
                }
                break;

            case "TargetLocation_8": /////////////////8번타일
                if (Random.Range(1, 100) >= back)//앞으로 감
                {
                    mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[10].GetComponent<Tile>().tar_lo;
                    mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[10];
                    StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                }//뒤로감
                else
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[5].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[5];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 1:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[7].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[7];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 2:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[9].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[9];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                    }

                }
                break;


            case "TargetLocation_9"://///////////////9번타일
                if (Random.Range(1, 100) >= back)//앞으로 감
                {
                    mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[8].GetComponent<Tile>().tar_lo;
                    mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[8];
                    StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                }//뒤로감
                else
                {


                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[5].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[5];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                        case 1:
                            {
                                mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[6].GetComponent<Tile>().tar_lo;
                                mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[6];
                                StartCoroutine(mon.GetComponent<Monster>().GotoTarget());

                                break;
                            }
                    }
                }
                break;


            case "TargetLocation_h"://///////////////9번타일

                {
                    mon.GetComponent<Monster>().Targetlocation = TileManager.Instance.tiles[8].GetComponent<Tile>().tar_lo;
                    mon.GetComponent<Monster>().Targettile = TileManager.Instance.tiles[8];
                    StartCoroutine(mon.GetComponent<Monster>().GotoTarget());
                }

                break;
        }



    }


    public void InTile(GameObject mon) //목표타일에 전투 대상이 있는지 확인
    {
        Tile Current_tile = mon.GetComponent<Monster>().CurrentTile;//타일할당
        if (Current_tile.GetComponent<Tile>().Mons[0] == null && isUnit(Current_tile))
        {   //현재타일에 몬스터배열 3개가 차있지 않고 적유닛이 있을경우
            Current_tile.GetComponent<Tile>().Mons[0] = mon; //타일에있는 몬스터 배열에 몬스터 할당
            mon.transform.position = Current_tile.Mons_po[0].transform.position;//위치 할당
            mon.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));//회전도 할당
            mon.GetComponent<Monster>().animator.SetBool("isIdle", true);//애니메이션 변수 변경
            StartCoroutine(mon.GetComponent<Monster>().Battle_Monster());//몬스터 전투시작 코루틴

        }
        else if (Current_tile.GetComponent<Tile>().Mons[1] == null && isUnit(Current_tile))
        {
            Current_tile.GetComponent<Tile>().Mons[1] = mon;
            mon.transform.position = Current_tile.Mons_po[1].transform.position;
            mon.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
            mon.GetComponent<Monster>().animator.SetBool("isIdle", true);
            StartCoroutine(mon.GetComponent<Monster>().Battle_Monster());
        }
        else if (Current_tile.GetComponent<Tile>().Mons[2] == null && isUnit(Current_tile))
        {
            Current_tile.GetComponent<Tile>().Mons[2] = mon;
            mon.transform.position = Current_tile.Mons_po[2].transform.position;
            mon.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
            mon.GetComponent<Monster>().animator.SetBool("isIdle", true);
            StartCoroutine(mon.GetComponent<Monster>().Battle_Monster());
        }
        else {//꽉차있으면
            Move(mon);//다시 움직임
        }


    }



    public bool isUnit(Tile Tile)//유닛이 하나도 없으면 false  하나라도 있으면 트루 
    {
        if (Tile.GetComponent<Tile>().Unit[0] != null || Tile.GetComponent<Tile>().Unit[1] != null || Tile.GetComponent<Tile>().Unit[2] != null ) 
            //|| Tile.GetComponent<Tile>().Unit[0].activeSelf ==false || Tile.GetComponent<Tile>().Unit[1].activeSelf == false || Tile.GetComponent<Tile>().Unit[2].activeSelf == false)
        {
            return true;
        }
        else
        { 
            return false;
        }

    }


    public void DamageFont_produce(int dmg, GameObject target) {
        int digit = (int)Mathf.Floor(Mathf.Log10(dmg)) + 1;

        GameObject Damage_font = Instantiate(Damage, target.transform.position+ new Vector3(0.1f*digit,1.5f,0), Quaternion.Euler(new Vector3(0, 0, 0)));
        Damage_font.GetComponent<Damage_Font>().Damage_int = dmg;



    }
}
