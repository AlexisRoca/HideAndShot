using UnityEngine;
using System.Collections;

public class PlayerSelection_Persistent : MonoBehaviour
{
    public static int nbPlayers             = 0;
    public static string CursorPlayer_ID    = "";
    public static string [] HidePlayers;

    void Awake()
    {
        // Do not destroy this game object
        DontDestroyOnLoad(this);
    }
}
