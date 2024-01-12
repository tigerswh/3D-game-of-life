
using System.Collections.Generic;
using UnityEngine;


public class GameLogic : MonoBehaviour
{
    [Header("Initialaztion")]
    [SerializeField] int width, height, deepth, gridCellSize;
    Grid grid;
    public bool runOnce = false;
    float timer = 0;
    public float iterationTimer;

    public class Grid
    {
        public int x, y, z;
        public float size;
        public GridCell[,,] gridArray; // leaves

        // tree 
        public Grid(int width, int height, int deepth, float gridCellSize, Transform transform)
        {
            x = width;
            y = height;
            z = deepth;
            size = gridCellSize;
            //UnityEngine.Color lightRed = new UnityEngine.Color(1, 0, 0, 0.5f);
            UnityEngine.Color lightWhite = new UnityEngine.Color(1, 1, 1, 0.5f);

            gridArray = new GridCell[x, y, z];



            for (int z = 0; z <= deepth; z++)
            {
                for (int y = 0; y <= height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Vector3 start = new Vector3(x, y, z);
                        start += transform.position;

                        Vector3 end = new Vector3((x + size), y, z);
                        end += transform.position;
                        Debug.DrawLine(start, end, lightWhite, 1000);
                    }
                }

                for (int x = 0; x <= width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Vector3 start = new Vector3(x, y, z);
                        start += transform.position;

                        Vector3 end = new Vector3(x, (y + size), z);
                        end += transform.position;
                        Debug.DrawLine(start, end, lightWhite, 1000);
                    }
                }
            }

            for (int z = 0; z < deepth; z++)
            {
                for (int y = 0; y <= height; y++)
                {
                    for (int x = 0; x <= width; x++)
                    {
                        Vector3 start = new Vector3(x, y, z);
                        start += transform.position;

                        Vector3 end = new Vector3(x, y, (z + size));
                        end += transform.position;
                        Debug.DrawLine(start, end, lightWhite, 1000);
                    }
                }
            }

