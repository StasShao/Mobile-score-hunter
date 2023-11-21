using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterMechanicSystems
{
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
        private void MoveToDirection(Vector3 directionPoint,float moveForce,float stopDistance,float rotateSpeed)
        {
            if(_characterRigidbody != null)
            {
                var distance = Vector3.Distance(_characterRigidbody.transform.position,directionPoint);
                var direction = directionPoint - _characterRigidbody.transform.position;
                var directionXZ = new Vector3(direction.x,0,direction.z);
                var lookRotation = Quaternion.LookRotation(directionXZ);
                if (distance <= stopDistance) return;
                _characterRigidbody.AddForce(directionXZ * moveForce,ForceMode.Acceleration);
                _characterRigidbody.rotation = Quaternion.Slerp(_characterRigidbody.rotation,lookRotation,rotateSpeed * Time.deltaTime);
            }
        }
        public Vector3 SetDirection(Camera cam,LayerMask interactionLayer)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,_cameraRayDistance,interactionLayer))
            {
                _cameraRayDistance = hit.distance;
                _targetPointDistance = Vector3.Distance(_characterTransform.position,hit.point);
                return hit.point;
            }
            else
            {
                _cameraRayDistance = 900;
                return _characterTransform.position;
            }
           
        }
        public void OnClickMove(Vector3 point,float moveForce,float stopDistance,float rotateSpeed)
        {
            
            /*if (_targetPointDistance <= stopDistance) return;*/
            MoveToDirection(point, moveForce,stopDistance,rotateSpeed);
        }
        #endregion
    }
}