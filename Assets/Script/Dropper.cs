using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using static UnityEngine.InputSystem.InputAction;


public class Dropper : MonoBehaviour
{
    [HideInInspector] public int CurrentAmmo { get; set; } = 1;
    public int MaxAmmo { get; set; } = 1;
    public GameObject prefab;
    private TileMapManager manager;
    private Tilemap map;
    Timer dropDealy;
    public float droppingDealy = 1.5f;

    private void Awake()
    {
        manager = FindObjectOfType<TileMapManager>();
        map = manager.DestroyableLayer;
        dropDealy = new Timer(droppingDealy);
    }

    public void Drop()
    {
        if (CanDrop())
        {
            CreateObject();
            CurrentAmmo -= 1;
            dropDealy.StartCount();
        }
    }

    private void CreateObject()
    {
        Vector3Int cellPosition = map.WorldToCell(transform.position);
        Vector3 CellPositionCenterinWorld = map.GetCellCenterWorld(cellPosition);
        GameObject droppedObj = Instantiate(prefab, CellPositionCenterinWorld, transform.rotation);
        ObjectBehavior droppedObjectBehaviour = droppedObj.GetComponent<ObjectBehavior>();
        droppedObjectBehaviour.LinkDropper(this);
    }

    private bool CanDrop()
    {
        if (CurrentAmmo == 0) return false;
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position, LayerMask.GetMask("Object"));
        return colliders.Length == 0;
    }

    public void DropPlayer(CallbackContext v)
    {
        Drop();
    }

    public void GetAmmo()
    {
        CurrentAmmo += 1;
    }


    public void DroppedObjectDestroyed()
    {
        GetAmmo();
    }

    private void Update()
    {
        if (dropDealy.IsOver) dropDealy.Reset();
    }
}