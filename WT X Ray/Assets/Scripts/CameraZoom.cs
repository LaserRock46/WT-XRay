using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class CameraZoom : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private float _min;
        [SerializeField] private float _max;
        [SerializeField] private float _target;

        #endregion

        #region Functions

        #endregion

        #region Methods
        void Start()
        {
            
        }
        void Update()
        {
            _target += Input.GetAxis("Mouse ScrollWheel") * 3.5f;
            _target = Mathf.Clamp(_target, _min, _max);

            float z = Mathf.Lerp(transform.localPosition.z, -_target, Time.deltaTime * 10);
            transform.localPosition = new Vector3(0, 0, z);
        }
        #endregion
    }
}
