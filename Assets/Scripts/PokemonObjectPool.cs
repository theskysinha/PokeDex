using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PokemonObjectPool : MonoBehaviour
{
    // public GameObject pokemonListItemPrefab;
    // private ObjectPool<GameObject> pool;
    // public Transform parent;

    // void Start()
    // {
    //     pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, 20, 100);
    // }

    // public GameObject Get()
    // {
    //     return pool.Get();
    // }

    // public void Release(GameObject obj)
    // {
    //     pool.Release(obj);
    // }

    // private GameObject CreatePooledItem()
    // {
    //     GameObject obj = Instantiate(pokemonListItemPrefab, parent);
    //     obj.SetActive(false);
    //     return obj;
    // }

    // private void OnTakeFromPool(GameObject obj)
    // {
    //     obj.SetActive(true);
    // }

    // private void OnReturnedToPool(GameObject obj)
    // {
    //     obj.SetActive(false);
    //     obj.transform.SetParent(transform);
    // }

    // private void OnDestroyPoolObject(GameObject obj)
    // {
    //     Destroy(obj);
    // }
}
