using System;
using System.Collections.Generic;

namespace Rich.Base.Editor.Concrete.CodeGenerationRich.Code.ContextList
{
    public class SimpleReorderableList
    {
    }

    public class ReorderableList<T> : SimpleReorderableList
    {
        public List<T> List;
    }

    [Serializable]
    public class ReorderableContextList : ReorderableList<ContextVo>
    {
    }

 
}