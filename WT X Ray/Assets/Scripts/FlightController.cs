using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
   [AddComponentMenu(nameof(Project) + "/" + nameof(Uncategorized) + "/FlightController")]
    public class FlightController : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private LayerMask _armorLayerMask;
        public float coverage { get; private set; }
        public float FlightDistance { get; private set; }
        public Vector3 FlightStart { get; private set; }
        public Vector3 FlightEnd { get; private set; }
        public float FlightTime { get; private set; }

        #endregion

        #region Functions

        #endregion



        #region Methods
        public void FlightToTankSetup(float flightSpeed,float rayOffset = 0, bool drawDebug= false)
        {
            RaycastHit hit;

            Vector3 offset = transform.forward * rayOffset;
            if (Physics.Raycast(transform.position + offset, transform.forward, out hit, 100.0f, _armorLayerMask))
            {
                FlightDistance = Vector3.Distance(transform.position, hit.point);
                FlightStart = transform.position;
                FlightEnd = hit.point;
                FlightTime = FlightDistance / flightSpeed;
                StartCoroutine(Flight(flightSpeed));
                if (drawDebug)
                {               
                    Debug.DrawLine(FlightStart, FlightEnd, Color.green, 2f);
                }
            }
        }
        public void FlightThroughTankSetup(float flightSpeed, float rayOffset = 0, bool drawDebug = false)
        {        
            RaycastHit[] targets = Physics.RaycastAll(transform.position, transform.forward, 100,_armorLayerMask);

            FlightStart = transform.position;
            FlightDistance = 0;
            for (int i = 0; i < targets.Length; i++)
            {
                if(FlightDistance < targets[i].distance)
                {
                    FlightDistance = targets[i].distance;
                    FlightEnd = targets[i].point;
                }
            }
            if (drawDebug)
            {
                Debug.DrawLine(FlightStart, FlightEnd, Color.green, 2f);
            }
            FlightTime = FlightDistance / flightSpeed;
            StartCoroutine(Flight(flightSpeed));
        }
        public void FlightAwayFromTankSetup(float flightSpeed, Vector3 direction)
        {
            float flightAwayDistance = 2;
            
            FlightStart = transform.position;
            FlightEnd = transform.position + (direction * flightAwayDistance);
            FlightDistance = Vector3.Distance(FlightStart, FlightEnd);
            FlightTime = FlightDistance / flightSpeed;
            StartCoroutine(Flight(flightSpeed));
        }
        IEnumerator Flight(float flightSpeed)
        {
            coverage = 0;
            while (coverage < FlightDistance)
            {
                float step = flightSpeed * Time.deltaTime;
                coverage += step;
               
                float time = Mathf.Clamp(Mathf.InverseLerp(0, FlightDistance, coverage), 0, 1);
                transform.position = Vector3.Lerp(FlightStart, FlightEnd, time);
                
                yield return null;
            }       
            yield return null;
        }
        #endregion
    }
}
