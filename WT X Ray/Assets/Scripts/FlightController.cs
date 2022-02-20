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
        public float flightDistance { get; private set; }
        public Vector3 flightStart { get; private set; }
        public Vector3 flightEnd { get; private set; }

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
                flightDistance = Vector3.Distance(transform.position, hit.point);
                flightStart = transform.position;
                flightEnd = hit.point;
                StartCoroutine(Flight(flightSpeed));
                if (drawDebug)
                {               
                    Debug.DrawLine(flightStart, flightEnd, Color.green, 2f);
                }
            }
        }
        public void FlightThroughTankSetup(float flightSpeed, float rayOffset = 0, bool drawDebug = false)
        {        
            RaycastHit[] targets = Physics.RaycastAll(transform.position, transform.forward, 100,_armorLayerMask);

            flightStart = transform.position;
            flightDistance = 0;
            for (int i = 0; i < targets.Length; i++)
            {
                if(flightDistance < targets[i].distance)
                {
                    flightDistance = targets[i].distance;
                    flightEnd = targets[i].point;
                }
            }
            if (drawDebug)
            {
                Debug.DrawLine(flightStart, flightEnd, Color.green, 2f);
            }
            StartCoroutine(Flight(flightSpeed));
        }
        public void FlightAwayFromTankSetup(float flightSpeed, Vector3 direction)
        {
            float flightAwayDistance = 2;
            
            flightStart = transform.position;
            flightEnd = transform.position + (direction * flightAwayDistance);
            flightDistance = Vector3.Distance(flightStart, flightEnd);

            StartCoroutine(Flight(flightSpeed));
        }
        IEnumerator Flight(float flightSpeed)
        {
            coverage = 0;
            while (coverage < flightDistance)
            {
                float step = flightSpeed * Time.deltaTime;
                coverage += step;
               
                float time = Mathf.Clamp(Mathf.InverseLerp(0, flightDistance, coverage), 0, 1);
                transform.position = Vector3.Lerp(flightStart, flightEnd, time);
                
                yield return null;
            }       
            yield return null;
        }
        #endregion
    }
}
