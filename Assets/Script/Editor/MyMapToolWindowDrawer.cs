using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum MapToolMode
{
    None, 
    Edit,
    Create,
}

public class MyMapToolWindowDrawer : EditorWindow
{
    [MenuItem("MyTool/Open My Map Tool")]
    static void Open()
    {
        MyMapToolWindowDrawer myWindow = GetWindow<MyMapToolWindowDrawer>();
        myWindow.title = "Hello Map Tool";
    }

    MapToolMode mode = MapToolMode.None;

    private void OnGUI()
    {
        Draw();
    }

    private void OnEnable()
    {
        ChangeMode(MapToolMode.Create);
    }

    private void OnDisable()
    {

    }



    void Draw()
    {
        switch (mode)
        {
            case MapToolMode.Edit: DrawEditMode(); break;
            case MapToolMode.Create: DrawCreateMode(); break;
            default: Debug.Log("모드 없음"); break;
        }
    }

    void DrawEditMode()
    {
        
    }

    bool isCreateAble => false; // readonly
    void DrawCreateMode()
    {
        EditorHelper.DrawCenterLabel(new GUIContent("Hello Create Mode"), Color.green, 20, FontStyle.BoldAndItalic);

        using(var scope = new GUILayout.VerticalScope(GUI.skin.window))
        {

        }

        // GUI 비활성화
        GUI.enabled = isCreateAble;
        // GUI 비활성화 상태로 버튼 그리면 클린 안됨
        EditorHelper.DrawCenterButton("생성하기", new Vector2(100, 50));
        GUI.enabled = false;
    }

    void ChangeMode(MapToolMode _newMode)
    {
        if (mode == _newMode) return;

        mode = _newMode;
        Draw();
    }
}
