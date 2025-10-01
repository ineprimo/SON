using UnityEngine;

public class MovementBird : MonoBehaviour
{

    Transform m_Transform;
    Vector3 dir;
    float elapsedTime;
    float gir = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Transform = GetComponent<Transform>();
        dir = new Vector3(1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime > gir) {
            dir *= -1;
            elapsedTime = 0;
        }
        m_Transform.position += dir * Time.deltaTime * 10;
        elapsedTime++;
    }
}
