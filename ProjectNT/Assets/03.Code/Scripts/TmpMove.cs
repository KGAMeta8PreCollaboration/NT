using UnityEngine;

public class TmpMove : MonoBehaviour
{
    public enum Axis { X, Y, Z, }
    public enum Direction { Forward, Backward, }
    
    public Axis axis;
    public Direction direction;
    public float speed;
    
    private void Update()
    {
        float moveDirection = direction == Direction.Forward ? 1 : -1;
        if (axis == Axis.X)
            transform.Translate(moveDirection * Time.deltaTime * speed, 0, 0);
        else if (axis == Axis.Y)
            transform.Translate(0, moveDirection * Time.deltaTime * speed, 0);
        else if (axis == Axis.Z)
            transform.Translate(0, 0, moveDirection * Time.deltaTime * speed);
    }
}
