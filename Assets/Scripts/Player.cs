using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

[RequireComponent(typeof(ConstantForce))]
[RequireComponent(typeof(PhotonView))]
public class Player : ObjectHealth, IPunObservable
{
    [SerializeField] private float _forwardForce;
    [SerializeField] private float _strafeForce;
    [SerializeField] private float _torqueSensitivity;
    [SerializeField] private float _sensitivityX;
    [SerializeField] private float _sensitivityY;

    [Header("Shoot Settings")]
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _range = 500f;
    [SerializeField] private float _damage = 10f;

    private MiniMap _miniMap;
    private Spawner _spawner;
    private ConstantForce _constantForce;
    private Rigidbody _rigidbody;
    private PhotonView _photonView;
    private Vector3 _startPosition;

    private void Awake()
    {
        SetMaxHealth();
        _miniMap = FindObjectOfType<MiniMap>();
        _spawner = FindObjectOfType<Spawner>();        
        _constantForce = GetComponent<ConstantForce>();
        _rigidbody = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();
        _startPosition = transform.position;
    }

    private void Start()
    {
        _miniMap.Create(transform);
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            _constantForce.relativeForce = Vector3.forward * _forwardForce * Input.GetAxis("Vertical")
                + Vector3.right * _strafeForce * Input.GetAxis("Horizontal");

            _constantForce.relativeTorque = -Vector3.forward * _sensitivityX * Input.GetAxis("Mouse X")
                + -Vector3.right * _sensitivityY * Input.GetAxis("Mouse Y")
                + Vector3.up * _torqueSensitivity * Input.GetAxis("Roll");

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot();
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //—трим дл€ посто€нной передаче чего либо 
    }

    [PunRPC]
    public void ApplyDamage(int id, float damage)
    {
        PhotonView photonView = PhotonView.Find(id);

        if (photonView == null)
            return;

        var player = photonView.GetComponent<Player>();

        if (player == null)
            return;

        player.TakeDamage(damage);
    }

    [PunRPC]
    private void SpawnLaser(Vector3 start, Vector3 end)
    {
        _spawner.Pull(start, end);
    }

    protected override void Die()
    {
        Respawn();
    }

    private void Respawn()
    {
        if (_photonView.IsMine)
        {
            transform.position = _startPosition;
            SetMaxHealth();
        }
    }

    private void Shoot()
    {
        if (Physics.Raycast(_shootPoint.position, _shootPoint.forward, out RaycastHit hit, _range))
        {
            var victim = hit.rigidbody?.GetComponent<Player>();

            if (victim != null)
            {
                victim.TakeDamage(_damage);
                _photonView.RPC("ApplyDamage", RpcTarget.Others, victim.gameObject.GetPhotonView().ViewID, _damage);
            }

            _photonView.RPC("SpawnLaser", RpcTarget.All, _shootPoint.position, hit.point);
        }
        else
        {
            _photonView.RPC("SpawnLaser", RpcTarget.All, _shootPoint.position, _shootPoint.position + _shootPoint.forward * _range);
        }
    }
}
