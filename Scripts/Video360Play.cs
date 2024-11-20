using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video360Play : MonoBehaviour
{
    
    //비디오 플레이어 컴포넌트
    VideoPlayer vp;
    //재생해야 할 VR 360 영상을 위한 설정
    public VideoClip[] vcList;//다수의 비디오 클립을 배열로 만들어 관리한다.
    int curVCidx; // 현재 재생 중인 클립 번호를 저장한다.
    // Start is called before the first frame update
    void Start()
    {
        
        //비디오 플레이어 컴포넌트의 정보를 받아온다.
        vp = GetComponent<VideoPlayer>();
        vp.clip = vcList[0];
        curVCidx = 0;
        vp.Stop();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //컴퓨터에서 영상을 변경하기 위한 기능
        if (Input.GetKeyDown(KeyCode.LeftBracket)) //왼쪽 대괄호 입력시 이전 영상
        {
            SwapVideoClip(false);
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket)) //오른쪽 대괄호 입력시 이전 영상
        {
            SwapVideoClip(true);
        }

    }
    // 인터랙션을 위해 함수를 퍼블릭으로 선언한다.
    // 배열의 인덱스 번호를 기준으로 영상을 교체, 재생하기 위한 함수
    // 인자 값인 isNext가 true이면 다음 영상, false이면 이전 영상 재생
    public void SwapVideoClip(bool isNext)
    {
        //현재 재생 중인 영상의 넘버를 기준으로 체크한다.
        //이전 영상 번호는 현재 영상보다 배열에서 인덱스 번호가 1이 작다.
        //다음 영상 번호는 현재 영상 보다 배열에서 인덱스 번호가 1이 크다.
        int setVCnum = curVCidx; //현재 재생 중인 영상의 넘버를 입력한다.
        vp.Stop();               //현재 재생 중인 비디오클립을 중지한다.
                                 // 재생될 영상을 고르기 위한 과정
        if (isNext)
        {
            //리스트 전체 길이보다 크면 클립을 리스트의 첫 번째 영상으로 지정한다.
            setVCnum = (setVCnum + 1) % vcList.Length;
        }
        else 
        {
            //배열의 이전 영상을 재생한다.
            setVCnum=((setVCnum-1)+vcList.Length) % vcList.Length;
        }
        vp.clip = vcList[setVCnum];//클립을 변경한다.
        vp.Play();//바뀐 클립을 재생한다.
        curVCidx = setVCnum;
    }
    public void SetVideoPlay(int num)
    {
        //현재 재생 중인 번호가 전달받은 번호와 다를 때만 실행
        if (curVCidx != num)
        {
            vp.Stop();
            vp.clip = vcList[num];
            curVCidx = num;
            vp.Play();
        }


    }
}
