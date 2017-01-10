using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : Controller
{
    public override float horizontalAxis()  { return Input.GetAxisRaw("Mouse X"); }
    public override float verticalAxis()    { return Input.GetAxisRaw("Mouse Y"); }
    public override bool actionButton()     { return Input.GetMouseButtonDown(0); }
}