using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
    public class TurretBlowOff : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]

        private bool _state = true;
        private bool _blowOffPerformed;

        [SerializeField] private MeshCollider[] _allNonConvexTurretAndCannonColliders;
        [SerializeField] private Collider[] _turretTankersColliders;
        [SerializeField] private Collider[] _turretArmorColliders;

        [SerializeField] private Rigidbody _turretRB;
        [SerializeField] private GameObject _fireStream;
        [SerializeField] private float _upForce;
        [SerializeField] private float _torque;
        [SerializeField] private Transform _blowOffDirection;

        private Vector3 _initialTurretPosition;
        private Quaternion _initialTurretRotation;
        #endregion

        #region Functions

        #endregion

        #region Methods

        void Start()
        {
            _initialTurretPosition = transform.position;
            _initialTurretRotation = transform.rotation;        
        }
        public void ResetEffect()
        {
            if (_blowOffPerformed)
            {
            _blowOffPerformed = false;
            SetupRigidbodyAndColliders(false);
            transform.SetPositionAndRotation(_initialTurretPosition, _initialTurretRotation);
            _fireStream.SetActive(false);
            }
        }

        public void TriggerEffect()
        {
            if (_state && _blowOffPerformed == false)
            {
                _blowOffPerformed = true;
                SetupRigidbodyAndColliders(true);
                StartCoroutine(BlowOff());
                StartCoroutine(ReenableTurretColliders());
            }

        }
        IEnumerator BlowOff()
        {
            yield return new WaitForSeconds(0.75f);
            _turretRB.isKinematic = false;
            _turretRB.AddForce(_blowOffDirection.up * _upForce, ForceMode.Impulse);
            _turretRB.AddTorque(Random.insideUnitSphere * _torque, ForceMode.Impulse);
            _fireStream.SetActive(true);
            yield return null;

        }
        void SetupRigidbodyAndColliders(bool blowOff)
        {
            if (blowOff)
            {
                foreach (MeshCollider item in _allNonConvexTurretAndCannonColliders)
                {
                    item.convex = true;               
                }
                foreach (Collider item in _turretArmorColliders)
                {
                   item.enabled = false;
                }
                foreach (Collider item in _turretTankersColliders)
                {
                    item.enabled = false;
                }
             
            }
            else
            {
                foreach (MeshCollider item in _allNonConvexTurretAndCannonColliders)
                {
                    item.convex = false;
                }             
                foreach (Collider item in _turretTankersColliders)
                {
                    item.enabled = true;
                }              

                _turretRB.isKinematic = true;
                _turretRB.velocity = Vector3.zero;
                _turretRB.angularVelocity = Vector3.zero;
            }
        }
        IEnumerator ReenableTurretColliders()
        {
            yield return new WaitForSeconds(1.25f);
            foreach (Collider item in _turretArmorColliders)
            {
                item.enabled = true;
            }
            yield return null;
        }
        public void SetState(bool state)
        {
            _state = state;
            if (_state == false)
            {
                ResetEffect();
            }
        }
        #endregion
    }
}
