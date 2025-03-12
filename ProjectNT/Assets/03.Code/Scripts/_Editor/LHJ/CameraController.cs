using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [Header("마우스 이동 속도")]
    [SerializeField] private float movementSpeed = 5f;
    [Header("마우스 민감도")]
    [SerializeField] private float mouseSensitivity = 2f;
    [Header("메인 카메라")]
    [SerializeField] private Camera mainCamera;
    [Header("카메라 시작 포인트")]
    [SerializeField] private Vector3 initCameraPos;

    //마우스 오른쪽 클릭 o -> true / x -> false
    public bool _isRotating = false;
    private Vector2 _savedMousePos = Vector2.zero;
    private Vector3 _moveDirection;
    private float _mouseX, _mouseY;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        //초기 카메라 위치 설정
        mainCamera.transform.position = initCameraPos;
    }

    private void Update()
    {
        HandleMouseInput();
        if (_isRotating == true)
        {
            HandleKeyboardInput();
            MoveCamera();
            RotateCamera();
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButton(1))
        {
            if (!_isRotating)
            {
                _isRotating = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                _savedMousePos = Mouse.current.position.ReadValue();
            }
            _mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
            _mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            _mouseY = Mathf.Clamp(_mouseY, -90f, 90f);
        }
        else if (_isRotating)
        {
            _isRotating = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Mouse.current.WarpCursorPosition(_savedMousePos);
        }
    }

    private void HandleKeyboardInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float upDown = 0f;
        if (Input.GetKey(KeyCode.Space))
            upDown += 1f;
        if (Input.GetKey(KeyCode.LeftControl))
            upDown -= 1f;

        _moveDirection =
            mainCamera.transform.right * horizontal +
            mainCamera.transform.forward * vertical +
            Vector3.up * upDown;
    }

    private void MoveCamera()
    {
        if (_moveDirection.magnitude >= 0.1f)
        {
            mainCamera.transform.position += _moveDirection.normalized * (movementSpeed * Time.deltaTime);
        }
    }

    private void RotateCamera()
    {
        if (_isRotating)
        {
            mainCamera.transform.rotation = Quaternion.Euler(_mouseY, _mouseX, 0f);
        }
    }

    //InputSystem 사용시 아래껄 R&D 하자
    ////Oh boy new Unity Input System POGCHAMP
    //public void OnMoveCamera(CallbackContext context)
    //{
    //    //Take our movement vector and manipulate it to work how we want.
    //    //Our X component (A and D) should move us left/right (X)
    //    //Our Y component (W and S) should move us forward/backward (Z)
    //    var movement = context.ReadValue<Vector2>();
    //    x = movement.x;
    //    z = movement.y;
    //}

    //// God I hate this
    //public void OnElevateCamera(CallbackContext context) => elevateHeld = context.performed;
    //public void OnLowerCamera(CallbackContext context) => lowerHeld = context.performed;
    //public void OnMoveCameraLeft(CallbackContext context) => leftHeld = context.performed;
    //public void OnMoveCameraRight(CallbackContext context) => rightHeld = context.performed;
    //public void OnMoveCameraForward(CallbackContext context) => forwardHeld = context.performed;
    //public void OnMoveCameraBackward(CallbackContext context) => backwardHeld = context.performed;

    //public void OnRotateCamera(CallbackContext context)
    //{
    //    var deltaMouseMovement = context.ReadValue<Vector2>();
    //    mouseX = deltaMouseMovement.x * mouseSensitivity / 10f;
    //    mouseY = deltaMouseMovement.y * mouseSensitivity / 10f;
    //}

    //public void OnHoldtoMoveCamera(CallbackContext context)
    //{
    //    if (customStandaloneInputModule.IsPointerOverGameObject<GraphicRaycaster>(0, true)) return;
    //    canMoveCamera = context.performed;
    //    if (canMoveCamera)
    //        CMInputCallbackInstaller.DisableActionMaps(typeof(CameraController), actionMapsDisabledWhileMoving);
    //    else if (context.canceled)
    //        CMInputCallbackInstaller.ClearDisabledActionMaps(typeof(CameraController), actionMapsDisabledWhileMoving);
    //}

    //public void OnAttachtoNoteGrid(CallbackContext context)
    //{
    //    if (RotationCallbackController.IsActive && context.performed && noteGridTransform.gameObject.activeInHierarchy && !playerCamera)
    //        LockedOntoNoteGrid = !LockedOntoNoteGrid;
    //}

    //public void OnToggleFullscreen(CallbackContext context)
    //{
    //    if (!Application.isEditor && context.performed) Screen.fullScreen = !Screen.fullScreen;
    //}
}
