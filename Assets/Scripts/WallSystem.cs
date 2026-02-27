using UnityEngine;
using System.Collections.Generic;

public class WallSystem : MonoBehaviour
{
    public List<Wall> walls = new List<Wall>(); //벽 리스트
    public float speed;
    private int randWallCount;

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
        //벽 이동
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (deadLine)
        {
            dist = Vector3.Distance(new Vector3(0, 0, transform.position.z), new Vector3(0, 0, deadLine.position.z));
            if (dist <= 2.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    //벽 리스트 초기화
    public void WallInitialize(int wallCount, int min1, int max1, int min2, int max2, Transform t)
    {
        if(wallCount == 1) //벽을 하나만 활성화 시킨다면
        {
            //0부터 벽리스트의 갯수(2)사이에서 무작위 수 저장 -> 0 or 1
            int randOpenWall = Random.Range(0, walls.Count);

            switch (randOpenWall)
            {
                case 0:
                    {
                        randWallCount = Random.Range(min1, max1); //최솟값과 최댓값 사이에 수를 무작위로 저장
                        walls[0].GetComponent<Wall>().InitNumber(randWallCount);
                        break;
                    }
                case 1:
                    {
                        randWallCount = Random.Range(min2, max2); //최솟값과 최댓값 사이에 수를 무작위로 저장
                        walls[1].GetComponent<Wall>().InitNumber(randWallCount);
                        break;
                    }
            }
            
            //전체 벽들 중에서
            for (int i = 0; i < walls.Count; i++)
            {
                if(i == randOpenWall) //i가 활성화 시킬 벽의 번호와 같다면
                {
                    //해당 벽은 켜준다.
                    walls[i].gameObject.SetActive(true);
                    walls[i].GetComponent<Wall>().InitNumber(randWallCount);
                }
                else
                {
                    //수가 같지 않은 벽은 비활성화한다.
                    walls[i].gameObject.SetActive(false);
                }
            }
        }
        else //벽 두개 활성화
        {
            int randCnt1 = Random.Range(min1, max1);
            int randCnt2 = Random.Range(min2, max2);
            walls[0].GetComponent<Wall>().InitNumber(randCnt1);
            walls[1].GetComponent<Wall>().InitNumber(randCnt2);
        }
        deadLine = t;
    }
}
