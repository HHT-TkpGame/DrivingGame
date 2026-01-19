using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    ICheckPointReceiver receiver;
    MeshRenderer mr;
    int index;
    public void Initialize(
        int index,
        float radius,
        float height,
        ICheckPointReceiver receiver
    )
    {
        mr = GetComponent<MeshRenderer>();
        this.index = index;
        transform.localScale = new Vector3(radius/2, height/2, radius/2);
        Vector3 pos = transform.position;
        pos.y += transform.localScale.y;
        transform.position = pos;
        this.receiver = receiver;
        SetVisible(false);
    }
    public void SetVisible(bool visible)
    {
        mr.enabled = visible;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other == null) { return; }
        if (!other.gameObject.CompareTag("CheckCollider")) { return; }
        receiver.OnCheckPointPassed(index);
    }
}
