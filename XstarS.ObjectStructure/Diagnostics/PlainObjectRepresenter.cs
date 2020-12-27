using System;
using System.Collections.Generic;

namespace XstarS.Diagnostics
{
    [Serializable]
    internal sealed class PlainObjectRepresenter<T> : StructuralObjectRepresenterBase<T>
    {
        public PlainObjectRepresenter() { }

        protected override string RepresentCore(T value, ISet<object> represented)
        {
            return ObjectRepresenter<T>.Default.Represent(value);
        }
    }
}
