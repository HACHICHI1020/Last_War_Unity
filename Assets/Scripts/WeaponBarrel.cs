using UnityEngine;
using UnityEngine.UI;

public class WeaponBarrel : MonoBehaviour
{
    public Transform barrelAxis; //오크통
    public float rotSpeed; //회전 속도

    [SerializeField]
    private int barrelNum; //숫자
    public Text barrelText; //텍스트

    private bool brokenSign = true; //통이 깨질 수 있는 신호

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //오크통 회전
        barrelAxis.Rotate(rotSpeed * Time.deltaTime, 0, 0);
    }

    //오크통 숫자 지정
    public void BarrelNumberOn(int i)
    {
        barrelNum = i;
        barrelText.text = barrelNum.ToString(); //텍스트 표시
    }

    //오크통 숫자 감소
    public void ReduceBarrelCount()
    {
        if(brokenSign) //신호가 true일 때
        {
            barrelNum--; //숫자 감소
            barrelText.text = barrelNum.ToString(); //텍스트 표시
            //숫자가 0 이하로 떨어지면
            if (barrelNum <= 0)
            {
                barrelText.text = "0"; //텍스트 표시
                //플레이어 병사들 공격 속도 향상
                Player.instance.SoldierShootFastOn();
                Destroy(gameObject); //오크통 삭제
                SpawnManager.instance.SpawnLevelUp(); //소환 레벨 1 올림
                brokenSign = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //충돌 대상의 태그가 Soldier일 경우
        if(other.transform.CompareTag("Soldier"))
        {
            //충돌 대상이 Soldier 컴포넌트가 있다면
            if (other.transform.GetComponent<Soldier>())
            {
                //충돌 대상의 HP감소함수를 호출한다. -> 사망 애미네이션
                other.transform.GetComponent<Soldier>().SoldierHPDown();

                //플레이어의 병사 리스트에서 통에 치인 병사를 제거
                other.transform.GetComponent<Soldier>().BarrelRemoveSoldier();
            }
        }
    }
}
