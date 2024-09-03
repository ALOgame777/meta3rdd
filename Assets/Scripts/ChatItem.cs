using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatItem : MonoBehaviour
{
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
    }
}
