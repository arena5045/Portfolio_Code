
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement; // 씬 관리자 관련 코드
using UnityEngine.UI; // UI 관련 코드

// 필요한 UI에 즉시 접근하고 변경할 수 있도록 허용하는 UI 매니저
public class UIManager : MonoBehaviour {
    // 싱글톤 접근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance; // 싱글톤이 할당될 변수

    public Text ammoText; // 탄약 표시용 텍스트
    public Text scoreText; // 점수 표시용 텍스트
    public Text waveText; // 적 웨이브 표시용 텍스트
    public GameObject gameoverUI; // 게임 오버시 활성화할 UI 

    public GameObject ammoui;//
    public GameObject ammoimage;
    public GameObject loc;
    public GameObject[] ammos = new GameObject[4];

    public Image smg, ar, r45, mlrs;
    public Text smg_text, ar_text, r45_text, mlrs_text;
    public bool unlock_ar= false,unlock_r45 = false,unlock_mlrs = false;

    int Magazine;
    public PlayerShooter playerShooter;

    // 탄약 텍스트 갱신
    public void UpdateAmmoText(int magAmmo, int remainAmmo) {
        ammoText.text = magAmmo + "/" + remainAmmo;
    }


    public void UpdateAmmoImage(int remainAmmo)//남은 탄환수를 받아옴
    {


        switch (playerShooter.gunnum) { //총기 종류에따라 탄창에들어간 탄 갯수
            case 1: Magazine = 25; break;
            case 2: Magazine = 40; break;
            case 3: Magazine = 12; break;
            case 4: Magazine = 6; break;
        }


        for (int i = 0; i < ammos.Length; i++) {
            Destroy(ammos[i]);//처음 실행될때 모든 이미지 폐기
        }

        int ammocount = remainAmmo / Magazine; //생성될 이미지 숫자
        ammos = new GameObject[ammocount+1]; //게임오브젝트 배열생성
        for (int i = 0; i <= ammocount; i++) //생성수만큼 반복
        {
            //이미지 생성
            ammos[i]= Instantiate(ammoimage, loc.transform.position, Quaternion.identity, GameObject.Find("Ammo Display").transform); 
            RectTransform r = ammos[i].GetComponent<RectTransform>(); //좌표 변수 생성
            r.anchoredPosition = new Vector3(40 * i, -50, 0); //좌표 지정
            if (i == ammocount) //마지막 이미지는 fill로 채워지게 생성
            {
                //남은 탄환 / 만발탄창 비율만큼 fill
                ammos[i].GetComponent<Image>().fillAmount = (remainAmmo% Magazine) / (float)Magazine;
            }
        }



    }

    // 점수 텍스트 갱신
    public void UpdateScoreText(int newScore) {
        scoreText.text = "Score : " + newScore;
        UnLockWeapon();//무기 해금 실행
    }

    // 적 웨이브 텍스트 갱신
    public void UpdateWaveText(int waves, int count) {
        waveText.text = "Wave : " + waves + "\nEnemy Left : " + count;
    }

    // 게임 오버 UI 활성화
    public void SetActiveGameoverUI(bool active) {
        gameoverUI.SetActive(active);
    }

    // 게임 재시작
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 게임 종료
    public void GameExit()
    {
        Application.Quit();
    }


    public void UnLockWeapon() {
        if (GameManager.instance.score >= 100 && !unlock_ar)//100점이상 ar이 언록되지 않았을경우
        {
            ar_text.fontSize = 30;//ar버튼 텍스트 폰트 값 변경
            ar_text.text ="Num.2"; // 스코어 조건=>변경키 설명
            unlock_ar = true;//해금
        }

        if (GameManager.instance.score >= 500 && !unlock_r45)
        {
            r45_text.fontSize = 30;//r45버튼 텍스트 폰트 값 변경
            r45_text.text = "Num.3";// 스코어 조건=>변경키 설명
            unlock_r45 = true;//해금
        }

        if (GameManager.instance.score >= 1000 && !unlock_mlrs)
        {
            mlrs_text.fontSize = 30;//mlrs버튼 텍스트 폰트 값 변경
            mlrs_text.text = "Num.4";// 스코어 조건=>변경키 설명
            unlock_mlrs = true;//해금
        }


    }

    public void ChangeGunUI(int num) //총기 버튼 ui 변경
    {   Color nomal = Color.white; //활성화 색
        Color translucency = Color.white; //반투명 색 지정
        translucency.a = 0.3f; //반투명 정도


        switch (num) { //플레이어의 총기번호를 가져욤
            case 1: //smg일경우
                smg.color = nomal;
                ar.color = translucency;
                r45.color = translucency;
                mlrs.color = translucency;
                break;
            case 2://ar일경우
                smg.color = translucency;
                ar.color = nomal;
                r45.color = translucency;
                mlrs.color = translucency;
                break;
            case 3://r45일경우
                smg.color = translucency;
                ar.color = translucency;
                r45.color = nomal;
                mlrs.color = translucency;
                break;
            case 4://mlrs일경우
                smg.color = translucency;
                ar.color = translucency;
                r45.color = translucency;
                mlrs.color = nomal;
                break;
        }


    }
}