using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // UI Image 기능을 사용하기 위한 네임스페이스
using UnityEngine.Video; //VideoPlayer를 제어하기 위한 네임스페이스

//카메라의 시선을 처리하기 위한 기능
public class GazePointerCtrl : MonoBehaviour
{
    public Video360Play vp360; //360스피어에 추가된 영상 플레이 기능


    public Transform uiCanvas;//일정 시간 동안 시선이 머무는 것을 보여주기 위한 UI
    public Image gazeImg;     //시선이 머무는 동안 변화를 표현하기 위한 UI image 컴포넌트

    Vector3 defaultScale;     //UI 기본 스케일을 저장해두기 위한 값
    public float uiScaleVal = 1f;// UI 카메라가 1m일 때 배율

    bool isHitObj; //인터렉션이 일어나는 오브젝트에 시선이 닿으면 true, 닿지 않으면 false
    GameObject prevHitObj;// 이전 프레임의 시선이 머물렀던 오브젝트 정보를 담기 위한 변수
    GameObject curHitObj; // 현재 프레임의 시선이 머물렀던 오브젝트 정보를 담기 위한 변수
    float curGazeTime = 0; //시선이 머무르는 시간을 저장하기 위한 변수
    public float gazeChargeTime = 3f; // 시선이 머문 시간을 체크하기 위한 기준 시간 3초(필요에 따라 수정)
    
    //히트된 오브젝트 타입별로 작동 방식을 구분한다.
    void HitObjChecker(GameObject hitObj,bool isActive)
    {
        //hit이 비디오 플레이 컴포넌트를 갖고 있는지 확인한다.
        if (hitObj.GetComponent<VideoPlayer>())
        {
            if(isActive)
            {
                hitObj.GetComponent<VideoFrame>().CheckVideoFrame(true);

            }
            else
            {
                hitObj.GetComponent<VideoFrame>().CheckVideoFrame(false);

            }
            
        }
        if (gazeImg.fillAmount >= 1)
        {
            vp360.SetVideoPlay(hitObj.transform.GetSiblingIndex());
        }
        if(curGazeTime/gazeChargeTime>=1)
        {
            //비디오 플레이어가 없는 Mesh_Collider 오브젝트의 이름에 따라 이전/ 다음 영상으로 재생
            if (hitObj.name.Contains("Right"))
            {
                vp360.SwapVideoClip(true);//다음 영상
            }
            else if (hitObj.name.Contains("Left"))
            {
                vp360.SwapVideoClip(false);//이전 영상
            }
            else
            {
                //360 스피어에 특정 클립 번호를 전달해 플레이한다.
                vp360.SetVideoPlay(hitObj.transform.GetSiblingIndex());
            }
            curGazeTime = 0; //누적 시간을 초기화 해 코드가 반복해서 불리는 것을 방지
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultScale = uiCanvas.localScale;//오브젝트가 갖는 기본 스케일 값
        curGazeTime = 0; //시선을 유지하는지 체크하기 위한 변수 초기화
    }

    // Update is called once per frame
    /*
    1. 카메라를 기준으로 전방 방향의 좌표를 구한다.
    2. 카메라 위치와 전방방향 좌표로 레이를 생성한다.
    3. 레이에 부딪힌 경우에는 거리 값을 이용하여 uiCanvas 크기를 조절한다.
    4. 아무것도 부딪히지 않으면 기본 스케일 값으로 uiCanvas 크기를 조절한다.
     */
    void Update()
    {
        //캔버스 오브젝트의 스케일을 거리에 따라 조절한다.
        //1. 카메라를 기준으로 전방 방향의 좌표를 구한다
        Vector3 dir = transform.TransformPoint(Vector3.forward);
        //2. 카라를 기준으로 전방의 레이를 설정한다.
        Ray ray = new Ray(transform.position, dir);
        RaycastHit hitInfo; //히트된 오브젝트의 정보를 담는다.
        //3. 레이에 부딪힌 경우네는 거리 값을 이용해 uiCanvas의 크기를 조절한다.
        if(Physics.Raycast(ray,out hitInfo))
        {
            uiCanvas.localScale = defaultScale * uiScaleVal * hitInfo.distance;
            uiCanvas.position = transform.forward * hitInfo.distance;
            if(hitInfo.transform.tag == "GazeObj")
            {
                isHitObj = true;
            }
            curHitObj = hitInfo.transform.gameObject;
        }
        else
        {//4. 아무것도 부딪히지 않으면 기본 스케일 값으로 uiCanvas의 크기를 조절한다.
            uiCanvas.localScale = defaultScale * uiScaleVal;
            uiCanvas.position = transform.position + dir;
        }
        //5. uiCanvas가 항상 카메라 오브젝트를 바라보게 한다.
        uiCanvas.forward = transform.forward * -1;

        //GazeObj에 레이가 닿았을 때 실행
        if (isHitObj)
        {
            if (curHitObj == prevHitObj)
            {
                curGazeTime += Time.deltaTime;
            }
            else
            {
                prevHitObj = curHitObj;
            }
            HitObjChecker(curHitObj, true);
        }
        else
        {
            if (prevHitObj != null)
            {
                HitObjChecker(prevHitObj, false);
                prevHitObj = null;
            }
            curGazeTime = 0;
        }
        //시선이 머문 시간을 0과 최댓값 사이로 한다.
        curGazeTime = Mathf.Clamp(curGazeTime, 0, gazeChargeTime);
        //ui image의 fillamount를 업데이트한다.
        gazeImg.fillAmount = curGazeTime / gazeChargeTime;

        isHitObj = false;
        curHitObj = null;
        
    }
    //정해진 시간이 되면 360 스피어에 특정 클립 번호를 전달해 플레이한다.
    
    
}
