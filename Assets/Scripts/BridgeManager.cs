using UnityEngine;

public class BridgeManager : MonoBehaviour
{
    public enum BridgeState
    {
        None, Bridge01, Bridge02
    }
    public BridgeState bridgeState = BridgeState.None; //다리 상태

    [Header("다리 1")]
    public Transform bridge01; //1번 다리
    public Transform bridge01Tale; //1번 다리 꼬리

    [Header("다리 2")]
    public Transform bridge02; //2번 다리
    public Transform bridge02Tale; //2번 다리 꼬리

    [Header("도착선")]
    public Transform finishLine; //도착지점

    public float speed; //속도

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (bridgeState) //다리 상태
        {
            case BridgeState.Bridge01:
                {
                    //1번 다리를 도착지점까지 speed의 속도로 일정하게 이동한다.
                    bridge01.position = Vector3.MoveTowards(bridge01.position, 
                        finishLine.position, speed * Time.deltaTime);
                    //1번 다리 위치가 도착지점에 도달하면
                    if(bridge01.position == finishLine.position)
                    {
                        //2번 다리의 부모를 다리매니저로 지정
                        bridge02.SetParent(transform);
                        //1번다리의 부모를 2번 다리의 꼬리로 지정
                        bridge01.SetParent(bridge02Tale);
                        //1번 다리의 위치를 2번 다리의 꼬리로 이동시킨다.
                        bridge01.transform.position = bridge02Tale.position;
                        //2번 다리 상태로 변경
                        bridgeState = BridgeState.Bridge02;
                    }
                    break;
                }
            case BridgeState.Bridge02:
                {
                    //2번 다리를 도착지점까지 speed의 속도로 일정하게 이동한다.
                    bridge02.position = Vector3.MoveTowards(bridge02.position,
                        finishLine.position, speed * Time.deltaTime);

                    //2번 다리 위치가 도착지점에 도달하면
                    if (bridge02.position == finishLine.position)
                    {
                        //1번 다리의 부모를 다리 매니저로 지정
                        bridge01.SetParent(transform);
                        //2번 다리의 부모를 1번 다리의 꼬리로 지정
                        bridge02.SetParent(bridge01Tale);
                        //2번 다리의 위치를 1번 다리의 꼬리로 이동시킨다.
                        bridge02.transform.position = bridge01Tale.position;
                        //1번 다리 상태로 변경
                        bridgeState = BridgeState.Bridge01;
                    }
                    break;
                }
        }
    }
}
