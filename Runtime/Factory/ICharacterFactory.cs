using UnityEngine;

namespace RD_SimpleDI.Runtime.DI.Factory
{
    public interface ICharacterFactory
    {
        public T SpawnCharacter<T>(T prefab, Vector3 position, Quaternion rotation, bool needBind) where T : Character;
    }
}