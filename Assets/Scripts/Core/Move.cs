using System.Collections.Generic;
using UnityEngine;

namespace BubblePops.Core
{
    public class Move : MonoBehaviour
    {
        /// <summary>
        /// direction the bubble is facing
        /// </summary>
        [SerializeField] private Vector2 _direction = Vector2.zero;

        /// <summary>
        /// speed at which the bubble is moving
        /// </summary>
        [SerializeField] private float _speed = 6f;

        /// <summary>
        /// 
        /// </summary>
        private Grid _grid = null;
        
        /// <summary>
        /// 
        /// </summary>
        private Transform _target = null;

        /// <summary>
        /// 
        /// </summary>
        private Queue<Transform> _targets = new Queue<Transform>();

        /// <summary>
        /// 
        /// </summary>
        private Generator _generator = null;

        private void Awake()
        {
            _grid = FindObjectOfType<Grid>();
            _generator = FindObjectOfType<Generator>();
        }

        public void Execute(GameObject target)
        {
            _target = target.transform;
            InvokeRepeating("OnMove", 0, Time.deltaTime);
        }

        public void Execute(Queue<Transform> targets)
        {
            _targets = targets;
            SetTarget(_targets.Dequeue().transform);
        }

        private void SetTarget(Transform target)
        {
            _target = target;
            InvokeRepeating("OnMove", 0, Time.deltaTime);
        }

        private void OnMove()
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);

            if(Mathf.Abs(transform.position.y - _target.position.y) < 0.4f)
            {
                OnMoveComplete();
                if(_targets.Count > 0)
                {
                    SetTarget(_targets.Dequeue().transform);
                }
                else
                {
                    PostMoveCompelte();
                }
            }
        }

        private void OnMoveComplete()
        {
            CancelInvoke("OnMove");
            transform.position = _target.position;
        }

        private void PostMoveCompelte()
        {
            var row = int.Parse(_target.name.Split('_')[1]);
            var col = int.Parse(_target.name.Split('_')[2]);
            gameObject.name = $"Bubble_{row}_{col}";
            gameObject.transform.parent = _generator.transform;

            Destroy(gameObject.GetComponent<Move>());

            FindObjectOfType<BubbleManager>().UpdateQueue();

            var cell = _target.GetComponent<Cell>();
            cell.SetBubble(gameObject.GetComponent<Bubble>());
            _grid.GetComponent<MatchColors>().GroupBubbles(cell);
        }
    }
}