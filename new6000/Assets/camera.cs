using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.InputSystem;
using TMPro; 

public class cameraControl : MonoBehaviour
{
    public Transform target; 
    public float XDistance = 5f; 
    public float YDistance = 10f; 
    public float ZDistance = 20f;


    void Start()
    {
    }
    
    void Update()
    {
        Vector3 v1 = new Vector3(XDistance, 0, ZDistance);
        Vector3 wantedPosition = target.position - v1;
        wantedPosition.y = YDistance; 
        transform.position = wantedPosition; 
        transform.LookAt(target);
         GetComponent<AudioSource>().Play();
        
    }
}

