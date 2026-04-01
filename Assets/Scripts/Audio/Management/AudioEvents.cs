
using System;
using UnityEngine;
namespace SpaceCadets.Audio
{
 
    public static class AudioEvents
    {   

        public static event Action OnWallExploded;
        public static void WallExploded() => OnWallExploded?.Invoke();
        public static event Action OnElevatorUp;
        public static void ElevatorUp() => OnElevatorUp?.Invoke();
        public static event Action OnDetach;
        public static void Detach() => OnDetach?.Invoke();
        public static event Action OnAttach;
        public static void Attach() => OnAttach?.Invoke();
        public static event Action OnBombExplode;
        public static void BombExlpode() => OnBombExplode?.Invoke();

    }
}
