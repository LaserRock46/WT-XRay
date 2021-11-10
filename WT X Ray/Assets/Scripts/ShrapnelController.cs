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
        [SerializeField] private SphereCollider _collider;
        [SerializeField] private LayerMask _armorLayerMask;

        [SerializeField] private HitPointsPool _hitPointsPool;

        private int _shrapnelDamage;
        private VehicleComponent.DamageMode _damageMode;

        private Vector3 _flightStartingPoint;
        private float _flightDistance;
        #endregion

        #region Functions

        #endregion



        #region Methods
       
       void FixedUpdate()
        {
            DetectArmorOvershoot();
        }
       
        public void Shot(VehicleComponent.DamageMode damageMode,float force, int shrapnelDamage = 15)
        {
            gameObject.SetActive(true);        
            _constantForce.force = force * transform.forward;
            _shrapnelDamage = shrapnelDamage;
            _damageMode = damageMode;

            FlightSetup();
            SetArmorOvershootDistance();
            if (damageMode == VehicleComponent.DamageMode.Visualisation)
            {
                _shrapnelTrail.gameObject.SetActive(true);
            }
            else
            {
                _shrapnelTrail.gameObject.SetActive(false);
            }
        } 
        void FlightSetup()
        {         
            RaycastHit hit;
           
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100.0f))
            {
                if (hit.collider.gameObject.TryGetComponent(out ArmorPanelAnimation armorPanelAnimation))
                {
                    SetTrailGradient(true);                 
                }
                else if (hit.collider.gameObject.TryGetComponent(out VehicleComponentCollider vehicleComponentCollider))
                {
                    SetTrailGradient(false);
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
        void SetTrailGradient(bool hitArmor)
        {
            if (hitArmor)
            {
                _shrapnelTrail.colorGradient = _armorDamageTrail;
            }
            else
            {
                _shrapnelTrail.colorGradient = _componentDamageTrail;
            }
        }
        void SetArmorOvershootDistance()
        {
            _flightStartingPoint = transform.position;
            RaycastHit hit;
         
            if (Physics.SphereCast(transform.position,_collider.radius, transform.forward, out hit, Mathf.Infinity, _armorLayerMask))
            {
                _flightDistance = Vector3.Distance(hit.point, _flightStartingPoint);
                //Debug.DrawLine(_flightStartingPoint, hit.point, Color.green, 1f);
            }
        }
        void DetectArmorOvershoot()
        {
            if (_rigidbody.velocity != Vector3.zero)
            {
                if (Vector3.Distance(_flightStartingPoint, transform.position) > _flightDistance + _collider.radius)
                {
                    StopShrapnel();
                    StartCoroutine(CountdownAfterArmorPanelHit());
                    //Debug.Log(nameof(DetectArmorOvershoot));
                    //Debug.DrawLine(_flightStartingPoint, transform.position, Color.red, 1f);
                }
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
            }
        }
        #endregion
    }
}
