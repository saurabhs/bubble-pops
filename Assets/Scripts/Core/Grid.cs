using System.Collections.Generic;
using UnityEngine;

namespace BubblePops.Core
{
    public class Grid : MonoBehaviour
    {
        /// <summary>
        /// max rows
        /// </summary>
        [SerializeField] private int _rows = 6;

        /// <summary>
        /// max coloumn
        /// </summary>
        [SerializeField] private int _coloumns = 4;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private Vector2 _hexOffset = new Vector2(0.5f, 0.5f);

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private float _offsetY = 0.5f;

        /// <summary>
        /// 
        /// </summary>
        public List<GameObject> _grid = new List<GameObject>();

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private Transform _parent = null;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private Transform _pivot = null;

        public GameObject _prefab = null;

        private void Awake() => Create();

        Dictionary<int, Color> _colors = new Dictionary<int, Color>()
        {
            { 0, Color.red },
            { 1, Color.blue },
            { 2, Color.green },
            { 3, Color.yellow },
        };

        private void Create()
        {
            for (var j = 0; j < _coloumns; j++)
            {
                AddRow(j);
            }
        }

        private void AddRow(int coloumn)
        {
            // var color = _colors[Random.Range(0, _colors.Count)];
            for (var i = 0; i < _rows; i++)
            {
                var x = _pivot.position.x + (_hexOffset.x * i);
                var y = _pivot.position.y + (_offsetY * -coloumn) + ((i % 2 == 1) ? _hexOffset.y : 0);

                var cell = Instantiate(_prefab, new Vector2(x, y), _prefab.transform.rotation, _parent);
                cell.name = $"Cell_{i}_{coloumn}";
                // cell.GetComponent<SpriteRenderer>().color = color;
                _grid.Add(cell);
            }
        }

        private void RemoveRow(int coloumn)
        {
            var toDelete = _grid.FindAll(ob => ob.name.Split('_')[2] == coloumn.ToString());
            for(var i = 0; i < toDelete.Count; i++)
            {
                _grid.Remove(toDelete[i]);
                Destroy(toDelete[i]);
            }

            MoveRowsUp(coloumn + 1);
            _coloumns--;
        }

        private void MoveRowsUp(int coloumn)
        {
            var toMove = _grid.FindAll(obj => obj.name.Split('_')[2] == coloumn.ToString());
            if(toMove == null || toMove.Count == 0) return;

            for(var i = 0; i < _rows; i++)
            {
                var y = _pivot.position.y + (_offsetY * -i + _offsetY) + ((coloumn % 2 == 1) ? _hexOffset.y : 0);
                var pos = toMove[i].transform.position;
                toMove[i].transform.position = new Vector2(pos.x, GetYPosition(coloumn, i));
                toMove[i].name = $"Cell_{i}_{coloumn - 1}";
            }
            MoveRowsUp(coloumn + 1);
        }

        private float GetYPosition(int row, int coloumn) => _pivot.position.y + (_offsetY * -row + _offsetY) + ((coloumn % 2 == 1) ? _hexOffset.y : 0);

        [NaughtyAttributes.Button]
        public void Add()
        {
            AddRow(_coloumns++);
        }

        [NaughtyAttributes.Button]
        public void Remove()
        {
            RemoveRow(0);
        }
    }
}