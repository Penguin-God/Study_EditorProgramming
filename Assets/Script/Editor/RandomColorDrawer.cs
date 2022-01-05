using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PlayerData))]
[CustomPropertyDrawer(typeof(PlayerData2))]
public class RandomColorDawer : PropertyDrawer
{
    // PropertyDrawer의 OnGUI에는 GUILayout, EditorGUILayout과 같은 자동 레이아웃 클래스 사용 불가
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // positon에는 GUI전체 영역에 대한 값이 들어가 있음
        GUI.Box(position, GUIContent.none, GUI.skin.window);

        // 인자값으로 날아온 label을 그림
        EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        
        // GUI 재정의 시작
        EditorGUI.indentLevel++;
        Rect rt = new Rect(position.x, position.y + GUIStyle.none.CalcSize(label).y + 2, position.width, 16);

        foreach(SerializedProperty prop in property)
        {
            GUI.color = new Color(Random.value, Random.value, Random.value);
            EditorGUI.PropertyField(rt, prop);
            rt.y += 18;
        }
        GUI.color = Color.white;
        EditorGUI.indentLevel--;
    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int count = 0;
        foreach (var prop in property) count++;
        float margin = count * 2;
        count++;

        return EditorGUIUtility.singleLineHeight * count + margin;
    }
}