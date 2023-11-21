using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionSystems;
public class Player : Character
{
    private Transform _characterTransform;
    private Camera CameraMain;
    [SerializeField] protected LayerMask MovableInteractionLayer;
    [SerializeField] protected float MoveForce;
    [SerializeField] protected float CharacterRotationSpeed;
    [SerializeField] protected float StopingDistance;
    private Vector3 _mouseClick;
    public override void Begin()
    {
        CameraMain = Camera.main;
        _characterTransform = this.transform;
        _mouseClick = _characterTransform.position;
        Init(_characterTransform);
        SetHealth(3);
    }
    private void OnEnable()
    {
        SetDirectionPoint(transform.position);
    }
    private void OnDisable()
    {
        SetDirectionPoint(transform.position);
    }

    public override void FixedTick()
    {
        OnPointMove();
    }

    public override void OnPointMove()
    {
        controller.OnClickMove(DirectionPoint, MoveForce, StopingDistance, CharacterRotationSpeed);
    }

    public override void Tick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SetDirectionPoint(controller.DirectionMouse(CameraMain, MovableInteractionLayer, this));
        }
        PLayerStatistics.SetPlayerHitPoints(Health);

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
