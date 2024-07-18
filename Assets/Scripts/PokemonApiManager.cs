using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PokemonApiManager : MonoBehaviour
{
    public GameObject pokemonListItemPrefab; // Drag your PokemonListItem prefab here in the Inspector
    public Transform contentTransform; // Drag your Content GameObject from the Scroll View here in the Inspector
    private string apiURL = "https://pokeapi.co/api/v2/pokemon?limit=20"; // Example URL, adjust as per your needs

    void Start()
    {
        StartCoroutine(FetchPokemonData());
    }

    private IEnumerator FetchPokemonData()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiURL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            PokemonList pokemonList = JsonUtility.FromJson<PokemonList>(request.downloadHandler.text);
            PopulatePokemonList(pokemonList.results);
        }
    }

    private void PopulatePokemonList(Pokemon[] pokemonArray)
    {
        foreach (Pokemon pokemon in pokemonArray)
        {
            GameObject pokemonListItem = Instantiate(pokemonListItemPrefab, contentTransform);
            pokemonListItem.GetComponent<PokemonListItem>().Setup(pokemon);
        }
    }
}

[System.Serializable]
public class PokemonList
{
    public Pokemon[] results;
}
