using UnityEngine;

namespace RD_SimpleDI.Runtime.DI.Factory
{
    public class Factory<T> : IFactory<T> where T : MonoBehaviour
    {
        private readonly T _prefab;
        private readonly Transform _spawnPoint;

        public Factory(T prefab, Transform spawnPoint)
        {
            _prefab = prefab;
            _spawnPoint = spawnPoint;
        }

        public T Create()
        {
            return Object.Instantiate(_prefab, _spawnPoint.position, Quaternion.identity);
        }
    }
}