using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private MiniMapMarker _prefab;
    [SerializeField] private float _scale = 1f;

    private RectTransform _rectTransform;
    private Transform _owner;

    public Transform Owner => _owner;

    public void Initialize(Transform owner)
    {
        _rectTransform = transform as RectTransform;
        _owner = owner;
    }

    public void Create(Transform target)
    {
        var marker = Instantiate(_prefab, _rectTransform);
        marker.Initialize(this, target, _scale);
        Debug.Log("Создание");
    }    
}
