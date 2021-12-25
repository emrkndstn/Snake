using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapParts : MonoBehaviour
{
    public SnakeController _snakeController;

    private void Update()
    {
        MapPartsFollowBody();
    }

    private void MapPartsFollowBody()
    {
        List<GameObject> bodyParts = _snakeController.BodyParts;
        for (int i = 0; i < bodyParts.Count; i++)
        {
            transform.GetChild(i).position = new Vector3(bodyParts[i].transform.position.x,
                transform.position.y,
                bodyParts[i].transform.position.z
            );
        }
    }
    
    
}
