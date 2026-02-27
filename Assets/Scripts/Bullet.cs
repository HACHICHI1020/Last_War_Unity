using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed; //속도
    private float damage; //대미지
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //전방으로 speed의 속도로 이동한다.
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }

    //대미지 초기화
    public void InitDamage(float dmg)
    {
        damage = dmg; //설정한 대미지량을 저장
    }

    //트리거 충돌 시작 함수
    private void OnTriggerEnter(Collider other)
    {
        //충돌 대상의 태그 이름이 Enemy라면
        if(other.transform.CompareTag("Enemy"))
        {
            //충돌 대상의 HP감소 함수를 호출한다.
            other.transform.GetComponent<Enemy>().EnemyHPDown(damage);

            Destroy(gameObject); //총알 삭제
        }

        //충돌 대상의 태그가 Wall이라면
        if(other.transform.CompareTag("Wall"))
        {
            //벽 숫자 증가 함수 호출
            other.transform.GetComponent<Wall>().AddWallNumber();
            Destroy(gameObject); //총알 삭제
        }

        //충돌 대상의 태그가 Barrel이라면
        if(other.transform.CompareTag("Barrel"))
        {
            //오크통 숫자 감소 호출
            other.transform.GetComponent<WeaponBarrel>().ReduceBarrelCount();
            Destroy(gameObject); //총알 삭제
        }
    }
}
