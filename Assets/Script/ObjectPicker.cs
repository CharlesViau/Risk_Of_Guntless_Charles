using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    //Could be change to other 2D type of collider
    private BoxCollider2D col2D;

    private void Awake()
    {
        col2D = GetComponent<BoxCollider2D>();
    }
}
