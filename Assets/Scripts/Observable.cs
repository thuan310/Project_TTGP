using System;
using UnityEngine;

[Serializable]
public class Observable<T>
{
    // Event to notify when the value changes
    public event Action<T, T> OnValueChanged;

    // Backing field for the value
    [SerializeField] private T _value;


    #region If your application is multi-threaded, use a lock to ensure thread safety when updating the value
    //private readonly object _lock = new object();

    //public T Value
    //{
    //    get
    //    {
    //        lock (_lock)
    //        {
    //            return _value;
    //        }
    //    }
    //    set
    //    {
    //        lock (_lock)
    //        {
    //            if (!Equals(_value, value))
    //            {
    //                T oldValue = _value;
    //                _value = value;
    //                OnValueChanged?.Invoke(oldValue, _value);
    //            }
    //        }
    //    }
    //}
    #endregion

    #region Add validation logic in the Value property to ensure the new value meets certain criteria.
    //public T Value
    //{
    //    get => _value;
    //    set
    //    {
    //        if (!IsValid(value)) // Add your validation logic here
    //        {
    //            throw new ArgumentException("Invalid value");
    //        }

    //        if (!Equals(_value, value))
    //        {
    //            T oldValue = _value;
    //            _value = value;
    //            OnValueChanged?.Invoke(oldValue, _value);
    //        }
    //    }
    //}
    //private bool IsValid(T value)
    //{
    //    // Add your validation logic here
    //    return true;
    //}

    #endregion
    // Property to encapsulate the value
    public T Value
    {
        get => _value;
        set
        {
            if (!Equals(_value, value)) // Check if the value is actually changing
            {
                T oldValue = _value; // Store the old value
                _value = value;         // Update to the new value

                // Trigger the event with the old and new values
                OnValueChanged?.Invoke(oldValue, _value);
            }
        }
    }

    // Constructor to initialize the value
    public Observable(T initialValue)
    {
        _value = initialValue;
    }
}