using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject을 상속받으면 해당 클래스를 Create에 새로운 경로를 만들고 C#스크립트 만들듯이 Assets 파일로  만들어서 사용 가능
[CreateAssetMenu(menuName = "Custom Grid/Create Palette")]
public class GridPalette : ScriptableObject
{
    public List<PaletteItem> items = new List<PaletteItem>();

    public PaletteItem GetItem(int _id)
    {
        return items.Find(_item => _item.id == _id);
    }
}