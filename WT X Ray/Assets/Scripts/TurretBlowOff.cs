using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
    public class TurretBlowOff : MonoBehaviour, IVehicleComponentEffect
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]

        private bool _state = true;
        [SerializeField] private MeshCollider[] _allTurretAndCannonColliders;
        [SerializeField] private Rigidbody _turretRB;
        #endregion

        #region Functions

        #endregion

        #region Methods
        public void ResetEffect()
        {
            throw new System.NotImplementedException();
        }

        public void TriggerEffect()
        {
            if (_state)
                throw new System.NotImplementedException();
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
