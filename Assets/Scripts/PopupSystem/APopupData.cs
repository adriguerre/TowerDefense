using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace PopupSystem
{
    public abstract class APopupData
    {
        public Priority Priority { get; private set; }
        public string Message { get; private set; }
        public string Title { get; private set; }

        public APopupData(Priority priority, string message, string title)
        {
            Priority = priority;
            Message = message;
            Title = title;
        }
    }
}