using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BeatMapData
{
    public string songName;
    public float songLength;
    public List<NodeData> nodes = new List<NodeData>();
    public List<BoomMarkData> bookmarks = new List<BoomMarkData>();
    public GridSetting gridSetting;
}

//노드의 정보
[System.Serializable]
public class NodeData
{
    public Vector2Int position;
    public float timing;
    public NodeType type;
}

//그리드의 정보
[System.Serializable]
public class GridSetting
{
    //가로 칸
    public int row = 4;
    //새로 칸 = 노래 길이에 비례, 100은 기본값
    public int column = 100;
    //셀의 사이즈는 사실 (노래의 길이) Length / (행의 수) column 으로 결정이 될 듯 
    public float cellSize = 1f;
    //초당 셀의 크기
    public float secondsPerCell = 1f;
}

//북마크의 정보
[System.Serializable]
public class BoomMarkData
{
    public float timing;
}

//노드 타입(피아노, 드럼 예정?)
public enum NodeType
{
    None,
    Piano,
    Drum
}