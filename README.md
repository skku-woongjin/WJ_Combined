# Combined Demo Project

> 통합 데모를 위한 메타버스를 유니티 프로젝트로 구현하였습니다. 
> 프로젝트의 모든 AI 모델이 메타버스에서 활용되는 예시를 볼 수 있습니다. 

<img width="850" alt="image" src="https://user-images.githubusercontent.com/121273065/209572423-c305e7c6-f5c0-44c1-b339-9ccef642b49a.png">


<br />

## Prerequisite

`Unity` (version: 2021.3.6f1)
<br />

## Getting Started


### Clone Repository

```shell script
$ git clone https://github.com/skku-woongjin/WJ_Combined.git
$ cd RecommendAgent_Demo
```

### How to Run

Unity Hub에서 프로젝트 경로 추가 후 실행

## 사용법 


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

- [Assets/Scene_Main](https://github.com/skku-woongjin/WJ_Combined/tree/main/Assets/Scene_Main) : Main Scene 을 구성하는 Asset 모음
- [Assets/Scene_Main/Recommend_Agent](https://github.com/skku-woongjin/WJ_Combined/tree/main/Assets/Scene_Main/Recommend_Agent) : Main Scene의 추천 모델을 구성하는 Asset모음
- [Assets/Scene_VisualGame](https://github.com/skku-woongjin/WJ_Combined/tree/main/Assets/Scene_VisualGame) : Visual Game Scene 을 구성하는 Asset 모음

## Components

- **[ClickDetector.cs](https://github.com/skku-woongjin/RecommendAgent_Demo/blob/main/Assets/Demo/Scripts/ClickDetector.cs)**
  - 장소 클릭 시 유저가 해당 장소로 이동하도록 함
  <br />
- **[Flag.cs](https://github.com/skku-woongjin/RecommendAgent_Demo/blob/main/Assets/Demo/Scripts/Flag.cs)**
  - 장소 class 정의
  <br />
- **[GameManager.cs](https://github.com/skku-woongjin/RecommendAgent_Demo/blob/main/Assets/Demo/Scripts/GameManager.cs)** 
  - 장소 특성 저장, 장소 방문 처리
  - UI 업데이트
  - 장소 배치, 초기화 등
  <br />
- **[JunwonAgent.cs](https://github.com/skku-woongjin/RecommendAgent_Demo/blob/main/Assets/Demo/Scripts/JunwonAgent.cs)** 
  - Junwon.onnx의 input과 output 처리
  <br />
- **[RecommendAgent.cs](https://github.com/skku-woongjin/RecommendAgent_Demo/blob/main/Assets/Demo/Scripts/RecommendAgent.cs)** 
  - getmean_tri.onnx의 input과 output 처리 
  <br />
- **[TrailEnergyDecrease.cs](https://github.com/skku-woongjin/RecommendAgent_Demo/blob/main/Assets/Demo/Scripts/TrailEnergyDecrease.cs),[TrailGenerator.cs](https://github.com/skku-woongjin/RecommendAgent_Demo/blob/main/Assets/Demo/Scripts/TrailGenerator.cs),[TrailPoint.cs](https://github.com/skku-woongjin/RecommendAgent_Demo/blob/main/Assets/Demo/Scripts/TrailPoint.cs)** 
  - 유저 이동 시 나타나는 발자취 처리 
  <br />
    

