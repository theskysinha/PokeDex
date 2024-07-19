using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PokemonAPIManager : MonoBehaviour
{
    public string baseUrl = "https://pokeapi.co/api/v2/pokemon";
    public Transform content;
    public GameObject detailPanel;
    public PokemonObjectPool objectPool;
    private int limit = 20;
    private int offset = 0;
    private bool isLoading = false;

    void Start()
    {
        LoadMorePokemon();
    }

    public void LoadMorePokemon()
    {
        if (!isLoading)
        {
            StartCoroutine(FetchPokemonData());
        }
    }

    private IEnumerator FetchPokemonData()
    {
        isLoading = true;
        string url = $"{baseUrl}?limit={limit}&offset={offset}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            PokemonList pokemonList = JsonUtility.FromJson<PokemonList>(request.downloadHandler.text);
            PopulatePokemonList(pokemonList.results);
            offset += limit;
        }
        isLoading = false;
    }

    private void PopulatePokemonList(Pokemon[] pokemonArray)
    {
        foreach (var pokemon in pokemonArray)
        {
            GameObject listItem = objectPool.Get();
            listItem.transform.SetParent(content, false);
            listItem.GetComponent<PokemonListItem>().Setup(pokemon, objectPool);
            listItem.GetComponent<Button>().onClick.AddListener(() => ShowDetailPanel(pokemon));
        }
    }

    public void ShowDetailPanel(Pokemon pokemon)
    {
        detailPanel.SetActive(true);
        detailPanel.GetComponent<PokemonDetailPanel>().Setup(pokemon);
    }
}

[System.Serializable]
public class PokemonList
{
    public Pokemon[] results;
}
