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
        [SerializeField] private VehicleComponent.DamageMode _damageMode;
        #endregion

        #region Functions

        #endregion



        #region Methods
        void Start()
        {
            
        }
       void Update()
        {
            
        }      
        [ContextMenu("Test Projectile")]
        void TestProjectile()
        {          
            foreach (ShrapnelController shrapnel in _shrapnels)
            {
                shrapnel.Shot(_damageMode,_damageMode == VehicleComponent.DamageMode.Visualisation ? _shrapnelForce: _forceSimulation,_shrapnelDamage);
            }
        }
        public void Shot()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            
        }

        #endregion
    }
}
