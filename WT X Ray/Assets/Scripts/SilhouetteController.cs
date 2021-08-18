using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
   [AddComponentMenu(nameof(Project) + "/" + nameof(Uncategorized) + "/SilhouetteController")]
    public class SilhouetteController : MonoBehaviour
    {
        #region Temp
        [Header("Temporary Things", order = 0)]
        public Vector3 screenPosition;
        public Vector3 silhouetteViewPosition;
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Camera _camera;
        [SerializeField] private Material _silhouetteMat;
        [SerializeField] private string _propertyPosition;
        [SerializeField] private string _propertyRadius;
        private int _propertyIdPosition;
        private int _propertyIdRadius;
        public float radiusSpeed;
        public Transform _silhouetteWorldCenter;
        [SerializeField] private float silhouetteQuadWidth;
        [SerializeField] private float silhouetteQuadHeight;

        #endregion

        #region Functions

        #endregion



        #region Methods
        void Start()
        {
            _propertyIdPosition = Shader.PropertyToID(_propertyPosition);
            _propertyIdRadius = Shader.PropertyToID(_propertyRadius);
        }
       void Update()
        {
            Vector3 screenPosition = _camera.WorldToScreenPoint(_silhouetteWorldCenter.position);
            float x = Mathf.Lerp(-silhouetteQuadWidth, silhouetteQuadWidth, Mathf.InverseLerp(0, Screen.width, screenPosition.x));
            float y = Mathf.Lerp(-silhouetteQuadHeight, silhouetteQuadHeight, Mathf.InverseLerp(0, Screen.height, screenPosition.y));
            Vector3 silhouetteViewPosition = new Vector3(x, y, 0);

            this.screenPosition = screenPosition;
            this.silhouetteViewPosition = silhouetteViewPosition;
            _silhouetteMat.SetVector(_propertyIdPosition, _silhouetteWorldCenter.localPosition);
        }
        #endregion
    }
}
