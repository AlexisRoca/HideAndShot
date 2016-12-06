using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerSelector : MonoBehaviour
{
    enum Substate
    {
        Static,
        Moving  
    };

    public Vector3 CursorPlayerSelectionPosition;
    public Text textToStart;
    public float gamepadMaxSpeed;

    private bool m_hidePlayerSelectionPositionFree = true;
    private GamepadSelector [] m_gamepads;
    private GamepadSelector m_currentSelectedGamepad;

    // Use this for initialization
    void Start()
    {
        textToStart = GameObject.Find("StartGameText").GetComponent<Text>();

        int gamepadNb = Input.GetJoystickNames().Length;
        m_gamepads = new GamepadSelector[gamepadNb];

        for(int i=0; i<4; i++)
        {
            GameObject gamepadSprite = GameObject.Find("XboxOne_GamePad_" + (i + 1).ToString());

            if(i<gamepadNb)
            {
                m_gamepads[i] = gamepadSprite. AddComponent<GamepadSelector>();
                m_gamepads[i].m_id = (i+1).ToString();
                m_gamepads[i].m_initialPosition = gamepadSprite.transform.position;
                m_gamepads[i].m_maxSpeed = gamepadMaxSpeed;
            }

            else
            {
                gamepadSprite.GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,0.3f);
            }
        }
	}
	
	// Update is called once per frame
	void Update()
    {
        for(int i=0; i< m_gamepads.Length; i++)
        {
            string currentGamepad = "LeftJoystickY_P" + m_gamepads[i].m_id;
            float speedY = Input.GetAxis(currentGamepad);

            if(m_hidePlayerSelectionPositionFree)
            {
                if(speedY > 0)
                {
                    m_gamepads[i].m_substate = GamepadSelector.Substate.Moving;
                    m_gamepads[i].m_wantedPosition = CursorPlayerSelectionPosition;
                    m_currentSelectedGamepad = m_gamepads[i];

                    m_hidePlayerSelectionPositionFree = false;
                    textToStart.color = new Color(.0f,.0f,.0f,1.0f);
                }
            }

            else
            {
                if((m_currentSelectedGamepad == m_gamepads[i])
                && (speedY < 0))
                {
                    m_gamepads[i].m_substate = GamepadSelector.Substate.Moving;
                    m_gamepads[i].m_wantedPosition = m_gamepads[i].m_initialPosition;
                    m_currentSelectedGamepad = null;
                    m_hidePlayerSelectionPositionFree = true;
                    textToStart.color = new Color(.0f,.0f,.0f,.0f);
                }
            }

            m_gamepads[i].update(Time.deltaTime);
        }

        if(!m_hidePlayerSelectionPositionFree)
        {
            if(m_currentSelectedGamepad.m_substate == GamepadSelector.Substate.Static)
            {
                if(Input.GetButtonDown("A_P" + m_currentSelectedGamepad.m_id))
                {
                    PlayerSelection_Persistent.CursorPlayer_ID = m_currentSelectedGamepad.m_id;

                    PlayerSelection_Persistent.HidePlayers = new string[m_gamepads.Length - 1];
                    for(int i=0; i<m_gamepads.Length; i++)
                    {
                        if(m_gamepads[i] == m_currentSelectedGamepad)
                            continue;
                        PlayerSelection_Persistent.HidePlayers[i] = m_gamepads[i].m_id;
                    }

                    SceneManager.LoadScene("Scene_1");
                }
            }
        }
    }
}