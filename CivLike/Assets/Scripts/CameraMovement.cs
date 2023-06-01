using UnityEngine;
using UnityEngine.InputSystem;


public class CameraMovement : MonoBehaviour
{
    private Controls controls;
    private InputAction movement;
    private Transform cameraTransform;


    [SerializeField]
    private float maxSpeed = 5f;
    private float speed;
    [SerializeField]
    private float acceleration = 10f;
    [SerializeField]
    private float damping = 15f;

    [SerializeField]
    private float stepSize = 2f;
    [SerializeField]
    private float zoomDampening = 7.5f;
    [SerializeField]
    private float minHeight = 5f;
    [SerializeField]
    private float maxHeight = 50f;
    [SerializeField]
    private float zoomSpeed = 2f;


    [SerializeField]
    [Range(0f, 0.1f)]
    private float edgeTolerance = 0.05f;

    //value set in various functions 
    //used to update the position of the camera base object.
    private Vector3 targetPosition;

    private float zoomHeight;

    //used to track and maintain velocity w/o a rigidbody
    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;

    //tracks where the dragging action started
    Vector3 startDrag;

    private void Awake()
    {
        controls = new Controls();
        cameraTransform = this.transform;
    }

    private void OnEnable()
    {
        zoomHeight = cameraTransform.localPosition.z;     

        lastPosition = this.transform.position;

        movement = controls.Camera.Movement;
        controls.Camera.Zoom.performed += ZoomCamera;
        controls.Camera.Enable();
    }

    private void OnDisable()
    {
        controls.Camera.Zoom.performed -= ZoomCamera;
        controls.Camera.Disable();
    }


    private void Update()
    {
        //inputs
        GetKeyboardMovement();
        CheckMouseAtScreenEdge();

        //move base and camera objects
        UpdateVelocity();
        UpdateBasePosition();
        UpdateCameraPosition();
    }

    private void UpdateVelocity()
    {
        horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.z = 0f;
        lastPosition = this.transform.position;
    }

    private void GetKeyboardMovement()
    {
        Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight()
                    + movement.ReadValue<Vector2>().y * GetCameraUp();

        inputValue = inputValue.normalized;

        if (inputValue.sqrMagnitude > 0.1f)
            targetPosition += inputValue;
    }

    private void CheckMouseAtScreenEdge()
    {
        //mouse position is in pixels
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        //horizontal scrolling
        if (mousePosition.x < edgeTolerance * Screen.width)
            moveDirection += -GetCameraRight();
        else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
            moveDirection += GetCameraRight();

        //vertical scrolling
        if (mousePosition.y < edgeTolerance * Screen.height)
            moveDirection += -GetCameraUp();
        else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
            moveDirection += GetCameraUp();

        targetPosition += moveDirection;
    }

    private void UpdateBasePosition()
    {
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            //create a ramp up or acceleration
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += targetPosition * speed * Time.deltaTime;
        }
        else
        {
            //create smooth slow down
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
        }

        //reset for next frame
        targetPosition = Vector3.zero;
    }

    private void ZoomCamera(InputAction.CallbackContext obj)
    {
        float inputValue = -obj.ReadValue<Vector2>().y / 100f;

        if (Mathf.Abs(inputValue) > 0.1f)
        {
            zoomHeight = cameraTransform.localPosition.z + inputValue * stepSize;

            if (zoomHeight < minHeight)
                zoomHeight = minHeight;
            else if (zoomHeight > maxHeight)
                zoomHeight = maxHeight;
        }
    }

    private void UpdateCameraPosition()
    {
        //set zoom target
        Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, cameraTransform.localPosition.y, zoomHeight);
        //add vector for forward/backward zoom
        zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.z) * Vector3.forward;

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
        
    }

   

    //gets the horizontal forward vector of the camera
    private Vector3 GetCameraUp()
    {
        Vector3 up = cameraTransform.up;
        up.z = 0f;
        return up;
    }

    //gets the horizontal right vector of the camera
    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.z = 0f;
        return right;
    }

    


}

