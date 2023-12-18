using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoadAndObstacle : MonoBehaviour
{

    void Update()
    {
        transform.position -= new Vector3(0, 0, 1) * GameController.Instance.Speed * Time.deltaTime;
    }
}
