using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
   [AddComponentMenu(nameof(Project) + "/" + nameof(Uncategorized) + "/ProjectileLauncher")]
    public class ProjectileLauncher : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private SimulationController _simulationController;
        [SerializeField] private PenetrationCalculator _penetrationCalculator;

        [SerializeField] private Projectile _HE105MM;
        [SerializeField] private Projectile _AP90MM;
        [SerializeField] private Projectile _APCR37MM;
        [SerializeField] private Projectile _machineGunAP;

        private Projectile _selectedProjectile;    
        [SerializeField] private PenetrationCalculator.ShellType _shellType;

        [SerializeField] private Transform _projectileParent;
        [SerializeField] private LayerMask _projectileTarget;
        [SerializeField] private Vector3 _projectileOffsetFromCamera = Vector3.forward;
        [SerializeField] private LayerMask _armorLayer;
        #endregion

        #region Functions
        
        #endregion

        

        #region Methods
        void Start()
        {
            SetShellType(0);
        }
       void Update()
        {
            Shot();
        }
        void Shot()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, _projectileTarget,_armorLayer))
                {
                    _selectedProjectile.ResetProjectile();
                    _simulationController.ResetComponents();
                    _projectileParent.position = ray.origin;
                    _projectileParent.forward = ray.direction;
                    _selectedProjectile.transform.localPosition = _projectileOffsetFromCamera;
                    _selectedProjectile.transform.forward = _projectileParent.forward;

                    _selectedProjectile.SetHitResult(_penetrationCalculator.HitResult);
                    _selectedProjectile.Shot();


                    _simulationController.SetMode(SimulationController.Mode.PreviewDamage);
                }
            }
            if(_selectedProjectile.InAction == false && _simulationController.CurrentMode == SimulationController.Mode.PreviewDamage)
            {
                _simulationController.SetMode(SimulationController.Mode.PreviewPenetration);
            }
        }
        public void SetShellType(int enumIndex)
        {
            _shellType = (PenetrationCalculator.ShellType)enumIndex;

            switch (_shellType)
            {
                case PenetrationCalculator.ShellType.HE105MM:
                    _selectedProjectile = _HE105MM;
                    break;
                case PenetrationCalculator.ShellType.AP90MM:
                    _selectedProjectile = _AP90MM;
                    break;
                case PenetrationCalculator.ShellType.APCR37MM:
                    _selectedProjectile = _APCR37MM;
                    break;
                case PenetrationCalculator.ShellType.MachineGunAP:
                    _selectedProjectile = _machineGunAP;
                    break;
            }
        }
        #endregion
    }
}
