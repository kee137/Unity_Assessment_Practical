using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 15f;
    CharacterController cc;

    void Awake() { cc = GetComponent<CharacterController>(); }

    void Update()
    {
        var d = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        cc.SimpleMove(d * speed);
        if (d != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(d), turnSpeed * Time.deltaTime);
    }
}