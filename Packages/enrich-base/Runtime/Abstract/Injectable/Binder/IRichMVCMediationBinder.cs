using strange.extensions.mediation.api;
using UnityEngine;

namespace Rich.Base.Runtime.Abstract.Injectable.Binder
{
    public interface IRichMVCMediationBinder : IMediationBinder
    {

        void ActivateRoot(GameObject root);
    }
}