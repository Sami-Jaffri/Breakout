using System.Collections;
using UnityEngine;
using _Scripts.Utils;

public class Brick : MonoBehaviour
{
    private Coroutine destroyRoutine = null;
    private HotSwapColor hotSwapColor;
    private Color originalColor;

    private void Start()
    {
        hotSwapColor = GetComponent<HotSwapColor>();
        if (hotSwapColor != null)
        {
            originalColor = hotSwapColor.color; 
        }
        else
        {
            Debug.LogWarning("Missing Colour");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (destroyRoutine != null) return;
        if (!other.gameObject.CompareTag("Ball")) return;
        destroyRoutine = StartCoroutine(DestroyWithDelay());
    }

    /* private IEnumerator DestroyWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.OnBrickDestroyed(transform.position);
        Destroy(gameObject);
    } */

    public IEnumerator DestroyWithDelay()
    {
        float duration = 1.5f; 
        float flashInterval = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            hotSwapColor.SetColor(Color.white);
            yield return new WaitForSeconds(flashInterval);

            hotSwapColor.SetColor(originalColor); 
            yield return new WaitForSeconds(flashInterval);

            elapsedTime += flashInterval * 2;
        }

        GameManager.Instance.OnBrickDestroyed(transform.position);
        Destroy(gameObject);
    }
}
