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
    public float gamepadMaxSpeed;
    public int nbGamepadMax = 4;

    public Text m_startGameText;
    public Text m_chooseShooterText;
    public Text m_startChoosingShooterText;
    public Text m_errorNbPlayerText;
    public Text m_joinGameText;
    public GameObject m_redSniperTarget;

    private Substate m_substate;
    private bool m_hidePlayerSelectionPositionFree = true;
    private GamepadSelector[] m_gamepads;
    private GamepadSelector m_currentSelectedGamepad;

    private int m_nbActiveplayers;
    bool m_readyToPlay = false;

    // Use this for initialization
    public void initPlayerSelector()
    {
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

        m_nbActiveplayers = 0;
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
                if((m_nbActiveplayers >= 2) && (Input.GetButtonDown("Continue")))
                {
                    m_joinGameText.color = new Color(m_joinGameText.color.r,m_joinGameText.color.g,m_joinGameText.color.b,0.0f);
                    m_startChoosingShooterText.color = new Color(m_startChoosingShooterText.color.r,m_startChoosingShooterText.color.g,m_startChoosingShooterText.color.b,0.0f);
                    m_redSniperTarget.GetComponent<SpriteRenderer>().enabled = true;

                    return Substate.ShooterSelection;
                }

                else if(Input.GetKeyDown(KeyCode.Return))
                {
                    // Persistent data for keyboard
                    PlayerSelection_Persistent.nbHidePlayers = 1;
                    PlayerSelection_Persistent.keyboardControl = true;

                    return Substate.ReadyForStart;
                }
            break;

            case Substate.ShooterSelection:
                if((!m_hidePlayerSelectionPositionFree) 
                && ((m_currentSelectedGamepad != null) && (m_currentSelectedGamepad.m_substate == GamepadSelector.Substate.Static)))
                {
                    if(Input.GetButtonDown("Continue"))
                    {
                        PlayerSelection_Persistent.nbHidePlayers = m_nbActiveplayers;

                        PlayerSelection_Persistent.shooterGamepadID = m_currentSelectedGamepad.m_id;
                        PlayerSelection_Persistent.hiddenPlayerGamepadID = new string[m_nbActiveplayers - 1];

                        for(int i = 0; i < m_gamepads.Length; i++)
                        {
                            if(m_gamepads[i] == m_currentSelectedGamepad)
                                continue;

                            if(m_gamepads[i].m_active)
                                PlayerSelection_Persistent.hiddenPlayerGamepadID[i] = m_gamepads[i].m_id;
                        }

                        return Substate.ReadyForStart;
                    }
                }

                else if((m_currentSelectedGamepad == null)
                    && Input.GetButtonDown("Back"))
                {
                    m_joinGameText.color = new Color(m_joinGameText.color.r,m_joinGameText.color.g,m_joinGameText.color.b,1.0f);
                    m_startChoosingShooterText.color = new Color(m_startChoosingShooterText.color.r,m_startChoosingShooterText.color.g,m_startChoosingShooterText.color.b,1.0f);

                    m_chooseShooterText.color = new Color(m_chooseShooterText.color.r,m_chooseShooterText.color.g,m_chooseShooterText.color.b,0.0f);
                    m_redSniperTarget.GetComponent<SpriteRenderer>().enabled = false;

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
                            m_nbActiveplayers++;
                        }

                        else
                        {
                            m_gamepads[i].m_active = false;
                            m_gamepads[i].gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,0.3f);
                            m_nbActiveplayers--;
                        }
                    }
                }

                if(m_nbActiveplayers < 2)
                {
                    m_errorNbPlayerText.color = new Color(m_errorNbPlayerText.color.r,m_errorNbPlayerText.color.g,m_errorNbPlayerText.color.b,1.0f);
                    m_startChoosingShooterText.color = new Color(m_startChoosingShooterText.color.r,m_startChoosingShooterText.color.g,m_startChoosingShooterText.color.b,0.0f);
                }
                else
                {
                    m_errorNbPlayerText.color = new Color(m_errorNbPlayerText.color.r,m_errorNbPlayerText.color.g,m_errorNbPlayerText.color.b,0.0f);
                    m_startChoosingShooterText.color = new Color(m_startChoosingShooterText.color.r,m_startChoosingShooterText.color.g,m_startChoosingShooterText.color.b,1.0f);
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
                                m_startGameText.color = new Color(.0f,.0f,.0f,1.0f);
                            }
                        }

                        else
                        {
                            if((m_currentSelectedGamepad == m_gamepads[i])
                            && (speedY < -0.20))
                            {
                                m_gamepads[i].m_substate = GamepadSelector.Substate.Moving;
                                m_gamepads[i].m_wantedPosition = m_gamepads[i].m_initialPosition;
                                m_startGameText.color = new Color(.0f,.0f,.0f,.0f);

                                m_hidePlayerSelectionPositionFree = true;
                                m_currentSelectedGamepad = null;
                            }
                        }
                    }

                    m_gamepads[i].update(Time.deltaTime);
                }

                if(m_hidePlayerSelectionPositionFree)
                {
                    m_chooseShooterText.color = new Color(m_chooseShooterText.color.r,m_chooseShooterText.color.g,m_chooseShooterText.color.b,1.0f);
                    m_startGameText.color = new Color(m_startGameText.color.r,m_startGameText.color.g,m_startGameText.color.b,0.0f);
                }
                else
                {
                    m_chooseShooterText.color = new Color(m_chooseShooterText.color.r,m_chooseShooterText.color.g,m_chooseShooterText.color.b,0.0f);
                    m_startGameText.color = new Color(m_startGameText.color.r,m_startGameText.color.g,m_startGameText.color.b,1.0f);
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