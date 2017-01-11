using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour
{
    Sprite m_target;
    GameObject m_cursor;
    GameObject m_canvas;
    float m_sensibility = 20.0f;
    public Controller m_controller;

    public int m_nbShoot;

    public int m_shootCooldown = 2;
    private float m_lastShootTime;

	// Use this for initialization
	void Start()
    {
        m_canvas = GameObject.FindGameObjectWithTag("Canvas");
        m_target = Resources.Load<Sprite>("Sprites/Targets/redSniperTarget");
        m_cursor = new GameObject("Cursor");

        SpriteRenderer renderer = m_cursor.AddComponent<SpriteRenderer>();
        renderer.sprite = m_target;

        m_cursor.transform.localScale = new Vector3(2,2,2);
        m_cursor.transform.rotation = Camera.main.transform.rotation;

        m_cursor.transform.SetParent(m_canvas.transform);
        m_cursor.transform.localPosition = new Vector3(0,0,0);


        if(PlayerSelection_Persistent.keyboardControl)
            m_controller = new MouseController();
        else
        {
            GamepadController gamepadController = new GamepadController(PlayerSelection_Persistent.shooterGamepadID);
            m_controller = gamepadController;
        }
    }

    void clampCursorOnCanvas(ref Vector3 position)
    {
        if(position.x < -(Screen.width/2))
            position.x = -(Screen.width/2);
        else if(position.x > (Screen.width/2))
            position.x = (Screen.width/2);

        if(position.y < -(Screen.height/2))
            position.y = -(Screen.height/2);
        else if(position.y > (Screen.height/2))
            position.y = (Screen.height/2);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition= new Vector3(m_controller.horizontalAxis(), m_controller.verticalAxis(), 0.0f);
        clampCursorOnCanvas(ref newPosition);
        m_cursor.transform.localPosition = newPosition;

        if(m_controller.actionButton() && ((Time.time- m_lastShootTime)>m_shootCooldown) && (m_nbShoot > 0))
        {
            m_lastShootTime = Time.time;
            m_nbShoot--;

            Ray ray = new Ray(Camera.main.transform.position,(m_cursor.transform.position - Camera.main.transform.position).normalized);
            RaycastHit hit;

            float rayLenght = 100000;
            Debug.DrawLine(ray.origin, ray.direction * rayLenght);

            if(Physics.Raycast(ray, out hit, rayLenght))
            {
                if(hit.collider.GetComponent<Agent>())
                {
                    if (hit.collider.tag == "PlayerHide")
                        print("Hit Player!");
                    else
                        print("Hit Agent, Try Again");
                }
            }
        }
    }
}
