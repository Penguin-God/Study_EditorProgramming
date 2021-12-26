using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGUI : MonoBehaviour
{
    // 실제 게임에서 UI가 나옴
    // 인게임에서만 UI가 그려지고 씬 화면에서는 확인이 불가능하며 관련 오브젝트도 생성되지 않음
    private void OnGUI()
    {
        GUI.Label(new Rect(100, 100, 200, 300), "This is GUI");
        GUILayout.Label("This is GUILayout");
    }
}
