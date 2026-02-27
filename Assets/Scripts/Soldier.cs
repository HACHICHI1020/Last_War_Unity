using System.Collections;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public enum SoldierState
    {
        Live, Dead
    }
    public SoldierState soldierState = SoldierState.Live; //생존상태
    public float deathSpeed; //사망시 이동 속도

    #region 사격
    public enum ShootState
    {
        None, Shoot, Wait
    }
    [Header("사격")]
    public ShootState shootState = ShootState.None; //사격 상태
    private float shootInterval; //사격 간격
    public float shootIntervalOffset;//사격 간격 설정 시간
    private float shootTime; //사격 시간
    public AnimationClip shootClip; //사격 애니메이션 클립
    public Animator shootAnim; //사격 애니메이터
    private bool shootSign = true; //사격 신호
    public Transform firePos; //사격 위치
    public GameObject bulletPrefab; //총알 프리팹
    public float damage; //대미지
    #endregion


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(soldierState)
        {
            case SoldierState.Live:
                {
                    SoldierShoot();
                    break;
                }
            case SoldierState.Dead:
                {
                    //사망시 캐릭터가 뒤로 이동한다.
                    transform.Translate(Vector3.back * deathSpeed * Time.deltaTime);
                    break;
                }
        }
    }

    //사격 함수
    void SoldierShoot()
    {
        switch (shootState)
        {
            case ShootState.Wait:
                {
                    shootInterval += Time.deltaTime; //대기 시간 시작
                    //대기시간이 설정대기시간을 넘어설 때
                    if(shootInterval >= shootIntervalOffset)
                    {
                        shootSign = true;
                        shootInterval = 0; //대기시간 초기화
                        shootTime = 0; //사격시간 초기화
                        shootAnim.SetBool("Shoot", true); //사격 애니메이션 실행
                        shootState = ShootState.Shoot;//사격상태 전환
                    }
                    break;
                }
            case ShootState.Shoot:
                {
                    shootTime += Time.deltaTime; //사격 시간 재생
                    //사격 시간이 사격 클립 길이의 5%를 넘어서면
                    if(shootTime >= shootClip.length * 0.05f )
                    {
                        if(shootSign)
                        {
                            CreateBullet();
                            shootInterval = 0; //대기시간 초기화
                            shootTime = 0; //사격시간 초기화
                            shootAnim.SetBool("Shoot", false); //사격 애니메이션 종료
                            shootState = ShootState.Wait; //대기상태로 전환
                            shootSign = false;
                        }
                    }
                    break;
                }
        }
    }

    //최초 사격 시작
    public void ShootStart()
    {
        CreateBullet(); //총알 발사
        shootInterval = 0; //대기시간 초기화
        shootTime = 0; //사격시간 초기화
        shootState = ShootState.Wait; //대기 상태로 전환
    }

    //총알 생성 함수
    void CreateBullet()
    {
        //총알 생성(원본, 초기위치, 초기회전, 부모)
        GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation, null);

        //총알 그룹을 부모로 설정한다.
        bullet.transform.SetParent(SpawnManager.instance.bulletGruoup);
        
        //총알이 Bullet 컴포넌트를 가지고 있다면
        if(bullet.GetComponent<Bullet>())
        {
            //총알의 대미지를 저장한다.
            bullet.GetComponent<Bullet>().InitDamage(damage);
        }

        Destroy(bullet, 5.0f); //5초 뒤에 총알 삭제
    }

    //HP감소 함수
    public void SoldierHPDown()
    {
        
        transform.SetParent(null); //부모 해제
        GetComponent<CapsuleCollider>().enabled = false; //충돌체 중지
        shootAnim.SetLayerWeight(1, 0); //(레이어 인덱스, 웨이트 값)
        shootAnim.SetInteger("SoldierState", 1); //사망 애니메이션 실행
        shootState = ShootState.None; //사격 종료
        StartCoroutine(SoldierDead()); //코루틴함수 SoldierDead함수 호출
    }

    //통에 치인 병사 제거
    public void BarrelRemoveSoldier()
    {
        Player.instance.BarrelRemoveSoldier(gameObject); //통에 치인 병사를 플레이어에게 알림
    }

    IEnumerator SoldierDead()
    {
        yield return new WaitForSeconds(1.0f); //1초 뒤에 실행
        soldierState = SoldierState.Dead; //사망 상태로 변경
        Destroy(gameObject, 3.0f); //3초 뒤에 캐릭터 삭제
    }

    //공격 속도 올리기
    public void ShootFastOn()
    {
        shootIntervalOffset = shootIntervalOffset * 0.75f; //25%정도 빨라지기
    }

    //데미지상승 함수
    public void BulletDamageUp()
    {
        damage = damage + 3.0f; //데미지 상승
    }
}
