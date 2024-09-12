using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public List<GameObject> points = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < points.Count; i++)
        {
            if (i == points.Count - 1)
            {
                Gizmos.DrawLine(points[i].transform.position, points[0].transform.position);
            }
            else
            {
                Gizmos.DrawLine(points[i].transform.position, points[i + 1].transform.position);
            }
        }
    }
}
