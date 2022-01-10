using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public enum MapToolMode
{
    None, 
    Edit,
    Create,
}

public enum EditMapMode
{
    Paint,
    Erase,
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
    public CustomGrid targetGrid;

    EditMapMode editMode = EditMapMode.Paint;
    GUIContent[] editContents;

    private void OnGUI()
    {
        Draw();
    }

    private void OnEnable()
    {
        editContents = new GUIContent[]
        {
            EditorGUIUtility.TrIconContent("Grid.PaintTool", "그리기 모드"),
            EditorGUIUtility.TrIconContent("Grid.EraserTool", "지우기 모드")
        };
        ChangeMode(MapToolMode.Create);
    }

    private void OnDisable()
    {
        
    }

    private void OnSceneGUI(SceneView obj)
    {
        Vector2 myMousePos = Event.current.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(myMousePos);
        EditorHelper.RayCast(ray.origin, ray.origin + (ray.direction * 300), out Vector3 hitPos);
        Debug.Log(FindObjectOfType<CustomGrid>().GetCellPos(hitPos));
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
        EditorGUILayout.BeginHorizontal();
        {
            if(GUILayout.Button("생성 모드로 이동", EditorStyles.toolbarButton))
            {
                ClearAllGrid();
                ChangeMode(MapToolMode.Create);
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("불러오기", EditorStyles.toolbarButton))
            {

            }

            if (GUILayout.Button("저장하기", EditorStyles.toolbarButton))
            {

            }

        }EditorGUILayout.EndHorizontal();   

        EditorHelper.DrawCenterLabel(new GUIContent("편집 모드"), Color.cyan, 20, FontStyle.BoldAndItalic);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            editMode = (EditMapMode)GUILayout.Toolbar((int)editMode, editContents, "LargeButton", GUILayout.Width(100), GUILayout.Height(40));
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
    }
    
    bool isCreateAble => cellCount.x > 0 && cellCount.y > 0 && cellSize.x > 0 && cellSize.y > 0; // readonly

    // 가로, 세로 셀 개수
    public Vector2Int cellCount;
    // 셀 가로, 세로 길이
    public Vector2 cellSize;
    void DrawCreateMode()
    {
        EditorHelper.DrawCenterLabel(new GUIContent("Hello Create Mode"), Color.green, 20, FontStyle.BoldAndItalic);

        using(var scope = new GUILayout.VerticalScope(GUI.skin.window))
        {
            cellCount = EditorGUILayout.Vector2IntField("Cell 개수", cellCount);
            cellSize = EditorGUILayout.Vector2Field("Cell 크기", cellSize);
        }

        // 프로퍼티를 이용해 cell값이 설정되어있을 때애만 버튼을 클릭할 수 있게 함
        GUI.enabled = isCreateAble;
        // GUI 비활성화 상태로 버튼 그리면 클린 안됨
        if(EditorHelper.DrawCenterButton("생성하기", new Vector2(100, 50)))
        {
            targetGrid = BuildGrid(this.cellCount, this.cellSize);
            ChangeMode(MapToolMode.Edit);
        }
        GUI.enabled = true;
    }

    private CustomGrid BuildGrid(Vector2Int cellCount, Vector2 cellSize)
    {
        ClearAllGrid();

        // gameObject 생성 후 그리드 컴포넌트 추가
        CustomGrid _newGrid = new GameObject("Grid").AddComponent<CustomGrid>();
        _newGrid.config = new CustomGridConfig();
        _newGrid.config.cellCount = cellCount;
        _newGrid.config.cellSize = cellSize;
        return _newGrid;
    }

    void ChangeMode(MapToolMode _newMode)
    {
        if (mode == _newMode) return;

        mode = _newMode;
    }

    void ClearAllGrid()
    {
        CustomGrid[] grids = FindObjectsOfType<CustomGrid>();
        for (int i = 0; i < grids.Length; i++) DestroyImmediate(grids[i].gameObject);
        targetGrid = null;
    }
}
