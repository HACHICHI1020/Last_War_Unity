using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{
    public static Player instance;

    public enum PlayerState
    {
        None, Play, GameOver
    }
    public PlayerState playerState = PlayerState.None; //플레이어 상태

    [Header("이동")]
    public float speed; //이동 속도

    public enum SensorState
    {
        Ready, Detect
    }
    [Header("감지")]
    public SensorState sensorState = SensorState.Ready; //감지 준비 상태
    public float sensorRange; //감지 범위
    public LayerMask detectLayer; //감지 대상 레이어

    [Header("병사")]
    public int soldierCount; //병사 수
    public GameObject soldierPrefab; //병사 프리팹
    public List<GameObject> soldiers = new List<GameObject>(); //병사 리스트
    public float spawnRange; //소환 범위

    //기즈모 그리는 함수
    private void OnDrawGizmos()
    {
        //감지 범위만큼 와이어 스피어를 그린다.(위치, 반경)
        Gizmos.DrawWireSphere(transform.position, sensorRange);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;

        for (int i = 0; i < soldierCount; i++)
        {
            CreateSoldier();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(playerState)
        {
            case PlayerState.Play: //게임 중
                {
                    PlayerMove(); //이동 함수 호출
                    DetectWall(); //벽 감지 함수 호출
                    break;
                }
        }
    }

    //이동 함수
    void PlayerMove()
    {
        //왼쪽 화살표 키를 누르고 있을 때
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        //오른쪽 화살표 키를 누르고 있을 때
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

    //벽 감지 함수
    void DetectWall()
    {
        //스피어 캐스트를 사용한다.(위치, 반경, 방향, 거리, 레이어마스크)
        RaycastHit[] hits = Physics.SphereCastAll
            (transform.position, sensorRange, Vector3.up, 0, detectLayer);
        switch (sensorState) //감지 상태
        {
            case SensorState.Ready: //감지 준비 상태
                {
                    //감지 대상의 갯수가 최소 1개일 때
                    if (hits.Length > 0)
                    {
                        //단수의 hit로 재정의
                        foreach (RaycastHit hit in hits)
                        {
                            //충돌한 대상이 Wall컴포넌트를 가지고 있다면
                            if (hit.transform.GetComponent<Wall>())
                            {
                                //벽의 숫자를 받아오는 함수 실행
                                hit.transform.GetComponent<Wall>().AddWallCount();
                                sensorState = SensorState.Detect; //감지된 상태로 변경
                            }
                        }
                    }
                    break;
                }
            case SensorState.Detect: //감지된 대상이 있는 상태
                {
                    //감지된 대상이 없다면
                    if(hits.Length <= 0)
                    {
                        sensorState = SensorState.Ready; //감지 준비 상태로 변경
                    }
                    break;
                }
        }
    }

    //병사 충원 함수
    public void AddSoldier(int num)
    {
        

        if(soldiers.Count + num < 30) //충원될 숫자와 현재 병사 합이 최대 정원(30)보다 적을 경우
        {
            soldierCount += num; //충원된 병사 추가
            //충원할 숫자만큼 병사 생성
            for (int i = 0; i < num; i++)
            {
                CreateSoldier();
            }
        }
        else //충원될 숫자와 현재 병사 합이 최대 정원(30)보다 많아진다면
        {
            if (soldiers.Count < 30) //현재 병사 수가 30미만일 때
            {
                int tempCount = 30 - soldiers.Count; //최대 정원에서 현재 병사 수만큼 차감
                soldierCount += tempCount;
                                                     
                for (int i = 0; i < tempCount; i++) //필요한 만큼만 병사 추가 생산
                {
                    CreateSoldier();
                }
            }
            else //현재 병사 수가 30일 경우
            {
                //병사 하나의 총알 대미지를 늘리면 좋을 것 같다.
                for(int i = 0; i < soldiers.Count; i++)
                {
                    //병사들의 총알 데미지 상향 함수 호출
                    soldiers[i].GetComponent<Soldier>().BulletDamageUp();
                }
            }
        }

        SpawnManager.instance.SpawnLevelUp(); //소환 레벨 1 올림
        soldierCount = soldiers.Count; //병사 수 배열 동기화

        //생성된 병사들 사격 시작
        for (int i = 0; i < soldiers.Count; i++)
            {
                soldiers[i].GetComponent<Soldier>().ShootStart();
            }
    }

    //벽에 부딪힌 병사 감소 함수
    public void RemoveSoldier(int num)
    {
        //병사 수의 벽의 숫자의 절대값을 비교하여 병사 수가 더 클 때
        if(soldierCount > Mathf.Abs(num))
        {
            soldierCount -= Mathf.Abs(num); //현재 병사 수에서 벽의 숫자만큼 뺀다.

            for (int i = 0; i < soldiers.Count; i++)
            {
                //i가 현재 병사 수보다 크거나 같다면
                if (i >= soldierCount)
                {
                    soldiers[i].GetComponent<Soldier>().SoldierHPDown(); //해당 병사는 사망시킨다.
                    soldiers.RemoveAt(i); //해당 배열 번호 제거
                }
            }
        }
        else //병사 수보다 벽의 절대값이 더 크면
        {
            soldierCount = 0; //병사 수 0
            for (int i = 0; i < soldiers.Count; i++)
            {
                soldiers[i].GetComponent<Soldier>().SoldierHPDown(); //해당 병사는 사망시킨다.
            }
            soldiers.Clear(); //배열을 초기화한다.
        }

        soldierCount = soldiers.Count; //병사 수 배열 동기화

        if (soldiers.Count == 0)
        {
            playerState = PlayerState.GameOver;
            GameManager.instance.GameOver();

        }
    }
    
    //통에 치인 병사를 리스트에서 제거
    public void BarrelRemoveSoldier(GameObject g)
    {
        //전체 병사 중에서
        for (int i = 0; i < soldiers.Count; i++)
        {
            //g와 같은 병사가 있다면
            if (soldiers[i] == g)
            {
                //해당 병사의 배열을 리스트에서 제거한다.
                soldiers.RemoveAt(i);
            }
        }
        soldierCount = soldiers.Count; //병사 수 배열 동기화

        if (soldiers.Count == 0)
        {
            playerState = PlayerState.GameOver;
            GameManager.instance.GameOver();

        }
    }

    //병사 생성 함수
    void CreateSoldier()
    {
        //병사 생성(원본, 위치, 회전, 부모)
        GameObject soldier = Instantiate(soldierPrefab, 
            transform.position, Quaternion.identity, transform);
        //현재 병사 수가 1보다 많다면
        if(soldierCount > 1)
        {
            //플레이어의 위치에서 0.5범위만큼의 구체 범위내에서 무작위로 Vector3 추출
            Vector3 randPos = 
                Random.insideUnitSphere * spawnRange + transform.position;

            //병사의 위치를 randPos의 X,Z 좌표로 이동시킨다.
            soldier.transform.position = 
                new Vector3(randPos.x, transform.position.y, randPos.z);
        }
        StartCoroutine(SoldierPosFix(soldier));//병사 고정 함수 호출
        soldiers.Add(soldier); //생산된 병사를 병사리스트에 추가
    }

    //병사 위치 고정 함수
    IEnumerator SoldierPosFix(GameObject g)
    {
        yield return new WaitForSeconds(0.1f);
        g.GetComponent<Rigidbody>().isKinematic = true;//강체 고정
    }

    //플레이어 시작
    public void PlayerStart()
    {
        playerState = PlayerState.Play; //플레이 상태로 변경

        for (int i = 0; i < soldiers.Count; i++)
        {
            soldiers[i].GetComponent<Soldier>().ShootStart();
        }
    }

    //병사 공격 속도 업
    public void SoldierShootFastOn()
    {
        //병사들의 공격 속도를 올리는 함수 호출
        for (int i = 0; i < soldiers.Count; i++)
        {
            soldiers[i].GetComponent<Soldier>().ShootFastOn();
        }
    }
}
