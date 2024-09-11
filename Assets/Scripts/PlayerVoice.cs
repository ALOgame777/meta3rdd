using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.PUN;
public class PlayerVoice : MonoBehaviour
{
    // Photon Voice View
    public PhotonVoiceView pvv;
    // image Speaker
    public GameObject imgSpeaker;
    // Image Speech Bubble
    public GameObject imgSpeechBubble;

    
    void Start()
    {

    }
    
    void Update()
    {
        // 만약에 내가 말하고 있다면
        // imgSpeechBubble 활성화
        // 그렇지 않으면 비활성화
        imgSpeechBubble.SetActive(pvv.IsRecording);

        // 만약에 말하는 소리가 들린다면
        //imgSpeaker 활성화
        // 그렇지 않으면 비활성화
        imgSpeechBubble.SetActive(pvv.IsSpeaking);
    }

}
