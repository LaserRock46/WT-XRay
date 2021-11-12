using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
    [AddComponentMenu(nameof(Project) + "/" + nameof(Uncategorized) + "/SimulationController")]
    public class SimulationController : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private VehicleComponents[] _vehicleComponents;
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
        public void NotifyAboutDestroyedComponent(VehicleComponent.ComponentType componentType)
        {

        }
        [ContextMenu(nameof(ResetComponents))]
        void ResetComponents()
        {
            foreach (VehicleComponents item in _vehicleComponents)
            {
                item.ResetComponents();
            }
        }
        #endregion
    }

    [System.Serializable]
    public class VehicleComponents
    {
        [SerializeField] private VehicleComponent[] _vehicleComponents;
        public void ResetComponents()
        {
            foreach (VehicleComponent item in _vehicleComponents)
            {
                item.ResetComponent();
            }
        }
    }
}
