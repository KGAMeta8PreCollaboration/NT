using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortNoteSpawnData : NoteSpawnData
{
	public double targetDspTime;

	public ShortNoteSpawnData(Note notePrefab, AudioClip hitSound, double spawnDspTime, double targetDspTime)
		: base(notePrefab, hitSound, spawnDspTime)
	{
		this.targetDspTime = targetDspTime;
	}
}

public class LongNoteSpawnData : NoteSpawnData
{
	public double startTargetDspTime;
	public double endTargetDspTime;

	public LongNoteSpawnData(Note notePrefab, AudioClip hitSound, double spawnDspTime, double startTargetDspTime, double endTargetDspTime)
		: base(notePrefab, hitSound, spawnDspTime)
	{
		this.startTargetDspTime = startTargetDspTime;
		this.endTargetDspTime = endTargetDspTime;
	}
}

public class NoteSpawnData
{
	public Note notePrefab;
	public double spawnDspTime;
	public AudioClip hitSound;

	public NoteSpawnData(Note notePrefab, AudioClip hitSound, double spawnDspTime)
	{
		this.notePrefab = notePrefab;
		this.hitSound = hitSound;
		this.spawnDspTime = spawnDspTime;
	}
}
