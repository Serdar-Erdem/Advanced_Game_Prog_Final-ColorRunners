using System.Collections.Generic;
using UnityEngine;

namespace Rich.Base.Editor.Concrete.CodeGenerationRich
{
    [CreateAssetMenu(fileName = "CodeGenerationOperationConfig", menuName = "Rich/Admin/Code Generation Operation Config")]
    public class CodeGenerationOperationConfig : ScriptableObject
    {
        [SerializeField] private List<CodeGenerationOperation> _operations;
        public CodeGenerationOperation[] Operations
        {
            get => _operations.ToArray();
        }
    }
}