using UnityEngine;

namespace Utils
{
    public static class ChangeMaterial
    {
        public static void SetMaterialAlpha(Renderer renderer, float alpha)
        {
            Color color = Color.white;
            color.a = alpha;
            renderer.material.color = color;
        }
    }
}
