# Integrated Demo Project

> 통합 데모를 위한 메타버스를 유니티 프로젝트로 구현하였습니다. <br/>
> 프로젝트의 모든 AI 모델이 메타버스에서 활용되는 예시를 볼 수 있습니다. 

<img width="850" alt="image" src="https://user-images.githubusercontent.com/103883786/209795504-488971cc-69a2-411e-aeb0-28a946ebd805.png">

<br />

## Prerequisite

`Unity` (version: 2021.3.6f1)
<br />

## Getting Started


### Clone Repository

```shell script
$ git clone https://github.com/skku-woongjin/WJ_Integrated.git
$ cd WJ_Integrated
```

### How to Run

Unity Hub에서 프로젝트 경로 추가 후 실행

## 사용법 - Main Scene
시작 Scene입니다. 3D 메타버스 공간으로 구성되어 있습니다. 

### 조작법
- 왼쪽 회살표: 왼쪽으로 시점 회전 
- 오른쪽 화살표: 오른쪽으로 시점 회전
- 위 화살표: 앞으로 이동
- 아래 화살표: 뒤로 이동
- "1" 숫자키: 점프 

### chatbot npc
- 로봇 npc에 가까이 가면 채팅을 통해 대화를 나눌 수 있습니다. <br/>
- 위로 목적의 로봇 npc와 질의응답 목적의 로봇 npc가 있습니다. <br/> 
- 활용된 AI repository: [위로봇](https://github.com/skku-woongjin/comfort_chatbot), 
[위키봇](https://github.com/skku-woongjin/WikiChatbot) 
<img width="200" alt="image" src="https://user-images.githubusercontent.com/121273065/209631329-6c0e5ffa-10c6-49c4-9801-6dd8a79b95c7.png">
<br/>

### conversation group 
- 구 모양 텐트 안에서 다른 유저와 대화할 수 있습니다. 
- 텐트 내 대화 내용의 욕설 비율이 높아지면, 텐트 겉면의 무늬가 바뀌어 다른 유저에게 위험을 알립니다. 
- 활용된 AI repository: [욕설 감지 API](https://github.com/skku-woongjin/HateSpeechAPI)
<img width="200" alt="image" src="https://user-images.githubusercontent.com/121273065/209631805-49d6385b-4e09-42fd-8d77-a1004817a438.png">

### quest
- 오른쪽 위의 퀘스트 아이콘을 클릭하면 세 가지 장소가 추천됩니다.
- 유저가 세 장소 중 하나를 고르면, 펫이 퀘스트 문장을 생성해 주고, 해당 장소로 가는 길을 안내합니다. 
<img width="200" alt="image" src="https://user-images.githubusercontent.com/121273065/209631981-ec62603c-cb12-4fc0-b32f-fb6a31d95ea7.png">

활용된 AI repository: [장소 추천 모델](https://github.com/skku-woongjin/RecommendAgent_Demo), [퀘스트 생성](https://github.com/skku-woongjin/QuestGenerator_wj)

### pet npc
- 펫이 유저 근처를 배회합니다. 유저와의 거리가 멀어지면, A* 알고리즘을 활용하여 유저에게 가까이 다가갑니다.
- 펫은 유저에게 퀘스트를 전달합니다.
- 펫은 욕설 그룹이 감지되면 유저에게 경고합니다. 
<img width="200" alt="image" src="https://user-images.githubusercontent.com/121273065/209631448-fd293dba-0ec9-4586-8843-bbfec2f76825.png">

### HTTP URL
- Project Hierachy 상의 MainScene > World 오브젝트의 Request 컴포넌트에서 request를 보낼 주소를 입력합니다. 
- 포트 번호 뒤의 상세 주소도 입력해야 합니다.(예시: http://52.79.197.23:5000/prediction) 
</br>
<img width="632" alt="image" src="https://user-images.githubusercontent.com/121273065/209629972-541e00e0-c569-4fc9-8f10-b2ecdbfffab6.png">

## 사용법 - Visual Game Scene
### 입장
미니맵 왼쪽 위 모서리의 게임기에 가까이 다가가면, 입장할 수 있는 버튼이 나타납니다. 
</br>
<img width="200" alt="image" src="https://user-images.githubusercontent.com/121273065/209631133-819dcb4b-5078-4c33-ba01-69134805813d.png">
### 플레이
난이도를 선택하면 게임 공간으로 입장합니다. 
</br>
<img width="200" alt="image" src="https://user-images.githubusercontent.com/121273065/209640760-63c84bc1-887b-4ec7-997a-22089632e764.png">

<img width="200" alt="image" src="https://user-images.githubusercontent.com/121273065/209628920-97986a9b-858a-4d8f-b4a3-d736c7034b4c.png">

활용된 AI repository: [visual game]()
### HTTP URL
- Project Hierachy 상의  Solo_Game > ChatManager 오브젝트의 ChatManager 컴포넌트에서 request를 보낼 주소를 입력합니다. 
- 포트 번호 뒤의 상세 주소는 생략합니다. (예시: http://3.37.129.107:5000)
</br>
<img width="603" alt="image" src="https://user-images.githubusercontent.com/121273065/209630926-9141cf5e-f39e-407c-b1b7-b1acc17bc03e.png">


## 파일 구조

```
.
├── README.md
├── Assets
│   ├── AssetPackage
│   ├── Logs
│   ├── Resources
│   │   ├── BadConv1.csv
│   │   └── GoodConv1.csv
│   ├── Scene_Main
│   │   ├── Animation
│   │   ├── Materials
│   │   ├── Meshes
│   │   ├── Prefabs
│   │   ├── Recommend_Agent
│   │   │   ├── Models
│   │   │   └── Scripts
│   │   ├── Scenes
│   │   │   ├── MainScene
│   │   │   │   └── NavMesh.asset
│   │   │   ├── MainScene.unity
│   │   │   └── Start_Scene.unity
│   │   ├── Scripts
│   │   └── Sprites
│   ├── Scene_VisualGame
│   │   ├── Assets
│   │   │   ├── Font
│   │   │   ├── Prefabs
│   │   │   ├── Scenes
│   │   │   ├── Script
│   │   │   └── picture
│   │   └── Packages
│   ├── ML-Agents
│   │   └── Timers
│   └── Plugins
│       ├── Borodar
│       │   └── RainbowHierachy
│       └── TextMesh Pro
├── Packages
└── ProjectSettings
```

<br />

- [Assets/Resources](https://github.com/skku-woongjin/WJ_Integrated/tree/main/Assets/Resources): 대화 그룹 유저들의 대화 내용 csv 파일들
- [Assets/Scene_Main](https://github.com/skku-woongjin/WJ_Integrated/tree/main/Assets/Scene_Main) : Main Scene 을 구성하는 Asset 모음
- [Assets/Scene_Main/Recommend_Agent](https://github.com/skku-woongjin/WJ_Integrated/tree/main/Assets/Scene_Main/Recommend_Agent) : Main Scene의 추천 모델을 구성하는 Asset모음
- [Assets/Scene_VisualGame](https://github.com/skku-woongjin/WJ_Integrated/tree/main/Assets/Scene_VisualGame) : Visual Game Scene 을 구성하는 Asset 모음
    

