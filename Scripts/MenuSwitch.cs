using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//바라보는 방향이 아래를 향하면 메뉴를 활성화한다.
public class MenuSwitch : MonoBehaviour
{
    public GameObject videoFrameMenu;
    //public float minAngle = 65; //메뉴를 보여주기 위한 최소 각도
    //public float maxAngle = 90; //메뉴를 보여주기 위한 최대 각도
    public float dot;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dot = Vector3.Dot(transform.forward, Vector3.up);
        if (dot <-0.5)
        {
            videoFrameMenu.SetActive(true);//메뉴 활성화
        }
        else
        {
            videoFrameMenu.SetActive(false);//메뉴 비활성화
        }
    }
}
