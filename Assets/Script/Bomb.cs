using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
public class Bomb : MonoBehaviour
{
    public bool isAI;
    private Vector3Int BombTilePosition;
    [SerializeField] public int explosionRange;
    public GameObject explosion;
    private Tilemap Destructible;
    private Tilemap Indestructible;
    private TileMapManager manager;
    private PlayerBehavior pb;
    private int bonusRange = 0;

    private void Awake()
    {
        manager = FindObjectOfType<TileMapManager>();
        Destructible = manager.DestroyableLayer;
        Indestructible = manager.UndestroyableLayer;

    }

    private void Start()
    {
        if (!isAI)
        {
            pb = FindObjectOfType<PlayerBehavior>();
            bonusRange = pb.bonusRange;
        }
    }

    private bool blowTile(Vector3Int tilePosition)
    {
        if (Indestructible.GetTile(tilePosition)) return true;
        Instantiate(explosion, Destructible.GetCellCenterWorld(tilePosition), transform.rotation);
        if (Destructible.GetTile(tilePosition))
        {
            Destructible.SetTile(tilePosition, null);
            return true;
        }
        Collider2D[] results = Physics2D.OverlapBoxAll(Destructible.GetCellCenterWorld(tilePosition), Destructible.cellSize, 0);
        if (results.Length > 0)
        {
            ObjectBehavior behavior;
            HumanoidBehavior hBehavior;
            Bomb bomb;

            foreach (var obj in results)
            {
                if (obj.TryGetComponent<ObjectBehavior>(out behavior) && behavior.isDestroyable)
                {
                    behavior.SetInactive();
                }
                if (obj.TryGetComponent<HumanoidBehavior>(out hBehavior))
                {
                    hBehavior.SetInactive();
                }
                if(obj.TryGetComponent<Bomb>(out bomb) && !hBehavior)
                {
                    bomb.Explode();
                    if(behavior)
                    {
                        behavior.SetInactive();
                    }
                }
            }
        }

        return false;
    }

    private void ExplodeInDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                for (int i = 1; i <= explosionRange + bonusRange; i++)
                {
                    if (blowTile(BombTilePosition + new Vector3Int(0, i, 0))) return;
                    else continue;
                }
                break;
            case Direction.Down:
                for (int i = 1; i <= explosionRange + bonusRange; i++)
                {
                    if (blowTile(BombTilePosition - new Vector3Int(0, i, 0))) return;
                    else continue;
                }
                break;
            case Direction.Left:
                for (int i = 1; i <= explosionRange + bonusRange; i++)
                {
                    if (blowTile(BombTilePosition - new Vector3Int(i, 0, 0))) return;
                    else continue;
                }
                break;
            case Direction.Right:
                for (int i = 1; i <= explosionRange + bonusRange; i++)
                {
                    if (blowTile(BombTilePosition + new Vector3Int(i, 0, 0))) return;
                    else continue;
                }
                break;
            default:
                break;
        }
    }

    public void Explode()
    {
        BombTilePosition = Destructible.WorldToCell(transform.position);
        foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList())
        {
            blowTile(BombTilePosition);
            ExplodeInDirection(direction);
        }
    }
}
