using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // UnityEditor 관련 API 사용하기 위한 지시문
using System;

// Editor 폴더는 Unity 지정폴더로 이 폴더의 스크립트는 에디터에 기능을 추가하는 스크립트로 간주되면 런타임 떄 빌드에 사용할 수 없음
public class MyEditorWindow : EditorWindow // EditorWindow 상속
{
    // Edit, Window가 있는 메뉴 창에 함수를 실행시킬 경로 추가
    [MenuItem("MyTool/OpenMyWindow %g")]
    static void OpenWindow()
    {
        MyEditorWindow myWindow = GetWindow<MyEditorWindow>();
        myWindow.title = "Hello Window";
    }


    // Unity Event 함수
    private void OnGUI()
    {
        //정리();
        //TestBeginEnd();
        //DrawTextField();
        //DrawWindow();
        //DrawScrollBar();
        //DrawLabelButton();
        //TestGUIContent();
        //TestGUIStyle();
        //TestApplyStyle();
        //OnGUIEvent();
        //OnGUI_ByOtherEvent();
        //StopEvent();
    }

    // GUI 관련 내용 정리
    void 정리()
    {
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


        // GUI 함수들은 Update같이 딱 봐도 안될것 같은 곳에서 쓰면 역시 에러뜸
        //GUILayout.Label("Hello Label");
        //GUILayout.TextField("Hello TextField");
        //GUILayout.Button("Hello Button");

        // Editor라는 이름을 가진 Folder 안에 스크립트들은 EditorScript로 간주되며 object에 컴포넌트로 붙일 수 없음
        // 또한 런타임 시점에서는 사용 불가능함


        // 스타일 관련 클래스 사용 시 주의사항
        // EidtorStyles 같은 클래스의 경우 유니티 내부에서도 사용하는 가능들이 있음
        // 따라서 EidtorStyles의 속성값을 바꾸면 유니티 인스펙터 창에도 그대로 적용됨

        // 모든 인스펙터 창의 fontSize를 키워버리는 미친 코드
        // GUIStyle gui = EditorStyles.label;
        // gui.fontSize = 35;
        // GUILayout.Label("ABC", gui);

        // Unity에 영향을 주지않는 올바른 코드
        // GUIStyle gui = new GUIStyle(EditorStyles.label);
        // gui.fontSize = 35;
        // GUILayout.Label("ABC", gui);
    }

    // Begin, End 테스트
    void TestBeginEnd()
    {
        DrawHorizontalGUI(3, () => EditorGUILayout.LabelField("aaaa"));
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

    // TextField 깔쌈하게 그려보기, 전역값을 바꿨다가 원래대로 돌리는 이유
    void DrawTextField()
    {
        for (int i = 0; i < 5; i++)
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
            }
            EditorGUILayout.EndHorizontal();

            // EditorGUIUtility.labelWidth 은 전역 값으로 다른 곳에서도 사용함 즉 바꿔썻다가 그대로 놔두면 다음 GUI에도 그대로 적용됨
            EditorGUIUtility.labelWidth = saveWidth_1;
            EditorGUIUtility.fieldWidth = saveWidth_2;
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10);

