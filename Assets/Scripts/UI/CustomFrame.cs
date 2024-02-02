using System.Collections.Generic;
using UnityEngine;

public class CustomFrame : MonoBehaviour
{
    [SerializeField] private GameObject customFrames;               // 커스텀 프레임을 갖고 있는 오브젝트
    [SerializeField] private float scrollSpeed = 500;               // 마우스 스크롤 스피드

    private List<GameObject> frames = new List<GameObject>();       // 프레임을 저장할 배열
    private List<Vector3> frameDefaultPos = new List<Vector3>();    // 프레임의 초기 위치를 저장할 배열
    
    private float sumMouseWheel = 0;            // 현재 마우스 휠 정도
    private float beforeSumMouseWheel = 0;      // 이전 마우스 휠 정도
    private float frameWidth = 0;               // 한 프레임의 너비

    void Start()
    {
        Debug.Assert(customFrames != null, "커스텀으로 할 수 있는 프레임이 없습니다.");

        // 모든 프레임의 정보와 위치를 저장함
        for (int i = 0; i < customFrames.transform.childCount; i++)
        {
            GameObject frame = customFrames.transform.GetChild(i).gameObject;

            frames.Add(frame);
            frameDefaultPos.Add(frame.transform.position);
        }

        // 프레임의 너비를 입력 받음
        frameWidth = frames[frames.Count - 1].GetComponent<RectTransform>().rect.width;
    }

    void Update()
    {
        // 마우스 휠 정도를 입력
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        sumMouseWheel += mouseWheel * scrollSpeed;
        
        // 프레임들이 계속 왼쪽으로 가는 것을 방지
        if (sumMouseWheel > 0)
            sumMouseWheel = 0;

        float lastFrameRightPos = frameDefaultPos[frameDefaultPos.Count - 1].x + sumMouseWheel + frameWidth / 2;

        // 프레임들이 계속 오른쪽으로 가는 것을 방지
        if ((mouseWheel * scrollSpeed < 0) && (lastFrameRightPos < 1137))
            sumMouseWheel = 1137 - frameDefaultPos[frameDefaultPos.Count - 1].x - frameWidth / 2;

        // 휠을 움직이지 않은 경우
        if (beforeSumMouseWheel == sumMouseWheel)
            return;

        // 휠이 움직임에 따라 프레임도 그 위치만큼 이동한것으로 지정
        for (int i = 0; i < frameDefaultPos.Count; i++)
            frames[i].transform.position = new Vector3(frameDefaultPos[i].x + sumMouseWheel, frameDefaultPos[i].y, frameDefaultPos[i].z);

        // 이동 처리
        beforeSumMouseWheel = sumMouseWheel;
    }
}
