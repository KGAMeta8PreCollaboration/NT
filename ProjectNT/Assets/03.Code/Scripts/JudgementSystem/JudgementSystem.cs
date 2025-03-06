using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementSystem : MonoBehaviour
{
	public List<GameObject> boxNoteList = new List<GameObject>();

	[SerializeField] private Transform _center;
	[SerializeField] private Transform _timingTrans;
	private Vector2[] timingBoxs;

}
