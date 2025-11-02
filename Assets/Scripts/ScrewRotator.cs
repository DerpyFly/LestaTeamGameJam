using UnityEngine;

public class ScrewRotator : MonoBehaviour
{
    public enum RotationAxis { X, Y, Z }

    [Header("Rotation Settings")]
    public RotationAxis rotationAxis = RotationAxis.Z;
    public float rotationSpeed = 90f; // градусов в секунду
    public bool clockwise = true;

    [Header("State Control")]
    public bool isRotating = true;

 

    void Update()
    {
        if (isRotating)
        {
            RotateObject();
        }
    }

    void RotateObject()
    {
        float direction = clockwise ? 1f : -1f;
        float rotationAmount = rotationSpeed * direction * Time.deltaTime;

        switch (rotationAxis)
        {
            case RotationAxis.X:
                transform.Rotate(rotationAmount, 0, 0, Space.Self);
                break;
            case RotationAxis.Y:
                transform.Rotate(0, rotationAmount, 0, Space.Self);
                break;
            case RotationAxis.Z:
                transform.Rotate(0, 0, rotationAmount, Space.Self);
                break;
        }
    }

    // Публичные методы для управления из других скриптов
    public void StartRotation()
    {
        isRotating = true;
    }

    public void StopRotation()
    {
        isRotating = false;
    }

    public void ToggleRotation()
    {
        isRotating = !isRotating;
    }

    public void ReverseDirection()
    {
        clockwise = !clockwise;
    }

    public void SetRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
    }

    public void SetRotationAxis(RotationAxis newAxis)
    {
        rotationAxis = newAxis;
    }
}