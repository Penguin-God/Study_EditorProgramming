using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Component_Connector : EditorWindow
{
    // Edit, Window가 있는 메뉴 창에 함수를 실행시킬 경로 추가
    [MenuItem("MyTool/Open Component Window")]
    static void OpenWindow()
    {
        Component_Connector myWindow = GetWindow<Component_Connector>();
        myWindow.title = "Hello Component Window";
    }

    Dictionary<SerializedObject, List<SerializedProperty>> targetDic = new Dictionary<SerializedObject, List<SerializedProperty>>();
    bool isFocus = false;
    private void Update()
    {
        // EditorWindow 를 포커스하고 있지 않으면 컴포넌트에서 바뀐 값이 실시간으로 적용되지 않으므로 이를 해결하기 위한 코드
        if (!isFocus)
        {
            UpdateTargetDic();
            Repaint(); // Repaint 이벤트를 호출해서 OnGUI()를 실행시킴 
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Get CustomComponent!!"))
        {
            targetDic.Clear();
            CustomComponent[] allCustoms = FindObjectsOfType<CustomComponent>();
            if (allCustoms == null)
            {
                Debug.Log("None Custom!!");
                return;
            }

            for (int i = 0; i < allCustoms.Length; i++)
            {
                // 이때 serialObj는 MonoBehaviour을 상속받으며 직렬화가 가능해야 함
                SerializedObject serialObj = new SerializedObject(allCustoms[i]);
                List<SerializedProperty> seriaProperties = new List<SerializedProperty>()
                {
                    serialObj.FindProperty(nameof(CustomComponent.myGameObject)),
                    serialObj.FindProperty(nameof(CustomComponent.myName)),
                    serialObj.FindProperty(nameof(CustomComponent.hp)),
                };

                targetDic.Add(serialObj, seriaProperties);
            }
        }

        // ChangeCheck : BeginChangeCheck와 EndChangeCheck로 둘러싸인 GUI에 어떤 변경을 실행했을때, EndChangeCheck가 true를 반환.
        foreach (KeyValuePair<SerializedObject, List<SerializedProperty>> pair in targetDic)
        {
            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.LabelField(pair.Key.targetObject.name, EditorStyles.boldLabel);
                EditorGUI.indentLevel++; // 한칸 오른쪽으로 밀린 곳에서 GUI를 그림
                foreach (SerializedProperty property in pair.Value) EditorGUILayout.PropertyField(property);
                EditorGUI.indentLevel--; // 원상복귀
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); // 선긋기
            }
            // 변화가 있다면 SerializedObject안의 프로퍼티들 업데이트
            if (EditorGUI.EndChangeCheck()) pair.Key.ApplyModifiedProperties();
        }
    }

    // Focus : 창을 클릭하면 막 선에 강조 표시뜨면서 조종 가능한 상태 말 그대로 집중
    private void OnFocus()
    {
        isFocus = true;
        UpdateTargetDic();
    }

    // 집중을 잃다
    private void OnLostFocus()
    {
        isFocus = false;
    }

    void UpdateTargetDic()
    {
        // KeyValuePair<T1, T2> 딕셔너리의 요소를 가져옴, Key와 Value에 둘 다 접근할 수 있음
        // pair.Key, pair.Value
        foreach (KeyValuePair<SerializedObject, List<SerializedProperty>> item in targetDic) item.Key.Update();
    }
}
