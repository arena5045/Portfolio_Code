포트폴리오에 사용된 프로젝트들의 코드와 노션링크들을 모아놓은 깃입니다


기능을 설명하는 대부분의 이미지는 gif로 로딩시간이 있을 수 있습니다. 

# Unity Game Developer Portfolio

안녕하세요. Unity 기반으로 게임플레이와 클라이언트 개발을 연습하고 있는 개발자 **서정우**입니다.  
로그라이크, 디펜스, 액션 RPG, 멀티플레이, XR 등 여러 장르의 프로토타입을 Unity로 구현해 보며 개발 경험을 쌓고 있습니다.

<p align="center">
  <img src="https://github.com/arena5045/Portfolio_Code/assets/64789660/02bf43dd-ad88-4bd6-8b37-b3bf86cceac0" width="474" height="256" alt="전선지휘 게임플레이"  />
  <img src="https://github.com/arena5045/Portfolio_Code/assets/64789660/b86db90b-3702-476b-8d6f-81e1731d0ceb"width="474" height="256"  alt="HnS 핵앤슬래시"  />
  <img src="https://github.com/user-attachments/assets/5df6df66-3b8f-4f92-8112-0eb830d0a99f" width="474" height="256" alt="라이클 재활 자전거 프로젝트"  />
</p>

- 📌 **목표 포지션**: Unity 클라이언트 / 게임플레이 프로그래머  
- 📧 **E-mail**: `dnwjdtj10@naver.com`  
- 🐙 **GitHub**: https://github.com/arena5045/Portfolio_Code  

---

## 🛠 Tech Stack

**Engine & Language**
- Unity (2D/3D)
- C#

**Gameplay / System**
- 로그라이크 디펜스, 핵앤슬래시, 타워 디펜스
- FSM 기반 플레이어/몬스터 상태 관리
- 스킬, 인벤토리, 상점, 강화/조합, 웨이브/스폰 시스템

**Network / Multiplayer**
- Photon PUN  
  - 룸 생성/입장, 플레이어/좀비 동기화  
  - RPC 기반 공격/발사체 처리  
  - 인게임 채팅 구현 경험

**XR / 기타**
- XR Interaction / Input System  
- Unity UI / Canvas, 모바일 조작(UI 조이스틱, 버튼)  
- Java(Android 간단 앱), JavaScript/HTML 간단 웹

---

## ⭐ Representative Projects

