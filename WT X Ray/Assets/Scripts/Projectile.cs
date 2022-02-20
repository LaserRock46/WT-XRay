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
        [SerializeField] private bool _inAction;
        public bool InAction { get { return _inAction; } private set { _inAction = value; } }


        [Header("Penetration Calculator Values")]
        [SerializeField] private int _armorPenetration;
        [SerializeField] private int _angleOfAttackNoRicochet;
        [SerializeField] private int _angleOfAttackRicochet;
        public int ArmorPenetration { get { return _armorPenetration; } private set { _armorPenetration = value; } }
        public int AngleOfAttackNoRicochet { get { return _angleOfAttackNoRicochet; } private set { _angleOfAttackNoRicochet = value; } }
        public int AngleOfAttackRicochet { get { return _angleOfAttackRicochet; } private set { _angleOfAttackRicochet = value; } }


        [Header("Damage Simulation Values")]
        [SerializeField] private ShrapnelController[] _shrapnels;
        [SerializeField] private float _blastRadius = 1;
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

        private bool _firstHullContactPerformed = false;   
        private Vector3 _firstContactPosition = new Vector3();
        private bool _exploded = false;
        [SerializeField] private Transform _parent;


        [Header("Hit Result Values")]
        [SerializeField] private PenetrationCalculator.PenetrationPossibility _hitResult;
    
        #endregion

        #region Functions

        #endregion



        #region Methods
        [ContextMenu("Test Projectile")]
        void TestProjectile()
        {
            Shot();
        }
        public void SetHitResult(PenetrationCalculator.PenetrationPossibility hitResult)
        {
            _hitResult = hitResult;
        }
        public void Shot()
        {
            gameObject.SetActive(true);
            _inAction = true;
          
            switch (_hitResult)
            {
                case PenetrationCalculator.PenetrationPossibility.Penetration_Is_Possible:
                    _flightController.FlightThroughTankSetup(_projectileForce, 0, true);
                    GenerateProjectileCollisions(true);
                    break;
                case PenetrationCalculator.PenetrationPossibility.Penetration_Not_Possible:
                    _flightController.FlightToTankSetup(_projectileForce);
                    GenerateProjectileCollisions(false);
                    break;
                case PenetrationCalculator.PenetrationPossibility.Ricochet:
                    _flightController.FlightToTankSetup(_projectileForce);
                    break;
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
        void GenerateProjectileCollisions(bool throughEntireTank)
        {
            RaycastHit[] targets = new RaycastHit[1];
            if (throughEntireTank)
            {
                targets = Physics.RaycastAll(transform.position, transform.forward, _flightController.flightDistance);
            }
            else
            {
                Physics.Raycast(transform.position, transform.forward,out targets[0], _flightController.flightDistance);
            }
            _projectileTargets.Clear();
            _projectileTargets.AddRange(targets);
            _executedHit = new bool[targets.Length];
          
            StartCoroutine(CollisionUpdate());
        }
        IEnumerator CollisionUpdate()
        {
            while (_flightController.coverage < _flightController.flightDistance)
            {
                ExecuteProjectileCollision(_flightController.coverage);
                yield return null;
            }

            DetectLastArmorContact();

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
                        if(_projectileTargets.Count == 1)
                        {
                            //DetectFirstArmorContact(_projectileTargets[i]);
                        }
                    }
                    if (_projectileTargets[i].collider.TryGetComponent(out ArmorPanelAnimation armorPanelAnimation))
                    {
                        armorPanelAnimation.Hit();
                        _hitPointsPool.GetHitPointHere(_projectileTargets[i].point, true);
                        //DetectFirstArmorContact(_projectileTargets[i]);
                    }
                    if (i == 0)
                    {
                        DetectFirstArmorContact(_projectileTargets[i]);
                        _hitPointsPool.GetHitPointHere(_projectileTargets[i].point, true, _blastRadius);
                    }
                    _executedHit[i] = true;
                }
            }
        }
        void DetectFirstArmorContact(RaycastHit hit)
        {
            if (_firstHullContactPerformed == false)
            {
                _firstHullContactPerformed = true;
                _firstContactPosition = hit.point;

                _silhouetteController.Reveal(hit.point);

                if(_hitResult == PenetrationCalculator.PenetrationPossibility.Penetration_Is_Possible)
                {
                Vector3 explosionPosition = hit.point + (transform.forward * _afterPenetrationOffset);
                Explode(explosionPosition);               
                }
           
            }
        }
        void DetectLastArmorContact() 
        {       
            if (_flightController.coverage >= _flightController.flightDistance)
            {
                StartCoroutine(FadeOutProjectile());
                StartCoroutine(CountdownAfterSecondPenetration());
            }
        }
        void Ricochet()
        {

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
            _firstContactPosition = Vector3.zero;
            _firstHullContactPerformed = false;
            _projectileMat.SetFloat("_Alpha", 1);
            _exploded = false;
            _inAction = false;
           
        }
        #endregion
    }
}
