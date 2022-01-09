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
}
