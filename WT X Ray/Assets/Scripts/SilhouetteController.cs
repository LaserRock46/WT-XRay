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
        public int[] testSorted = new int[4];
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
        [SerializeField] private Transform _silhouetteWorldCenter;
        [SerializeField] private MeshFilter _silhouetteQuad;

        #endregion

        #region Functions

        #endregion



        #region Methods
        void Start()
        {
            _propertyIdPosition = Shader.PropertyToID(_propertyPosition);
            _propertyIdRadius = Shader.PropertyToID(_propertyRadius);
            FitQuadInNearPlane();
        }
        void FitQuadInNearPlane()
        {
            Vector3[] frustumCorners = new Vector3[4];
            _camera.CalculateFrustumCorners(new Rect(0,0,1,1),0.1f, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);
            Vector3[] sortedCorners = { frustumCorners[0], frustumCorners[3], frustumCorners[1], frustumCorners[2] };
            _silhouetteQuad.mesh.vertices = sortedCorners;
            _silhouetteQuad.mesh.RecalculateBounds();
        }
       void Update()
        {
           
            _silhouetteMat.SetVector(_propertyIdPosition,Vector3.zero);
            FitQuadInNearPlane();
        }
        #endregion
    }
}
