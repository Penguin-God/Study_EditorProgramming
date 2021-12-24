using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // UnityEditor ���� API ����ϱ� ���� ���ù�

public class MyEditorWindow : EditorWindow // ���
{
    // Edit, Window�� �ִ� �޴� â�� �Լ��� �����ų ��� �߰�
    [MenuItem("MyTool/OpenMyWindow %g")]
    static void OpenWindow()
    {
        MyEditorWindow myWindow = GetWindow<MyEditorWindow>();
        myWindow.title = "Hello Window";
    }
}
