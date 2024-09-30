# 문명(Civilization)

## 프로젝트 개요
메타버스 아카데미에서 제작한 문명 모작 게임
약간 기획을 변경하여 한,중,일 삼국 중 하나를 선택하여 나머지를 정벌하는 전략 시뮬레이션 게임으로 제작
## 작업 내용
- HexMap 제작 및 관리 기능 구현
- A* 알고리즘을 활용한 길찾기 기능 구현
- 전장의 안개 구현
- 유닛 기능(전투,경작,건설,이동 등) 구현
- 각 타일 산출 및 관리 시스템 구현

## 핵심 스크립트
1. ClickManager_HJH
- 유닛 혹은 건물을 선택하면 그에 따른 UI가 표시 & 해당 객체의 상태 변화
2. CurrentUnit_HJH
- A* 알고리즘을 이용하여 선택 유닛 이동 관리
3. HexCell 관련 스크립트
- [레퍼런스](https://catlikecoding.com/unity/tutorials/hex-map/)
- 레퍼런스 블로그를 참고하여 HexCell 시스템 제작 및 전장의 안개 구현
4. ArcherAttack_HJH,CreateBuilding_HJH 등 유닛 행동 스크립트
- 유닛 혹은 건물의 행동을 해당 오브젝트의 컴포넌트에서 정의를 한 것이 아닌, 특정 UI에 종속시켜 놓고 해당 유닛이 선택되었을 때 해당하는 UI가 나오도록 설정
- 해당 UI의 버튼을 눌렀을 때 기능이 작동하도록 설정
5. AIManager
- FSM을 활용해서 Ai 구현
- 다만 Ai 밸런스 조정에 실패해서 게임 후반이 되면 Ai의 물량이 너무 많이 나오게 되는 문제가 있었음.
- 마감 기한이 부족했기에 Ai의 자원수급에 디메리트를 주는 방식으로 일단 임시방편 해결

## 회고
- 깃허브를 처음으로 사용해본 프로젝트이다.
- 깃에 익숙하지 않아서 컨플릭트를 내면서 여러번 날려먹기도 하고 우여곡절이 있었다.
- 알고리즘에 대해서 잘 몰랐었는데, 유닛 이동 시스템을 적용하면서 A* 알고리즘을 공부하게 되었고,알고리즘의 중요성을 느끼고 백준등의 알고리즘 공부를 시작하게 되는 계기가 되었다.
- FSM을 활용해서 Ai를 구현(유닛 생성,유닛 이동,건물건설등의 상태들을 이동)하였고, 약간 부족한 부분들이 있었음. 지금 다시 작업한다면 행동트리 등으로 더 발전시킬 수 있을 것 같다. 
- 처음으로 싱클톤 디자인페턴을 활용해서 메니저들 구축해보았다.