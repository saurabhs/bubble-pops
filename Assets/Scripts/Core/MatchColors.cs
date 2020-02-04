using System.Collections.Generic;
using UnityEngine;

namespace BubblePops.Core
{
    public class On2048Reached : UnityEngine.Events.UnityEvent<int> { }
    public class OnBubblesMatch : UnityEngine.Events.UnityEvent<int> { }

    public class MatchColors : MonoBehaviour
    {
        ///// <summary>
        ///// 
        ///// </summary>
        //private Cell cell;

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

        private List<GameObject> GetMatchingNeighbours(Cell cell)
        {
            var similarBubbles = new List<GameObject>();
            if(cell.BubbleObj == null)
                return similarBubbles;

            var type = cell.BubbleObj.Type;

            for (var i = 0; i < _neighbours.Count; i++)
            {
                var temp = new List<GameObject>();
                var neighbour = _neighbours[i].GetComponent<Cell>();
                if (neighbour.BubbleObj != null && neighbour.BubbleObj.Type == type)
                {
                    similarBubbles.Add(neighbour.gameObject);
                    temp = neighbour.Neighbours;
                }

                foreach (var newCell in temp)
                {
                    if (!_neighbours.Contains(newCell))
                        _neighbours.Add(newCell);
                }
            }
            return similarBubbles;
        }

        public void GroupBubbles(Cell current)
        {
            var similarBubbles = OnMatchColours(current);
            if (similarBubbles.Count < 2)
            {
                FindObjectOfType<Grid>().UpdateGrid();
                return;
            }

            var resultValue = (int)current.BubbleObj.Type * (int)Mathf.Pow(2, similarBubbles.Count - 1);
            if (resultValue >= (int)EType.TwentyFortyEight)
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

                for (var i = 0; i < similarBubbles.Count; i++)
                {
                    var cell = similarBubbles[i].GetComponent<Cell>();
                    //StartCoroutine(AnimateThenDestroy(cell));

                    Destroy(cell.BubbleObj.gameObject);
                    cell.SetBubble(null);
                }

                var prefab = Resources.Load($"Prefabs/Bubble{resultValue}") as GameObject;
                var newBubble = FindObjectOfType<Generator>().CreateBubble(prefab, row, coloumn, current.gameObject.transform.position);
                newBubble.name = $"Bubble_{row}_{coloumn}";

                current.SetBubble(newBubble.GetComponent<Bubble>());
                current.SetNeighbours();
                FindObjectOfType<Grid>().UpdateGrid();
                
                GroupBubbles(current);
            }
        }

        private List<GameObject> OnMatchColours(Cell cell)
        {
            _neighbours = new List<GameObject>();
            _neighbours.Add(cell.gameObject);
            for (var i = 0; i < cell.Neighbours.Count; i++)
                _neighbours.Add(cell.Neighbours[i]);

            return GetMatchingNeighbours(cell);
        }
    }
}