            if (i < 4) EditorGUILayout.LabelField("", GUI.skin.horizontalScrollbar);
        }
    }

    // Window 형식으로 Area그려보기
    void DrawWindow()
    {
        // 이제부터 영역 안에 GUI를 그리겠다.
        // position.width는 전체 창의 크기, 2번째 인자는 타입. 즉, window 형식으로 영역을 그리겠다.
        GUILayout.BeginArea(new Rect(20, 40, position.width - 40, 200), GUI.skin.window);
        {
            // 영역 안에 그릴 경우 영역의 왼쪽 위 모서리를 (0, 0)으로 잡고 GUI를 그림
            DrawHorizontalGUI(4, () => EditorGUILayout.LabelField("Hello Window"));
        }
        GUILayout.EndArea();
    }


    // 현재 스크롤 위치
    Vector2 VerScrollPos = Vector2.zero;
    Vector2 HorScrollPos = Vector2.zero;
    // Area를 2개로 나눠서 각각 다른 ScrollView 그려보기
    void DrawScrollBar()
    {
        // 부모 Window rect
        Rect rect = new Rect(20, 40, position.width - 40, 200);
        GUILayout.BeginArea(rect, GUI.skin.window);
        {
            // 왼쪽 Window rect
            Rect leftRect = new Rect(0, 0, rect.width * 0.5f, rect.height);
            GUILayout.BeginArea(leftRect, "left",GUI.skin.window);
            {
                // VerScrollPos에 백터 값이 반환되고 그 값에 따른 화면이 보이게 됨
                // 또한 Vector는 전역 변수로 둬야 작동함
                // 아마 계속 값이 바뀌어랴 하는데 함수 내부에 변수를 선언하면 실행이 끝나고 없어져서 그런 듯
                VerScrollPos = EditorGUILayout.BeginScrollView(VerScrollPos);
                DrawVirticalGUI(30, () => EditorGUILayout.LabelField("Hellow Scroll"));
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();


            // 오른쪽 Window rect
            Rect rigthRect = new Rect(position.width * 0.5f, 0, rect.width * 0.5f, rect.height);
            GUILayout.BeginArea(rigthRect, "right", GUI.skin.window);
            {
                HorScrollPos = EditorGUILayout.BeginScrollView(HorScrollPos);
                DrawHorizontalGUI(30, () => EditorGUILayout.LabelField("Hellow Scroll"));
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }
        GUILayout.EndArea();
    }

    // using과 scope(변수)를 이용해서 Label을 버튼으로 동작하게 하기
    void DrawLabelButton()
    {
        // using()안에 형식(여기서는 Horizontal)을 정의하면 내부에 있는 코드는 Begin, End를 선언한 것과 동일하게 작동함
        // 또한 내부에서 그린 GUI 전체에 scope로 접근해 여러가지 작업을 할 수 있음
        using (var scope = new EditorGUILayout.HorizontalScope(GUILayout.Height(50)))
        {
            // 버튼 누를시 로그 뛰우기
            // scope.rect는 using 내부에서 그린 GUI인 scope의 rect를 가져옴. 즉 using에서 그린 Label 전체를 버튼으로 사용.
            if (GUI.Button(scope.rect, GUIContent.none)) Debug.Log("OnClick");

            DrawHorizontalGUI(5, () => EditorGUILayout.LabelField("Hello Button"));
        }
    }

    // 귀찮아서 만든 함수
    void DrawHorizontalGUI(int count, Action action)
    {
        EditorGUILayout.BeginHorizontal(); // 이제부터 가로로 그리겠다.
        for (int i = 0; i < count; i++) action();
        EditorGUILayout.EndHorizontal(); // 이제 가로로 그리는걸 끝내겠다.
    }
    void DrawVirticalGUI(int count, Action action)
    {
        EditorGUILayout.BeginVertical(); // 이제부터 세로로 그리겠다.
        for (int i = 0; i < count; i++) action();
        EditorGUILayout.EndVertical(); // 이제 세로로 그리는걸 끝내겠다.
    }


    // GUIContent 살펴보기
    void TestGUIContent()
    {
        // GUIContent 는 말 그대로 GUI에 어떤 내용, 무엇을 넣을지 종류를 결정(텍스트, 버튼 등등)
        GUIContent content = new GUIContent();
        content.text = "textContent";
        content.image = EditorGUIUtility.FindTexture("BuildSettings.Editor");
        EditorGUILayout.LabelField(content);

        content.tooltip = "tooltip Content";
        GUILayout.Button(content);
    }

    // GUIStyle 살펴보기
    void TestGUIStyle()
    {
        // GUIStyle은 GUI를 어떻게 그릴 것인가, 말 그대로 Style을 정의함
        GUIStyle style = new GUIStyle();

        style.fontSize = 22;
        style.fontStyle = FontStyle.BoldAndItalic;
        style.normal.textColor = Color.green;

        // 2번쨰 인자값으로 넘겨서 text를 어떻게 그릴지 정의함
        // 통째로 값을 넘기는 GUIContent와 다름
        GUILayout.Label("Hello GUIStyle", style);

        // 생성자에 인수를 전달하여 GUIContent를 특정할 수 있음
        GUIStyle buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 33;
        buttonStyle.normal.textColor = Color.red;
        buttonStyle.hover = new GUIStyleState() {textColor = Color.green };

        GUILayout.Button("Hello GUIStyleButton", buttonStyle);
    }


    // 간단하게 스타일 적용하기
    void TestApplyStyle()
    {
        // 여러가지 스타일을 가지고 있는 클래스들
        // EditorStyles
        // GUI.skin
        GUILayout.Label("Hello EditorStyle", EditorStyles.boldLabel);
        GUILayout.Box("Hello GUI.skin", GUI.skin.window);

        // 다른 속성(style)을 넘겨줬을 경우
        // 기능은 버튼인데 생긴건 textArea로 적용됨. 즉, Content는 그래로지만 Style은 달라짐
        if (GUILayout.Button("Hello BoxButton", GUI.skin.textArea)) Debug.Log("Hi요");
    }


    // OnGUI 살펴보기
    void OnGUIEvent()
    {
        // OnGUI는 Event로 마우스 클릭 등 예약되어 있는 특정 상황에서 호츌됨
        // 하지만 모든 이벤트마다 그려대면 비효율적이니까 repaint type으로 호출됐을 때만 Darw작업을 실행함
        // repaint가 다하는건 아니고 그리기 전에 관련 정보를 수집하는 Layout 이벤트가 실행됨
        // Layout 이벤트에서 얻은 정보를 바탕으로 repaint가 실행되므로 두 이벤트의 정보가 다르면 에러 뜸

        Debug.Log(Event.current.type);

        var area = EditorGUILayout.BeginVertical(GUILayout.Width(200));
        GUI.Box(area, GUIContent.none);

        EditorGUILayout.LabelField("Hi");
        EditorGUILayout.LabelField("Hi");

        EditorGUILayout.EndVertical();
    }


    // Repaint를 제외한 이벤트 살펴보기
    void OnGUI_ByOtherEvent()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Q)
            Debug.Log("KeyDown is Q!!");
        else if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            Debug.Log("Left Mouse Button Down!!");
    }


    void StopEvent()
    {
        // 마우스 관련 이벤트라면
        if (Event.current.isMouse)
        {
            Debug.Log(Event.current.type);
            /*
             마우스 관련 코드가 있다고 가정
             */

            // 이제 마우스 관련 이벤트는 끝내겠다.
            Event.current.Use(); 
        }

        // 위에서 Event.current.Use(); 를 통해 마우스 관련 처리를 막았으므로 버튼을 클릭해도 코드가 실행되지 않음
        // Event.current.Use(); 을 실행하면 현재 이벤트 타입이 Used로 바뀜
        if (GUILayout.Button("Try Click!!")) Debug.Log("Success");
    }
}
