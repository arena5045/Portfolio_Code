포트폴리오에 사용된 프로젝트들의 코드들을 모아놓은 깃입니다

기능을 설명하는 대부분의 이미지는 gif로 로딩시간이 있을 수 있습니다. 
--------------------------------------------------------
 # 0. 라이클 프로젝트(Unity_C#)
|프로젝트 명|라이클 프로젝트|
|------|---|
|노션 주소|https://www.notion.so/18e6ee3f0926802e89dccf448e17f63a?pvs=4|


주요 구현 내용
1. 메인화면과 로비등 Ui 기능개발
2. 인 게임내의 플레이어 상호작용
3. 프로젝트 내 미니게임 8종 이상 개발
4. 프로젝트 내 슈팅로그라이크 게임과 같은 소규모 게임 개발
5. 프로그램 <-> 서버 db와의 연동, 데이터 전달

사용자의 재활훈련, 운동활동을 위해 자전거형 기구 & 해당 기구에 들어가는 유니티 응용프로그램 개발 프로젝트입니다.

해당 유니티 프로젝트의 1인 개발자로써,
기획과 포트연동, 그래픽 부분을 제외한 개발을 전담하였습니다.

--------------------------------------------------------
 # 1. 전선지휘(Unity_C#)
![전지](https://github.com/arena5045/Portfolio_Code/assets/64789660/a64d005e-b440-41a3-8897-4bf028d06fb1)
   
로그라이크 형식의 디펜스 게임입니다

|프로젝트 명|전선지휘|
|------|---|
|프로젝트 주소|https://github.com/Team24-NewAge/FrontLine|
|소개 영상|https://youtu.be/h9TNRrmcwDk(10분) // https://youtu.be/SHcQa_yFBNI(1분)|
|팀원|서정우(메인 프로그래머) 외 3명|
|주요 구현 내용|메인 UI 구현과 상호작용,몬스터와 플레이어 스킬 구현, 전투시스템 개발, 유닛의 강화와 조합|
<!--|개발 기간|2020.05 ~ 2020.12|-->

 - 1.BattleManager | 전투에 활용된 싱글턴 코드입니다
 - 2.PaperManager | 메인 선택 화면에서 활용되는 paper오브젝트를 관리하는 싱글턴 코드입니다
 - 3.MonsterManager | 몬스터 관리에 사용되는 싱글턴 코드입니다
 - 4.CheckingInvenManager | 인벤토리와 조합 강화에 사용되는 싱글턴 코드입니다
 - 5.Damage_Font | 전투 시 데미지에 따라 데미지폰트를 출력해주는 코드입니다
 - 6.Monster | 몬스터 오브젝트에 내장되어있는 코드입니다
 - 7.TodayPaper | 클릭가능한 paper에 사용되는 코드입니다
 - 8.UnitBuy_Popup | 유닛 구매 팝업에 사용되는 코드입니다
 - 9.Unit | 유닛 오브젝트에 내장되어있는 코드입니다
 - 10.Rebuild_Popup | 보강 기능에 사용되는 코드입니다

<details>
    <summary> 자세히 </summary> 
 
<img src="https://github.com/arena5045/Portfolio_Code/assets/64789660/a5581bc6-e964-4d12-a769-b24cdeaa0d8d" width="474" height="256"/>

ㄴ메인 화면 ui 구현

 
<img src="https://github.com/arena5045/Portfolio_Code/assets/64789660/02bf43dd-ad88-4bd6-8b37-b3bf86cceac0" width="474" height="256"/>

ㄴ전투 플레이 화면 

몬스터는 플레이어가 미리 배치해둔 유닛과 자동으로 전투를 하며
플레이어는 영웅의 스킬을 조작할 수 있습니다


<img src="https://github.com/arena5045/Portfolio_Code/assets/64789660/f0299113-a878-4807-89ce-b4faadecec30" width="474" height="256"/>

ㄴ이벤트와 보강기능 시연 장면

<!-- summary 아래 한칸 공백 두고 내용 삽입 -->
</details>

 --------------------------------------------------------
 # 2. RTD랜덤타워디펜스(Unity_C#)
<img src="https://github.com/arena5045/Portfolio_Code/assets/64789660/ce207dd4-c148-49ec-97fd-936bf630093b" width="474" height="256"/>

|프로젝트 명|RTD랜덤타워디펜스|
|------|---|
|프로젝트 주소|https://github.com/Team-RTD/Project_RTD|
|소개 영상|https://youtu.be/Hw0U9L20Tqg|
|팀원|서정우(팀장) 외 3명|
|주요 구현 내용|메인 화면 ui 상호작용, 타워의 생성 판매 업그레이드,카메라 무브|
<!--|개발 기간|2023.08 (약 3주)|-->

스타크래프트 유즈맵 랜덤타워디펜스를 모티브로 만든 타워디펜스 게임입니다

 - 1.Camera_Controller | 휠, 커서 모서리 위치에 반응하는 카메라 코드입니다
 - 2.ClickSystem | 타워 건설, 조합, 인포창을 띄울때 사용되는 화면을 클릭할 때 사용되는 코드입니다
 - 3.Data_Manager | 게임 내 재화,프로퍼티 변수 , 타이머 등을 관리하는 싱글턴 매니저입니다
 - 4.Tower_Manager | 타워 생성, 업그레이드, 판매에 관여하는 싱글턴 매니저입니다
 - 5.Ui_Manager | 게임 내의 UI를 전반적으로 관리하는 싱글턴 매니저입니다
--------------------------------------------------------
 # 3. HnS핵앤슬래시(Unity_C#)
 ![핵슬](https://github.com/arena5045/Portfolio_Code/assets/64789660/b86db90b-3702-476b-8d6f-81e1731d0ceb)
 
  핵 앤 슬래시 장르를 기반으로 만든 액션 RPG 프로젝트입니다
  
|프로젝트 명|HnS핵앤슬래시|
|------|---|
|프로젝트 주소|https://github.com/Team-HnS/Project_HnS|
|소개 영상|https://youtu.be/XHCT59WcP9w|
|팀원|서정우(팀장) 외 3명|
|주요 구현 내용|플레이어 이동과 공격,스킬 등을 비롯한 FSM 구현, 유니티 navmeshagent 네비게이션 시스템 |
<!--|개발 기간|2023.10 (약 3주)|-->


 - 1.Player | 플레이어 FSM을 관리하는 코드입니다
 - 2.PlayerMovement | 플레이어 이동,NAVMESH를 관리하는 코드입니다 
 - 3.PlayerSound | 플레이어의 발걸음소리, 평타 소리를 출력하는 코드입니다 
 - 4.NomalAttack | 기본공격 애니메이션안에 들어있는 OnState~ 코드들입니다 
 - 5.SkillSet_Library | 스킬의 효과들을 저장해놓는 코드입니다
 - 6.SkillBuildSlot | 스킬을 꼈다 뺐다 하며 원하는 위치에 놓을수있는 스킬 슬롯 UI에 사용된 코드입니다
 - 7.KeyInputManager | 키를 누르면 SkillBuildSlot에있는 스킬데이터를 가져와 해당 데이터에있는 SkillSet_Library를 찾아서 실행시키는 코드입니다.

 --------------------------------------------------------
 # 4.  XRVR_MA(Unity_C#)
 <img src="https://github.com/arena5045/Portfolio_Code/assets/64789660/0850fd56-8725-4ddd-9794-0b9464b57694" width="474" height="256"/>
 
괴리성 밀리언 아서 vr을 모티브로 만들어본 XR 기반 게임입니다

|프로젝트 명|XRVR_MA|
|------|---|
|프로젝트 주소|https://github.com/Team-HnS/Project_HnS|
|소개 영상|-|
|팀원|서정우 개인 작품|
|주요 구현 내용|xr 기반 멀티플랫폼 Inputsystem사용, Vr 컨트롤러를 이용한 ui상호작용|
<!--|개발 기간|2023.11 (약 18일)|-->



 - 1.BattleManager | 전투, 배틀 턴 등을 실행하는 코드입니다
 - 2.Card | 카드 데이터의 저장과 처리를 담당하는 코드입니다
 - 3.CardBtn| 카드UI, 클릭 시 처리를 담당하는 코드입니다
 - 4.Monster | 몬스터 데이터와, 데미지 등 처리를 담당하는 코드입니다

--------------------------------------------------------
 # 5. The Zombie(Unity_C#)
  <img src="https://github.com/arena5045/Portfolio_Code/assets/64789660/d59361ec-acf7-41c8-88dc-94cdfb5becd7" width="474" height="256"/>

맵에스폰되는 좀비를 잡으며 생존하는 스테이지형식의 탑뷰 멀티플레이(Photon) 기능을 포함하는 미니게임입니다

|프로젝트 명|The Zombie|
|------|---|
|팀원|서정우 개인 작품|
|주요 구현 내용|Photon을 사용한 멀티플레이 구현, 총기에 따른 여러 발사방식 구현|
<!--|개발 기간|약 14일|-->

 - 1.Enemy | 좀비에 사용되는 코드입니다
 - 2.EnemySpawner | 좀비를 리젠시켜주는 코드입니다
 - 3.Gun | 총 오브젝트에 사용되는 코드입니다.
 - 4.Gun4 | 유도 미사일에 사용되는 코드입니다
 - 5.Intro_Effect | 인트로화면 카메라 이펙트에 사용되는 코드입니다
 - 6.LivingEntity | 전투를 할 수 있는 오브젝트의 인터페이스입니다
 - 7.LobbyManager |  방 생성, 참가 등 멀티기능을 구현한 코드입니다
 - 8.Missile_bullet |  유도미사일 탄환에 사용되는 코드입니다
 - 9.PlayerHealth | 플레이어 체력에 관하여 사용되는 코드입니다
 - 10.UIManager | 점수, 탄창 등 UI를 관리하는 싱글턴 코드입니다
 - 11.Chatmanager | 멀티 중 채팅기능을 구현한 코드입니다

 <img src="https://github.com/arena5045/Portfolio_Code/assets/64789660/84856728-b7c8-40a0-975e-f090cd8d1b7d" width="474" height="256"/>

 ㄴ무기변경 시연 모습 1번무기와 2번무기는 laycast로 구현, 3번무기는 발사체 형식이며 4번무기는 유도미사일 형식입니다

 <img src="https://github.com/arena5045/Portfolio_Code/assets/64789660/28125bf9-8254-4467-8939-3604c85f0649" width="474" height="256"/>
 
 ㄴ채팅 시연 모습입니다

--------------------------------------------------------
 # 6. Tank in White(Unity_C#)
 
 <img src="https://github.com/arena5045/Portfolio_Code/assets/64789660/54304bf4-8451-45aa-9434-a472a0399099" width="474" height="256"/>

맵에 스폰되는 적 탱크를 잡는 3인칭 슈팅 게임입니다

|프로젝트 명|Tank in White|
|------|---|
|팀원|서정우 개인 작품|
|주요 구현 내용|인터페이스 상속을 이용한 아이템 구현,마우스 위치에 따른 포신 로테이션 트래킹|
<!--|개발 기간|약 7일|-->


 - 1.Cannon | 탱크가 쏘는 탄환에 사용된 코드입니다
 - 2.Enemy |  적 탱크에 사용된 코드입니다
 - 3.FireCannon |  탄환 발사에 사용되는 코드입니다
 - 4.GameManager |  점수, 게임오버 등 을 담당하는 싱글턴 코드입니다
 - 5.ItemSpawner | 아이템을 플레이어 주변에 스폰시켜주는 코드입니다
 - 6.Tank_Rand |  인트로 화면에서 탱크의 연출에 사용된 코드입니다
 - 7.TrackAnim | 탱크 무한궤도 애니메이션 연출에 사용된 코드입니다
 - 8.TurretCtrl |  터렛을 마우스 클릭 지점을 따라 보게하는 코드입니다
 - 9.UIManager |  화면에 UI를 관리하는 싱글턴 코드입니다
--------------------------------------------------------
 # 7. TCGGAME(Unity_C#)

 <img src="https://github.com/arena5045/Portfolio_Code/assets/64789660/80cc9c87-26dc-4bcf-b2e7-9bef91c83780" width="474" height="256"/>

 UNITY UI를 이용하여 구현해본 카드게임의 프로토 타입입니다
 
|프로젝트 명|TCGGAME|
|------|---|
|팀원|서정우 개인 작품|
|주요 구현 내용|ui 상호작용을 통한 카드게임 구현|
<!--|개발 기간|약 10일|-->




 - 1.CardManager | 패에서 카드를 필드에 드래그 드롭하고, 적을 공격하게 만드는 기본적인 상호작용 매니저입니다
 - 2.EnemyAiManager |  상대방 턴에 적의 행동을 간단하게 구현한 AI코드입니다

--------------------------------------------------------
 # 8.Photon Tank(Unity_C#)
  <img src="https://github.com/arena5045/Portfolio_Code/assets/64789660/d2da3575-5f4b-477c-a31d-f9f50c2ecafe" width="474" height="256"/>

  UNITY와 Photon을 사용하여 만든 멀티플레이 테스트 게임입니다
  
|프로젝트 명|Photon Tank|
|------|---|
|팀원|서정우 개인 작품|
|주요 구현 내용|Photon을 이용한 멀티플레이 구현|
<!--|개발 기간|약 10일|-->

 - 1.PhotonInit | 포톤을 사용해 방(로비)를 만들고 입장하는 멀티플레이 구현 코드입니다
 - 2.FireCannon |  RPC를 사용하여 포탄 발사를 구현한 코드입니다
 - 3.TankMove |  포톤뷰를 이용하여 이동하는 코드입니다
 - 4.TurretCtrl |  포톤뷰를 이용하여 포신을 회전하는 코드입니다

--------------------------------------------------------
 # 9. Prena(app_Java)

자바를 이용하여 만든 프라모델 정보 어플리케이션입니다

 - 1.PickList |  찜 화면을 구현한 코드입니다
 - 2.Gundam_pick_Info |  찜 화면에서 클릭하면 나오는 정보창을 구현한 코드입니다
 - 3.GundamList | 한정판 목록을 구현한 코드입니다
 - 4.Webfragment | 웹 화면을 구현한 코드입니다

--------------------------------------------------------
 # 10. Prena(web_JS)

자바스크립트와 HTML을 이용하여 만든 프라모델 정보 사이트입니다
 - 1.index.html | 홈페이지 메인 프레임을 구현한 html코드입니다
 - 2.qna.js | 질의응답에 사용된 자바스크립트 코드입니다 
--------------------------------------------------------

