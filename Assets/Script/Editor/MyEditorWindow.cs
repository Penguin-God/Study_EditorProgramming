using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // UnityEditor 관련 API 사용하기 위한 지시문

// Editor 폴더는 Unity 지정폴더로 이 폴더의 스크립트는 에디터에 기능을 추가하는 스크립트로 간주되면 런타임 떄 빌드에 사용할 수 없음
public class MyEditorWindow : EditorWindow // 상속
{
    // Edit, Window가 있는 메뉴 창에 함수를 실행시킬 경로 추가
    [MenuItem("MyTool/OpenMyWindow %g")]
    static void OpenWindow()
    {
        MyEditorWindow myWindow = GetWindow<MyEditorWindow>();
        myWindow.title = "Hello Window";
    }


    // 몇몇 class들의 차이
    // GUI.Button(new Rect(0,0,250,300), "Hi");
    // GUILayout : GUI에서는 직접 지정해줘야 하는 부분들을 자동으로 지정해줌
    // GUILayout.Button("Haai");

    // EditorGUI.LabelField(new Rect(0, -90, 200, 200), "aaNUM");
    // EditorGUILayout : EditorGUI에서는 직접 지정해줘야 하는 부분들을 자동으로 지정해줌
    // EditorGUILayout.LabelField("NUM");

    // GUI  VS  EditorGUI

    // GUI, GUILayout : 게임 내에서도 가능
    // 게임 내에서 깔짝깔짝대는 UI제작 가능. 잠깐 뜨고 없어지는 Text, Button 등등

    // EditorGUI, EditorGUILayout : Only Editor

    // Unity Event 함수
    private void OnGUI()
    {
        // GUI 함수들은 Update같이 딱 봐도 안될것 같은 곳에서 쓰면 역시 에러뜸
        //GUILayout.Label("Hello Label");
        //GUILayout.TextField("Hello TextField");
        //GUILayout.Button("Hello Button");

        DrawLabel();
    }

    void DrawLabel()
    {
        for(int i = 0; i < 5; i++)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField($"title : {i}");

            float saveWidth_1 = EditorGUIUtility.labelWidth;
            float saveWidth_2 = EditorGUIUtility.fieldWidth;
            EditorGUIUtility.labelWidth = 50;
            EditorGUIUtility.fieldWidth = 50;

            EditorGUILayout.BeginHorizontal();
            {
                for (int j = 0; j < 3; j++)
                {
                    EditorGUILayout.TextField(j.ToString(), "Hello TextFiled");
                    // 마지막이 아니면 TextField끼리 띄어쓰기 하기
                    if (j < 2) EditorGUILayout.Space(5);
                }
            } EditorGUILayout.EndHorizontal();

            // EditorGUIUtility.labelWidth 은 전역 값으로 다른 곳에서도 사용함 즉 바꿔썻다가 그대로 놔두면 다음 GUI에도 그대로 적용됨
            EditorGUIUtility.labelWidth = saveWidth_1;
            EditorGUIUtility.fieldWidth = saveWidth_2;
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10);

            if (i < 4) EditorGUILayout.LabelField("", GUI.skin.horizontalScrollbar);
        }
    }

    void TestBeginEnd()
    {
        EditorGUILayout.BeginHorizontal(); // 이제부터 가로로 그리겠다.
        EditorGUILayout.LabelField("aaaa");
        EditorGUILayout.LabelField("aaaa");
        EditorGUILayout.LabelField("aaaa");
        EditorGUILayout.EndHorizontal(); // 컴백

        EditorGUILayout.BeginVertical(); // 이제부터 세로로 GUI를 그리겠다.
        EditorGUILayout.LabelField("aaaa");
        EditorGUILayout.LabelField("aaaa");
        EditorGUILayout.LabelField("aaaa");
        EditorGUILayout.EndVertical(); // 다시 원래대로 돌아오겠다.

        // End를 하지 않고 Begin 해도 가장 최근에 호출한 Begin 형식으로 그리지만 이는 없어진게 아니라 우선순위가 밀린 느낌으로 결국 남아있음
        EditorGUILayout.LabelField("aaaa");
        EditorGUILayout.LabelField("aaaa");
        EditorGUILayout.LabelField("aaaa");

        // 또한 End를 하면 알아서 줄바꿈을 함
        EditorGUILayout.BeginHorizontal(); // 이제부터 가로로 그리겠다.
        EditorGUILayout.LabelField("aaaa");
        EditorGUILayout.LabelField("aaaa");
        EditorGUILayout.LabelField("aaaa");
        EditorGUILayout.EndHorizontal(); // 컴백

        EditorGUILayout.BeginHorizontal(); // 이제부터 가로로 그리겠다.
        EditorGUILayout.LabelField("aaaa");
        EditorGUILayout.LabelField("aaaa");
        EditorGUILayout.LabelField("aaaa");
        EditorGUILayout.EndHorizontal(); // 컴백
    }
}
