using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
    public class EngineFire : MonoBehaviour, IVehicleComponentEffect
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private GameObject _fire;
        private bool _state = true;
        #endregion

        #region Functions

        #endregion

        #region Methods

        public void ResetEffect()
        {
            _fire.SetActive(false);
        }

        public void TriggerEffect()
        {
            if (_state)
            _fire.SetActive(true);
        }
        public void SetState(bool state)
        {
            _state = state;
            if(_state == false)
            {
                ResetEffect();
            }
        }
        #endregion
    }
}
