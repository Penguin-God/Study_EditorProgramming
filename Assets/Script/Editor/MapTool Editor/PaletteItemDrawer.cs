using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PaletteItemDrawer
{
    public static bool Draw(Vector2 _slotSize, bool _isSelected, PaletteItem _item)
    {
        Rect _area = GUILayoutUtility.GetRect(_slotSize.x, _slotSize.y, GUIStyle.none, GUILayout.MaxWidth(_slotSize.x), GUILayout.MaxHeight(_slotSize.y));
        bool _isClicked = GUI.Button(_area, AssetPreview.GetAssetPreview(_item.targetObj));
        GUI.Label(new Rect(_area.center.x, _area.center.y, 100, 50), _item.myName);

        if (_isSelected)
        {
            Rect _selectMarkArea = _area;
            _selectMarkArea.x = _selectMarkArea.center.x - 30;
            _selectMarkArea.width = 30;
            _selectMarkArea.height = 30;
            GUI.DrawTexture(_selectMarkArea, EditorGUIUtility.FindTexture("d_FilterSelectedOnly@2x"));
        }

        return _isClicked;
    }
}