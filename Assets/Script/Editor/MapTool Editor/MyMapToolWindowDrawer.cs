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

    GridPalette targetPalette = null;
    PaletteDrawer paletteDrawer = new PaletteDrawer();

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
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        ClearAllGrid();
    }

    void Draw()
    {
        switch (mode)
        {
            case MapToolMode.Create: DrawCreateMode(); break;
            case MapToolMode.Edit: DrawEditMode(); break;
            default: Debug.Log("모드 없음"); break;
        }
    }

    void ChangeMode(MapToolMode _newMode)
    {
        if (mode == _newMode) return;

        switch (_newMode)
        {
            case MapToolMode.Create: break;
            case MapToolMode.Edit: SceneView.lastActiveSceneView.in2DMode = true; break;
        }

        mode = _newMode;
    }

    bool IsCreateAble => cellCount.x > 0 && cellCount.y > 0 && cellSize.x > 0 && cellSize.y > 0 && targetPalette != null; // readonly
    // 가로, 세로 셀 개수
    public Vector2Int cellCount;
    // 셀 가로, 세로 길이
    public Vector2 cellSize;
    void DrawCreateMode()
    {
        EditorHelper.DrawCenterLabel(new GUIContent("Hello Create Mode"), Color.green, 20, FontStyle.BoldAndItalic);

        using (var scope = new GUILayout.VerticalScope(GUI.skin.window))
        {
            cellCount = EditorGUILayout.Vector2IntField("Cell 개수", cellCount);
            cellSize = EditorGUILayout.Vector2Field("Cell 크기", cellSize);
            targetPalette = (GridPalette)EditorGUILayout.ObjectField("연결할 팔레트", targetPalette, typeof(GridPalette));
            paletteDrawer.targetPalette = targetPalette;
        }

        // 프로퍼티를 이용해 cell값이 설정되어있을 때애만 버튼을 클릭할 수 있게 함
        GUI.enabled = IsCreateAble;
        // GUI 비활성화 상태로 버튼 그리면 클린 안됨
        if (EditorHelper.DrawCenterButton("생성하기", new Vector2(100, 50)))
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



    void DrawEditMode()
    {
        // Begin에 인자값을 넘겨서 전체 스타일을 정의할 수 있음
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        {
            if (GUILayout.Button("생성 모드로 이동", EditorStyles.toolbarButton))
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

        }
        EditorGUILayout.EndHorizontal();

        EditorHelper.DrawCenterLabel(new GUIContent("편집 모드"), Color.cyan, 20, FontStyle.BoldAndItalic);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            editMode = (EditMapMode)GUILayout.Toolbar((int)editMode, editContents, "LargeButton", GUILayout.Width(100), GUILayout.Height(40));
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();

        // 마지막에 그린 요소의 Rect를 가져옴
        // Rect설명
        // x, y : 그리기 시작하는 위치. x는 오른쪽으로, y는 아래쪽으로 갈수록 +
        // width, height : 시작위치부터 얼마나 그릴 것인가
        Rect _lastRect = GUILayoutUtility.GetLastRect();
        Rect _boxArea = new Rect(0, _lastRect.yMax, position.width, position.height - _lastRect.yMax - 1);
        GUI.Box(_boxArea, GUIContent.none, GUI.skin.window);

        paletteDrawer.Draw(new Vector2(position.width, position.height));
    }


    private void OnSceneGUI(SceneView obj)
    {
        //Vector2 myMousePos = Event.current.mousePosition;
        //Ray ray = HandleUtility.GUIPointToWorldRay(myMousePos);
        //EditorHelper.RayCast(ray.origin, ray.origin + (ray.direction * 300), out Vector3 hitPos);
        //Debug.Log(FindObjectOfType<CustomGrid>().GetCellPos(hitPos));

        if (mode != MapToolMode.Edit) return;

        // 왼쪽 마우스를 다운했다면
        if(Event.current.button == 0 && Event.current.type == EventType.MouseDown)
        {
            Vector2 _mousePos = Event.current.mousePosition;
            Ray _ray = HandleUtility.GUIPointToWorldRay(_mousePos);
            EditorHelper.RayCast(_ray.origin, _ray.origin + _ray.direction * 300, out Vector3 _hitPos);
            Vector2Int _cellPos = targetGrid.GetCellPos(_hitPos);
            if (targetGrid.CheckCellAreaInPos(_cellPos))
            {
                if (editMode == EditMapMode.Paint) Paint(_cellPos);
                else Erase(_cellPos);
            }
        }
    }

    void Paint(Vector2Int _cellPos)
    {
        PaletteItem _item = paletteDrawer.Selected_Item;
        if (_item == null) return;

        if (targetGrid.CheckItemExist(_cellPos))
        {
            Debug.Log("Remove");
            DestroyImmediate(targetGrid.GetItem(_cellPos).gameObject);
            targetGrid.RemoveItem(_cellPos);
        }

        targetGrid.AddItem(_cellPos, _item);
        Event.current.Use();
    }

    void Erase(Vector2Int _cellPos)
    {
        if (targetGrid.CheckItemExist(_cellPos))
        {
            Debug.Log("Remove");
            DestroyImmediate(targetGrid.GetItem(_cellPos).gameObject);
            targetGrid.RemoveItem(_cellPos);
        }
        Event.current.Use();
    }


    void ClearAllGrid()
    {
        CustomGrid[] grids = FindObjectsOfType<CustomGrid>();
        for (int i = 0; i < grids.Length; i++) DestroyImmediate(grids[i].gameObject);
        targetGrid = null;
    }
}
