using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private MiniMap _miniMap;

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            var player = PhotonNetwork.Instantiate(_playerPrefab.name, transform.position, Quaternion.identity).GetComponent<Player>();
            _cameraTransform.SetParent(player.transform.GetChild(0));
            _cameraTransform.localPosition = Vector3.zero;
            _cameraTransform.localRotation = Quaternion.identity;
            _healthBar.Initialize(player);
            _miniMap.Initialize(player.transform);
        }
    }
}
