using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour
{
    Sprite m_target;
    GameObject m_cursor;
    GameObject m_canvas;
    public float m_sensibility = 50.0f;

	// Use this for initialization
	void Start ()
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
    void Update ()
    {
        Vector2 speed = Vector2.zero;

        speed += (Input.GetKey(KeyCode.UpArrow)) ? new Vector2(0, 1) : Vector2.zero;
        speed += (Input.GetKey(KeyCode.DownArrow)) ? new Vector2(0, -1) : Vector2.zero;
        speed += (Input.GetKey(KeyCode.LeftArrow)) ? new Vector2(-1, 0) : Vector2.zero;
        speed += (Input.GetKey(KeyCode.RightArrow)) ? new Vector2(1, 0) : Vector2.zero;

        Vector3 newPosition = m_cursor.transform.localPosition + new Vector3(speed.x, speed.y, 0.0f) * m_sensibility * Time.deltaTime;
        clampCursorOnCanvas(ref newPosition);
        m_cursor.transform.localPosition = newPosition;


        if(Input.GetKey(KeyCode.Space))
        {
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
