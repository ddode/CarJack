﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CarJack.Common.WhipRemix
{
    [RequireComponent(typeof(DrivableCar))]
    public class RecolorableCar : MonoBehaviour
    {
        public Material[] RecolorableMaterials;
        private List<RecolorableRenderer> _recolorableRenderers;

        private void Awake()
        {
            _recolorableRenderers = new();
            var renderers = GetComponentsInChildren<Renderer>();
            foreach(var renderer in renderers)
            {
                var sharedMats = renderer.sharedMaterials;
                for(var i = 0; i < sharedMats.Length; i++)
                {
                    var sharedMat = sharedMats[i];
                    for(var n = 0; n < RecolorableMaterials.Length; n++)
                    {
                        var recolorableMat = RecolorableMaterials[n];
                        if (sharedMat == recolorableMat)
                        {
                            var recolorable = new RecolorableRenderer();
                            recolorable.Renderer = renderer;
                            recolorable.MaterialIndex = i;
                            recolorable.OriginalMaterial = recolorableMat;
                            _recolorableRenderers.Add(recolorable);
                        }
                    }
                }
            }
        }

        public void ApplyDefaultColor()
        {
            foreach(var recolorable in _recolorableRenderers)
            {
                recolorable.Renderer.sharedMaterials[recolorable.MaterialIndex] = recolorable.OriginalMaterial;
            }
        }

        public class RecolorableRenderer
        {
            public Renderer Renderer;
            public int MaterialIndex = 0;
            public Material OriginalMaterial;
        }
    }
}
