using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
   [AddComponentMenu(nameof(Project) + "/" + nameof(Uncategorized) + "/HitCamera")]
    public class HitCamera : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Vector3 _offsetToTrackedProjectile;
        [SerializeField] private Vector3 _rotationWhileTracking;
        [SerializeField] private Transform _rotationTarget;
        [SerializeField] private Transform _camera;
        [SerializeField] private AnimationCurve _rotationProgress;
        #endregion

        #region Functions
        
        #endregion

        

        #region Methods     
        public void TrackProjectile(Transform projectile)
        {
            transform.SetParent(projectile);
            transform.localPosition = Vector3.zero;
            transform.rotation = projectile.rotation;
            _camera.localPosition = _offsetToTrackedProjectile;
            _camera.localEulerAngles = _rotationWhileTracking;        
        }
        public void RotateAroundHitPoint(FlightController flightController)
        {
            transform.parent = null;
            StartCoroutine(Rotate(flightController));
        }
        IEnumerator Rotate(FlightController flightController)
        {         
            Quaternion _rotStart = transform.rotation;
            Quaternion _rotEnd = _rotationTarget.rotation;
            Vector3 _postionStart = Vector3.zero;
            Vector3 _positionEnd =flightController.FlightStart + (transform.InverseTransformDirection(-_camera.forward) * (flightController.FlightDistance*3));

            while (flightController.coverage < flightController.FlightDistance)
            {
                float progress = Mathf.InverseLerp(0, flightController.FlightDistance, flightController.coverage);
                transform.rotation = Quaternion.Slerp(_rotStart, _rotEnd, _rotationProgress.Evaluate(progress));
                _camera.localPosition = Vector3.Lerp(_postionStart,_positionEnd,progress);
                yield return null;
            }
            yield return null;
        }
        #endregion
    }
}
