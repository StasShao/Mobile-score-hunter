using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterMechanicSystems
{
    using UnityEngine.AI;
    /// <summary>
    /// Класс реализующий управление перемещением и поворотами
    /// </summary>
    public class Controller
    {
        #region Variables
        private Transform _characterTransform;
        private Rigidbody _characterRigidbody;
        private float _cameraRayDistance;
        private float _targetPointDistance;
        #endregion

        #region CTOR
        /// <summary>
        /// Конструктор класса Controller
        /// </summary>
        /// <param name="characterTransform"></param>
        public Controller(Transform characterTransform)
        {
            _characterTransform = characterTransform;
            _characterTransform.TryGetComponent<Rigidbody>(out Rigidbody rb);
            #region Try get Rigidbody
            if (rb != null)
            {
                _characterRigidbody = rb;
            }
            #endregion
        }
        #endregion

        #region Voides
        public void MoveToDirection(Vector3 directionPoint,float moveForce,float stopDistance,float rotateSpeed)
        {
            if(_characterRigidbody != null)
            {
                var distance = Vector3.Distance(_characterRigidbody.transform.position,directionPoint);
                var direction = directionPoint - _characterRigidbody.transform.position;
                var directionXZ = new Vector3(direction.x,0,direction.z);
                var lookRotation = Quaternion.LookRotation(directionXZ);
                if (distance <= stopDistance) return;
                _characterRigidbody.AddForce(directionXZ * moveForce,ForceMode.Force);
                _characterRigidbody.rotation = Quaternion.Slerp(_characterRigidbody.rotation,lookRotation,rotateSpeed * Time.deltaTime);
            }
        }
        public Vector3 DirectionMouse(Camera cam,LayerMask interactionLayer,IControllable icontrollable)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,_cameraRayDistance,interactionLayer))
            {
                _cameraRayDistance = hit.distance;
                _targetPointDistance = Vector3.Distance(_characterTransform.position,hit.point);
                icontrollable.SetDirectionPoint(hit.point);
                return hit.point;
            }
            else
            {
                _cameraRayDistance = 900;
                icontrollable.SetDirectionPoint(_characterTransform.position);
                return _characterTransform.position;
            }
           
        }
        public void NavigationMoveToDirection(NavMeshAgent agent,Vector3 point)
        {
            agent.SetDestination(point);
        }
        public void OnClickMove(Vector3 point,float moveForce,float stopDistance,float rotateSpeed)
        {

            if (_targetPointDistance <= stopDistance) return;
            MoveToDirection(point, moveForce,stopDistance,rotateSpeed);
        }
        #endregion
        
    }
    public class AIController
    {

        private Animator _searcherAnimator;
        private Controller _controller;
        private Transform _wayPoint;
        private Collider _wayPointColliderTrace;
        public AIController(Controller controller, Animator searcherAnimator, Transform wayPoint)
        {
            _controller = controller;
            _searcherAnimator = searcherAnimator;
            _wayPoint = wayPoint;
        }
        public void OnMoveBehavior(bool isDetected, NavMeshAgent agent, Transform target)
        {
            if (isDetected)
            {
                if (target == null) return;
                _controller.NavigationMoveToDirection(agent, target.position);
            }
            else
            {
                _controller.NavigationMoveToDirection(agent, _wayPoint.position);
            }
        }
        public void EnemySearching(IAIControllable iaiControllable, float searchDistance, LayerMask targetLayer)
        {
            Ray ray = new Ray(_searcherAnimator.transform.position, _searcherAnimator.transform.forward * searchDistance);
            Debug.DrawRay(ray.origin, ray.direction * searchDistance, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, searchDistance, targetLayer))
            {
                iaiControllable.SetEnemy(hit.collider.transform);
                iaiControllable.SetEnemyDetected(true);
                _searcherAnimator.enabled = false;
                _searcherAnimator.transform.LookAt(iaiControllable.EnemyTransform);
            }
            else
            {
                iaiControllable.SetEnemy(null);
                iaiControllable.SetEnemyDetected(false);
                _searcherAnimator.enabled = true;
            }
        }
        public void WayPointRandomizerPosition(float changePositionDistance,LayerMask traceColliderLayer,Transform defoultParent)
        {
            var distanceToAi = Vector3.Distance(_searcherAnimator.transform.position, _wayPoint.position);
             _wayPoint.parent = defoultParent;
            if (distanceToAi > changePositionDistance) return;
            Ray ray = new Ray(_wayPoint.position,-_wayPoint.up * 10);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,10,traceColliderLayer))
            {
                    _wayPoint.parent = hit.transform;
                    var randomPoint = new Vector3(Random.Range(-0.3f,0.3f),1f, Random.Range(-0.3f, 0.3f));
                    _wayPoint.localPosition = randomPoint;
            }
            

        }
    }

}