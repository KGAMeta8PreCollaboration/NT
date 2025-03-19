using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BeatMapData
{
    public SongData songData = new SongData();
    public List<NodeData> nodes = new List<NodeData>();
    public GridSetting gridSetting;
}

//노드의 정보
[System.Serializable]
public class NodeData
{
    public Vector2Int index;
    public NodeType nodeType;
    public string keySound;
}

[System.Serializable]
public class SongData
{
    public string songName;
    public float songLength;
    public int phase2;
    public int phase3;
}

//그리드의 정보
[System.Serializable]
public class GridSetting
{
    public float BPM;
    public int Column;
    public int BeatNum;
}

//노드 타입(상단노트, 하단 닷 노트, 하단 롱 노트)
public enum NodeType
{
    BottomBotNode,
    BottomLongNode,
    TopNode
}

public enum NodeInstrument
{
    Piano,
    Drum,
}

//북마크의 정보
[System.Serializable]
public class BoomMarkData
{
    public float timing;
}
