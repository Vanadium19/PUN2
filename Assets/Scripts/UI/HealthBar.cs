using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private ObjectHealth _health;
    private float _maxSizeX;
    private float _maxSizeY;
    private RectTransform _healthBar;

    public void Initialize(ObjectHealth health)
    {
        _healthBar = transform as RectTransform;
        _health = health;
        _health.HealthChanged += ChangeHealth;
        _maxSizeX = _healthBar.sizeDelta.x;
        _maxSizeY = _healthBar.sizeDelta.y;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= ChangeHealth;
    }

    private void ChangeHealth(float health)
    {
        _healthBar.sizeDelta = new Vector2(_maxSizeX * health / _health.MaxHealth, _maxSizeY);
    }
}
