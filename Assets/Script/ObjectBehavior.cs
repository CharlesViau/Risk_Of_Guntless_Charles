using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ObjectBehavior : MonoBehaviour
{
    public enum ObjectState
    {
        REST,
        ACTIVE,
        WAITING,
        INACTIVE
    }

    private Collider2D col;
    private SpriteRenderer spr;
    private AudioSource src;
    private Animator animator;
    public bool isDestroyable;
    public LayerMask CanBePickUpBy;
    public UnityEvent<Collision2D> OnPickUp;
    public UnityEvent OnDrop;
    public UnityEvent OnDestruction;

    Dropper dropper;

    public bool isActive { get; private set; } = true;

    [HideInInspector] public ObjectState objectState;


    private void Awake()
    {
        col = GetComponent<Collider2D>();
        spr = GetComponent<SpriteRenderer>();
        TryGetComponent<AudioSource>(out src);
        TryGetComponent<Animator>(out animator);
    }

    public void LinkDropper(Dropper _dropper)
    {
        dropper = _dropper;
    }

    // Update is called once per frame
    void Update()
    {
        switch (objectState)
        {
            case ObjectState.REST:
                col.isTrigger = true;
                objectState = ObjectState.WAITING;
                break;
            case ObjectState.ACTIVE:
                if(!isActive && CheckAudioSource())
                    objectState = ObjectState.INACTIVE;
                break;
            case ObjectState.INACTIVE:
                OnDestruction?.Invoke();
                if(!IsInvoking())
                     GameObject.Destroy(gameObject);
                break;
            case ObjectState.WAITING:
                if(!Physics2D.OverlapBox(transform.position, new Vector2(1,1), 0, LayerMask.GetMask("Player")))
                {
                    col.isTrigger = false;
                    objectState = ObjectState.ACTIVE;
                    OnDrop?.Invoke();
                }
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnPickUp?.Invoke(collision);
    }

    public void Hide()
    {
        col.enabled = false;
        spr.enabled = false;
        if (animator) animator.enabled = false;
    }

    public void SetInactive()
    {
        isActive = false;
    }

    public void NotifyDropperObjectIsDead()
    {
        if (dropper)
        dropper.DroppedObjectDestroyed();
    }

    private bool CheckAudioSource()
    {
        if (!src) return true;
        else return !src.isPlaying;
    }
}
