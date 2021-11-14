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

        [SerializeField] private float _shrapnelRadius;

        private int _shrapnelDamage;
        private VehicleComponent.DamageMode _damageMode;     
        private List<RaycastHit> _shrapnelTargets = new List<RaycastHit>();    
        private Transform _parent;

        [SerializeField] private FlightController _flightController;
        #endregion

        #region Functions

        #endregion



        #region Methods       
        public void Shot(Vector3 explosionPosition,Transform shrapnelParent, VehicleComponent.DamageMode damageMode,float shrapnelSpeedForce, int shrapnelDamage = 15)
        {
            _parent = shrapnelParent;
            transform.SetParent(null);
            gameObject.SetActive(true);        
            _shrapnelDamage = shrapnelDamage;
            _damageMode = damageMode;
            transform.position = explosionPosition;
            _flightController.FlightSetup(shrapnelSpeedForce);
            DisableShrapnelWithoutTarget();
            if (gameObject.activeSelf)
            {
                GenerateShrapnelCollisions();
            }
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
            RaycastHit[] targets = Physics.RaycastAll(transform.position, transform.forward, _flightController.flightDistance);
            _shrapnelTargets.Clear();
            _shrapnelTargets.AddRange(targets);
            StartCoroutine(CollisionUpdate());
        }     
        IEnumerator CollisionUpdate()
        {          
            while (_flightController.coverage < _flightController.flightDistance) 
            {            
                ExecuteShrapnelCollision(_flightController.coverage);           
                yield return null;
            }
            ExecuteShrapnelCollision(_flightController.flightDistance);
            StartCoroutine(CountdownAfterArmorPanelHit());
            yield return null;
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
