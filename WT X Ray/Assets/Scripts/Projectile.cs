using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Uncategorized
{
    [AddComponentMenu(nameof(Project) + "/" + nameof(Uncategorized) + "/Projectile")]
    public class Projectile : MonoBehaviour
    {
        #region Temp
        [Header("Temporary Things", order = 0)]
        public Collider[] hits;
        public float coverage;
        public float[] distance;

        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private bool _inAction;
        public bool InAction { get { return _inAction; } private set { _inAction = value; } }

        [SerializeField] private ShrapnelController[] _shrapnels;
        [SerializeField] private float _shrapnelForce = 0.5f;
        [SerializeField] private float _projectileForce = 1f;       
        [SerializeField] private float _forceSimulation = 1000f;
        [SerializeField] private int _shrapnelDamage = 15;
        [SerializeField] private int _projectileDamage = 500;
        [SerializeField] private float _afterPenetrationOffset = 0.1f;
        [SerializeField] private float _countdownAfterSecondPenetration = 0.5f;
        [SerializeField] private VehicleComponent.DamageMode _damageMode;
        [SerializeField] private FlightController _flightController;
        [SerializeField] private HitPointsPool _hitPointsPool;
        [SerializeField] private SilhouetteController _silhouetteController;
        [SerializeField] private Material _projectileMat;

        private List<RaycastHit> _projectileTargets = new List<RaycastHit>();
        private bool[] _executedHit;

        private bool _firstHullPenetrationPerformed = false;   
        private Vector3 _penetrationPosition = new Vector3();
        private bool _exploded = false;
        [SerializeField] private Transform _parent;

        [SerializeField] private HitCamera _hitCamera;


        #endregion

        #region Functions

        #endregion



        #region Methods
        private void Update()
        {
            hits = new Collider[_projectileTargets.Count];
            distance = new float[_projectileTargets.Count];
            coverage = _flightController.coverage;
            for (int i = 0; i < _projectileTargets.Count; i++)
            {
                hits[i] = _projectileTargets[i].collider;
                distance[i] = _projectileTargets[i].distance;
            }
        }
        [ContextMenu("Test Projectile")]
        void TestProjectile()
        {
            Shot();
        }
        public void Shot()
        {
            gameObject.SetActive(true);
            _inAction = true;
            _flightController.FlightSetupAll(_damageMode == VehicleComponent.DamageMode.Visualisation ? _projectileForce : _forceSimulation, 0, true);
            GenerateProjectileCollisions();

            if (_hitCamera)
            {
                _hitCamera.TrackProjectile(transform);
            }
        }
        void Explode(Vector3 position)
        {
            if (_exploded == false)
            {
                _exploded = true;
                foreach (ShrapnelController shrapnel in _shrapnels)
                {
                    shrapnel.Shot(position, transform, _damageMode, _damageMode == VehicleComponent.DamageMode.Visualisation ? _shrapnelForce : _forceSimulation, _shrapnelDamage);
                }
            }
        }
        void GenerateProjectileCollisions()
        {
            RaycastHit[] targets = Physics.RaycastAll(transform.position, transform.forward, _flightController.flightDistance);
            _projectileTargets.Clear();
            _projectileTargets.AddRange(targets);
            _executedHit = new bool[targets.Length];
            int count = 0;
            for (int i = 0; i < _projectileTargets.Count; i++)
            {
                if (_projectileTargets[i].collider.gameObject.TryGetComponent(out VehicleComponentCollider vehicleComponentCollider))
                {
                    count++;
                }
            }
            StartCoroutine(CollisionUpdate());
        }
        IEnumerator CollisionUpdate()
        {
            while (_flightController.coverage < _flightController.flightDistance)
            {
                ExecuteProjectileCollision(_flightController.coverage);
                yield return null;
            }

            DetectSecondArmorPenetration();

            yield return null;
        }
        private void ExecuteProjectileCollision(float coverage)
        {
            for (int i = 0; i < _projectileTargets.Count; i++)
            {
                if (coverage >= _projectileTargets[i].distance && _executedHit[i] == false)
                {
                    if (_projectileTargets[i].collider.gameObject.TryGetComponent(out VehicleComponentCollider vehicleComponentCollider))
                    {
                        vehicleComponentCollider.vehicleComponent.Hit(_projectileDamage, _damageMode);
                        _hitPointsPool.GetHitPointHere(_projectileTargets[i].point);
                    }
                    if (_projectileTargets[i].collider.TryGetComponent(out ArmorPanelAnimation armorPanelAnimation))
                    {
                        armorPanelAnimation.Hit();
                        _hitPointsPool.GetHitPointHere(_projectileTargets[i].point, true);
                        DetectFirstArmorPenetration(_projectileTargets[i]);
                    }
                    _executedHit[i] = true;
                }
            }
        }
        void DetectFirstArmorPenetration(RaycastHit hit)
        {
            if (_firstHullPenetrationPerformed == false)
            {
                _firstHullPenetrationPerformed = true;
                _penetrationPosition = hit.point;

                Vector3 explosionPosition = hit.point + (transform.forward * _afterPenetrationOffset);
                Explode(explosionPosition);               
                _silhouetteController.Reveal(hit.point);

                if (_hitCamera)
                {
                    _hitCamera.RotateAroundHitPoint(_flightController);
                }
            }
        }
        void DetectSecondArmorPenetration() 
        {       
            if (_flightController.coverage >= _flightController.flightDistance)
            {
                StartCoroutine(FadeOutProjectile());
                StartCoroutine(CountdownAfterSecondPenetration());
            }
        }
    
       
        private IEnumerator CountdownAfterSecondPenetration()
        {
            yield return new WaitForSeconds(_countdownAfterSecondPenetration);
            ResetProjectile();
        }
        private IEnumerator FadeOutProjectile()
        {
            float alpha = 1;
            while (alpha > 0)
            {
                alpha -= Time.deltaTime;
                _projectileMat.SetFloat("_Alpha", alpha); 
                yield return null;
            }
            yield return null;
        }
        public void ResetProjectile()
        {
            _silhouetteController.Hide();
            gameObject.SetActive(false);
            transform.localPosition = Vector3.zero;
            _penetrationPosition = Vector3.zero;
            _firstHullPenetrationPerformed = false;
            _projectileMat.SetFloat("_Alpha", 1);
            _exploded = false;
            _inAction = false;
           
        }
        #endregion
    }
}
