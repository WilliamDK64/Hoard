using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Dragon : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 360.0f;
    [SerializeField] private float movementSpeed = 10.0f;

    private void Update()
    {
        // MOVEMENT
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        Quaternion qTo = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, rotateSpeed * Time.deltaTime);

        //transform.position += transform.right * movementSpeed * Time.deltaTime;
        float theta = transform.localEulerAngles.z + 90;

        float newDirX = Mathf.Cos(theta * Mathf.Deg2Rad);

        float newDirY = Mathf.Sin(theta * Mathf.Deg2Rad);

        GetComponent<Rigidbody2D>().velocity = new Vector2(newDirX, newDirY) * -movementSpeed;

        // COMBAT

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Prey")
        {
            Destroy(collision.gameObject);
        }
    }
}
