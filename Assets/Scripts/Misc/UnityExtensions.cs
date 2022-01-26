using UnityEngine;

namespace Misc
{
    public static class UnityExtensions
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }
    }
}