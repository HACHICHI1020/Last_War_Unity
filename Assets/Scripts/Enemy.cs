using UnityEngine;
using UnityEngine.UI;//UGUI 사용

public class Enemy : MonoBehaviour
{
    public enum EnemyLiveState
    {
        Live, Dead
    }
    [Header("생존상태")]
    public EnemyLiveState enemyLiveState = EnemyLiveState.Live;
    public Animator enemyAnim; //애니메이터

    [Header("이동")]
    public float speed; //이동 속도

    public enum EnemyHPState
    {
        None, HPDown
    }

    [Header("체력")]
    public EnemyHPState enemyHPState = EnemyHPState.None; //적 HP 상태
    public float enemyHP; //적 HP
    private float maxHP; //최대 HP
    private float targetHP; //변동 목표 HP
    private float hpSpeed = 200.0f; //HP 변동 속도
    public Image hpGauge; //HP게이지

    [Header("점수")]
    public float enemyScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHP = enemyHP; //최대 HP 초기화
        targetHP = enemyHP; //목표 HP 초기화
    }

    // Update is called once per frame
    void Update()
    {
        DeathMove();
        EnemyHP(); //HP 함수 호출
    }

    void DeathMove()
    {
        switch (enemyLiveState)
        {
            case EnemyLiveState.Dead:
                {
                    //죽었을 때 이동
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    break;
                }
        }
    }

    //적 HP 설정
    public void EnemyHPSetUp(float hp)
    {
        enemyHP = hp;
    }

    //적 점수 설정
    public void EnemyScoreSetUp(float score)
    {
        enemyScore = score;
    }

    //HP 함수
    void EnemyHP()
    {
        switch (enemyHPState)
        {
            case EnemyHPState.HPDown: //HP 감소 상태
                {
                    //현재HP를 목표HP까지 hpSpeed의 속도로 일정하게 변동시킨다.
                    enemyHP = Mathf.MoveTowards(enemyHP, targetHP, hpSpeed * Time.deltaTime);

                    //HP게이지를 현재HP/전체HP의 값만큼 변동시킨다.
                    hpGauge.fillAmount = enemyHP / maxHP;

                    //현재HP와 목표HP량이 같아진다면
                    if (enemyHP == targetHP)
                    {
                        //현재 HP가 0이라면 ->사망
                        if(enemyHP == 0)
                        {
                            GameManager.instance.ScoreUp(enemyScore);
                            transform.SetParent(null); //부모 해제
                            float deathSpeed = -(speed / 2);
                            speed = deathSpeed; //캐릭터가 반대로 이동하게 만든다.
                            enemyAnim.SetInteger("EnemyState", 1); //사망 애니메이션 실행
                            enemyLiveState = EnemyLiveState.Dead; //사망상태로 변경
                            Destroy(gameObject, 3.0f); //3초 후에 삭제
                        }
                        enemyHPState = EnemyHPState.None; //대기상태로 변경
                    }
                    break;
                }
        }
    }
    //HP 감소 함수
    public void EnemyHPDown(float damage)
    {
        //현재 HP량이 damage보다 많다면
        if(enemyHP > damage)
        {
            targetHP -= damage; //목표 HP에 damage만큼 감소시킨다.
        }
        //현재 HP량이 damage보다 적거나 같다면 = 사망
        else
        {
            targetHP = 0; //목표 HP량을 0으로 저장
            GetComponent<CapsuleCollider>().enabled = false; //콜라이더 중지
        }
        enemyHPState = EnemyHPState.HPDown; //HP감소 상태로 변경
    }

    //트리거 충돌 시작 함수
    private void OnTriggerEnter(Collider other)
    {
        //충돌 대상의 태그가 Soldier라면
        if(other.transform.CompareTag("Soldier"))
        {
            //HP 감소 함수 호출
            other.transform.GetComponent<Soldier>().SoldierHPDown();
            //플레이어의 병사 리스트에서 적에 치인 병사를 제거
            other.transform.GetComponent<Soldier>().BarrelRemoveSoldier();

            GetComponent<CapsuleCollider>().enabled = false; //콜라이더 중지
            enemyHP = 0;
            targetHP = 0;
            hpGauge.fillAmount = 0; //HP 0으로 변경
            enemyHPState = EnemyHPState.None; //HP 대기 상태로 변경
            float deathSpeed = -(speed / 2);
            speed = deathSpeed; //캐릭터가 반대로 이동하게 만든다.
            enemyAnim.SetInteger("EnemyState", 1); //사망 애니메이션 실행
            enemyLiveState = EnemyLiveState.Dead; //사망상태로 변경
        }
    }
}
