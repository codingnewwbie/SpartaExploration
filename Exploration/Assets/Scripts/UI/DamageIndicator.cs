using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;
    public float flashSpeed;
    
    private Coroutine coroutine;
    
    void Start()
    {
        CharacterManager.Instance.Player.playerCondition.onTakeDamage += Flash;
    }

    public void Flash()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        
        image.enabled = true;
        image.color = new Color(255f / 255f, 87f / 255f, 85f / 255f);
        coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = image.color = new Color(255f, 87f / 255f, 85f / 255f, a);
            yield return null;
        }

        image.enabled = false;
    }
}

