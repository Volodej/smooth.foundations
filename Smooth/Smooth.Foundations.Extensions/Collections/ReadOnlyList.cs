using System.Collections;
using System.Collections.Generic;

namespace Smooth.Extensions.Collections
{
    public class ReadOnlyList<T> : IEnumerable<T> 
    {
        #region IEnumerable implementation
        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
        #endregion
    
        public ReadOnlyList(List<T> list)
        {
            this.list = list;
        }

        public int Count { get { return list.Count; } }
        public T this[int index] { get { return list[index]; } }
        private List<T> list;
    }
}