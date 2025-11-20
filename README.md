# 🧟DeadCamp-3-Day-Survival🧟
<img width="1149" height="643" alt="3일생존" src="https://github.com/user-attachments/assets/002bc750-10d6-497a-b4b6-25d7f87592b5" />

---
## 🎮 게임 소개
|게임 시연|
|:---:|
|(유튜브 링크가 들어 갈 |
- 생존, 디펜스 게임
- 오전에는 좀비를 피하면서 자원을 파밍
- 저녁에는 좀비 웨이브로부터 생존
- 파밍한 자원으로 좀비를 막을 수 있는 건축물을 만들고 지을 수 있음

  <br>

---
## 💁‍♂️ 프로젝트 팀원 및 역할
|**팀원**| <img width="48" height="48" alt="곽호빈" src="https://github.com/user-attachments/assets/e86bfe7f-5499-4101-a2b3-04b167f58d9f" /> <br>(팀장)곽호빈 |<img width="48" height="48" alt="강동현" src="https://github.com/user-attachments/assets/dbd4ebda-1112-4cb2-8148-bad3c4b9410d" /> <br>**강동현**| <img width="48" height="48" alt="김태환" src="https://github.com/user-attachments/assets/71e1782e-141c-49e2-a65e-cddcb339337d" /> <br>김태환|<img width="48" height="48" alt="박상현" src="https://github.com/user-attachments/assets/9784db5f-5e9e-49d7-bbbc-aa9c980dc107" /> <br>박상현 | <img width="48" height="48" alt="장현우" src="https://github.com/user-attachments/assets/3099def3-6b02-4070-80e5-4397c90df955" /> <br>장현우|
|:---:|:---:|:---:|:---:|:---:|:---:|
|**역할**|빌드 시스템 <br> 크래프팅 시스템| 몬스터 <br> 레이드 시스 | NPC,튜토리얼 | 발표, 아이템 <br> 자원 리스폰 시스템 <br> 제작 시스템 |와이어프레임, 배경<br> 캐릭터, 사격 기능|
<br>

---
## 🔧주요기능

<br>

### 자원 채집
- 도끼로 상호작용이 가능한 나무, 돌에 가까이 가 좌클릭을 하면 자원이 나옴
  
|자원채집|
|:---:|
| ![자원채집](https://github.com/user-attachments/assets/e98b1adc-6b48-4118-895e-43f3ac819655) |


<br>

### 크래프팅 및 설치
-  채집한 자원으로 Tab을 눌러 인벤토리를 연 후 원하는 건축물을 누른 후 Build 버튼을 누르면 설치가 가능한 아이템이 인벤토리로 들어온다
-  만든 아이템의 Build버튼 좌클릭을 누르면 해당 바라보고 있는 방향의 땅에 해당 아이템이 설치된다

|크래프팅|설치|
|:---:|:---:|
|![크레프팅](https://github.com/user-attachments/assets/3177040f-cced-4555-94a1-ab3035b843a3)|![설치](https://github.com/user-attachments/assets/46bfc9aa-360c-4aab-950a-033f46b9d36e)|


<br>

### 좀비 웨이브
- 게임 속 시간 21시에 도달하면 문구와 함께 좀비 웨이브가 몰려온다

|좀비 웨이브|
|:---:|
| ![좀비웨이브](https://github.com/user-attachments/assets/7bebaaf5-3f27-42b4-957e-a0535bdf7b96) |

<br>

---

## 🛠️트러블 슈팅

### 메테리얼 핑크 오류(URP 전용 Shader 불러오기 실패)

**🔍문제**
- 외부 에셋을 가져오면 일부 오브젝트가 핑크색으로 표시됨

**🧭원인**
- 해당 메테리얼이 URP 전용 Shader를 사용하지만, 프로젝트는 BRP 환경이라 호환되지 않음

**🛠해결**
- 프로젝트 전체를 URP로 바꾸면 팀 전체 작업에 영향이 크다고 판단
- 문제 메테리얼만 Standard / Rendering Mode: Transparent로 수동 변경
- 색상·투명 조절하여 플리뷰용 메테리얼로 정상 사용 가능하게 함

**🎯결과**
- 핑크 메테리얼 문제가 해결되고 프로젝트 구조 변경 없이 에셋 사용 가능해짐

<br>

---

<br>

### 건축 시스템 충돌 판정 오류

**🔍문제**
- 건축 시스템 테스트 중 설치 가능한 위치인지 판정하는 기능에서 건축 가능/불가능 색깔 표시가 정상적으로 작동하지 않는 문제 발생

**🧭원인**
- 건축 판정을 위해 검사를 해야하는 오브젝트가 **해당 위치가 비어 있는지** 판정하지 못해 색상 표시가 잘못됨

**🛠해결**
- 건축 시스템에서 위치 확인에 사용되는 오브젝트에 **Box Collider Component** 를 추가하여 충돌 체크가 정상적으로 이루어지도록 구조 보완

**🎯결과**
- 건축 가능/불가능 여부 색상이 정상적으로 반영
- 설치 판정 로직이 의도대로 정확하게 작동하게 됨

<br>

---

<br>

### 이펙트가 게임 화면에서 두 번 렌더링되는 문제

**🔍문제**
- Scene 뷰에서는 이펙트가 정상적으로 한 번만 보이지만 Game 화면에서는 같은 이펙트가 두 번 겹쳐서 렌더링되는 현상이 발생

**🧭원인**
- PlayerCamera와 특정 Layer만 렌더링하도록 설계된 EquipCamera 총 두 개의 카메라가 존재
- 하지만 PlayerCamera가 모든 Layer를 렌더링하도록 설정되어 있어 두 카메라 모두 같은 이펙트가 출력됨

**🛠해결**
- PlayerCamera의 Culling Mask에서 equip Layer를 제외, EquipCamera는 equip Layer만 렌더하도록 유지
- 두 카메라의 Layer 분리로 렌더 중복 제거

**🎯결과**
- 이펙트가 한 번만 정상 렌더링
- 카메라 간 역할 분리가 명확해져 UI/Equp/이펙트 렌더링 안정화

<br>

---

<br>

### 자원이 중복 위치에 생성되는 문제

**🔍문제**
- 게임 내 자원이 같은 스폰 위치에 겹쳐서 생성되는 문제가 지속적으로 발생
- 자원이 서로 뭉치거나 겹쳐 스폰되어 플레이 흐름을 방해함

**🧭원인**
- spawnPoints 배열의 인덱스를 랜덤으로 뽑아 사용했지만 사용한 인덱스를 제거하지 않아, 다른 자원도 동일한 위치를 다시 선택함
- 스폰 위치를 "중복 사용 금지"로 관리하는 리스트가 제대로 소모되지 않아 여러 자원이 동일한 스폰포인트에서 반복 생성됨

**🛠해결**
- 스폰 가능한 위치 인덱스를 저장하는 "spwanPool" 리스트를 만들고 스폰할 때마다
  - 1. 랜덤 인덱스 선택
    2. 해당 진짜 인덱스 읽기
    3. spawnPool에서 Remove 하여 재사용 불가능하게 처리
- 스폰된 Resouce 객체에 자신의 spawnIndex를 저장하여 제거 시 해당 위치만 다시 풀에 반환하도록 관리

**🎯결과**
- 자원이 서로 겹쳐 생성되는 문제 해결
- 스폰 위치가 한 번씩만 사용되며 랜덤성이 정확하게 유지
- 자원 생성이 안정적이고 균일한 분포로 개선됨

<br>

<br>

---

## ⚙️ 개발 환경 및 기술 스택
- **엔진** : Unity 2022.3.62f2
- **언어** : C#
- **버전 관리** : Git / GitHub
- **협업 툴** : ZEP / Slack
- **플랫폼** : PC

  <br>
  <br>
