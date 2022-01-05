using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomComponent))] // 재정의를 원하는 타입 넣기
public class MyCustomEditor : Editor
{
    // SerializedProperty : 직렬화된 프로퍼티
    SerializedProperty GameObjectProperty = null;
    SerializedProperty nameProperty = null;
    SerializedProperty hpProperty = null;

    // 런타임 중에 사용되는 OnEnable()과 다르게 하이어라키 창에서 컴포넌트를 가진 오브젝트를 클릭하면 실행됨
    void OnEnable()
    {
        // 직렬화된 프로퍼티들을 이름으로 찾아서 가져옴
        // nameof() : 변수, 형식 또는 멤버의 이름을 문자열 상수로 가져온다.
        GameObjectProperty = serializedObject.FindProperty($"{nameof(CustomComponent.myGameObject)}");
        nameProperty = serializedObject.FindProperty($"{nameof(CustomComponent.myName)}");
        hpProperty = serializedObject.FindProperty($"{nameof(CustomComponent.hp)}");
    }

    // Unity가 인스펙터에 에디터를 표시할 때마다 실행됨
    public override void OnInspectorGUI()
    {
        // 다른 곳에서 값이 변경되었을 수도 있으므로 값을 업데이트하고 시작
        serializedObject.Update();

        // hpProperty.intValue : 실제 값이 들어있음
        if (hpProperty.intValue < 500) GUI.color = Color.red;
        else GUI.color = Color.green;
        // 슬라이더 적용
        hpProperty.intValue = EditorGUILayout.IntSlider("current hp", hpProperty.intValue, 0, 1000);


        EditorGUILayout.BeginHorizontal();
        {
            GUI.color = Color.blue;
            EditorGUILayout.PrefixLabel("name");
            GUI.color = Color.white;
            nameProperty.stringValue = EditorGUILayout.TextArea(nameProperty.stringValue);
        } EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(GameObjectProperty);

        // 바뀐 값 적용
        serializedObject.ApplyModifiedProperties();
    }
}
