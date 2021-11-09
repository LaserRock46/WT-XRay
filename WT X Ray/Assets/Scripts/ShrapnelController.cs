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

        [SerializeField] private TrailRenderer _shrapnelTrail;
        [SerializeField] private Gradient _componentDamageTrail;
        [SerializeField] private Gradient _armorDamageTrail;

        [SerializeField] private ConstantForce _constantForce;
        [SerializeField] private Rigidbody _rigidbody;

        [SerializeField] private HitPointsPool _hitPointsPool;

        private int _shrapnelDamage;
        private VehicleComponent.DamageMode _damageMode;
        #endregion

        #region Functions

        #endregion



        #region Methods
       
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
                DisableEverySecondShrapnel();
                
            }
            if (other.gameObject.TryGetComponent(out ArmorPanelAnimation armorPanelAnimation))
            {
                StopShrapnel();
                StartCoroutine(CountdownAfterArmorPanelHit());
            }

        }
        void Hit(VehicleComponentCollider vehicleComponentCollider)
        {
            vehicleComponentCollider.vehicleComponent.Hit(_shrapnelDamage,_damageMode);
            _hitPointsPool.GetHitPointHere(transform.position);
        }
        private IEnumerator CountdownAfterArmorPanelHit()
        {
            yield return new WaitForSeconds(1f);
            ResetShrapnel();
        }
        void ResetShrapnel()
        {
            StopShrapnel();
            transform.localPosition = Vector3.zero;
            _shrapnelTrail.Clear();
            gameObject.SetActive(false);
        }
        void StopShrapnel()
        {
            _constantForce.force = Vector3.zero;
            _rigidbody.velocity = Vector3.zero;          
        }
        void DisableEverySecondShrapnel()
        {
            if(transform.GetSiblingIndex() % 2 is 0)
            {
                StopShrapnel();
                StartCoroutine(CountdownAfterArmorPanelHit());
                Debug.Log("Disabled");
            }
        }
        #endregion
    }
}
