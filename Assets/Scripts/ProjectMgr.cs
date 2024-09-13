using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectMgr : MonoBehaviour
{
    // 드래그 드랍이 없어야 가능 아니면 코드로 다 찾던가
    // 자기 자신을 가지고 있는 static 변수
    static ProjectMgr instance;

    // instance를 변환하는 static 함수
    public static ProjectMgr Get()
    {
        // 만약에 instance가 null이라면
        if(instance == null)
        {
            // GameObject 하나 만든다. (이름을 ProjectMgr)
            GameObject go = new GameObject(nameof(ProjectMgr));

            // 만들어진 GameObject에 ProjectMgr 컴포넌트 추가
            go.AddComponent<ProjectMgr>();
            

        }
        return instance; 
    }

    // 내가 Room에 몇 번째로 들어왔는지 
    public int orderInRoom;

    private void Awake()
    {
        // 만약에 instance가 null이면 this 
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }



}
