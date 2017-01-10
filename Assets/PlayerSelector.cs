using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerSelector :MonoBehaviour
{
    enum Substate
    {
        NbPlayerSetup,
        ShooterSelection,
        ReadyForStart
    };

    public Vector3 CursorPlayerSelectionPosition;
    public Text textToStart;
    public float gamepadMaxSpeed;
    public int nbGamepadMax = 4;

    private Substate m_substate;
    private bool m_hidePlayerSelectionPositionFree = true;
    private GamepadSelector[] m_gamepads;
    private GamepadSelector m_currentSelectedGamepad;

    private int m_activeplayers;
    bool m_readyToPlay = false;

    // Use this for initialization
    public void initPlayerSelector()
    {
        textToStart = GameObject.Find("StartGameText").GetComponent<Text>();
        m_gamepads = new GamepadSelector[nbGamepadMax];

        for(int i = 0; i < nbGamepadMax; i++)
        {
            GameObject gamepadSprite = GameObject.Find("XboxOne_GamePad_" + (i + 1).ToString());
            gamepadSprite.GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,0.3f);

            m_gamepads[i] = gamepadSprite.AddComponent<GamepadSelector>();
            m_gamepads[i].m_active = false;
            m_gamepads[i].m_id = (i + 1).ToString();
            m_gamepads[i].m_maxSpeed = gamepadMaxSpeed;
            m_gamepads[i].m_initialPosition = gamepadSprite.transform.position;
        }

        m_activeplayers = 0;
    }

    // Update is called once per frame
    public void updatePlayerSelector()
    {
        m_substate = checkSubstateChange();
        updateSubstate();
    }

    Substate checkSubstateChange()
    {
        switch(m_substate)
        {
            case Substate.NbPlayerSetup:
            if((m_activeplayers >= 2) && (Input.GetButtonDown("Continue")))
                return Substate.ShooterSelection;

            else if(Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                // Persistent data for keyboard
                return Substate.ReadyForStart;
            }
            break;

            case Substate.ShooterSelection:
            if((!m_hidePlayerSelectionPositionFree) && (m_currentSelectedGamepad.m_substate == GamepadSelector.Substate.Static))
            {
                if(Input.GetButtonDown("Continue"))
                {
                    PlayerSelection_Persistent.CursorPlayer_ID = m_currentSelectedGamepad.m_id;

                    PlayerSelection_Persistent.HidePlayers = new string[m_gamepads.Length - 1];
                    for(int i = 0; i < m_gamepads.Length; i++)
                    {
                        if(m_gamepads[i] == m_currentSelectedGamepad)
                            continue;
                        PlayerSelection_Persistent.HidePlayers[i] = m_gamepads[i].m_id;
                    }

                    return Substate.ReadyForStart;
                }
            }

            else if((m_currentSelectedGamepad == null)
                && Input.GetButtonDown("Back"))
            {
                return Substate.NbPlayerSetup;
            }
            break;
        }

        return m_substate;
    }

    void updateSubstate()
    {
        switch(m_substate)
        {
            case Substate.NbPlayerSetup:
            for(int i = 0; i < m_gamepads.Length; i++)
            {
                bool activeButton = Input.GetButtonDown("A_P" + (i + 1));
                if(activeButton)
                {
                    if(m_gamepads[i].m_active == false)
                    {
                        m_gamepads[i].m_active = true;
                        m_gamepads[i].gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,1.0f);
                        m_activeplayers++;
                    }

                    else
                    {
                        m_gamepads[i].m_active = false;
                        m_gamepads[i].gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,0.3f);
                        m_activeplayers--;
                    }
                }
            }
            break;

            case Substate.ShooterSelection:
            for(int i = 0; i < m_gamepads.Length; i++)
            {
                if(m_gamepads[i].m_active == true)
                {
                    string currentGamepad = "LeftJoystickY_P" + m_gamepads[i].m_id;
                    float speedY = Input.GetAxis(currentGamepad);

                    if(m_hidePlayerSelectionPositionFree)
                    {
                        if(speedY > 0.20)
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
                        && (speedY < -0.20))
                        {
                            m_gamepads[i].m_substate = GamepadSelector.Substate.Moving;
                            m_gamepads[i].m_wantedPosition = m_gamepads[i].m_initialPosition;
                            textToStart.color = new Color(.0f,.0f,.0f,.0f);
                        }
                    }
                }

                m_gamepads[i].update(Time.deltaTime);

                if(m_gamepads[i] == m_currentSelectedGamepad)
                {
                    // BERK
                    if((m_gamepads[i].m_wantedPosition == m_gamepads[i].m_initialPosition)
                    && (m_gamepads[i].m_substate == GamepadSelector.Substate.Static))
                    {
                        m_hidePlayerSelectionPositionFree = false;
                        m_currentSelectedGamepad = null;
                    }
                }
            }
            break;
        }
    }

    // Test if the player selection is done
    public bool isReady()
    {
        return (m_substate == Substate.ReadyForStart);
    }
}