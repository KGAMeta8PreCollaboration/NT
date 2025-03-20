using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortNoteSpawnData : NoteSpawnData
{
    public double targetTime;

    public ShortNoteSpawnData(double targetTime)
    {
        this.targetTime = targetTime;
    }
}

public class LongNoteSpawnData : NoteSpawnData
{
    public double startTargetTime;
    public double endTargetTime;

    public LongNoteSpawnData(double startTargetTime, double endTargetTime)
    {
        this.startTargetTime = startTargetTime;
        this.endTargetTime = endTargetTime;
    }
}

public class NoteSpawnData
{
}
