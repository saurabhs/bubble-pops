using System.Collections.Generic;
using UnityEngine;

namespace BubblePops.Core
{
    public class On2048Reached : UnityEngine.Events.UnityEvent<int> { }
    public class OnBubblesMatch : UnityEngine.Events.UnityEvent<int> { }

    public class MatchColors : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private List<Cell> _neighbours = new List<Cell>();

        /// <summary>
        /// 
        /// </summary>
        public On2048Reached on2048Reached = new On2048Reached();

        /// <summary>
        /// 
        /// </summary>
        public OnBubblesMatch onBubblesMatch = new OnBubblesMatch();

        /// <summary>
        /// 
        /// </summary>
        private Grid _grid = null;

        private void Awake() => _grid = GetComponent<Grid>();

        private List<Cell> GetMatchingNeighbours(Cell cell)
        {
            var similarBubbles = new List<Cell>();
            if(cell.BubbleObj == null)
                return similarBubbles;

            var type = cell.BubbleObj.Type;

            for(var i = 0; i < _neighbours.Count; i++)
            {
                var temp = new List<Cell>();
                var neighbour = _neighbours[i];
                if(neighbour.BubbleObj != null && neighbour.BubbleObj.Type == type)
                {
                    similarBubbles.Add(neighbour);
                    temp = neighbour.Neighbours;
                }

                foreach(var newCell in temp)
                {
                    if(!_neighbours.Contains(newCell))
                        _neighbours.Add(newCell);
                }
            }
            return similarBubbles;
        }

        public void GroupBubbles(Cell current)
        {
            var similarBubbles = OnMatchColours(current);
            if(similarBubbles.Count < 2)
            {
                FindObjectOfType<Grid>().UpdateGrid();
                return;
            }

            var resultValue = (int)current.BubbleObj.Type * (int)Mathf.Pow(2, similarBubbles.Count - 1);
            var bFound = false;
            foreach(var cell in similarBubbles)
            {
                foreach(var n in cell.Neighbours)
                {
                    var bubble = n.BubbleObj;
                    if(bubble && bubble.Type == (EType)resultValue)
                    {
                        current = cell;
                        bFound = true;
                        break;
                    }
                }
                if(bFound)
                    break;
            }

            if(resultValue >= (int)EType.TwentyFortyEight)
            {
                on2048Reached.Invoke(resultValue);
                StartCoroutine(FindObjectOfType<Generator>().Reset());
            }
            else
            {
                onBubblesMatch.Invoke(resultValue);

                var ss = current.BubbleObj.name.Split('_');
                var row = int.Parse(ss[1]);
                var coloumn = int.Parse(ss[2]);

                for(var i = 0; i < similarBubbles.Count; i++)
                {
                    var cell = similarBubbles[i];
                    if(cell.BubbleObj != null)
                    { 
                        Destroy(cell.BubbleObj.gameObject);
                        cell.SetBubble(null);
                    }
                }

                PostBubbleMergeAnimation(resultValue, row, coloumn, current);
            }

            AddNewRowFromTop();
        }

        private void AnimateBubbleMerge(List<Cell> similarBubbles, Cell current)
        {
            for(var i = 0; i < similarBubbles.Count; i++)
            {
                var cell = similarBubbles[i];
                cell.SetBubble(null);
            }
        }

        private void PostBubbleMergeAnimation(int resultValue, int row, int coloumn, Cell current)
        {
            var prefab = Resources.Load($"Prefabs/Bubble{resultValue}") as GameObject;
            var generator = FindObjectOfType<Generator>();
            var newBubble = generator.CreateBubble(prefab, row, coloumn, current.gameObject.transform.position, generator.transform);

            current.SetBubble(newBubble.GetComponent<Bubble>());
            current.SetNeighbours();
            FindObjectOfType<Grid>().UpdateGrid();

            GroupBubbles(current);
        }

        private List<Cell> OnMatchColours(Cell cell)
        {
            _neighbours = new List<Cell>() { cell };
            _neighbours.AddRange(cell.Neighbours);

            return GetMatchingNeighbours(cell);
        }

        [SerializeField] private int _minBubbleRatioToCreateNewRow = 6;

        private void AddNewRowFromTop()
        {
            var minCount = (_grid.Coloumn * _grid.Rows) / _minBubbleRatioToCreateNewRow;
            var bubblesCount = 0;
            var lastColoumn = -1;
            for(var j = 1; j < _grid.MaxColoumn; j++)
            {
                var lastCount = bubblesCount;
                for(var i = 0; i < _grid.Rows; i++)
                {
                    if(_grid.GridData[(j * _grid.Rows) + i].BubbleObj != null)
                        bubblesCount++;
                }
                if(bubblesCount > minCount)
                    break;

                if(bubblesCount == lastCount)
                {
                    lastColoumn = j;
                    break;
                }
            }
            if(lastColoumn > -1)
                ExecuteAddNewRow(lastColoumn);
        }

        private void ExecuteAddNewRow(int col)
        {
            for(var j = col; j > 0; j--)
            {
                for(var i = 0; i < _grid.Rows; i++)
                {
                    var oldCell = _grid.GridData[((j - 1) * _grid.Rows) + i];
                    if(oldCell.BubbleObj == null)
                        continue;

                    var newCell = _grid.GridData[(j * _grid.Rows) + i];
                    var bubble = oldCell.BubbleObj;
                    bubble.name = oldCell.BubbleObj.name;

                    newCell.SetBubble(oldCell.BubbleObj);
                    bubble.transform.position = newCell.transform.position;
                    oldCell.SetBubble(null);
                }
            }

            var generator = FindObjectOfType<Generator>();
            for(var i = 0; i < _grid.Rows; i++)
            {
                var cell = _grid.GridData[i];
                var bubble = generator.CreateBubble(generator.GetRandomBubble(), i, 0, cell.transform.position, generator.transform);
                cell.SetBubble(bubble.GetComponent<Bubble>());
            }
        }
    }
}