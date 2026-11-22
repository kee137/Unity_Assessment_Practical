using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 14, -14); // farther = bigger view
    public float smooth = 5f;
    public LayerMask wallMask;      // set to the "Wall" layer
    public float camRadius = 0.3f;

    void LateUpdate()
    {
        Vector3 pivot = target.position + Vector3.up;
        Vector3 desired = target.position + offset;

        // pull the camera in front of any wall between player and camera
        Vector3 dir = desired - pivot;
        if (Physics.SphereCast(pivot, camRadius, dir.normalized, out RaycastHit hit,
            dir.magnitude, wallMask, QueryTriggerInteraction.Ignore))
        {
            desired = pivot + dir.normalized * hit.distance;
        }

        transform.position = Vector3.Lerp(transform.position, desired, smooth * Time.deltaTime);
    }
}