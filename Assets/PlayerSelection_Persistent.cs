using UnityEngine;
using System.Collections;

public class PlayerSelection_Persistent : MonoBehaviour
{
    public static int nbHidePlayers             = 0;
    public static bool keyboardControl;

    public static string shooterGamepadID    = "";
    public static string [] hiddenPlayerGamepadID;

    void Awake()
    {
        // Do not destroy this game object
        DontDestroyOnLoad(this);
    }
}
