using System;
using Unity.Burst.Intrinsics;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.DedicatedServer;

[System.Serializable]
public class Observable<T>
{
    #region Action vs EventHandler
    /*
//     The core difference comes down to standardization vs. flexibility in how you handle events. Let me break it down clearly:

        //1. Method Signature (The Biggest Difference)
        //Class Action Approach:

        //You can use any method signature you want

        //Example:

        //csharp
        //Copy
        //button.Click += MyCustomHandler;  // Can be void MyCustomHandler() or whatever you need
        //EventHandler Approach:

        //Must follow the standard .NET event pattern:

        //csharp
        //Copy
        //void MethodName(object sender, EventArgs e)
        //Example:

        //csharp
        //Copy
        //button.Click += (sender, e) => { /* handler code */
        //};
        //2.Information Available
        //Class Action:

        //Typically only knows about the event itself

        //Doesn't automatically get information about who raised the event (the sender)

        //Doesn't get standard event data (EventArgs)

        //EventHandler:

        //Always receives:

        //sender(the object that raised the event)

        //e(event arguments with details about what happened)

        //Example accessing info:

        //csharp
        //Copy
        //void Button_Click(object sender, EventArgs e) 
        //{
        //    var button = (Button)sender; // Can access the button that was clicked
        //var mouseEvent = (MouseEventArgs)e; // Can access mouse position, etc.
        //}
        //3.Where Each Is Used
        //Class Actions Are Good For:

        //Simple scenarios where you don't need event details

        //Internal event handling within your class

        //When you want maximum flexibility in method signatures

        //EventHandler Is Used For:

        //Public events in libraries/components

        //Cases where you need to know which object triggered the event

        //Situations where you need to pass detailed event data

        //Following .NET design guidelines for consistency

        //4. Real-world Analogy
        //Think of it like receiving mail:

        //Class Action = Getting a blank postcard (just knowing you got mail)

        //EventHandler = Getting a full envelope with return address (sender) and contents (EventArgs)

        //Which Should You Use?
        //If you're creating events for others to use → Use EventHandler

        //If you're consuming simple events internally → Class Action is fine

        //If you need event details → You must use EventHandler

        //The EventHandler approach is more powerful and standard, while class actions are simpler but more limited.
    #endregion

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