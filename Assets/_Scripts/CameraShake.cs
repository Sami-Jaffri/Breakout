using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    public void ShakeCamera(float intensity, float duration)
    {
        transform.DOShakePosition(duration, intensity)
            .OnComplete(() => transform.position = originalPosition);
    }
}
