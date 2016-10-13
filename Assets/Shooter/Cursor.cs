using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour
{
    public Sprite m_target;
    public GameObject m_cursor;

	// Use this for initialization
	void Start ()
    {
        m_target = Resources.Load<Sprite>("Sprites/Targets/redSniperTarget");

        m_cursor = new GameObject("Cursor");
        SpriteRenderer renderer = m_cursor.AddComponent<SpriteRenderer>();
        renderer.sprite = m_target;

        m_cursor.transform.position = new Vector3(0, 150, 0);
        m_cursor.transform.localScale = new Vector3(3, 3, 3);
        m_cursor.transform.rotation = Camera.main.transform.rotation;
    }

    // Update is called once per frame
    void Update ()
    {
        Vector2 speed = Vector2.zero;

        speed += (Input.GetKey(KeyCode.UpArrow)) ? new Vector2(0, 1) : Vector2.zero;
        speed += (Input.GetKey(KeyCode.DownArrow)) ? new Vector2(0, -1) : Vector2.zero;
        speed += (Input.GetKey(KeyCode.LeftArrow)) ? new Vector2(-1, 0) : Vector2.zero;
        speed += (Input.GetKey(KeyCode.RightArrow)) ? new Vector2(1, 0) : Vector2.zero;

        m_cursor.transform.position = m_cursor.transform.position + new Vector3(speed.x,0,speed.y) * 30 * Time.deltaTime;


        if(Input.GetKey(KeyCode.Keypad0))
        {
            Ray ray = new Ray(Camera.main.transform.position,(m_cursor.transform.position - Camera.main.transform.position).normalized);
            RaycastHit hit;

            float rayLenght = 100000;
            Debug.DrawLine(ray.origin, ray.direction * rayLenght);

            if(Physics.Raycast(ray, out hit, rayLenght))
            {
                if(hit.collider.GetComponent<Agent>())
                {
                    if (hit.collider.GetComponent<PlayerHide>())
                        print("Hit Player!");
                    else
                        print("Hit Agent, Try Again");
                }
            }
        }
    }
}
