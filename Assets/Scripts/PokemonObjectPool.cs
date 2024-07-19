using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PokemonObjectPool : MonoBehaviour
{
    public GameObject pokemonListItemPrefab;
    public int initialPoolSize = 20;

    private ObjectPool<GameObject> pool;

    void Awake()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(pokemonListItemPrefab),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: Destroy,
            defaultCapacity: initialPoolSize,
            maxSize: 50
        );

        // Pre-warm the pool with initial objects
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = pool.Get();
            pool.Release(obj);
        }
    }

    public GameObject Get()
    {
        return pool.Get();
    }

    public void Release(GameObject obj)
    {
        pool.Release(obj);
    }

    public void Clear()
    {
        pool.Clear();
    }

    
}
