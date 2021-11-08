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

        private int _shrapnelDamage;
        private VehicleComponent.DamageMode _damageMode;
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
       
        public void Shot(VehicleComponent.DamageMode damageMode,float force, int shrapnelDamage = 15)
        {
            gameObject.SetActive(true);        
            _constantForce.force = force * transform.forward;
            _shrapnelDamage = shrapnelDamage;
            _damageMode = damageMode;

            if (damageMode == VehicleComponent.DamageMode.Visualisation)
            {
                _shrapnelTrail.gameObject.SetActive(true);
                SetTrailGradient();
            }
            else
            {
                _shrapnelTrail.gameObject.SetActive(false);
            }
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
                else
                {
                    ResetShrapnel();
                }
            }
            else
            {
                ResetShrapnel();
            }

        }      
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out VehicleComponentCollider vehicleComponentCollider))
            {
                Hit(vehicleComponentCollider);
            }
            if (other.gameObject.TryGetComponent(out ArmorPanelAnimation armorPanelAnimation))
            {
                ResetShrapnel();
            }
        }
        void Hit(VehicleComponentCollider vehicleComponentCollider)
        {
            vehicleComponentCollider.vehicleComponent.Hit(_shrapnelDamage,_damageMode);
        }
        void ResetShrapnel()
        {
            _rigidbody.velocity = Vector3.zero;
            transform.localPosition = Vector3.zero;
            _shrapnelTrail.Clear();
            gameObject.SetActive(false);
        }
        #endregion
    }
}
