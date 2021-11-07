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
                shrapnel.Shot(_shrapnelForce);
            }
        }


        #endregion
    }
}
