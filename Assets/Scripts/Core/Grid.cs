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
            for (var i = 0; i < _rows; i++)
            {
                AddRow(i);
            }
        }

        private void AddRow(int row)
        {
            var color = _colors[Random.Range(0, _colors.Count)];
            for (var j = 0; j < _coloumns; j++)
            {
                var x = _pivot.position.x + (_hexOffset.x * j);
                var y = _pivot.position.y + (_offsetY * -row) + ((j % 2 == 1) ? _hexOffset.y : 0);

                var cell = Instantiate(_prefab, new Vector2(x, y), _prefab.transform.rotation, _parent);
                cell.name = $"Cell_{row}_{j}";
                // cell.GetComponent<SpriteRenderer>().color = color;
                _grid.Add(cell);
            }
        }

        private void RemoveRow(int row)
        {
            var toDelete = _grid.FindAll(ob => ob.name.Split('_')[1] == row.ToString());
            for(var i = 0; i < toDelete.Count; i++)
            {
                _grid.Remove(toDelete[i]);
                Destroy(toDelete[i]);
            }

            MoveRowsUp(row + 1);
            _rows--;
        }

        private void MoveRowsUp(int row)
        {
            var toMove = _grid.FindAll(obj => obj.name.Split('_')[1] == row.ToString());
            if(toMove == null || toMove.Count == 0) return;

            for(var j = 0; j < _coloumns; j++)
            {
                var y = _pivot.position.y + (_offsetY * -row + _offsetY) + ((j % 2 == 1) ? _hexOffset.y : 0);
                var pos = toMove[j].transform.position;
                toMove[j].transform.position = new Vector2(pos.x, GetYPosition(row, j));
                toMove[j].name = $"Cell_{row - 1}_{j}"; 
            }
            MoveRowsUp(row + 1);
        }

        private float GetYPosition(int row, int coloumn) => _pivot.position.y + (_offsetY * -row + _offsetY) + ((coloumn % 2 == 1) ? _hexOffset.y : 0);

        [NaughtyAttributes.Button]
        public void Add()
        {
            AddRow(_rows++);
        }

        [NaughtyAttributes.Button]
        public void Remove()
        {
            RemoveRow(2);
        }
    }
}