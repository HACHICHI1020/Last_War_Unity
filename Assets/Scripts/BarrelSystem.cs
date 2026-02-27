using UnityEngine;
using System.Collections.Generic;

public class BarrelSystem : MonoBehaviour
{
    public float speed; //이동 속도
    public Transform barrel; //오크통
    public List<Transform> barrelPos = new List<Transform>(); //오크통 위치

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
        //정면으로 이동
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

    //오크통 초기화
    public void BarrelInitialize(int num, Transform t)
    {
        int randPos = Random.Range(0, barrelPos.Count);
        barrel.position = barrelPos[randPos].position;

        barrel.GetComponent<WeaponBarrel>().BarrelNumberOn(num);
        deadLine = t;
    }
}
