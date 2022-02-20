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
        [SerializeField] private Collider[] _turretTankers;
        [SerializeField] private Collider[] _turretArmor;

        [SerializeField] private Rigidbody _turretRB;
        [SerializeField] private GameObject _fireStream;
        [SerializeField] private float _upForce;
        [SerializeField] private float _sideForce;
        [SerializeField] private float _torque;

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
                //transform.Translate(Vector3.up);
            }

        }
        IEnumerator BlowOff()
        {
            yield return new WaitForSeconds(1);
            _turretRB.isKinematic = false;

            foreach (MeshCollider item in _allNonConvexTurretAndCannonColliders)
            {
                //item.enabled = true;
            }

            _turretRB.AddForce(Vector3.up * _upForce, ForceMode.Impulse);
            _turretRB.AddForce(Random.insideUnitCircle * _sideForce, ForceMode.Impulse);
            _turretRB.AddTorque(Random.insideUnitSphere * _upForce, ForceMode.Impulse);
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
                foreach (Collider item in _turretArmor)
                {
                   item.enabled = false;
                }
                foreach (Collider item in _turretTankers)
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
                foreach (Collider item in _turretTankers)
                {
                    item.enabled = false;
                }

                _turretRB.isKinematic = true;
                _turretRB.velocity = Vector3.zero;
                _turretRB.angularVelocity = Vector3.zero;
            }
        }
        IEnumerator ReenableTurretColliders()
        {
            yield return new WaitForSeconds(2);
            foreach (Collider item in _turretArmor)
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
