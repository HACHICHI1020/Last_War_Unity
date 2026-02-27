using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquad : MonoBehaviour
{
    public GameObject enemyPrefab; //적 프리팹
    public List<GameObject> enemies = new List<GameObject>(); //적 리스트
    public float spawnRange; //소환 범위
    private int enemyCount = 1; //적 수

    [Header("이동")]
    public float speed; //이동 속도
    private Transform deadLine;
    [SerializeField]
    private float dist; //거리

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        EnemySquadMove(); //이동 함수 호출

        if(deadLine)
        {
            dist = Vector3.Distance(new Vector3(0,0,transform.position.z), new Vector3(0,0, deadLine.position.z));
            if(dist <= 2.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    //이동 함수
    void EnemySquadMove()
    {
        //전방을 향해 speed의 속도로 이동한다.
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    //적 분대 생성
    public void CreateEnemySquad(int j, float hp, Transform t, int spawnLevel)
    {
        enemyCount = j; 
        for(int i = 0; i < enemyCount; i++)
        {
            CreateEnemy(hp,spawnLevel);
        }
       deadLine = t; //한계선 등록
    }

    //적 생성 함수
    void CreateEnemy(float hp, int spawnLevel)
    {
        //적 생성(원본, 위치, 회전, 부모)
        GameObject enemy = Instantiate(enemyPrefab,
            transform.position, transform.rotation, transform);

        if(enemy.GetComponent<Enemy>())
        {
            enemy.GetComponent<Enemy>().EnemyHPSetUp(hp);
            enemy.GetComponent<Enemy>().EnemyScoreSetUp((float)spawnLevel*100.0f);
        }

        //현재 적 수가 1보다 많다면
        if (enemyCount > 1)
        {
            //적 분대의 위치에서 0.5범위만큼의 구체 범위내에서 무작위로 Vector3 추출
            Vector3 randPos = Random.insideUnitSphere * spawnRange + transform.position;

            //적의 위치를 randPos의 X,Z 좌표로 이동시킨다.
            enemy.transform.position =
                new Vector3(randPos.x, transform.position.y, randPos.z);
        }
        StartCoroutine(EnemyPosFix(enemy));//적 고정 함수 호출
        enemies.Add(enemy); //생산된 적을 적 리스트에 추가
    }

    //적 위치 고정 함수
    IEnumerator EnemyPosFix(GameObject g)
    {
        yield return new WaitForSeconds(0.1f);
        g.GetComponent<Rigidbody>().isKinematic = true;//강체 고정
        g.GetComponent<CapsuleCollider>().isTrigger = true; //트리거 설정
    }
}
