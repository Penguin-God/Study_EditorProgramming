using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    public CustomGridConfig config;
    Dictionary<Vector2Int, MapObject> itemDic = new Dictionary<Vector2Int, MapObject>();

    // 각각 gird를 배치할 크기를 나타내는 선을 가로선, 세로선을 그리는데 필요한 위치를 가지고 있는 배열
    // Handles.DrawLines를 이용할 것이기 때문에 짝수 번째 요소와 바로 다음 홀수 번째 요소를 이어서 선을 그림
    public Vector3[] horLines;
    public Vector3[] verLines;

    // cell 전용 포지션을 리턴
    public Vector2Int GetCellPos(Vector3 _worldPos)
    {
        int x = Mathf.FloorToInt(_worldPos.x / config.cellSize.x);
        int y = Mathf.FloorToInt(_worldPos.y / config.cellSize.y);
        return new Vector2Int(x, y);
    }

    Vector3 CellPosToCreatePoint(Vector2Int _cellPoint)
    {
        return new Vector3(_cellPoint.x * config.cellSize.x + config.cellSize.x * 0.5f, _cellPoint.y * config.cellSize.y + config.cellSize.y * 0.5f, 0);
    }

    public bool RePosition { get; set; } = false;

    // 인스펙터 또는 스크립트 값이 변경됐을 때 에디터에서 호출되며 플레이 모드가 아닐 때만 호출된다.
    // 값을 변경한 후 스크립트를 저장해 유니티에서 컴파일해서 다시 로드할 때도 실행된다.
    private void OnValidate()
    {
        RePosition = true;
    }

    public void RefreshPosition()
    {
        // 곱하기 2는 개수를 맞추기 위함. + 2는 마지막 선을 그리기 위해서 넣음
        // 만약 가로 3개를 그린다고 3개를 감싸야 하므로 필요한 선의 개수는 개수보다 하나 많은 4개
        // 선 하나당 2개의 위치가 필요하므로 개수에 * 2를 하고 마지막 선을 그릴 2개를 더하면 딱 필요한 위치의 개수가 됨
        int horLineCount = config.cellCount.x * 2 + 2;
        int verLineCount = config.cellCount.y * 2 + 2;
        horLines = new Vector3[horLineCount];
        verLines = new Vector3[verLineCount];

        // X는 같고 Y값만 다르게 해서 수직선을 그음
        for(int i = 0; i <= config.cellCount.x; i++)
        {
            horLines[i * 2] = new Vector3(i * config.cellSize.x, 0, 0);
            horLines[i * 2 + 1] = new Vector3(i * config.cellSize.x, config.cellSize.y * config.cellCount.y, 0);
        }

        // Y는 같고 X값만 다르게 해서 수평선을 그음
        for (int i = 0; i <= config.cellCount.y; i++)
        {
            verLines[i * 2] = new Vector3(0, i * config.cellSize.y, 0);
            verLines[i * 2 + 1] = new Vector3(config.cellSize.x * config.cellCount.x, i * config.cellSize.y, 0);
        }
    }

    public bool CheckCellAreaInPos(Vector2Int _pos)
    {
        return _pos.x >= 0 && _pos.x < config.cellCount.x && _pos.y >= 0 && _pos.y < config.cellCount.y; 
    }

    public bool CheckItemExist(Vector2Int _cellPos)
    {
        return itemDic.ContainsKey(_cellPos);
    }

    public MapObject GetItem(Vector2Int _key)
    {
        if (!itemDic.TryGetValue(_key, out MapObject _mapObj)) return null;
        return _mapObj;
    }

    public MapObject AddItem(Vector2Int _cellPos, PaletteItem _item)
    {
        if (itemDic.ContainsKey(_cellPos))
        {
            Debug.LogWarning("already exist item!!");
            return null;
        }

        GameObject _target = GameObject.Instantiate(_item.targetObj, transform);
        _target.transform.position = CellPosToCreatePoint(_cellPos);
        MapObject _mapObj = _target.AddComponent<MapObject>();
        _mapObj.id = _item.id;
        itemDic.Add(_cellPos, _mapObj);
        return _mapObj;
    }

    public void RemoveItem(Vector2Int _key)
    {
        if (itemDic.ContainsKey(_key)) itemDic.Remove(_key);
    }
}
