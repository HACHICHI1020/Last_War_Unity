using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wall : MonoBehaviour
{
    public int wallNum; //벽의 번호
    public Text wallText; //벽 텍스트
    public List<Color> wallColor = new List<Color>(); //벽 색깔
    public Renderer _renderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       //_renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //벽 숫자 초기화 함수
    public void InitNumber(int num)
    {
        wallNum = num;
        if (wallNum > 0)
        {
            wallText.text = "+" + wallNum.ToString(); //벽 숫자 표시
            WallColor(1); //파란색
        }
        else
        {
            wallText.text = wallNum.ToString(); //벽 숫자 표시
            WallColor(0); //빨간색
        }
    }

    //벽 숫자 증가 함수
    public void AddWallNumber()
    {
        wallNum++; //벽 숫자 증가
        if(wallNum > 0)
        {
            wallText.text = "+" + wallNum.ToString(); //벽 숫자 표시
            WallColor(1); //파란색
        }
        else
        {
            wallText.text = wallNum.ToString(); //벽 숫자 표시
        }
    }

    //벽 숫자 대입 함수
    public void AddWallCount()
    {
        if (wallNum >= 0) //벽 숫자가 0보다 크거나 같을 때
        {
            //벽의 숫자만큼 병사를 충원하는 함수를 호출
            Player.instance.AddSoldier(wallNum);
        }
        else //벽 숫자가 0보다 작을 때
        {
            //벽의 숫자만큼 병사를 감소시키는 함수 호출
            Player.instance.RemoveSoldier(wallNum);
        }
        Destroy(gameObject); //벽 삭제
    }

    //벽 색깔 지정 함수
    public void WallColor(int i)
    {
        _renderer.material.color = wallColor[i]; //벽 재질의 색을 i의 색으로 저장한다.
    }
}
