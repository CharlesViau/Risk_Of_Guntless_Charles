using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HumanoidBehavior : MonoBehaviour
{
    private Animator animator;
    public Move MovementScript;
    private Vector2 movVector;
    private SpriteRenderer spr;
    public bool isActive { get; private set; } = true;
    public UnityEvent OnDeath;
    private ObjectBehavior ob;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        TryGetComponent<ObjectBehavior>(out ob);
    }

    // Update is called once per frame
    private void Update()
    {
        movVector = MovementScript.movVector;
        animator.SetFloat("HorizontalSpeed", movVector.x);
        animator.SetFloat("VerticalSpeed", movVector.y);

        if(!isActive && ob)
        {
            ob.SetInactive();
        }
    }

    public void SetInactive()
    {
        spr.enabled = false;
        isActive = false;
        OnDeath?.Invoke();
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
