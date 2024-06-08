using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballMovement : MonoBehaviour
{
    [SerializeField] float initialVelocity;
    [SerializeField] float _Angle;
    [SerializeField] float step;
    [SerializeField] LineRenderer line;
    [SerializeField] Transform firePosition;

    private void Start()
    {
        line.enabled = true;
    }
    void Update()
    {
        float angle = _Angle * Mathf.Deg2Rad;
        DrawPath(initialVelocity, angle, step);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            StartCoroutine(Coroutine_Movement(initialVelocity, angle));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetBallPosition();
        }
        controls();
    }
    void ResetBallPosition()
    {
        line.enabled = true;
        transform.position = firePosition.position;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    private void DrawPath(float v0, float angle, float step)
    {
        step = Mathf.Max(0.01f, step);
        float totalTime = 10;
        int numPositions = (int)(totalTime / step) + 1;
        line.positionCount = numPositions;
        Vector3[] positions = new Vector3[numPositions];
        float t = 0f;
        for (int i = 0; i < numPositions; i++)
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - 0.5f * Physics.gravity.magnitude * Mathf.Pow(t, 2);
            positions[i] = firePosition.position + new Vector3(x, y, 0);
            t += step;
        }
        line.SetPositions(positions);
    }
    IEnumerator Coroutine_Movement(float v0, float angle)
    {
        yield return null;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        float x = v0 * Mathf.Cos(angle);
        float y = v0 * Mathf.Sin(angle);
        rb.velocity = new Vector3(x, y, 0);
    }
    void controls()
    {

        if (Input.GetMouseButton(0))
        {
            initialVelocity += 1 * 6 * Time.deltaTime;
        }

        if (initialVelocity > 20)
        {
            initialVelocity = 1;
        }

        if (Input.GetMouseButton(1))
        {
            _Angle += 1 * 20 * Time.deltaTime;
        }

        if (_Angle > 88)
        {
            _Angle = 1;
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("score"))
        {
            StopAllCoroutines();
            Debug.Log("Point");
            InstantiateNewBall();
        }

        if (collision.gameObject.CompareTag("Collidable"))
        {
            line.enabled = false;
        }
    }
    void InstantiateNewBall()
    {
        GameObject newBall = Instantiate(gameObject, firePosition.position, Quaternion.identity);
        Destroy(gameObject);
    }
}