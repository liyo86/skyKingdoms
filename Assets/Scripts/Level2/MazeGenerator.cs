using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class MazeGenerator : MonoBehaviour //Hunt-and-Kill algorithm
{
    public static MazeGenerator Instance;
    
    [SerializeField] private int _width, _height;
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] public GameObject _gemPrefab; // Prefab de la gema
    [SerializeField] private LoadScreenManager sceneManager;
    [HideInInspector]
    public Dictionary<Vector2Int, CellData> UnvisitedCells, VisitedCells;
    private long _memBefore, _memAfter;
    
    public int Width
    {
        get { return _width; }
    }

    public int Height
    {
        get { return _height; }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _memBefore = GC.GetTotalMemory(false);
        UnvisitedCells = new Dictionary<Vector2Int, CellData>();
        VisitedCells = new Dictionary<Vector2Int, CellData>();

        Vector2Int position = Vector2Int.zero;
        for (int i = 0; i < _width * _height; i++)
        {
            UnvisitedCells.Add(position, new CellData(position));
            UnvisitedCells[position].RefreshNeighbours(VisitedCells, _width, _height);

            Instantiate(_cellPrefab.gameObject, new Vector3(position.x, 0, position.y), Quaternion.identity, transform)
                .GetComponent<Cell>().SetData(UnvisitedCells[position]);


            if (position.x == _width - 1)
            {
                position.x = 0;
                position.y++;
            }
            else position.x++;
        }

        GenerateMaze(UnvisitedCells.Values.First());
        
        FindPaths();
    }
    
    private void FindPaths()
    {
        // Gema
        Vector2Int centerPosition = new Vector2Int(_width / 2, _height / 2);
        _gemPrefab.transform.position = new Vector3(centerPosition.x, 1f, centerPosition.y);

        // Enemy & Player Posicion inicial
        Vector2Int enemyStartPosition = new Vector2Int(_width - 1, 0);
        Vector2Int playerStartPosition = Vector2Int.zero;

        // Asegurar que haya un camino desde el jugador hasta la gema
        List<Vector2Int> playerPath = FindPath(playerStartPosition, centerPosition);
        
        while (playerPath == null || playerPath.Count == 0)
        {
            RegenerateMaze();
            playerPath = FindPath(playerStartPosition, centerPosition);
        }

        // Asegurar que haya un camino desde el enemigo hasta la gema
        List<Vector2Int> enemyPath = FindPath(enemyStartPosition, centerPosition);
        while (enemyPath == null || enemyPath.Count == 0)
        {
            RegenerateMaze();
            playerPath = FindPath(playerStartPosition, centerPosition);
            enemyPath = FindPath(enemyStartPosition, centerPosition);
        }

        if(playerPath != null)
            Debug.Log("Se ha encontrado camino para el player: " + playerPath.ToString());
    }

    private void GenerateMaze(CellData start)
    {
        var curr = start;
        var prev = new CellData(new Vector2Int(-1, -1));
        curr.RefreshNeighbours(VisitedCells, _width, _height);
        Random rand = new Random();

        while (UnvisitedCells.Count > 0)
        {
            UnvisitedCells.Remove(curr.Position);
            VisitedCells.Add(curr.Position, curr);

            curr.RefreshNeighbours(VisitedCells, _width, _height);

            if (prev.Position != new Vector2Int(-1, -1))
            {
                curr.RemoveWall(prev.Position);
                curr.MarkDirty(true);
                List<Vector2Int> playerPath = FindPath(Vector2Int.zero, new Vector2Int(_width / 2, _height / 2));
                if (playerPath == null || playerPath.Count == 0)
                {
                    RegenerateMaze();
                    return;
                }

            }

            if (curr.HasUnvisitedNeighbour)
            {
                var next = UnvisitedCells[curr.GetRandomNeighbour(rand)];
                next.RefreshNeighbours(VisitedCells, _width, _height);
                curr.RemoveWall(next.Position);
                curr.MarkDirty(true);
                List<Vector2Int> playerPath = FindPath(Vector2Int.zero, new Vector2Int(_width / 2, _height / 2));
                if (playerPath == null || playerPath.Count == 0)
                {
                    RegenerateMaze();
                    return;
                }

                prev = curr;
                curr = next;
            }
            else if (UnvisitedCells.Count > 0)
            {
                var unvisitedCell = UnvisitedCells.Values.First();
                unvisitedCell.RefreshNeighbours(VisitedCells, _width, _height);
                prev = VisitedCells[unvisitedCell.GetRandomVisitedNeighbour(rand)];
                prev.RemoveWall(unvisitedCell.Position);
                prev.MarkDirty(true);
                curr = unvisitedCell;
            }
        }

        _memAfter = GC.GetTotalMemory(false);
        Debug.Log(_memAfter - _memBefore);
    }

    private void RegenerateMaze()
    {
        // Reiniciar el diccionario de celdas visitadas
        VisitedCells.Clear();

        // Reiniciar las celdas no visitadas y generar un nuevo laberinto
        foreach (var cellData in UnvisitedCells.Values)
        {
            List<Vector2Int> playerPath = FindPath(Vector2Int.zero, new Vector2Int(_width / 2, _height / 2));
            if (playerPath == null || playerPath.Count == 0)
            {
                RegenerateMaze();
                return;
            }

            cellData.Reset();
        }

        GenerateMaze(UnvisitedCells.Values.First());
    }
    
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> parentMap = new Dictionary<Vector2Int, Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current == goal)
            {
                List<Vector2Int> path = new List<Vector2Int>();
                Vector2Int node = goal;
                while (node != start)
                {
                    path.Add(node);
                    node = parentMap[node];
                }
                path.Reverse();
                return path;
            }

            foreach (var neighbour in GetNeighbours(current))
            {
                if (!visited.Contains(neighbour))
                {
                    queue.Enqueue(neighbour);
                    visited.Add(neighbour);
                    parentMap[neighbour] = current;
                }
            }
        }
        
        return null; // Retorno nulo si no encuentro ningun camino
    }
    
    private List<Vector2Int> GetNeighbours(Vector2Int position)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (position.x > 0)
            neighbours.Add(new Vector2Int(position.x - 1, position.y));
        if (position.x < _width - 1)
            neighbours.Add(new Vector2Int(position.x + 1, position.y));
        if (position.y > 0)
            neighbours.Add(new Vector2Int(position.x, position.y - 1));
        if (position.y < _height - 1)
            neighbours.Add(new Vector2Int(position.x, position.y + 1));

        return neighbours;
    }
    
    public void LevelComplete()
    {
        sceneManager.LoadScene();   
    }
}