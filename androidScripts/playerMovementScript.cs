using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovementScript : MonoBehaviour
{
    public PlayerInput playerInput;

    // The joystick instances being used for the android build
    public Joystick leftJoyStick;
    public Joystick rightJoyStick;

    //The base sensitivity of the joystick (How much it needs to move until movement occurs)
    public float sensitivity;

    //public Transform player;
    //public float universalForce = 1000f;
    public float rotationSpeed;

    public float speed;
    private int State;
    //The following variables will be used to store movement states for the player
    private int forwards;
    private int backwards;
    private int left;
    private int right;
    private int neutral;
    private int rightDiagonalUp;
    private int rightDiagonalDown;
    private int leftDiagonalUp;
    private int leftDiagonalDown;

    void Awake()
    {
        State = 0; //player will start in a neutral/idle state by default

        neutral = 0;
        forwards = 1;
        backwards = 2;
        left = 3;
        right = 4;
        rightDiagonalUp = 5;
        rightDiagonalDown = 6;
        leftDiagonalUp = 7;
        leftDiagonalDown = 8;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    // void Update()
    // {
    //     Control();
    //     Move();
    //     playerRotation();
    //     gameManager.Instance.State = State; //for debugging
    //     Debug.Log("State: " + State); //for debugging
    // }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>

    void leftJoystickMovement(){
        Vector3 horizontalDirection = Vector3.right * leftJoyStick.Horizontal;
        Vector3 verticalDirection = Vector3.forward * leftJoyStick.Vertical;

        Vector3 totalDirection = (horizontalDirection + verticalDirection) * speed * Time.deltaTime;

        Debug.Log("Horizontal axis: " + horizontalDirection + "\n" + "Vertical axis: " + verticalDirection + "\n" + "Overall Direction: " + totalDirection);

        transform.Translate(totalDirection); 
    }

    void rightJoystickMovement(){
        float horizontal = transform.eulerAngles.y + (rightJoyStick.Horizontal * rotationSpeed);
        transform.rotation = Quaternion.AngleAxis(horizontal, Vector3.up);   
    }
    void FixedUpdate()
    {
        leftJoystickMovement();
        rightJoystickMovement();
        
    }

    // Update is called once per frame
    //Old movement code!
    // void FixedUpdate()
    // {
    //     if (Input.GetKey("a"))
    //     {
    //         //rotate towards the left at 90 degrees per second
    //         transform.Rotate(0, -(90 * Time.deltaTime), 0);
    //         //rb.AddForce(-(universalForce * Time.deltaTime), 0, 0);
    //     }
    //     if (Input.GetKey("s"))
    //     {
    //         //move the player backwards with a force relative to their local axis
    //         rb.AddRelativeForce(0, 0, -(universalForce * Time.deltaTime));
    //     }
    //     if (Input.GetKey("d"))
    //     {
    //         transform.Rotate(0, (90 * Time.deltaTime), 0);
    //         //rb.AddForce(universalForce * Time.deltaTime, 0, 0);
    //     }
    //     if (Input.GetKey("w"))
    //     {
    //         //move the player forwards with a force relative to their local axis
    //         rb.AddRelativeForce(0, 0, universalForce * Time.deltaTime);
    //     }

    // }

    ///<summary>
    ///This handles any event to which one of the WASD keys are pressed. The result is the current state of the player being chanegd
    ///, which can be useful for character models having animations.
    ///</summary>
    void PressDownKey(KeyCode key, int newState, int prevStateOne, int prevStateTwo, int diagonalStateOne, int diagonalStateTwo){
        //if the given key is pressed, it checks the current state and changes it based on the value

        /*One good example of this is the W key. If the W key is pressed, it will check if A or D are also being pressed. If they
        are being pressed, the state changes for the directional up(left) or up(right). Otherwise, it will change the state to
        the value that makes the player go forwards.
        
        The parameters would be (playerInput.keyW , forwards, left, right, leftDiagonalUp, rightDiagonalUp) */
        if(Input.GetKeyDown(key)){
            if(State == prevStateOne){
                State = diagonalStateOne;
            }
            else if (State == prevStateTwo){
                State = diagonalStateTwo;
            }
            else{
                State = newState;
            }
        }
    }

    ///<summary>
    ///This handles any event to which one of the WASD keys are released. The result is the current state of the player being chanegd
    ///, which can be useful for character models having animations.
    ///</summary>
    void PressUpKey(KeyCode key, int currentState, int coreKeyState, int oldStateOne, int oldStateTwo, int newStateOne, int newStateTwo){
        //If the given key is released, it checks the current state and changes it based on the value

        /*For example, if the W key is pressed, it would check if the state was previously diagonal left/right. If so, it would 
        go left/right. This makes movement smoother and caters to using diagonal movement. */
        if(Input.GetKeyUp(key) && currentState == oldStateOne){
            State = newStateOne;
        }
        if(Input.GetKeyUp(key) && currentState == oldStateTwo){
            State = newStateTwo;
        }
        //If nothing else was previously pressed alongside the key, then the player returns to a neutral state.
        if(Input.GetKeyUp(key) && currentState == coreKeyState){
            State = neutral;
        }
    }

    ///<summary>
    ///Function used to rotate player on Y axis using the mouse's X axis on the screen
    ///</summary>
    // void playerRotation(){
    //     float horizontal = transform.eulerAngles.y + (Input.GetAxis("Mouse X") * rotationSpeed);
    //     transform.rotation = Quaternion.AngleAxis(horizontal, Vector3.up);
    // }
    
    ///<summary>
    ///the overall control system for the game using the WASD keys (or any keys selected in the Unity editor)
    ///</summary>

    private void Control(){
        //Handles W key
        PressDownKey(playerInput.keyW , forwards, left, right, leftDiagonalUp, rightDiagonalUp);
        PressUpKey(playerInput.keyW, State, forwards, leftDiagonalUp, rightDiagonalUp, left, right);
        //Handles A key
        PressDownKey(playerInput.keyA, left, forwards, backwards, leftDiagonalUp, leftDiagonalDown);
        PressUpKey(playerInput.keyA, State, left, leftDiagonalUp, leftDiagonalDown, forwards, backwards);
        //Handles S key
        PressDownKey(playerInput.keyS, backwards, left, right, leftDiagonalDown, rightDiagonalDown);
        PressUpKey(playerInput.keyS, State, backwards, leftDiagonalDown, rightDiagonalDown, left, right);
        //Handles D key
        PressDownKey(playerInput.keyD, right, forwards, backwards, rightDiagonalUp, rightDiagonalDown);
        PressUpKey(playerInput.keyD, State, right, rightDiagonalUp, rightDiagonalDown, forwards, backwards);
    }

    ///</summary>
    ///Gives movement commands for the player to do based on what the current player state is
    ///</summary>
    private void Move(){
        switch(State){
            case 0:
                transform.Translate(0, 0, 0); //moves no where
                break;
            case 1:
                transform.Translate(0, 0, speed * Time.deltaTime ); //moves foward at a certain speed per second
                break;
            case 2:
                transform.Translate(0, 0, -(speed * Time.deltaTime)); //moves backward at a certain speed per second
                break;
            case 3: 
                transform.Translate(-(speed * Time.deltaTime), 0, 0); //moves left at a certain speed per second
                break;
            case 4:
                transform.Translate(speed * Time.deltaTime, 0, 0); //moves right at a certain speed per second
                break;
            case 5:
                transform.Translate(speed * Time.deltaTime, 0, speed * Time.deltaTime); //moves diagonal right(up) at a certain speed per second
                break;
            case 6:
                transform.Translate(speed * Time.deltaTime, 0, -(speed * Time.deltaTime)); //moves diagonal right(down) at a certain speed per second
                break;
            case 7:
                transform.Translate(-(speed * Time.deltaTime), 0, speed * Time.deltaTime); //moves left right(up) at a certain speed per second
                break;
            case 8:
                transform.Translate(-(speed * Time.deltaTime), 0, -(speed * Time.deltaTime)); //moves diagonal left(down) at a certain speed per second
                break;
        }
    }


}
