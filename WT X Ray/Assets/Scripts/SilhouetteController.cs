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
        [SerializeField] private Vector3[] _frustumCornersQuad = new Vector3[4];


        #endregion

        #region Functions
        Vector3 WorldToQuad(Vector3 worldSpacePosition)
        {
            Vector3 localPosition = _camera.transform.InverseTransformPoint(worldSpacePosition);
            Vector3[] frustumCornersWorld = new Vector3[4];
            _camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), localPosition.z, Camera.MonoOrStereoscopicEye.Mono, frustumCornersWorld);

            float verticalLinear = Mathf.InverseLerp(frustumCornersWorld[0].y, frustumCornersWorld[1].y,localPosition.y);
            float verticalInQuad = Mathf.Lerp(_frustumCornersQuad[0].y, _frustumCornersQuad[1].y,verticalLinear);
            float horizontalLinear = Mathf.InverseLerp(frustumCornersWorld[0].x, frustumCornersWorld[2].x, localPosition.x);
            float horizontalInQuad = Mathf.Lerp(_frustumCornersQuad[0].x, _frustumCornersQuad[2].x, horizontalLinear);
            Vector3 positionInQuad = new Vector3(horizontalInQuad,verticalInQuad,0);

            return positionInQuad;
        }
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
            _camera.CalculateFrustumCorners(new Rect(0,0,1,1),0.1f, Camera.MonoOrStereoscopicEye.Mono, _frustumCornersQuad);
            Vector3[] sortedCorners = { _frustumCornersQuad[0], _frustumCornersQuad[3], _frustumCornersQuad[1], _frustumCornersQuad[2] };
            _silhouetteQuad.mesh.vertices = sortedCorners;
            _silhouetteQuad.mesh.RecalculateBounds();
        }
       void Update()
        {
           
            _silhouetteMat.SetVector(_propertyIdPosition,WorldToQuad(_silhouetteWorldCenter.position));
        }
        #endregion
    }
}
