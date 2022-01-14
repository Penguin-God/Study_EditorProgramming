using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorHelper
{
    public static void DrawCenterLabel(GUIContent _content, Color _color, int _size, FontStyle _fontStyle)
    {
        GUIStyle _style = new GUIStyle();
        _style.fontSize = _size;
        _style.fontStyle = _fontStyle;
        _style.normal.textColor = _color;
        _style.padding.top = 10;
        _style.padding.bottom = 10;

        GUILayout.BeginHorizontal();
        {
            // 밀기
            GUILayout.FlexibleSpace();
            GUILayout.Label(_content, _style);
            GUILayout.FlexibleSpace();
        } GUILayout.EndHorizontal();

    }

    public static bool DrawCenterButton(string _text, Vector2 _size)
    {
        GUILayout.Space(5);
        bool isClick = false;

        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            isClick = GUILayout.Button(_text, GUILayout.Width(_size.x), GUILayout.Height(_size.y));
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        return isClick;
    }

    // destination : 도착점
    public static void RayCast(Vector3 originPos, Vector3 destinationPos, out Vector3 hitPos)
    {
        // 뭐 시계 방향으로 돌면서 외적을해야 씬 뷰 카메라를 바라보는 법선 백터가 나온다느니
        // 매개변수 방적식을 풀려면 내적을 해야한다느니 난 모르겠다~~
        Vector3 planePos_1 = Vector3.up;
        Vector3 planePos_2 = Vector3.right;
        Vector3 planePos_3 = Vector3.down;

        Vector3 planeDir = Vector3.Cross((planePos_2 - planePos_1).normalized, (planePos_3 - planePos_1).normalized);
        Vector3 lineDir = (destinationPos - originPos).normalized;

        float dotLinePlane = Vector3.Dot(lineDir, planeDir);
        float t = Vector3.Dot(planePos_1 - originPos, planeDir) / dotLinePlane;

        hitPos = originPos + (lineDir * t);
    }

    public static Vector2 DrawGridItem(int _gapSpace, int _itemCount, float _width, Vector2 _size, System.Action<int> _onDraw)
    {
        int _horCount = (int)(_width / _size.x);
        if (_horCount <= 0) _horCount = 1;
        int _verCount = _itemCount / _horCount;
        if (_itemCount % _horCount > 0) _verCount++;
        if (_verCount <= 0) _verCount = 1;


        Vector2 _scollPos = Vector2.zero;
        _scollPos = GUILayout.BeginScrollView(_scollPos);
        {
            GUILayout.BeginVertical();
            {
                for (int i = 0; i < _verCount; i++)
                {
                    GUILayout.BeginHorizontal();
                    {
                        for (int j = 0; j < _horCount; j++)
                        {
                            // _index = 현재 그리고 있는 item의 순서
                            int _index = j + (i * _horCount);
                            if (_index >= _itemCount) break;

                            // 아이템 그리기
                            _onDraw(_index);
                            GUILayout.Space(_gapSpace);
                        }
                    }GUILayout.EndHorizontal();
                }
            }GUILayout.EndVertical();
        }GUILayout.EndScrollView();

        return _scollPos;
    }

    public static GameObject ObjectCreateAndRegiterUndo(GameObject _gameObj)
    {
        GameObject _createObject = GameObject.Instantiate(_gameObj);
        // 생성된 오브젝트를 등록해 놓으면 Undo(취소) 했을 때 유니티가 알아서 삭제해줌
        Undo.RegisterCreatedObjectUndo(_createObject, "Create MapObject!!!");
        return _createObject;
    }

    public static void ObjectDestroyAndRegiterUndo(GameObject _gameObj) => Undo.DestroyObjectImmediate(_gameObj);
}
