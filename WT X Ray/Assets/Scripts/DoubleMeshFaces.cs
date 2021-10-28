using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
   [AddComponentMenu(nameof(Project) + "/" + nameof(Uncategorized) + "/DoubleMeshFaces")]
    public class DoubleMeshFaces : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private Mesh[] targets;
        #endregion

        #region Functions
        int[] GetDoubledIndices(int[] singleIndices)
        {
            List<int> newDoubledIndices = new List<int>();
            int[] newSide = new int[singleIndices.Length];
            for (int i = 0; i < newSide.Length - 3; i += 3)
            {
                newSide[i] = singleIndices[i + 2];
                newSide[i + 1] = singleIndices[i + 1];
                newSide[i + 2] = singleIndices[i];
            }

            newDoubledIndices.AddRange(singleIndices);
            newDoubledIndices.AddRange(newSide);
            return newDoubledIndices.ToArray();
        }
        #endregion



        #region Methods
        [ContextMenu("Double Faces")]
        void DoubleFaces()
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].triangles = GetDoubledIndices(targets[i].triangles);
            }
        }
        #endregion
    }
}
