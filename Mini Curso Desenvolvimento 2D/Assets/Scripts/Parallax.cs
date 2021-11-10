using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float lenght, startPos;
    private Transform cam;
    private Vector3 camPos;
    [SerializeField] private float parallax;

    void Start(){
        cam = Camera.main.transform;
        camPos = cam.position;
    }

    void LateUpdate(){
        float parallaxX = camPos.x - cam.position.x;
        float rePos = transform.position.x + parallaxX;

        Vector3 newPos = new Vector3(rePos, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPos, parallax * Time.deltaTime);

        camPos = cam.position;
    }
}
