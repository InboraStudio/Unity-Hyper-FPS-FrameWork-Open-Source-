using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Utility/MeshFade")]
    public class MeshFade : MonoBehaviour
    {
        /// <summary>
        /// which mesh the component is going to use
        /// </summary>
        public MeshType meshType = MeshType.MeshRenderer;
        /// <summary>
        /// how much time before fading
        /// </summary>
        public float fadeDelay = 0;
        /// <summary>
        /// how fast the fade
        /// </summary>
        public float roughness = 10;

        /// <summary>
        /// target mesh
        /// </summary>
        public MeshRenderer MeshRenderer { get; set; }

        /// <summary>
        /// target skinned mesh
        /// </summary>
        public SkinnedMeshRenderer SkinnedMeshRenderer { get; set; }

        /// <summary>
        /// used material
        /// </summary>
        public Material Material { get; set; }

        /// <summary>
        /// is the mesh fading
        /// </summary>
        public bool Fading { get; set; }

        private Color defaultColor;

        private void Start()
        {
            Fading = false;
            MeshRenderer = GetComponent<MeshRenderer>();
            SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

            switch (meshType)
            {
                case MeshType.MeshRenderer:
                    if (MeshRenderer) Material = MeshRenderer.material;
                    break;

                case MeshType.SkinnedMeshRenderer:
                    if (SkinnedMeshRenderer) Material = SkinnedMeshRenderer.material;
                    break;
            }

            Invoke(nameof(Fade), fadeDelay);
        }

        private void Update()
        {
            if (!Fading || !Material) return;
            Material.color = Color.Lerp(Material.color, Color.clear, Time.deltaTime * roughness);
        }

        /// <summary>
        /// initializes fading
        /// </summary>
        public void Fade()
        {
            Fading = true;
        }

        public enum MeshType
        {
            MeshRenderer,
            SkinnedMeshRenderer
        }
    }
}