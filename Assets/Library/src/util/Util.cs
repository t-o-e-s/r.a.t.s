using UnityEngine;

namespace Library.src.util
{
    public class Util
    {
        public static GameObject GetFromChildren(GameObject parent, string lookingFor)
        {
            foreach (var child in parent.GetComponentsInChildren<Transform>())
            {
                if (child.name.Equals(lookingFor)) return child.gameObject;
            }

            return null;
        }
    }
}