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

        private Transform _target;

        public void Execute(GameObject target)
        {
            _target = target.transform;
            InvokeRepeating("OnMove", 0, Time.deltaTime);
        }

        private void OnMove()
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
            if(Mathf.Abs(transform.position.y - _target.position.y) < 0.4f)
            {
                OnMoveComplete();
            }
        }

        private void OnMoveComplete()
        {
            CancelInvoke("OnMove");
            transform.position = _target.position;

            var row = int.Parse(_target.name.Split('_')[1]);
            var col = int.Parse(_target.name.Split('_')[2]);
            gameObject.name = $"Bubble_{row}_{col}";
            gameObject.transform.parent = null;

            Destroy(gameObject.GetComponent<Shoot>());
            Destroy(gameObject.GetComponent<Move>());

            FindObjectOfType<BubbleManager>().UpdateQueue();
            FindObjectOfType<Grid>().AddRow();

            _target.GetComponent<Cell>().bubble = gameObject.GetComponent<Bubble>();
            _target.GetComponent<MatchColors>().GroupBubbles();
        }
    }
}