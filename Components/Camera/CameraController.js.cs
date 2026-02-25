using UnityEngine;

[AddComponentMenu("ComponentsMythic/Controllers/Camera")]
public class CameraController : Controller
{
    public float yOffset = 2f;
    public float xRotation = 20f;
    public float yRotation = 0f;
    public float distance = 5f;

    public GameObject target;
    public Vector3 focus; // this one will now actually update

    void LateUpdate()
    {
        if (target == null) return;

        // Update the field, not a new local variable
        focus = target.transform.position + Vector3.up * yOffset;

        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);

        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        transform.position = focus + offset;
        transform.LookAt(focus);
    }
}