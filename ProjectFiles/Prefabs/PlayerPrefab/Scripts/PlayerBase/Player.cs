using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : Character
{
    private Transform _characterTransform;
    [SerializeField] protected Camera CameraMain;
    [SerializeField] protected LayerMask MovableInteractionLayer;
    [SerializeField] protected float MoveForce;
    [SerializeField] protected float CharacterRotationSpeed;
    [SerializeField] protected float StopingDistance;
    private Vector3 _mouseClick;
    public override void Begin()
    {
        _characterTransform = this.transform;
        _mouseClick = _characterTransform.position;
        Init(_characterTransform);
    }

    public override void FixedTick()
    {
        OnPointMove();
    }

    public override void OnPointMove()
    {
        controller.OnClickMove(_mouseClick, MoveForce, StopingDistance, CharacterRotationSpeed);
    }

    public override void Tick()
    {
        if(Input.GetMouseButtonDown(0))
        {
           _mouseClick = controller.SetDirection(CameraMain, MovableInteractionLayer);
        }
        
    }
    private void Start()
    {
        Begin();
    }
    private void FixedUpdate()
    {
        FixedTick();
    }
    private void Update()
    {
        Tick();
    }
}
