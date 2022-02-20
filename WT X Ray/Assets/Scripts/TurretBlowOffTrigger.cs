using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
    public class TurretBlowOffTrigger : MonoBehaviour, IVehicleComponentEffect
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private TurretBlowOff _turretBlowOff;

        public void ResetEffect()
        {         
            _turretBlowOff.ResetEffect();
        }

        public void TriggerEffect()
        {
            _turretBlowOff.TriggerEffect();
        }
        #endregion

        #region Functions

        #endregion

        #region Methods

        #endregion
    }
}
