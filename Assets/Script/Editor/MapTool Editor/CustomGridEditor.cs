using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomGrid))]
public class CustomGridEditor : Editor
{
    // 하이라키에서 선택되거나 되지 않았을 때. 즉 그냥 그림
    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    static void DrawHandles(CustomGrid _grid, GizmoType _gizmoType)
    {
        if (_grid.RePosition)
        {
            _grid.RefreshPosition();
            _grid.RePosition = false;
        }

        // Handles.DrawLines : 짝수 크기의 배열이 오면 0번째랑 1번째 잇고, 2번째랑  3번째 잇고 무한 반복
        // 배열의 크기가 홀수면 마지막 요소는 배재함
        Handles.DrawLines(_grid.horLines);
        Handles.DrawLines(_grid.verLines);
    }
}
