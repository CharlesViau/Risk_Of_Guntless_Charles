using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{
    private Vector2 movVector;
    [HideInInspector] public int bonusRange { get; set; } = 0;
    private HumanoidBehavior hb;

    private void Awake()
    {
        hb = GetComponent<HumanoidBehavior>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Physics2D.OverlapBox(transform.position,new Vector2(1,1), 0, LayerMask.GetMask("Ghost", "Ennemi")))
        {
            hb.SetInactive();
        }
    }

}
