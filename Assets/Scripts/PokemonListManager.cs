using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonListManager : MonoBehaviour
{
    public Transform contentPanel;
    public ObjectPool objectPool;
    public ScrollRect scrollRect;
    public int itemsToLoadAhead = 10;
    public float itemHeight = 160.91f; // Set this to match your item prefab height
    public int itemsPerChunk = 50;

    private int totalItemCount = 0;
    private int firstVisibleIndex = 0;
    private bool isLoading = false;
    private List<Pokemon> allPokemonData = new List<Pokemon>();
    private Dictionary<int, GameObject> activeItems = new Dictionary<int, GameObject>();

    private void Start()
    {
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
        StartCoroutine(LoadInitialPokemon());
    }

    private IEnumerator LoadInitialPokemon()
    {
        isLoading = true;
        yield return StartCoroutine(PokeAPIManager.Instance.GetPokemonList(0, itemsPerChunk, OnPokemonChunkReceived));
    }

    private void OnPokemonChunkReceived(PokemonList pokemonList)
    {
        allPokemonData.AddRange(pokemonList.results);
        totalItemCount = allPokemonData.Count;
        UpdateContentSize();
        isLoading = false;
        UpdateVisibleItems();

        // If this is the first load, scroll to the top
        if (allPokemonData.Count == itemsPerChunk)
        {
            scrollRect.verticalNormalizedPosition = 1;
        }
    }

    private void UpdateContentSize()
    {
        // Set the content size to accommodate all current items plus one more chunk
        float totalHeight = (totalItemCount + itemsPerChunk) * itemHeight;
        contentPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, totalHeight);
    }

    private void OnScrollValueChanged(Vector2 position)
    {
        if (!isLoading)
        {
            UpdateVisibleItems();
            CheckLoadMore();
        }
    }

    private void UpdateVisibleItems()
    {
        int newFirstVisibleIndex = Mathf.FloorToInt(scrollRect.content.anchoredPosition.y / itemHeight);
        newFirstVisibleIndex = Mathf.Clamp(newFirstVisibleIndex, 0, Mathf.Max(0, totalItemCount - 1));

        if (newFirstVisibleIndex != firstVisibleIndex)
        {
            int visibleItemCount = Mathf.CeilToInt(scrollRect.viewport.rect.height / itemHeight) + 1;
            int lastVisibleIndex = Mathf.Min(newFirstVisibleIndex + visibleItemCount + itemsToLoadAhead, totalItemCount - 1);

            // Remove items that are no longer visible
            List<int> itemsToRemove = new List<int>();
            foreach (var kvp in activeItems)
            {
                if (kvp.Key < newFirstVisibleIndex - itemsToLoadAhead || kvp.Key > lastVisibleIndex)
                {
                    itemsToRemove.Add(kvp.Key);
                }
            }
            foreach (int index in itemsToRemove)
            {
                objectPool.ReturnObject(activeItems[index]);
                activeItems.Remove(index);
            }

            // Add newly visible items
            for (int i = newFirstVisibleIndex - itemsToLoadAhead; i <= lastVisibleIndex; i++)
            {
                if (i >= 0 && i < totalItemCount && !activeItems.ContainsKey(i))
                {
                    GameObject obj = objectPool.GetObject();
                    obj.transform.SetParent(contentPanel, false);
                    obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -i * itemHeight);
                    obj.GetComponent<PokemonItem>().Setup(allPokemonData[i]);
                    activeItems[i] = obj;
                }
            }

            firstVisibleIndex = newFirstVisibleIndex;
        }
    }

    private void CheckLoadMore()
    {
        if (scrollRect.verticalNormalizedPosition <= 0.1f && !isLoading)
        {
            StartCoroutine(LoadMorePokemon());
        }
    }

    private IEnumerator LoadMorePokemon()
    {
        isLoading = true;
        yield return StartCoroutine(PokeAPIManager.Instance.GetPokemonList(totalItemCount, itemsPerChunk, OnPokemonChunkReceived));
    }
}
