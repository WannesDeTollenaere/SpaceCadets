
using System;
using UnityEngine;
namespace SpaceCadets.Audio
{
 
    public static class AudioEvents
    {   

        public static event Action OnWallExploded;
        public static void WallExploded() => OnWallExploded?.Invoke();

    }
}
