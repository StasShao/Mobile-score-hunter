using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionSystems;
using CharacterMechanicSystems;
[RequireComponent(typeof(Animator))]
public class Player : Character
{
    private Transform _characterTransform;
    private Camera CameraMain;
    protected Animator _animator;
    protected Animatronic animatronic;
    [SerializeField] protected LayerMask MovableInteractionLayer;
    [SerializeField] protected float MoveForce;
    [SerializeField] protected float CharacterRotationSpeed;
    [SerializeField] protected float StopingDistance;
    private Vector3 _mouseClick;
    public DamageAnimations damageAnimations;

    public enum DamageAnimations
    {
        DeathAnimation,
        DeathAnimation2
    }
    public override void Begin()
    {
        _animator = GetComponent<Animator>();
        CameraMain = Camera.main;
        _characterTransform = this.transform;
        _mouseClick = _characterTransform.position;
        Init(_characterTransform);
        SetHealth(3);
        SetCurentHealth(3);
        animatronic = new Animatronic(_animator);
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
        

    }
    public virtual void TakeDamage()
    {
        string curentAnimation = damageAnimations.ToString();
        animatronic.PlayAnimationTrigger(curentAnimation);
    }
    public virtual void SetDeactive()
    {
        gameObject.SetActive(false);
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
