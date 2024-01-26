using System;
using UnityEngine;

namespace Rich.Base.Editor.Concrete.CodeGenerationRich.Code.ContextList
{
  [Serializable]
  public class ContextVo
  {
    [SerializeField, AppContext] public int Context;

    [HideInInspector] public bool visible;
  }
}