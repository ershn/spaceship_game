using PathFinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldInternalIO : MonoBehaviour
{
    public Tilemap Tilemap;
    public PathRequestManager PathRequestManager;
    public ItemCreator ItemCreator;
    public ItemAllotter ItemAllotter;
    public TaskScheduler TaskScheduler;
}
