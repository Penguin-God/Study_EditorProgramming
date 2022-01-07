using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyAssetManager : EditorWindow
{
    [MenuItem("MyTool/Open Assaet Managent Window")]
    static void OpenWindow()
    {
        MyAssetManager myWindow = GetWindow<MyAssetManager>();
        myWindow.title = "Hello AssetManager";
    }

    // Unity가 Assets 폴더 내의 파일을 관리하는 방법
    // 같은 이름의 .meta 파일을 생성하는데 이 파일에는 guid라는 다른 파일과 중복되지 않는 고유 ID가 있다.
    // guid를 사용하여 에셋 파일을 식별하며, 이러한 방식을 사용하기 때문에 폴더 내의 파일 경로를 이리저리 옮겨도 문제가 생기지 않음
    void OnGUI()
    {
        if (GUILayout.Button("Loggin Mat guid and file path"))
        {
            // 필터에 걸린 에셋의 guid를 string 배열로 가져옴
            string[] guids = AssetDatabase.FindAssets("t:material"); // "t:material" : 필터. 메테리얼 타입만 가져와라
            for(int i = 0; i < guids.Length; i++)
            {
                string guid = guids[i];
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Debug.Log($"guid : {guid},  File Path : {path}");
            }
        }

        if (GUILayout.Button("Test All Mat By Cube"))
        {
            // 씬의 렌더러를 가진 오브젝트 전부 삭제
            Renderer[] allRenderer = FindObjectsOfType<Renderer>();
            // Editor 모드에서는 Destroy() 사용 못함
            for (int i = 0; i < allRenderer.Length; i++) DestroyImmediate(allRenderer[i].gameObject);

            // 필터에 걸린 에셋의 guid를 string 배열로 가져옴
            string[] guids = AssetDatabase.FindAssets("t:material"); // "t:material" : 필터. 메테리얼 타입만 가져와라
            for (int i = 0; i < guids.Length; i++)
            {
                string guid = guids[i];
                string path = AssetDatabase.GUIDToAssetPath(guid);
                // 파일 경로를 이용해 material을 로드함
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

                // Primitive : 가장 단순한 요소
                // 타입으로 유추할 때 Cupe, Plane 처럼 Unity에서 기본적으로 지원하는 오브젝트들을 생성하게 해주는 듯
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = Vector3.right * i * 2;
                cube.GetComponent<Renderer>().material = mat;
            }
        }

        if (GUILayout.Button("Create new Material Asset"))
        {
            Material newMat = new Material(Shader.Find("Standard"));
            AssetDatabase.CreateAsset(newMat, $"Assets/Mats/MyNewMaterial{Random.Range(1, 10000)}.mat");
        }
    }
}
