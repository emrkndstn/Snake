using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class SnakeController : MonoBehaviour
{

    public SplineComputer spline;
    public List<GameObject> BodyParts;
    public float speed;
    public LayerMask groundLayer;
    public GameObject targetBox;
    
    [HideInInspector] public GameObject head;
    [HideInInspector] public List<float> splineSizes = new List<float>();
    private Vector3 movePoint;

    private void Start()
    {
        head = BodyParts[0];
        movePoint = head.transform.position;
        ListSizes();
    }

    private void Update()
    {
        if(!GameManager.Instance.finished) Movement();
        SetSplinePos();
    }

    private void FixedUpdate()
    {
        if (head.GetComponent<Rigidbody>().velocity.magnitude > 2f) BodyFollow();
    }

    private void ListSizes()
    {
        for (int i = 0; i < BodyParts.Count-1; i++)
        {
            splineSizes.Add(spline.GetPointSize(i));
        }
    }
    
    private void Movement()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                movePoint = hit.point;
                targetBox.transform.position = movePoint;
            }
        }
        //Yılanın başı gidecegi konumda degilse
        if (Vector3.Distance(head.transform.position, new Vector3(movePoint.x, head.transform.position.y, movePoint.z)) > .5f)
        {
            head.GetComponent<Rigidbody>().isKinematic = false;
            LookTarget(head.transform, movePoint);
            Vector3 velocityTarget = head.transform.forward * speed;
            
            head.GetComponent<Rigidbody>().velocity = 
                new Vector3(velocityTarget.x, head.GetComponent<Rigidbody>().velocity.y, velocityTarget.z);
        }
        else
        {
            head.GetComponent<Rigidbody>().isKinematic = true;
        }

    }
    private void LookTarget(Transform part, Vector3 target)
    {
        Vector3 dir = target - part.transform.position;
        Quaternion rot = Quaternion.LookRotation(dir);
        part.transform.rotation = Quaternion.Lerp(part.transform.rotation, rot, 100 * Time.deltaTime);
    }

    private void BodyFollow()
    {
        for (int i = 1; i < BodyParts.Count; i++)
        {
            float followSpeed = head.GetComponent<Rigidbody>().velocity.magnitude;
            
            BodyParts[i].transform.position = Vector3.Lerp(BodyParts[i].transform.position,
                BodyParts[i - 1].GetComponent<SnakeBody>().lastPos, followSpeed*1.5f * Time.deltaTime);
            
            LookTarget(BodyParts[i].transform, BodyParts[i - 1].GetComponent<SnakeBody>().lastPos);
        }
    }

    private void SetSplinePos()
    {
        for (int i = 0; i < BodyParts.Count; i++)
        {
            spline.SetPointPosition(i,BodyParts[i].transform.position);
        }
    }
    
    
    
    
}
