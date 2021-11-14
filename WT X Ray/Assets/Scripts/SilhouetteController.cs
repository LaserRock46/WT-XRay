using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
   [AddComponentMenu(nameof(Project) + "/" + nameof(Uncategorized) + "/SilhouetteController")]
    public class SilhouetteController : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        public float outline;
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Camera _camera;
        [SerializeField] private Material _silhouetteMat;
        [SerializeField] private string _propertyPosition;
        [SerializeField] private string _propertyRadius;
        private int _propertyIdPosition;
        private int _propertyIdRadius;
        private bool _silhouetteRevealead = false;
        [SerializeField] private float _radiusSpeed = 1;
        [SerializeField] private float _radiusTarget = 10;
        [SerializeField] private Transform _silhouetteWorldCenter;
        [SerializeField] private MeshFilter _silhouetteQuad;

        [SerializeField] private HighlightPlus.HighlightEffect _redOutline;
        [SerializeField] private Material[] _xRayMaterials;
        private int _alphaNameID;
        [SerializeField] private float[] _maxXRayAlpha;
        private Vector3[] _frustumCornersQuad = new Vector3[4];


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
            _alphaNameID = Shader.PropertyToID("_Alpha");
            _silhouetteMat.SetFloat(_propertyIdRadius, 0);
            for (int i = 0; i < _xRayMaterials.Length; i++)
            {            
                _xRayMaterials[i].SetFloat(_alphaNameID, 0);
            }
            FitQuadInNearPlane();
        }
       void Update()
        {
            UpdatePosition();
            outline = _redOutline.outline;
        }
        public void Reveal(Vector3 position)
        {
            _silhouetteWorldCenter.position = position;
            _silhouetteRevealead = true;
            StartCoroutine(RevealUpdate());
        }
        IEnumerator RevealUpdate()
        {
            float radius = 0;
    
            while (radius < _radiusTarget)
            {
                radius += Time.deltaTime * _radiusSpeed;
                _silhouetteMat.SetFloat(_propertyIdRadius, radius);

                for (int i = 0; i < _xRayMaterials.Length; i++)
                {
                    float alpha = Mathf.Lerp(0,_maxXRayAlpha[i],Mathf.InverseLerp(0,_radiusTarget,radius));
                    _xRayMaterials[i].SetFloat(_alphaNameID,alpha);
                }
                _redOutline.outline = Mathf.InverseLerp(0, _radiusTarget, radius);             
                yield return null;
            }
          
            yield return null;
        }
        public void Hide()
        {
            if (_silhouetteRevealead)
            {
                _silhouetteRevealead = false;
                StartCoroutine(HideUpdate());
            }
        }
        IEnumerator HideUpdate()
        {
            float radius = _radiusTarget;
         
            while (radius >= 0)
            {
                radius -= Time.deltaTime * _radiusSpeed;
                _silhouetteMat.SetFloat(_propertyIdRadius, radius);

                for (int i = 0; i < _xRayMaterials.Length; i++)
                {
                    float alpha = Mathf.Lerp(_maxXRayAlpha[i], 0, Mathf.InverseLerp(_radiusTarget, 0, radius));
                    _xRayMaterials[i].SetFloat(_alphaNameID, alpha);
                }
                _redOutline.outline = Mathf.InverseLerp(0, _radiusTarget, radius);              
                yield return null;
            }
          
            yield return null;
        }
        void FitQuadInNearPlane()
        {       
            _camera.CalculateFrustumCorners(new Rect(0,0,1,1),0.1f, Camera.MonoOrStereoscopicEye.Mono, _frustumCornersQuad);
            Vector3[] sortedCorners = { _frustumCornersQuad[0], _frustumCornersQuad[3], _frustumCornersQuad[1], _frustumCornersQuad[2] };
            _silhouetteQuad.mesh.vertices = sortedCorners;
            _silhouetteQuad.mesh.RecalculateBounds();
        }     
        void UpdatePosition()
        {
            _silhouetteMat.SetVector(_propertyIdPosition,WorldToQuad(_silhouetteWorldCenter.position));
        }
        #endregion
    }
}
