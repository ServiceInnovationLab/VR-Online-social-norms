using UnityEngine;

public class Floatable : MonoBehaviour
{
    public float floatHeight = 1.0f;
    public Vector3 buoyancyCentreOffset;
    public float bounceDamp = 0.05f;

    bool onWater = false;
    float waterLevel;
    new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Water"))
            return;

        onWater = true;
        waterLevel = other.bounds.max.y;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Water"))
            return;

        onWater = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnTriggerEnter(collision.collider);
    }

    private void OnCollisionExit(Collision collision)
    {
        OnTriggerExit(collision.collider);
    }

    void FixedUpdate()
    {
        if (!onWater)
            return;

        Vector3 actionPoint = transform.position + transform.TransformDirection(buoyancyCentreOffset);
        float forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);

        if (forceFactor > 0f)
        {
            Vector3 uplift = -Physics.gravity * (forceFactor - rigidbody.velocity.y * bounceDamp);
            rigidbody.AddForceAtPosition(uplift, actionPoint);
        }
    }

}
