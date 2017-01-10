using UnityEngine;
using System.Collections;

public class PlayerSelection_Persistent : MonoBehaviour
{
    public static int nbHidePlayers             = 0;
    public static bool keyboardControl;

    public static string CursorPlayer_ID    = "";
    public static string [] HidePlayers_ID;

    void Awake()
    {
        // Do not destroy this game object
        DontDestroyOnLoad(this);
    }
}
