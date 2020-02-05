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
        private List<GameObject> _neighbours = new List<GameObject>();

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

        private List<GameObject> GetMatchingNeighbours(Cell cell)
        {
            var similarBubbles = new List<GameObject>();
            if(cell.BubbleObj == null)
                return similarBubbles;

            var type = cell.BubbleObj.Type;

            for(var i = 0; i < _neighbours.Count; i++)
            {
                var temp = new List<GameObject>();
                var neighbour = _neighbours[i].GetComponent<Cell>();
                if(neighbour.BubbleObj != null && neighbour.BubbleObj.Type == type)
                {
                    similarBubbles.Add(neighbour.gameObject);
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
            foreach(var bubbleGO in similarBubbles)
            {
                var cell = bubbleGO.GetComponent<Cell>();
                foreach(var n in cell.Neighbours)
                {
                    var bubble = n.GetComponent<Cell>().BubbleObj;
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
                    var cell = similarBubbles[i].GetComponent<Cell>();
                    //StartCoroutine(AnimateThenDestroy(cell));

                    Destroy(cell.BubbleObj.gameObject);
                    cell.SetBubble(null);
                }

                PostBubbleMergeAnimation(resultValue, row, coloumn, current);
            }

            AddNewRowFromTop();
        }

        private void AnimateBubbleMerge(List<GameObject> similarBubbles, Cell current)
        {
            for(var i = 0; i < similarBubbles.Count; i++)
            {
                var cell = similarBubbles[i].GetComponent<Cell>();

                //Destroy(cell.BubbleObj.gameObject);
                cell.SetBubble(null);
            }
        }

        private void PostBubbleMergeAnimation(int resultValue, int row, int coloumn, Cell current)
        {
            var prefab = Resources.Load($"Prefabs/Bubble{resultValue}") as GameObject;
            var newBubble = FindObjectOfType<Generator>().CreateBubble(prefab, row, coloumn, current.gameObject.transform.position);
            newBubble.name = $"Bubble_{row}_{coloumn}";

            current.SetBubble(newBubble.GetComponent<Bubble>());
            current.SetNeighbours();
            FindObjectOfType<Grid>().UpdateGrid();

            GroupBubbles(current);
        }

        private List<GameObject> OnMatchColours(Cell cell)
        {
            _neighbours = new List<GameObject>();
            _neighbours.Add(cell.gameObject);
            for(var i = 0; i < cell.Neighbours.Count; i++)
                _neighbours.Add(cell.Neighbours[i]);

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
                    if(_grid.GridData[(j * _grid.Rows) + i].GetComponent<Cell>().BubbleObj != null)
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
                    var oldCell = _grid.GridData[((j - 1) * _grid.Rows) + i].GetComponent<Cell>();
                    if(oldCell.BubbleObj == null)
                        continue;

                    var newCell = _grid.GridData[(j * _grid.Rows) + i].GetComponent<Cell>();
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
                var bubble = generator.CreateBubble(generator.GetRandomBubble(), i, 0, cell.transform.position);
                cell.GetComponent<Cell>().SetBubble(bubble.GetComponent<Bubble>());
            }
        }
    }
}