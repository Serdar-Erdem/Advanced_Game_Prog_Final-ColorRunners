using Rich.Base.Runtime.Abstract.View;
using UnityEngine;

namespace Rich.Base.Runtime.Concrete.View
{
    public class RichScreenManagerView : RichView
    {
        /// <summary>
        /// All layers inside on screenmanager gameobject on hierarchy
        /// </summary>
        public Transform[] Layers;
    }
}