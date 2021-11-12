using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
   [AddComponentMenu(nameof(Project) + "/" + nameof(Uncategorized) + "/Projectile")]
    public class Projectile : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]       
        [SerializeField] private ShrapnelController[] _shrapnels;
        [SerializeField] private float _shrapnelForce = 0.5f;
        [SerializeField] private float _projectileForce = 1f;
        [SerializeField] private float _forceSimulation = 1000f;
        [SerializeField] private int _shrapnelDamage = 15;
        [SerializeField] private int _projectileDamage = 500;
        [SerializeField] private float _afterPenetrationOffset = 0.1f;
        [SerializeField] private VehicleComponent.DamageMode _damageMode;
        [SerializeField] private FlightController _flightController;
        [SerializeField] private HitPointsPool _hitPointsPool;
        [SerializeField] private SilhouetteController _silhouetteController;

        private List<RaycastHit> _projectileTargets = new List<RaycastHit>();

        private bool _firstHullPenetrationPerformed = false;      
        private Vector3 _penetrationPosition = new Vector3();
        private bool _exploded = false;
        [SerializeField] private Transform _parent;
        #endregion

        #region Functions

        #endregion



        #region Methods
       
        [ContextMenu("Test Projectile")]
        void TestProjectile()
        {
           Shot();
        }
        public void Shot()
        {
            gameObject.SetActive(true);
            _flightController.FlightSetup(_damageMode == VehicleComponent.DamageMode.Visualisation ? _projectileForce : _forceSimulation);
            GenerateProjectileCollisions();
        }      
        void Explode(Vector3 position)
        {
            if (_exploded == false)
            {
                _exploded = true;
                foreach (ShrapnelController shrapnel in _shrapnels)
                {
                    shrapnel.Shot(position,transform, _damageMode, _damageMode == VehicleComponent.DamageMode.Visualisation ? _shrapnelForce : _forceSimulation, _shrapnelDamage);
                }
            }
        }
        void GenerateProjectileCollisions()
        {
            RaycastHit[] targets = Physics.RaycastAll(transform.position, transform.forward, _flightController.flightDistance);
            _projectileTargets.AddRange(targets);
            StartCoroutine(CollisionUpdate());
        }
        IEnumerator CollisionUpdate()
        {
            while (_flightController.coverage < _flightController.flightDistance)
            {            
                ExecuteProjectileCollision(_flightController.coverage);
                yield return null;
            }
            ExecuteProjectileCollision(_flightController.flightDistance);
           
            yield return null;
        }
        private void ExecuteProjectileCollision(float coverage)
        {
            for (int i = 0; i < _projectileTargets.Count; i++)
            {
                if (coverage >= _projectileTargets[i].distance)
                {
                    if (_projectileTargets[i].collider.gameObject.TryGetComponent(out VehicleComponentCollider vehicleComponentCollider))
                    {
                        vehicleComponentCollider.vehicleComponent.Hit(_projectileDamage, _damageMode);
                        _hitPointsPool.GetHitPointHere(_projectileTargets[i].point);
                    }
                    if (_projectileTargets[i].collider.TryGetComponent(out ArmorPanelAnimation armorPanelAnimation))
                    {
                        armorPanelAnimation.Hit();
                        _hitPointsPool.GetHitPointHere(_projectileTargets[i].point,true);
                        DetectArmorPenetration(_projectileTargets[i]);
                    }
                    
                    _projectileTargets.RemoveAt(i);
                }
            }           
        }
        void DetectArmorPenetration(RaycastHit hit)
        {
            if (_firstHullPenetrationPerformed == false)
            {
                _firstHullPenetrationPerformed = true;
                _penetrationPosition = hit.point;

                _flightController.FlightSetup(_damageMode == VehicleComponent.DamageMode.Visualisation ? _projectileForce : _forceSimulation,_afterPenetrationOffset,true);

                Vector3 explosionPosition = hit.point + (transform.forward * _afterPenetrationOffset);
                Explode(explosionPosition);
                GenerateProjectileCollisions();
                _silhouetteController.Reveal(hit.point);
            }
            if(_firstHullPenetrationPerformed && hit.point != _penetrationPosition)
            {
                StartCoroutine(CountdownAfterSecondPenetration());
            }           
        }
        private IEnumerator CountdownAfterSecondPenetration()
        {
            yield return new WaitForSeconds(0.4f);
            ResetProjectile();
        }
        void ResetProjectile()
        {
            _silhouetteController.Hide();
            gameObject.SetActive(false);
            transform.localPosition = Vector3.zero;
            _penetrationPosition = Vector3.zero;
            _firstHullPenetrationPerformed = false;
          
            _exploded = false;
        }
        #endregion
    }
}
