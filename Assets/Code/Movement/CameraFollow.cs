using UnityEngine;

public class CamereFollow : MonoBehaviour
{
    public float followSpeed;
    public Transform followTarget;
    public float YOffset;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(followTarget.position.x, followTarget.position.y + YOffset, -50f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }
}
