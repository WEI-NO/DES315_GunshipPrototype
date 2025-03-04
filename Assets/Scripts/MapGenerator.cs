using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;

    [Header("Components")]
    public Tilemap mainTilemap;

    [Header("Tiles")]
    public Tile normalTile;
    public Tile boundsTile;

    [Header("Map Properties")]
    public Vector2Int mapDimension;
    public int borderThickness = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        GenerateMap(mapDimension);
    }


    #region Map Generation

    public void GenerateMap(Vector2Int mapDimension)
    {
        if (mainTilemap == null || normalTile == null || boundsTile == null)
        {
            Debug.LogWarning($"{gameObject.name}: failed to create map.");
            return;
        }

        this.mapDimension = mapDimension;

        Vector2Int halfSize = mapDimension / 2;
        mainTilemap.ClearAllTiles();

        for (int i = -halfSize.x; i <= halfSize.y; i++)
        {
            for (int j = -halfSize.y; j <= halfSize.y; j++)
            {
                Vector3Int tilePos = new Vector3Int(i, j, 0);
                if (IsBorder(i, j, halfSize))
                {
                    mainTilemap.SetTile(tilePos, boundsTile);
                } else
                {
                    mainTilemap.SetTile(tilePos, normalTile);
                }
            }
        }


    }

    private bool IsBorder(int x, int y, Vector2Int halfSize)
    {
        return (x < -halfSize.x + borderThickness || x >= halfSize.x - borderThickness ||
            y < -halfSize.y + borderThickness || y >= halfSize.y - borderThickness);
    }

    #endregion map generation
}
