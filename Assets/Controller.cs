using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    public int gamepadId;

    public float horizontalAxis()   { return Input.GetAxis("LeftJoystickX_P" + gamepadId.ToString());   }
    public float verticalAxis()     { return Input.GetAxis("Vertical_J" + gamepadId.ToString());        }
    public bool actionButton()      { return Input.GetButtonDown("A_P" + gamepadId.ToString());         }
}