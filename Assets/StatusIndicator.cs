using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI timerText;
    public float duration;
    public float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
        timerText.text = $"{timer:F1}s";
        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Initialize(Sprite sprite, float duration)
    {
        this.duration = duration;
        timer = duration;
    }

}
