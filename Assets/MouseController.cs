using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : Controller
{
    //public override float horizontalAxis() { return Input.GetAxis("Mouse X"); }
    //public override float verticalAxis() { return Input.GetAxis("Mouse Y"); }

    public override float horizontalAxis() { return Input.mousePosition.x - (Screen.width / 2.0f); }
    public override float verticalAxis() { return Input.mousePosition.y - (Screen.height / 2.0f); }
    public override bool actionButton()     { return Input.GetMouseButtonDown(0); }
}