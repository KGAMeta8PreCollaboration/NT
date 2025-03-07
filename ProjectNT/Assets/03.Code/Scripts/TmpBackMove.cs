using UnityEngine;

public class TmpBackMove : MonoBehaviour
{
    private void Update()
    {
        transform.position += new Vector3(1f * Time.deltaTime, 0, 0);
    }
}
