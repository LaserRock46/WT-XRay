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

        [SerializeField] private HitPointsPool _hitPointsPool;

        [SerializeField] private float _shrapnelSpeed;
        [SerializeField] private float _shrapnelRadius;

        [SerializeField] private LayerMask _armorLayerMask;
        private int _shrapnelDamage;
        private VehicleComponent.DamageMode _damageMode;     
        private List<RaycastHit> _shrapnelTargets = new List<RaycastHit>();

      
        private float _flightDistance;
        private Vector3 _flightStart;
        private Vector3 _flightEnd;
        private Transform _parent;
        #endregion

        #region Functions

        #endregion



        #region Methods         
        public void Shot(Transform shrapnelParent, VehicleComponent.DamageMode damageMode,float force, int shrapnelDamage = 15)
        {
            _parent = shrapnelParent;
            transform.SetParent(null);
            gameObject.SetActive(true);
            _shrapnelSpeed = force;
            _shrapnelDamage = shrapnelDamage;
            _damageMode = damageMode;

            FlightSetup();
            DisableShrapnelWithoutTarget();
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
        void FlightSetup()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 100.0f,_armorLayerMask))
            {       
                _flightDistance = Vector3.Distance(transform.position, hit.point);
                _flightStart = transform.position;
                _flightEnd = hit.point;
                GenerateShrapnelCollisions();
                StartCoroutine(Flight());
            }
        }
        IEnumerator Flight()
        {
            float coverage = 0;
            while (coverage < _flightDistance) 
            {
                float step = _shrapnelSpeed * Time.deltaTime;
                coverage += step;
               
                transform.position += transform.forward * step;
                float time =Mathf.Clamp(Mathf.InverseLerp(0,_flightDistance,coverage),0,1);
                transform.position = Vector3.Lerp(_flightStart,_flightEnd,time);
                ExecuteShrapnelCollision(coverage);           
                yield return null;
            }
            ExecuteShrapnelCollision(_flightDistance);
            StartCoroutine(CountdownAfterArmorPanelHit());
            yield return null;
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
        void DisableShrapnelWithoutTarget()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 100.0f))
            {
                if (hit.collider.gameObject.layer == 0)
                {
                    ResetShrapnel();
                }            
            }
            else
            {
                ResetShrapnel();
            }
        }
        void GenerateShrapnelCollisions()
        {       
            RaycastHit[] targets = Physics.RaycastAll(transform.position, transform.forward, _flightDistance);
            _shrapnelTargets.AddRange(targets);
        }      
        private void ExecuteShrapnelCollision(float coverage)
        {
            for (int i = 0; i < _shrapnelTargets.Count; i++)
            {
                if (coverage >= _shrapnelTargets[i].distance)
                {
                    if (_shrapnelTargets[i].collider.gameObject.TryGetComponent(out VehicleComponentCollider vehicleComponentCollider))
                    {
                        Hit(vehicleComponentCollider, _shrapnelTargets[i].point);
                        DisableEverySecondShrapnel();
                    }
                    if (_shrapnelTargets[i].collider.TryGetComponent(out ArmorPanelAnimation armorPanelAnimation))
                    {
                        StartCoroutine(CountdownAfterArmorPanelHit());
                    }
                    _shrapnelTargets.RemoveAt(i);
                }                    
            }
            if(_shrapnelTargets.Count == 1 && _shrapnelTrail.colorGradient == _componentDamageTrail)
            {
                ResetShrapnel();
            }
        }
        void Hit(VehicleComponentCollider vehicleComponentCollider,Vector3 collsionPoint)
        {
            vehicleComponentCollider.vehicleComponent.Hit(_shrapnelDamage,_damageMode);
            _hitPointsPool.GetHitPointHere(collsionPoint);
        }
        private IEnumerator CountdownAfterArmorPanelHit()
        {
            yield return new WaitForSeconds(1f);         
            ResetShrapnel();
        }
        void ResetShrapnel()
        {
            transform.SetParent(_parent);
            transform.localPosition = Vector3.zero;
            _shrapnelTrail.Clear();
            gameObject.SetActive(false);          
        }    
        void DisableEverySecondShrapnel()
        {
            if(transform.GetSiblingIndex() % 2 is 0)
            {              
                StartCoroutine(CountdownAfterArmorPanelHit());
            }
        }
        #endregion
    }
}
