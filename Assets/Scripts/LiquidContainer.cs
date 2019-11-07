using UnityEngine;

public class LiquidContainer : MonoBehaviour
{
    LayerMask fillMask;
    [SerializeField] Transform liquidParent;
    [SerializeField] Transform liquidMesh;
    [SerializeField] GameObject particleEffect;

    [SerializeField] float sloshSpeed = 60;
    [SerializeField] float rotateSpeed = 15;
    [SerializeField] float difference = 25;
    [SerializeField] float tipAngle = 90;

    [SerializeField, Range(0, 1)] float liquidLevel;
    [SerializeField] float maxHeight;
    [SerializeField] float minHeight;

    [SerializeField] float groundLevel = 0;

    [SerializeField] float fillRate = 1.0f;
    [SerializeField] float loseRate = 0.01f;
    [SerializeField] float giveRate = 0.01f;

    [SerializeField] AudioClip fillingAudio;
    [SerializeField] AudioClip drainingAudio;

    AudioSource audioSource;

    float checkRadius;

    private void Awake()
    {
        fillMask = LayerMask.GetMask("LiquidContainers");

        audioSource = GetComponent<AudioSource>();

        var rend = GetComponent<Renderer>();
        if (rend)
        {
            checkRadius = Mathf.Max(rend.bounds.extents.x, rend.bounds.extents.z) * 2.0f;
        }
        else
        {
            checkRadius = 0.2f;
        }
    }

    private void FixedUpdate()
    {
        bool active = liquidLevel > 0.01;

        if (!active)
            return;

        float upAngle = Vector3.Angle(Vector3.up, transform.up);
       // float otherAngle = 90 - Vector3.Angle(Vector3.up, transform.forward);

        if (upAngle >= tipAngle)
        {
            liquidLevel -= loseRate;

            RaycastHit hit;

            if (Physics.SphereCast(transform.position, checkRadius, -Vector3.up, out hit, 30, fillMask))
            {
                var liquid = hit.collider.GetComponent<LiquidContainer>();
                if (liquid)
                {
                    liquid.Fill(giveRate);

                    if (liquid.audioSource && !liquid.audioSource.isPlaying && liquid.fillingAudio)
                    {
                        liquid.audioSource.clip = liquid.fillingAudio;
                        liquid.audioSource.Play();
                    }
                }
            }
            else if (audioSource && !audioSource.isPlaying && drainingAudio)
            {
                audioSource.clip = drainingAudio;
                audioSource.Play();
            }
        }
    }

    private void Update()
    {
        if (!liquidParent)
            return;

        bool active = liquidLevel > 0.01;
        if (particleEffect)
        {
            particleEffect.SetActive(active);
        }

        liquidParent.gameObject.SetActive(active);

        if (!active)
            return;

        Slosh();

        liquidMesh.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.Self);

        float height = minHeight + (maxHeight - minHeight) * liquidLevel;
        liquidParent.localPosition = new Vector3(liquidParent.localPosition.x, height, liquidParent.localPosition.z);

        if (particleEffect)
        {
            particleEffect.transform.localPosition = new Vector3(particleEffect.transform.localPosition.x, height, particleEffect.transform.localPosition.z);
        }
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

    public void Fill(float amount)
    {
        liquidLevel += amount * fillRate;
        liquidLevel = Mathf.Clamp(liquidLevel, 0, 1);
    }
    
}
