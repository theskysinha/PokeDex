using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class PokemonPoolManager : MonoBehaviour
{
    public GameObject pokemonListItemPrefab;
    public ScrollRect scrollRect;
    public PokemonAPIManager apiManager;
    public int poolSize = 50;
    public int itemsPerBatch = 20;
    public float loadThreshold = 0.9f;

    private ObjectPool<PokemonListItem> itemPool;
    private List<PokemonListItem> activeItems = new List<PokemonListItem>();
    private int currentIndex = 0;
    private bool isLoading = false;
    private int totalPokemonCount = 898; // Total number of Pokémon available in the API

    private void Start()
    {
        itemPool = new ObjectPool<PokemonListItem>(
            CreatePoolItem,
            OnTakeFromPool,
            OnReturnToPool,
            OnDestroyPoolObject,
            true,
            poolSize,
            poolSize);

        scrollRect.onValueChanged.AddListener(OnScroll);
        StartCoroutine(InitializeList());
    }

    private IEnumerator InitializeList()
    {
        yield return StartCoroutine(apiManager.FetchPokemonData(1, itemsPerBatch));
        PopulateList();
    }

    private void PopulateList()
    {
        List<PokemonData> pokemonList = apiManager.GetPokemonList();

        for (int i = 0; i < Mathf.Min(poolSize, pokemonList.Count); i++)
        {
            PokemonListItem item = itemPool.Get();
            item.SetData(pokemonList[i].name, pokemonList[i].sprites.front_default);
            activeItems.Add(item);
        }

        currentIndex = activeItems.Count;
    }

    private PokemonListItem CreatePoolItem()
    {
        GameObject go = Instantiate(pokemonListItemPrefab, scrollRect.content);
        return go.GetComponent<PokemonListItem>();
    }

    private void OnTakeFromPool(PokemonListItem item)
    {
        item.gameObject.SetActive(true);
    }

    private void OnReturnToPool(PokemonListItem item)
    {
        item.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(PokemonListItem item)
    {
        Destroy(item.gameObject);
    }

    private void OnScroll(Vector2 scrollPosition)
    {
        if (!isLoading && scrollPosition.y > loadThreshold && currentIndex < totalPokemonCount)
        {
            LoadMoreItems();
        }
    }

    private void LoadMoreItems()
    {
        StartCoroutine(LoadMoreItemsCoroutine());
    }

    private IEnumerator LoadMoreItemsCoroutine()
    {
        isLoading = true;
        yield return StartCoroutine(apiManager.FetchPokemonData(currentIndex + 1, itemsPerBatch));
        
        List<PokemonData> pokemonList = apiManager.GetPokemonList();

        for (int i = 0; i < itemsPerBatch && currentIndex < pokemonList.Count; i++)
        {
            PokemonListItem item;
            if (activeItems.Count < poolSize)
            {
                item = itemPool.Get();
                activeItems.Add(item);
            }
            else
            {
                item = activeItems[currentIndex % poolSize];
            }

            item.SetData(pokemonList[currentIndex].name, pokemonList[currentIndex].sprites.front_default);
            currentIndex++;
        }

        // Refresh the scroll rect to update the content size
        Canvas.ForceUpdateCanvases();
        scrollRect.content.GetComponent<VerticalLayoutGroup>()?.CalculateLayoutInputVertical();
        scrollRect.content.GetComponent<ContentSizeFitter>()?.SetLayoutVertical();
        scrollRect.verticalNormalizedPosition = 1 - (scrollRect.verticalNormalizedPosition * pokemonList.Count / currentIndex);

        isLoading = false;
    }
}