using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatItem : MonoBehaviour
{
    // 매개변수 함수 담을 변수
    public Action onAutoScroll;
    private void Awake()
    {
        //Text 컴포넌트 가져오자
        chattext = GetComponent<TMP_Text>();
        
    }

    //Text
    public TMP_Text chattext;
    void Start()
    {
    }

    void Update()
    {
        
    }

    public void SetText(string s)
    {
        // 텍스트 갱신
        chattext.text = s;

        // 사이즈 조절 코루틴 실행
        StartCoroutine(UpdateSize());
    }

    IEnumerator UpdateSize()
    {
        yield return null;
        // 텍스트의 내용에 맞춰서 크기를 조절
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, chattext.preferredHeight);

        yield return null;
        // 만약에 onAutoScroll 에 함수가 들어있다면
        if (onAutoScroll != null)
        {
            onAutoScroll();
        }
        //// Chatview 게임 오브젝트 찾자
        //GameObject go = GameObject.Find("ChatView");
        //// 찾은 오브젝트에서 ChatManager 컴포넌트 가져오자
        //ChatManager cm = go.GetComponent<ChatManager>();
        //// 가져온 컴포넌트 AutoScrollBottom 함수 호출
        //cm.AutoScrollBottom();
    }
}
