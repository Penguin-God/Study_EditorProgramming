using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteDrawer
{
    public GridPalette targetPalette;
    Vector2 slotSize = new Vector2(100, 100);
    Vector2 scrollPos;
    int selectedIndex = -1;

    public PaletteItem Selected_Item
    {
        get
        {
            if (selectedIndex == -1) return null;

            return targetPalette.items[selectedIndex];
        }
    }

    public void Draw(Vector2 _size)
    {
        if(targetPalette == null || targetPalette.items.Count == 0)
        {
            EditorHelper.DrawCenterLabel(new GUIContent("데이터 없음"), Color.red, 20, FontStyle.Bold);
            return;
        }

        if (selectedIndex == -1) selectedIndex = 0;
        scrollPos = EditorHelper.DrawGridItem(10, targetPalette.items.Count, _size.x, slotSize, (int _index) => 
        {
            bool _IsSelected = PaletteItemDrawer.Draw(slotSize, selectedIndex == _index, targetPalette.items[_index]);
            if (_IsSelected) selectedIndex = _index;
        });
    }
}
