using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // UnityEditor 관련 API 사용하기 위한 지시문

public class MyEditorWindow : EditorWindow // 상속
{
    // Edit, Window가 있는 메뉴 창에 함수를 실행시킬 경로 추가
    [MenuItem("MyTool/OpenMyWindow %g")]
    static void OpenWindow()
    {
        MyEditorWindow myWindow = GetWindow<MyEditorWindow>();
        myWindow.title = "Hello Window";
    }
}
