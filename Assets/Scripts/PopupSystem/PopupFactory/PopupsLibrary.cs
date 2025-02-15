using System.Collections.Generic;
using PopupSystem.PopupItems;
using UnityEngine;

namespace PopupSystem.PopupFactory
{
    [CreateAssetMenu]
    public class PopupsLibrary : ScriptableObject
    {
        public List<PopupItem> PopupItems;
    }
}