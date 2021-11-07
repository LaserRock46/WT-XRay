using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
   [AddComponentMenu(nameof(Project) + "/" + nameof(Uncategorized) + "/ShrapnelController")]
    public class ShrapnelController : MonoBehaviour
    {
         #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]     
        private MeshRenderer _hitPointRenderer = null;
        [SerializeField] private AnimationCurve _alphaAnimationCurve = default;

        [SerializeField] private TrailRenderer _shrapnelTrail;
        [SerializeField] private Gradient _componentDamageTrail;
        [SerializeField] private Gradient _armorDamageTrail;

        [SerializeField] private ConstantForce _constantForce;
        [SerializeField] private Rigidbody _rigidbody;
        #endregion

        #region Functions

        #endregion



        #region Methods
        void Start()
        {
            _hitPointRenderer = GetComponent<MeshRenderer>();       
        }
       void Update()
        {
            
        }
       
        public void Shot(float force)
        {
            _shrapnelTrail.Clear();
            transform.localPosition = Vector3.zero;
            _rigidbody.velocity = Vector3.zero;
            _constantForce.force = force * transform.forward;
            
            SetTrailGradient();
        } 
        void SetTrailGradient()
        {
            RaycastHit hit;
           
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100.0f))
            {
                if (hit.collider.gameObject.TryGetComponent(out ArmorPanelAnimation armorPanelAnimation))
                {
                    _shrapnelTrail.colorGradient = _armorDamageTrail;
                }
                else if (hit.collider.gameObject.TryGetComponent(out VehicleComponentCollider vehicleComponentCollider))
                {
                    _shrapnelTrail.colorGradient = _componentDamageTrail;
                }
            }

        }
        #endregion
    }
}
