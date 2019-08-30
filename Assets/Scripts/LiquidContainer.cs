using UnityEngine;

public class LiquidContainer : MonoBehaviour
{
    [SerializeField] Transform liquidParent;
    [SerializeField] Transform liquidMesh;

    [SerializeField] float sloshSpeed = 60;
    [SerializeField] float rotateSpeed = 15;

    [SerializeField] float difference = 25;

    private void Update()
    {
        Slosh();

        liquidMesh.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.Self);
    }

    private void Slosh()
    {
        var inverseRotation = Quaternion.Inverse(transform.rotation);

        var finalRotation = Quaternion.RotateTowards(liquidParent.localRotation, inverseRotation, sloshSpeed * Time.deltaTime).eulerAngles;

        finalRotation.x = ClampRotationValue(finalRotation.x, difference);
        finalRotation.z = ClampRotationValue(finalRotation.z, difference);

        liquidParent.localEulerAngles = finalRotation;
    }

    private float ClampRotationValue(float value, float difference)
    {
        if (value > 180)
        {
            return Mathf.Clamp(value, 360 - difference, 360);
        }

        return Mathf.Clamp(value, 0, difference);
    }
    
}
