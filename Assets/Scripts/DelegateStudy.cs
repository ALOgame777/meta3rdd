using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Delegate : 함수를 담는 변수를 만드는 자료형

public  delegate void CallBack();
public  delegate void CallBack2(string s);

public class DelegateStudy : MonoBehaviour
{
    CallBack callBack; // 함수를 담을수있는 반환자료형이 void, 매개변수가 없는 함수
    Action action;
    CallBack2 callBack2;
    Action<string> action2; 
    // Start is called before the first frame update
    void Start()
    {
        callBack = () =>
        {
            print(gameObject.name);
        };
        action = PrintName;
        callBack();
        PrintName();
        callBack2 = (string s) =>
        {
        print(s + ": " + gameObject.name);
        };
        callBack2 += PrintName3;
        PrintName2("직접 호출");
        callBack2("딜리게이트로 호출");
        action2 = PrintName2;
        action2("dddd");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrintName()
    {
        print(gameObject.name);
    }
    public void PrintName2(string s)
    {
        print(s + ": " +gameObject.name);
    }
    public void PrintName3(string s)
    {
        print(s);
    }
}
