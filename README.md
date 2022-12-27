# Combined Demo Project

> 통합 데모를 위한 메타버스를 유니티 프로젝트로 구현하였습니다. 
> 프로젝트의 모든 AI 모델이 메타버스에서 활용되는 예시를 볼 수 있습니다. 

<img width="850" alt="image" src="https://user-images.githubusercontent.com/121273065/209572423-c305e7c6-f5c0-44c1-b339-9ccef642b49a.png">

## 실행 화면

//TODO AWS HTTP를 입력하는 법 

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
│   ├── Demo
│   │   ├── Materials
│   │   ├── Models
│   │   │   ├── Junwon.onnx
│   │   │   └── getmean_tri.onnx
│   │   ├── Prefabs
│   │   │   ├── Dest.prefab
│   │   │   ├── Slider.prefab
│   │   │   ├── TrailPoint.prefab
│   │   │   ├── dot_agent.prefab
│   │   │   ├── dot_answer.prefab
│   │   │   ├── dot_userlog.prefab
│   │   │   └── user.prefab
│   │   ├── Scripts
│   │   │   ├── ClickDetector.cs
│   │   │   ├── Flag.cs
│   │   │   ├── GameManager.cs
│   │   │   ├── JunwonAgent.cs
│   │   │   ├── ModelOverrider.cs
│   │   │   ├── RecommendAgent.cs
│   │   │   ├── TrailEnergyDecrease.cs
│   │   │   ├── TrailGenerator.cs
│   │   │   └── TrailPoint.cs
│   │   └── TrainScene
│   │       └── NavMesh.asset
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

- [Assets/Demo](https://github.com/skku-woongjin/RecommendAgent_Demo/tree/main/Assets/Demo) : Demo Scene 을 구성하는 Asset 모음
- [Assets/Demo/Materials](https://github.com/skku-woongjin/RecommendAgent_Demo/tree/main/Assets/Demo/Materials) : 3D 오브젝트에 씌울 Material 모음
- [Assets/Demo/Models](https://github.com/skku-woongjin/RecommendAgent_Demo/tree/main/Assets/Demo/Models) : 훈련된 모델 모음
    - Junwon.onnx: 비교 모델, #visit, duration, distance 가 모두 높은 장소 추천
    - getmean_tri.onnx: [RecommendAgent_Train]()을 통해 훈련된 모델
- [Assets/Demo/Scripts](https://github.com/skku-woongjin/RecommendAgent_Demo/tree/main/Assets/Demo/Scripts) : C# 스크립트 모음

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
    

