using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortNoteSpawnData : NoteSpawnData
{
    public double targetDspTime;

    public ShortNoteSpawnData(Note notePrefab, AudioClip hitSound, double spawnDspTime, double targetDspTime, Quaternion rotation, Enums.PlayMode playMode)
        : base(notePrefab, hitSound, spawnDspTime, rotation, playMode)
    {
        this.targetDspTime = targetDspTime;
    }
}

public class LongNoteSpawnData : NoteSpawnData
{
    public double startTargetDspTime;
    public double endTargetDspTime;

    public LongNoteSpawnData(Note notePrefab, AudioClip hitSound, double spawnDspTime, double startTargetDspTime, double endTargetDspTime, Quaternion rotation, Enums.PlayMode playMode)
        : base(notePrefab, hitSound, spawnDspTime, rotation, playMode)
    {
        this.startTargetDspTime = startTargetDspTime;
        this.endTargetDspTime = endTargetDspTime;
    }
}
public class TopNoteSpawnData : NoteSpawnData
{
    public double targetDspTime;
    public string myTag;

    public TopNoteSpawnData(Note notePrefab, AudioClip hitSound, double spawnDspTime, double targetDspTime, Quaternion rotation, string myTag, Enums.PlayMode playMode)
        : base(notePrefab, hitSound, spawnDspTime, rotation, playMode)
    {
        this.targetDspTime = targetDspTime;
        this.myTag = myTag;
    }
}

public class NoteSpawnData
{
    public Note notePrefab;
    public double spawnDspTime;
    public AudioClip hitSound;
    public Quaternion rotation;
    public Enums.PlayMode playMode;

    public NoteSpawnData(Note notePrefab, AudioClip hitSound, double spawnDspTime, Quaternion rotation, Enums.PlayMode playMode)
    {
        this.notePrefab = notePrefab;
        this.hitSound = hitSound;
        this.spawnDspTime = spawnDspTime;
        this.rotation = rotation;
        this.playMode = playMode;
    }
}
