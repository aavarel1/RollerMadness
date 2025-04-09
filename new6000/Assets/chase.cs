using UnityEngine;

public class chase : MonoBehaviour
{
    public float speed = 10f; 
    public float minDistance = 1f;
    public Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 12f);
        if (target == null) {
            if (GameObject.FindWithTag("Player") != null) {
                target = GameObject.FindWithTag("Player").GetComponent<Transform>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) {
            Debug.Log("no target");
            return;
        }
        transform.LookAt(target); 
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > minDistance) {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
