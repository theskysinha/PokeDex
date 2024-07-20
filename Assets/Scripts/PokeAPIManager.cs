using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PokeAPIManager : MonoBehaviour
{
    public static PokeAPIManager Instance;
    private const string baseURL = "https://pokeapi.co/api/v2/pokemon";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator GetPokemonList(int offset, int limit, System.Action<PokemonList> callback)
    {
        string url = $"{baseURL}?offset={offset}&limit={limit}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                PokemonList pokemonList = JsonUtility.FromJson<PokemonList>(json);
                callback(pokemonList);
            }
            else
            {
                Debug.LogError("Failed to fetch data: " + request.error);
            }
        }
    }
}
