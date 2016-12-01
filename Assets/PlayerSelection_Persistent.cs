using UnityEngine;
using System.Collections;

public class PlayerSelection_Persistent : MonoBehaviour
{
    public static int CursorPlayer_ID = -1;
    public static int HidePlayer1_ID = -1;
    public static int HidePlayer2_ID = -1;
    public static int HidePlayer3_ID = -1;

    void Awake()
    {
        // Do not destroy this game object
        DontDestroyOnLoad(this);
    }
}
