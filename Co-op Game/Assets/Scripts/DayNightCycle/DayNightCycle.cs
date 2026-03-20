using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    [Header("Change Cycle settings")]
    [Tooltip("The Seconds how long this timer is.")][SerializeField] private float cycleDuration;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    private Image _image;
    private Scrollbar _scrollBar;
    private float _timer;

    void Start()
    {
        _image = GetComponent<Image>();
        _scrollBar = GetComponent<Scrollbar>();
    }

    void Update()
    {
        UpdateScroller();
    }

    private void UpdateScroller()
    {
        if (_timer < cycleDuration)
        {
            _timer += Time.deltaTime;

            float progress = _timer / cycleDuration;
            _scrollBar.value = progress;
            _image.color = Color.Lerp(startColor, endColor, progress);
        }
    }
}
