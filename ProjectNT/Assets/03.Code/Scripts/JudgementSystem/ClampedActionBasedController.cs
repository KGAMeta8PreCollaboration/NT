using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ClampedActionBasedController : ActionBasedController
{
    public float minPosY = -1f;
    public float maxPosY = 1f;

    public float minRotX = -10f;
    public float maxRotX = 10f;
    public float minRotY = -30f;
    public float maxRotY = 30f;

    public float positionSmoothSpeed = 10f;
    public float rotationSmoothSpeed = 10f;

    private Vector3 smoothedPosition;
    private Quaternion smoothedRotation;

    protected override void UpdateTrackingInput(XRControllerState controllerState)
    {
        if (controllerState == null)
            return;

        InputAction posAction = positionAction.action;
        InputAction rotAction = rotationAction.action;

        controllerState.inputTrackingState = InputTrackingState.None;

        if (posAction != null && posAction.bindings.Count > 0)
            controllerState.inputTrackingState |= InputTrackingState.Position;

        if (rotAction != null && rotAction.bindings.Count > 0)
            controllerState.inputTrackingState |= InputTrackingState.Rotation;

        if ((controllerState.inputTrackingState & InputTrackingState.Position) != 0 && posAction != null)
        {
            Vector3 rawPos = posAction.ReadValue<Vector3>();
            rawPos.y = Mathf.Clamp(rawPos.y, minPosY, maxPosY);

            if (smoothedPosition == Vector3.zero)
                smoothedPosition = rawPos;

            smoothedPosition = Vector3.Lerp(smoothedPosition, rawPos, Time.deltaTime * positionSmoothSpeed);
            controllerState.position = smoothedPosition;
        }

        if ((controllerState.inputTrackingState & InputTrackingState.Rotation) != 0 && rotAction != null)
        {
            Quaternion rawRot = rotAction.ReadValue<Quaternion>();
            Vector3 euler = rawRot.eulerAngles;

            euler.x = ClampSignedAngle(euler.x, minRotX, maxRotX);
            euler.y = ClampSignedAngle(euler.y, minRotY, maxRotY);
            euler.z = 0f;

            Quaternion clampedRot = Quaternion.Euler(euler);

            if (smoothedRotation == Quaternion.identity)
                smoothedRotation = clampedRot;

            smoothedRotation = Quaternion.Lerp(smoothedRotation, clampedRot, Time.deltaTime * rotationSmoothSpeed);
            controllerState.rotation = smoothedRotation;
        }
    }

    private float ClampSignedAngle(float angle, float min, float max)
    {
        angle = Mathf.Repeat(angle + 180f, 360f) - 180f;
        return Mathf.Clamp(angle, min, max);
    }
}
