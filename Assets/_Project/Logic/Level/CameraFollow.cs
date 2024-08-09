using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook cinemachine;
    [SerializeField] private float rotationAngleX = 6.5f;

    private Player _player;

    [Inject]
    private void Construct(Player player)
    {
        _player = player;   
    }

    private void Start()
    {
        cinemachine.Follow = _player.transform;

        transform.rotation = Quaternion.Euler(rotationAngleX, transform.rotation.eulerAngles.y, 
            transform.rotation.eulerAngles.z);
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(rotationAngleX, transform.rotation.eulerAngles.y,
    transform.rotation.eulerAngles.z);
    }

    public void SetPosition(Vector3 pos)
    {
        this.transform.position = pos;
        this.transform.rotation = Quaternion.identity;
    }

}
