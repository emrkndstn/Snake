using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    public bool head;
    public List<Vector3> posList = new List<Vector3>();
    public Vector3 lastPos;
    private void Update()
    {
        posList.Add(transform.position);
        if (posList.Count > 3)
        {
            lastPos = posList[0];
            posList.RemoveAt(0);
        }
    }
    
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Coin") && other.gameObject.GetComponent<Coins>().canCollect && head)
        {
            GameManager.Instance.coinScale();
            other.gameObject.GetComponent<Coins>().SnakeHit();
            GameManager.Instance.score += 25;
        }
    }
}
