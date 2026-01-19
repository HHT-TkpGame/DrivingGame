using System.Collections.Generic;
using UnityEngine;

public class CheckPointGenerator : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] WayPoint[] wayPoints;
    [SerializeField] float colliderHeight;
    ICheckPointReceiver receiver;

    private void Start()
    {
        Generate();
    }
    public void Initialize(ICheckPointReceiver receiver)
    {
        this.receiver = receiver;
    }
    public CheckPoint[] Generate()
    {
        List<CheckPoint> spawned = new List<CheckPoint>();
        
        for(int i = 0; i < wayPoints.Length; i++)
        {
            GameObject o = Instantiate(
                prefab,
                wayPoints[i].transform.position,
                Quaternion.identity
            );
            CheckPoint c = o.GetComponent<CheckPoint>();
            c.Initialize(i, wayPoints[i].Radius, colliderHeight, receiver);
            spawned.Add(c);
        }
        return spawned.ToArray();
    }
}
