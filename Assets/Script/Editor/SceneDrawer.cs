using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MyCube))]
public class SceneDrawer : Editor
{
    MyCube Target = null;
    MyCube CurrentTarget = null;
    private void OnEnable()
    {
        Target = base.target as MyCube;
        if (Target != CurrentTarget)
        {
            // 오브젝트가 선택되면 씬 뷰에 드로잉하는 이벤트 구독
            SceneView.duringSceneGui += DrawSceneGUI;
            CurrentTarget = Target;
        }
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DrawSceneGUI;
    }

    void DrawSceneGUI(SceneView obj)
    {
        if (Target == null) return;
        CurrentTarget = Target;
        Handles.Label(Target.transform.position, $"This is {Target.gameObject.name}");

        MyCube[] allCubes = FindObjectsOfType<MyCube>();
        for(int i = 0; i < allCubes.Length; i++)
        {
            if(allCubes[i] != Target)
            {
                Vector3 pos = allCubes[i].transform.position;
                Handles.DrawLine(Target.transform.position, pos); // 두개의 좌표를 잇는 선 그림

                Handles.color = Color.red;
                // 큐브 그림 인자는 center, size
                Handles.DrawWireCube(pos, Vector3.one);
                Handles.color = Color.white;
            }
        }

        Handles.DrawWireCube(Target.transform.position, new Vector3(2,3,2));

        DrawMoveButton();
    }

    void DrawMoveButton()
    {
        Handles.BeginGUI();
        {
            if(GUILayout.Button("OtherMove Right!!"))
            {
                MyCube[] allCubes = FindObjectsOfType<MyCube>();
                for (int i = 0; i < allCubes.Length; i++)
                {
                    if (allCubes[i] != Target)
                        allCubes[i].transform.position += Vector3.right;
                }
            }

            if (GUILayout.Button("OtherMove Left!!"))
            {
                MyCube[] allCubes = FindObjectsOfType<MyCube>();
                for (int i = 0; i < allCubes.Length; i++)
                {
                    if (allCubes[i] != Target)
                        allCubes[i].transform.position += Vector3.right * -1;
                }
            }
        } Handles.EndGUI();
    }
}