            float halfSize = size / 2;
            Vector3 offset = new Vector3(halfSize, halfSize, halfSize);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < deepth; z++)
                    {
                        //creating grid cells
                        gridArray[x, y, z] = new GridCell(x, y, z, transform, size, offset);
                    }
                }
            }

            /*
             * big for loop
             * get a grid cell
             * loop through 26 possible directions
             * check if that coordinate is valid
             * if true, add it to its neighbor list
             */

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < deepth; z++)
                    {
                        AddNeighbors(x, y, z);
                    }
                }
            }
        }

        //The coordinates are for bottom left cube
        public void MakeShapeInCells(int x, int y, int z, Transform transform)
        {
            //Initialization(x    , y    , z, transform);
            //Initialization(x + 1, y    , z, transform);
            //Initialization(x    , y - 1, z, transform);
            //Initialization(x + 1, y - 1, z, transform);

            //Initialization(x + 2, y, z, transform);
            //Initialization(x + 2, y - 1, z, transform);



            //Initialization(x    , y    , z - 1, transform);
            //Initialization(x + 1, y    , z - 1, transform);
            //Initialization(x    , y - 1, z - 1, transform);
            //Initialization(x + 1, y - 1, z - 1, transform);

            Initialization(x, y, z, transform);
            Initialization(x + 1, y, z, transform);
            Initialization(x + 2, y, z, transform);
            Initialization(x + 3, y, z, transform);

            Initialization(x, y -1 , z, transform);
            Initialization(x + 1, y -1 , z, transform);



            Initialization(x, y, z - 1, transform);
            Initialization(x + 1, y, z-1, transform);
            Initialization(x + 2, y, z - 1, transform);
            Initialization(x + 3, y, z - 1, transform);


            Initialization(x, y - 1, z-1, transform);
            Initialization(x + 1, y - 1, z - 1, transform);

        }

        public void AddNeighbors(int x, int y, int z)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        if (AreCoordinatesValid(x + i, y + j, z + k))
                        {
                            gridArray[x, y, z].neighbors.Add(gridArray[x + i, y + j, z + k]);
                        }
                    }
                }
            }
        }

        public void Initialization(int x, int y, int z, Transform transform)
        {
            if(!AreCoordinatesValid(x,y,z) || AreCoordinatesOccupied(x,y,z))
            {
                Debug.Log("INVALID CUBE POSITIONS OR OCCUPIED!!!");
                return;
            }

            //creating new cube mesh
            gridArray[x,y,z].occupied = true;
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = gridArray[x, y, z].location;
            gridArray[x,y,z].gameObject = cube;
        }

        public void Update()
        {
            foreach(GridCell gridCell in gridArray)
            {
                gridCell.UpdateGridCell();
            }
        }

        //public void MoveCubeMesh(int x, int y, int z, int newX, int newY, int newZ)
        //{
        //    if (!AreCoordinatesValid(x, y, z) || !AreCoordinatesOccupied(x, y, z) || 
        //        !AreCoordinatesValid(newX, newY, newZ) || AreCoordinatesOccupied(newX, newY, newZ))
        //    {
        //        Debug.Log("INVALID CUBE POSITIONS OR OCCUPIED, CANT MOVE!!!");
        //        return;
        //    }

        //    // change to new location, change if its occupied, and changing the reference of both nodes(gridCell)
        //    gridArray[x, y, z].gameObject.transform.position = gridArray[newX, newY, newZ].location;
        //    gridArray[x, y, z].occupied = false;
        //    gridArray[newX, newY, newZ].occupied = true;
        //    gridArray[newX, newY, newZ].gameObject = gridArray[x, y, z].gameObject;
        //    gridArray[x, y, z].gameObject = null; 
        //}

        bool AreCoordinatesValid(int xToCheck, int yToCheck, int zToCheck)
        {
            // if all of them are true it returns true!
            return xToCheck < x  && yToCheck < y  && zToCheck < z &&
                   xToCheck >= 0 && yToCheck >= 0 && zToCheck >= 0;
        }

        bool AreCoordinatesOccupied(int xToCheck, int yToCheck, int zToCheck)
        {
            return gridArray[xToCheck, yToCheck, zToCheck].occupied;
        }
    }

    public class GridCell
    {
        public Vector3 location;
        public bool occupied;
        public int x, y, z;
        public GameObject gameObject;

        public List<GridCell> neighbors = new List<GridCell>();

        public GridCell(int x, int y, int z, Transform transform, float size, Vector3 offset)
        {
            this.x = x;
            this.y = y;
            this.z = z;

            Vector3 gridPos = new Vector3(x * size, y * size, z * size) + offset;
            location = transform.position + gridPos;

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = location;
            gameObject = cube;
            gameObject.SetActive(false);
        }

        public void UpdateGridCell()
        {
            int aliveNeighbours = 0;

            for (int i = 0; i < neighbors.Count; i++)
            {
                if (neighbors[i].occupied)
                {
                    aliveNeighbours++;
                }
            }

            

            if (occupied && (aliveNeighbours < 2*3 || aliveNeighbours > 3*3))
            {
                gameObject.SetActive(false);
                occupied = false;
            }
            else if (!occupied && (aliveNeighbours == 6 || aliveNeighbours == 7 || aliveNeighbours == 8|| aliveNeighbours == 9))
            {
                gameObject.SetActive(true);
                occupied = true;
            }
        }
    }


    void Start()
    {
        grid = new Grid(width, height, deepth, gridCellSize, transform);
        grid.MakeShapeInCells(1, 2, 3, transform);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= iterationTimer)
        {
            grid.Update();
            timer = 0;
        }

        //if(!runOnce) grid.MoveCubeMesh(1, 2, 3, 6, 7, 8) ; runOnce = true;
    }
}
