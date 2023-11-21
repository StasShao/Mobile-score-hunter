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
        #region Variables
        private Animator _searcherAnimator;
        private Controller _controller;
        private Transform _wayPoint;
        #endregion

        #region CTOR
        public AIController(Controller controller, Animator searcherAnimator, Transform wayPoint)
        {
            _controller = controller;
            _searcherAnimator = searcherAnimator;
            _wayPoint = wayPoint;
        }
        #endregion

        #region Voides
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
        #endregion
    }
    public class Animatronic
    {
        private Animator _animator;
        public Animatronic(Animator animator)
        {
            _animator = animator;
        }

        public void PlayAnimationTrigger(string animationName)
        {
            _animator.SetTrigger(animationName);
        }

    }

}
namespace ActionSystems
{
    public class ActionInvoker<T>where T:Object
    {
        public static void InvokeMethod(Object invokedClass,string methodName)
        {
            
            var obj = (T)invokedClass;
            var methodInfo = obj.GetType().GetMethod(methodName);
            if(methodInfo != null)
            {
                var parameters = methodInfo.GetParameters();
                methodInfo.Invoke(invokedClass, parameters);
            }
        }
        public static void InvokeMethods(List<Object>invokedClasses,string methodName)
        {
            for (int i = 0; i < invokedClasses.Count; i++)
            {
                var obj = (T)invokedClasses[i];
                var methodInfo = obj.GetType().GetMethod(methodName);
                if (methodInfo != null)
                {
                    var parameters = methodInfo.GetParameters();
                    methodInfo.Invoke(invokedClasses[i], parameters);
                }
            }
        }
    }
    public class Damager
    {
        private Collider _cahedCollider;
        private IDamage _idamage;
        public Damager()
        {

        }
        public void OnColissionDamage(Collision col,int damage,string collideTag)
        {
            if(col.collider.tag == collideTag&&col.collider != _cahedCollider)
            {
                _cahedCollider = col.collider;
                col.collider.TryGetComponent<IDamage>(out IDamage idamage);
                _idamage = idamage;
                _idamage.Damage(damage);
                Debug.Log("Not Cached");
                Debug.Log(_idamage.Health);
                return ;
            }
            if(col.collider.tag == collideTag&&col.collider == _cahedCollider)
            {
                _idamage.Damage(damage);
                Debug.Log("Cached");
                Debug.Log(_idamage.Health);
            }
        }
        public void OnTriggerDamage(Collider col,int damage,string colliderTagName)
        {
            if(col.tag == colliderTagName && col != _cahedCollider)
            {
                _cahedCollider = col;
                col.TryGetComponent<IDamage>(out IDamage idamage);
                _idamage = idamage;
                _idamage.Damage(damage);
                _cahedCollider.gameObject.SetActive(false);
                Debug.Log("Not Cached");
                Debug.Log(_idamage.Health);
                return;
            }
            if (col.tag == colliderTagName && col == _cahedCollider)
            {
                _idamage.Damage(damage);
                _cahedCollider.gameObject.SetActive(false);
                Debug.Log(_idamage.Health);
                Debug.Log("Cached");
            }
        }
    }
    public class Pointer
    {
        private GameObject _go;
        public Pointer(GameObject go)
        {
            _go = go;
        }
        public void OnPointAdd(Collider col,string attachedColliderTag,int score)
        {
            if(col.tag == attachedColliderTag)
            {
                if(col.TryGetComponent<ISetScore>(out ISetScore isetScore))
                {
                    isetScore.AddPoints(score);
                    Debug.Log(isetScore.PointsCount);
                    _go.SetActive(false);
                }

            }
        }
    }

    public class PLayerStatistics
    {
        private static bool _isActive;
        public static List<bool> _isItemsActive = new List<bool>();
        public static void AddScorePoints(int score,ISetScore isetScore)
        {
            isetScore.AddPoints(score);
        }
        public static void OnItemDisable(List<Item> itemList,Object invokedClass,string methodName)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if(itemList[i] != null)
                {
                    if(itemList[i].gameObject.activeInHierarchy)
                    {
                        _isItemsActive[i] = true;
                    }
                    if (!itemList[i].gameObject.activeInHierarchy & _isItemsActive[i])
                    {
                        ActionSystems.ActionInvoker<PointSpawner>.InvokeMethod(invokedClass, methodName);
                        _isItemsActive[i] = false;
                    }
                }
            }
        }
        public static void OnPLayerDisable(Player player,Object invokedClass,string methodName)
        {
            
            if(player != null)
            {
                if(player.gameObject.activeInHierarchy)
                {
                    _isActive = true;
                }
                if(!player.gameObject.activeSelf&_isActive)
                {
                    ActionSystems.ActionInvoker<PlayerSpawner>.InvokeMethod(invokedClass, methodName);
                    _isActive = false;
                }
                
            }
        }
        public static void PlayerHit(Player player,Object invokedClass,string methodName,IHealth ihealth)
        {
            if(ihealth.CurentHealth > ihealth.Health)
            {
                ihealth.SetCurentHealth(ihealth.Health);
                ActionSystems.ActionInvoker<Player>.InvokeMethod(invokedClass, methodName);
            }
        }
    }
}
namespace PoolSystems
{
    using System;
    
    /// <summary>
    /// Клас generic реализует пул объектов
    /// </summary>
    /// <typeparam name="T"></typeparam>
        public class PoolMono<T> where T : MonoBehaviour
        {
          #region Variables
            public T prefab { get; }
            public bool IsAutoExpand { get; set; }
            public Transform container { get; }
            public List<T> pool { get; private set; }
        #endregion

          #region CTOR
        public PoolMono(T prefab, int count, Transform container,bool isAutoExpand)
        {
             this.prefab = prefab;
             this.container = container;
             this.IsAutoExpand = isAutoExpand;
             this.CreatePool(count);
        }
        #endregion

          #region VOides
        private void CreatePool(int count)
        {
          this.pool = new List<T>();
          for (int i = 0; i < count; i++)
          {
            this.CreateObject();
          }
        }
        private T CreateObject(bool isActiveByDefoult = false)
            {
                var createdObject = PoolCreator<T>.Instance(prefab,container);
                createdObject.gameObject.SetActive(isActiveByDefoult);
                this.pool.Add(createdObject);
                return createdObject;

            }
        public bool HasFreeElement(out T element)
            {
                foreach (var mono in pool)
                {
                    if (!mono.gameObject.activeInHierarchy)
                    {
                        element = mono;
                        mono.gameObject.SetActive(true);
                        return true;
                    }
                }
                element = null;
                return false;
            }
        public T GetFreeElement(Transform pos)
            {
            if (this.HasFreeElement(out var element))
            {
                element.transform.position = pos.position;
                element.transform.rotation = pos.rotation;
                return element;
            }
            if (this.IsAutoExpand)
            {
                return this.CreateObject(true);
            }
            return null;
               /* throw new Exception($"There is no element of type {typeof(T)}");*/
            
            }
        #endregion 
    }
    public class PoolCreator<T>:MonoBehaviour where T:MonoBehaviour
    {
        public static T Instance(T prefab,Transform container)
        {
            var createdObject = Instantiate(prefab,container.position,container.rotation,container);
            return createdObject;
        }
    }
    
}

