# SpartaExploration

## 🕹️ 기본 조작

| 동작       | 키/입력          | 설명                            |
|------------|------------------|---------------------------------|
| 이동       | `W`, `A`, `S`, `D` | 기본 이동 (Input System 기반)    |
| 달리기     | `Run` (토글)      | 토글 방식의 달리기, 스태미나 소모 |
| 점프       | `Space`           | 점프 (ForceMode.Impulse 사용)    |
| 방향 전환  | 마우스            | 마우스 이동으로 방향 전환        |
| 인벤토리   | `I`               | 인벤토리 창 열기                 |
| 상호작용   | `E`               | 아이템 등 상호작용               |
| 카메라 전환| `Tab`             | 1인칭 ↔ 3인칭 시점 전환          |

---

## ⚙️ 주요 시스템 구현

- **Input System + Rigidbody ForceMode**
  - 이동, 점프, 달리기, 사다리 등 물리 기반 이동 구현
  - 스태미나 시스템과 연동하여 달리기 및 점프 시 스태미나 소모 구현

- **Raycast 기반 상호작용**
  - 아이템이나 환경과 상호작용 시 UI 표시
  - 레이저 트랩 등과 충돌 시 데미지 처리

- **점프대**
  - `OnCollisionEnter` + `ForceMode.Impulse`로 강한 점프 구현

- **아이템 시스템 (ScriptableObject)**
  - 아이템 정보 정의 및 효과 적용 관리
  - Coroutine 기반으로 효과 지속 시간 제어
  - 예: 10초 무적, 10초 더블 점프 등

- **장비 장착**
  - 장비 착용 시 캐릭터 스탯 상승 효과 적용

- **카메라 시스템**
  - `1인칭 ↔ 3인칭` 전환 가능
  - 카메라 `transform.position`, `raycast` 거리 조절 등으로 구현

- **움직이는 발판**
  - `Vector3.SmoothDamp`를 활용해 부드러운 움직임 구현

- **사다리 및 매달리기**
  - Raycast로 `Ladder` 레이어 탐지 시 3D 이동에서 2D 이동으로 전환

- **레이저 트랩**
  - Raycast로 플레이어 탐지 시 데미지 적용
  - 피격 효과 및 무적 시간 처리
---

## 💡 기술 키워드

- Unity Input System
- Rigidbody / ForceMode
- Coroutine
- ScriptableObject
- Raycast
- SmoothDamp
- 1인칭/3인칭 카메라 시점 전환

---
