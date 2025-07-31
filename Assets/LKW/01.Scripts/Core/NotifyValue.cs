using UnityEngine;

namespace LKW._01.Scripts.Core
{
    public class NotifyValue<T>
    {
        public delegate void ValueChanged(T prev, T next);
        
        public event ValueChanged OnValueChanged;

        [SerializeField] private T value;

        public T Value
        {
            get
            {
                return value;
            }

            set
            {
                T before = this.value;
                this.value = value;
                if((before == null && value != null) || !before.Equals(value))
                    OnValueChanged?.Invoke(before, value);
            }
        }

        public NotifyValue()
        {
            this.value = default(T);
        }

        public NotifyValue(T value)
        {
            this.value = value;
        }
    }
}