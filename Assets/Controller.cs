using UnityEngine;
using System.Collections;

public class Controller
{
    public virtual float    horizontalAxis()    {return 0.0f;}
    public virtual float    verticalAxis()      {return 0.0f;}
    public virtual bool     actionButton()      {return false;}   
}