> 자세한 내용은 아래 각 섹션(#0 ~ #11)에서 확인 가능합니다.

### 0. [라이클 프로젝트 (Unity_C#)](#0-라이클-프로젝트unity_c) – 의료 재활 자전거 기구용 Unity 앱

- **역할**: Unity 1인 개발 (기획/포트 연동/그래픽 제외 전체 개발)
- **핵심 기능**
  - 메인 화면 · 로비 · 미니게임 8종 이상 UI/로직 구현
  - 자전거 기구 입력과 연동된 인게임 상호작용 처리
  - Unity ↔ 서버 DB 연동 및 운동 데이터 송수신
 
    
- **고민한 점 & 배운 점**
  - 로컬에서 수집한 운동 데이터를 서버 DB로 전송하는 과정을 구현하면서  
    비동기 처리에 대한 이해를 키우며 네트워크 끊김과 같은 돌발 상황에 데이터가 유실되지 않도록 깊게 고민했습니다.
  - 실제 장비와 연결된 거대한 맵에 많은 오브젝트가 존재하다 보니,  
    어떤 오브젝트를 언제 활성/비활성화하고 불필요한 연산을 어떻게 줄일지 계속 시험해 보며  
    비교적 간단한 최적화만으로도 프레임 드랍을 눈에 띄게 줄일 수 있다는 걸 경험했습니다.
  - 일정 후반부에는 크런치에 가까운 작업을 하면서,  
    체력 관리와 작업 우선순위가 무너지면 코드 퀄리티와 협업에 바로 영향을 준다는 걸 느꼈고,  
    스케줄과 컨디션 관리도 개발의 일부로 봐야 한다고 생각하게 됐습니다.

---

### 1. [전선지휘 (Unity_C#)](#1-전선지휘unity_c) – 로그라이크 디펜스

- **역할**: 메인 프로그래머 (4인 팀)
- **핵심 기능**
  - 메인 UI, 유닛 배치/강화/조합 시스템
  - 몬스터 웨이브, 영웅 스킬, 전투 로직

    
- **고민한 점 & 배운 점**
  - 초반에는 팀 전체의 로그라이크 디펜스 장르 이해도가 낮아서  
    기획 의도와 실제 구현이 어긋나는 문제가 자주 발생했습니다.  
    이 경험을 통해, 개발 전에 장르 구조와 핵심 메커니즘을 팀원들과 먼저 공유하는 것이  
    구현 속도와 완성도 모두에 중요하다는 걸 배웠습니다.
  - 팀 프로젝트를 진행하면서, 누가 어떤 기능과 코드를 책임지는지 명확히 나누지 않으면  
    작업 범위가 겹치거나 빠지는 일이 생긴다는 걸 체감했고,  
    역할 분담과 인터페이스 정의를 설계 단계에서 맞춰두는 것의 중요성을 느꼈습니다.
  - 전투/자원 관리를 위해 싱글턴 매니저를 많이 사용했는데,  
    프로젝트가 커질수록 매니저 간 의존성이 꼬이면서 가독성과 유지보수성이 떨어지는 문제를 겪었습니다.  
    그 뒤로는 “정말 싱글턴이 필요한 부분인가?”를 먼저 고민하게 되었고,  
    싱글턴 없이도 처리할 수 있는 부분은 더 단순한 구조로 풀 수 있는지 생각하면서
    의존성을 줄이는 방향을 의식적으로 설계하게 되었습니다.

---

### 2. [HnS 핵앤슬래시 (Unity_C#)](#3-hns핵앤슬래시unity_c) – 탑뷰 액션 RPG 프로토타입

- **역할**: 팀장 / 클라이언트 개발
- **핵심 기능**
  - 플레이어 이동/공격, 콤보 기반 근접 전투
  - 몬스터 AI, NavMesh를 활용한 추적/이동
  - FSM 기반 상태 관리(대기/이동/공격/피격/사망 등)
  - 스탯 · 스킬 · 레벨업 시스템 기초 구현
  - 
- **고민한 점 & 배운 점**
  - 프로젝트를 진행하며 간단한 셰이더 개념을 공부하고 적용해 보는 계기가 되었고,  
    시각 효과를 코드와 분리해서 관리하면 전투 연출을 훨씬 유연하게 다룰 수 있다는 걸 느꼈습니다.
  - 장판형, 투사체형, 즉발형 등 다양한 타입의 스킬을 직접 구현해 보면서  
    어떤 방식으로 설계하고 나누면 더 깔끔하게 동작할지 계속 고민할 수 있었고,  
    그 과정에서 스킬 구현 전반에 대한 경험을 차근차근 쌓을 수 있었습니다.
---

## 📂 Projects Overview

--------------------------------------------------------
 # 0. 라이클 프로젝트(Unity_C#)

 <img src="https://github.com/user-attachments/assets/5df6df66-3b8f-4f92-8112-0eb830d0a99f" width="474" height="256"/>
 <img src="https://github.com/user-attachments/assets/7e191eac-81af-43a9-8b77-0e47643dc6e5" width="474" height="256"/>
 
|프로젝트 명|라이클 프로젝트|
|------|---|
|노션 주소|https://www.notion.so/18e6ee3f0926802e89dccf448e17f63a?pvs=4|


주요 구현 내용
1. 메인화면과 로비등 Ui 기능개발
2. 인 게임내의 플레이어 상호작용
3. 프로젝트 내 미니게임 8종 이상 개발
4. 프로젝트 내 슈팅로그라이크 게임과 같은 소규모 게임 개발
5. 프로그램 <-> 서버 db와의 연동, 데이터 전달

사용자의 재활훈련, 운동활동을 위해 자전거형 기구와 해당 기구에 들어가는 유니티 응용프로그램 개발 프로젝트입니다.

해당 유니티 프로젝트의 1인 개발자로써,
기획과 포트연동, 그래픽 부분을 제외한 개발을 전담하였습니다.

--------------------------------------------------------
 # 1. 전선지휘(Unity_C#)
![전지](https://github.com/arena5045/Portfolio_Code/assets/64789660/a64d005e-b440-41a3-8897-4bf028d06fb1)
   
로그라이크 형식의 디펜스 게임입니다
<!--|프로젝트 주소|https://github.com/Team24-NewAge/FrontLine|-->
|프로젝트 명|전선지휘|
|------|---|
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
<!--|프로젝트 주소|https://github.com/Team-RTD/Project_RTD|-->

|프로젝트 명|RTD랜덤타워디펜스|
|------|---|
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
  <!--|프로젝트 주소|https://github.com/Team-HnS/Project_HnS|-->
  
|프로젝트 명|HnS핵앤슬래시|
|------|---|
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

<img src="https://github.com/user-attachments/assets/3bddc46a-54a5-4e0f-99e4-eaa1524b5558" width="256" height="474"/>

해당 프로젝트는 부트캠프 우수프로젝트로 유튜브 광고에 등재되었습니다
 --------------------------------------------------------
  # 4. 묵향귀무록(Unity_C#)
 ![GIF 2026-02-21 오전 3-16-33](https://github.com/user-attachments/assets/7477a65d-7c95-4bb0-a9fa-a38673c11d03)

 바이브 코딩의 시대, AI의 도움을 받아 빠르게 진행한 텍스트 로그라이크 프로젝트입니다.

 모든 그래픽 요소는 에셋 사용없이 전부 AI로 출력하였으며(컷씬, UI, 스프라이트 등),
 소스코드 또한 원하는 바를 AI에게 1차로 요구 후에 자잘한 부분을 수정하는 방식으로 진행하였습니다.
  <!--|프로젝트 주소|https://github.com/Team-HnS/Project_HnS|-->
  
|프로젝트 명|묵향귀무록|
|------|---|
|소개 영상|https://youtube.com/shorts/LF0lWc0P1nY|
|팀원|서정우 개인 작품|
|주요 구현 내용|조이스틱 ui 구현, 플레이어와 가구 상호작용, 손님 ai|
<!--|개발 기간|2023.10 (약 3주)|-->

 - 1.BattleManager | 전투로직을 관리하는 싱글턴 코드입니다. 
 - 2.ShopManager | 인게임 상점에서 사용되는 코드입니다. 
 - 3.DialogueManager | 이벤트 등에 사용되는 대사창을 구현한 코드입니다.
 - 4.MapManager | 게임 시작 후, 맵을 관리하는 싱글턴입니다. 맵생성, UI와 연결, 자료구조에 저장 등을 담당하고 있습니다.
 - 5.GridPathGenerator | 맵을 생성하는 코드로직입니다.

 <details>
  <summary> 작업과정&주요코드 자세히 보기 </summary> 
<img width="474" height="256" alt="귀무록2" src="https://github.com/user-attachments/assets/e7a2b97a-7025-4b44-aee5-8232015dc909" />
 AI와 함께 코드의 피드백을 주고받는 부분
   
<img width="474" height="256" alt="귀무록1" src="https://github.com/user-attachments/assets/610ef2fc-516a-4bb9-88cd-00fd1f2e1cea" />
 게임에서 사용될 유물 아이콘을 생성한 모습

<img alt="image" src="https://github.com/user-attachments/assets/fdcba316-c987-4f54-b693-85640cfe9317" />
 코루틴을 이용한 자동 턴제 전투 구현

<img alt="image" src="https://github.com/user-attachments/assets/a3960079-1c9f-4698-99a0-e0e9986ecc42" />
 상점에서 랜덤 유물 COUNT개를 뽑을때 사용하는 코드
<img alt="image" src="https://github.com/user-attachments/assets/621cd9fa-26e7-47f5-a77d-8e3bf521bcd7" />

  이벤트 데이터가 가지고있는 내용을 UI화면에 있는 버튼에 할당시켜주는 코드

  </details>
 
 --------------------------------------------------------
  # 4. 마이 스위트 베이커리(Unity_C#)
<img src="https://github.com/user-attachments/assets/6abeeb58-0523-4509-9973-f28ee8c78ebc">
<img src="https://github.com/user-attachments/assets/5d40a3ee-9dc9-4f0b-94ea-0c74ca1896d2">

|프로젝트 명|마이 스위트 베이커리|
|------|---|
|소개 영상|https://youtube.com/shorts/eFJoVU6jcW4?feature=share|
|팀원|서정우 개인 작품|
|주요 구현 내용|조이스틱 ui 구현, 플레이어와 가구 상호작용, 손님 ai|
<!--|개발 기간|2023.08 (약 3주)|-->


-  **해당 게임은 (주)슈퍼센트의 과제**를 기반으로 제작된 게임이며, 
  과제 리소스의 저작권은 원 저작자 또는 회사에 있습니다.
  리소스는 포트폴리오용으로만 사용되며, 외부 배포 및 상업적 사용은 없습니다.


 
 - 1.Tutorial_Manager | 첫 시작후에 플레이어에게 가이드 화살표를 띄우는 튜토리얼 싱글턴 매니저입니다
 - 2.Player | 기본적인 플레이어 상호작용을 구현한 코드입니다
 - 3.Customer | 손님의 이동, 대기 로직 등을 구현한 코드입니다
 - 4.COb_zone | 매대와 상호작용하는 코드입니다
 - 5.Sell_Ob | 판매물품에 들어가는 코드입니다
 - 6.SOb_zone | 오븐 등 생산가구와 상호작용하는 코드입니다
 - 7.JoyStick | 캔버스를 터치해서 이동하는 조이스틱을 구현한 코드입니다
 - 8.Counter | 손님과 계산할수있는 카운터 코드입니다
 - 9.UnLock_Zone | 돈을 사용해 해금할수있는 해금 존 코드입니다
 --------------------------------------------------------
 
 # 5.  XRVR_MA(Unity_C#)
 <img src="https://github.com/arena5045/Portfolio_Code/assets/64789660/0850fd56-8725-4ddd-9794-0b9464b57694" width="474" height="256"/>
 
괴리성 밀리언 아서 vr을 모티브로 만들어본 XR 기반 게임입니다
<!--|프로젝트 주소|https://github.com/Team-HnS/Project_HnS|-->
|프로젝트 명|XRVR_MA|
|------|---|
|소개 영상|-|
|팀원|서정우 개인 작품|
|주요 구현 내용|xr 기반 멀티플랫폼 Inputsystem사용, Vr 컨트롤러를 이용한 ui상호작용|
<!--|개발 기간|2023.11 (약 18일)|-->



 - 1.BattleManager | 전투, 배틀 턴 등을 실행하는 코드입니다
 - 2.Card | 카드 데이터의 저장과 처리를 담당하는 코드입니다
 - 3.CardBtn| 카드UI, 클릭 시 처리를 담당하는 코드입니다
 - 4.Monster | 몬스터 데이터와, 데미지 등 처리를 담당하는 코드입니다

--------------------------------------------------------
 # 6. The Zombie(Unity_C#)
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
 # 7. Tank in White(Unity_C#)
 
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
 # 8. TCGGAME(Unity_C#)

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
 # 9.Photon Tank(Unity_C#)
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
 # 10. Prena(app_Java)

자바를 이용하여 만든 프라모델 정보 어플리케이션입니다

 - 1.PickList |  찜 화면을 구현한 코드입니다
 - 2.Gundam_pick_Info |  찜 화면에서 클릭하면 나오는 정보창을 구현한 코드입니다
 - 3.GundamList | 한정판 목록을 구현한 코드입니다
 - 4.Webfragment | 웹 화면을 구현한 코드입니다

--------------------------------------------------------
 # 11. Prena(web_JS)

자바스크립트와 HTML을 이용하여 만든 프라모델 정보 사이트입니다
 - 1.index.html | 홈페이지 메인 프레임을 구현한 html코드입니다
 - 2.qna.js | 질의응답에 사용된 자바스크립트 코드입니다 
--------------------------------------------------------

