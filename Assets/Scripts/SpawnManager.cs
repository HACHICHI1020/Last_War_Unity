using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    public enum SpawnState
    {
        None, Spawn
    }
    public SpawnState spawnState = SpawnState.None; //소환 상태
    public List<GameObject> spawnList = new List<GameObject>(); //소환 리스트
    private int randSpawn; //랜덤 스폰
    private float spawnTime; //소환 시간
    public float spawnTimeOffset; //설정 소환 시간
    public Transform bulletGruoup; //총알 그룹
    public List<Transform> spawnPos = new List<Transform>();
    public Transform deadLine;

    [Header("소환 레벨")]
    public int spawnLevel; //소환 레벨
    private int rndEnemyCount; //적의 소환 수
    private float enemyHP = 1.0f; //적의 HP량
    private int wallNumMin1, wallNumMax1, wallNumMin2, wallNumMax2; //벽 최솟값, 최댓값
    private int barrelNum; //오크통 넘버
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnActor();
    }

    //소환 시작 함수
    public void SpawnStart()
    {
        GameObject spawn = Instantiate(spawnList[1],transform.position, transform.rotation, transform);

        wallNumMin1 = Random.Range(spawnLevel * -50, spawnLevel * -40);//왼쪽 벽 최솟값
        wallNumMax1 = Random.Range(spawnLevel * -40, spawnLevel * -30);//왼쪽 벽 최댓값
        wallNumMin2 = Random.Range(spawnLevel * 1, spawnLevel * 2);//오른쪽 벽 최솟값
        wallNumMax2 = Random.Range(spawnLevel * 2, spawnLevel * 3);//오른쪽 벽 최댓값

        int rndWallCount = Random.Range(1, 3);

        //벽 초기화 함수 호출(벽 갯수, 벽1최솟값, 벽1최댓값, 벽2최솟값, 벽2최댓값)
        spawn.GetComponent<WallSystem>().WallInitialize(rndWallCount,
            wallNumMin1, wallNumMax1, wallNumMin2, wallNumMax2, deadLine);

        spawnState = SpawnState.Spawn; //소환 상태로 변경
    }

    //소환 함수
    void SpawnActor()
    {
        switch (spawnState)
        {
            case SpawnState.Spawn:
                {
                    spawnTime += Time.deltaTime; //소환 시간 재생
                    //소환 시간이 설정 소환시간을 넘어설 때
                    if (spawnTime >= spawnTimeOffset)
                    {
                        CreateSpawnList(); //소환물 생성 함수
                        spawnTime = 0;
                    }
                    break;
                }
        }
    }
    void CreateSpawnList()
    {
        //0부터 소환리스트의 갯수까지 무작위 수를 저장
        randSpawn = Random.Range(0, spawnList.Count);
        //randSpawn = 2;

        //소환물 생성
        GameObject spawn = Instantiate(spawnList[randSpawn], 
            transform.position, transform.rotation, transform);

        switch(randSpawn)
        {
            case 0: //적
                {
                    int randPos = Random.Range(0, spawnPos.Count); //0~2 무작위 수

                    //해당 위치로 적 분대를 이동시킨다.
                    spawn.transform.position = spawnPos[randPos].position;

                    //생성할 적 분대의 구성원 수를 무작위로 저장
                    rndEnemyCount = Random.Range((spawnLevel * 1), (spawnLevel * 2));
                    if(rndEnemyCount > 50)
                    {
                        rndEnemyCount = Random.Range(45, 51); //너무 많아지면 45~50마리 수 조정
                    }

                    enemyHP = spawnLevel * 10; //레벨에 따라 체력이 늘어난다.

                    //소환물이 EnemySquad 컴포넌트를 가지고 있다면
                    if(spawn.GetComponent<EnemySquad>())
                    {
                        //소환물의 적 생성 함수를 호출 (소환 갯수, 객체별 체력)
                        spawn.GetComponent<EnemySquad>().CreateEnemySquad(rndEnemyCount, enemyHP, deadLine, spawnLevel);
                    }
                    break;
                }
            case 1: //벽
                {
                    int rndWallCount = Random.Range(1, 3); //벽을 1개 or 2개 생성 여부

                    //소환 대상이 WallSystem 컴포넌트를 가지고 있다면
                    if(spawn.GetComponent<WallSystem>())
                    {
                        int wallType = Random.Range(0, 3); //0-> 왼쪽 음수, 오른쪽 양수, 1->왼쪽 양수, 오른쪽 음수
                        
                        switch(wallType)
                        {
                            case 0: //왼쪽 벽 음수, 오른쪽 벽 양수
                                {
                                    wallNumMin1 = Random.Range(spawnLevel * -50, spawnLevel * -40);//왼쪽 벽 최솟값
                                    wallNumMax1 = Random.Range(spawnLevel * -40, spawnLevel * -30);//왼쪽 벽 최댓값
                                    wallNumMin2 = Random.Range(spawnLevel * 1, spawnLevel * 2);//오른쪽 벽 최솟값
                                    wallNumMax2 = Random.Range(spawnLevel * 2, spawnLevel * 3);//오른쪽 벽 최댓값
                                   
                                    //벽 초기화 함수 호출(벽 갯수, 벽1최솟값, 벽1최댓값, 벽2최솟값, 벽2최댓값)
                                    spawn.GetComponent<WallSystem>().WallInitialize(rndWallCount, 
                                        wallNumMin1, wallNumMax1, wallNumMin2, wallNumMax2, deadLine);
                                    break;
                                }
                            case 1: //왼쪽 벽 양수, 오른쪽 벽 음수
                                {
                                    wallNumMin1 = Random.Range(spawnLevel * 1, spawnLevel * 2);//왼쪽 벽 최솟값
                                    wallNumMax1 = Random.Range(spawnLevel * 2, spawnLevel * 3);//왼쪽 벽 최댓값
                                    wallNumMin2 = Random.Range(spawnLevel * -50, spawnLevel * -40);//오른쪽 벽 최솟값
                                    wallNumMax2 = Random.Range(spawnLevel * -40, spawnLevel * -30);//오른쪽 벽 최댓값
                                    //벽 초기화 함수 호출(벽 갯수, 벽1최솟값, 벽1최댓값, 벽2최솟값, 벽2최댓값)
                                    spawn.GetComponent<WallSystem>().WallInitialize(rndWallCount, 
                                        wallNumMin1, wallNumMax1, wallNumMin2, wallNumMax2, deadLine);
                                    break;
                                }
                            case 2:
                                {
                                    wallNumMin1 = Random.Range(spawnLevel * -70, spawnLevel * -60);//왼쪽 벽 최솟값
                                    wallNumMax1 = Random.Range(spawnLevel * -50, spawnLevel * -50);//왼쪽 벽 최댓값
                                    wallNumMin2 = Random.Range(spawnLevel * -50, spawnLevel * -40);//오른쪽 벽 최솟값
                                    wallNumMax2 = Random.Range(spawnLevel * -40, spawnLevel * -30);//오른쪽 벽 최댓값

                                    //벽 초기화 함수 호출(벽 갯수, 벽1최솟값, 벽1최댓값, 벽2최솟값, 벽2최댓값)
                                    spawn.GetComponent<WallSystem>().WallInitialize(rndWallCount,
                                        wallNumMin1, wallNumMax1, wallNumMin2, wallNumMax2, deadLine);

                                    break;
                                }
                        }
                    }

                    break;
                }
            case 2: //오크통
                {
                    barrelNum = Random.Range(spawnLevel * 20, spawnLevel * 50); //오크통의 숫자 무작위 저장

                    //오크통 초기화
                    spawn.GetComponent<BarrelSystem>().BarrelInitialize(barrelNum, deadLine);
                    break;
                }
        }
    }

    public void SpawnLevelUp()
    {
        spawnLevel++; //소환 레벨이 1씩 오른다.
        if(spawnTimeOffset > 1.0f)
        {
            spawnTimeOffset -= 0.1f;
        }
        else
        {
            spawnTimeOffset = 1.0f;
        }
            GameManager.instance.SpawnLevelTextOn(spawnLevel);

    }
}
