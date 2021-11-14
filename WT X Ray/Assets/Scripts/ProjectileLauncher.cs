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
        [SerializeField] private Projectile _projectile;
        [SerializeField] private Transform _projectileParent;
        [SerializeField] private LayerMask _projectileTarget;
        [SerializeField] private Vector3 _projectileOffsetFromCamera = Vector3.forward;
        [SerializeField] private SimulationController _simulationController;
        #endregion

        #region Functions
        
        #endregion

        

        #region Methods
        void Start()
        {
            
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
                if (Physics.Raycast(ray, out hit, _projectileTarget))
                {
                    _projectile.ResetProjectile();
                    _simulationController.ResetComponents();
                    _projectileParent.position = ray.origin;
                    _projectileParent.forward = ray.direction;
                    _projectile.transform.localPosition = _projectileOffsetFromCamera;

                    _projectile.Shot();
                }
            }
        }
        #endregion
    }
}
