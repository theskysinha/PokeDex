// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Networking;
// using UnityEngine.UI;

// public class PokemonAPIManager : MonoBehaviour
// {
//     public string baseUrl = "https://pokeapi.co/api/v2/pokemon";
//     public Transform content;
//     public PokemonObjectPool objectPool;
//     private int limit = 20;
//     private int offset = 0;
//     private bool isLoading = false;
//     public GameObject detailPanel;
//     private List<GameObject> activeListItems = new List<GameObject>();
//     void Start()
//     {
//         LoadMorePokemon();
//     }

//     public void LoadMorePokemon()
//     {
//         if (!isLoading)
//         {
//             StartCoroutine(FetchPokemonData());
//         }
//     }

//     public IEnumerator FetchPokemonData()
//     {
//         isLoading = true;
//         string url = $"{baseUrl}?limit={limit}&offset={offset}";
//         UnityWebRequest request = UnityWebRequest.Get(url);
//         yield return request.SendWebRequest();

//         if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
//         {
//             Debug.LogError(request.error);
//         }
//         else
//         {
//             PokemonList pokemonList = JsonUtility.FromJson<PokemonList>(request.downloadHandler.text);
//             PopulatePokemonList(pokemonList.results);
//             offset += limit;
//         }
//         isLoading = false;
//     }

//     private void PopulatePokemonList(Pokemon[] pokemonArray)
//     {
//         foreach (var pokemon in pokemonArray)
//         {
//             GameObject listItem = objectPool.Get();
//             listItem.GetComponent<PokemonListItem>().Setup(pokemon, objectPool);
//             listItem.GetComponent<Button>().onClick.AddListener(() => ShowDetailPanel(pokemon));
//             activeListItems.Add(listItem);
//         }
//     }

//     public void ShowDetailPanel(Pokemon pokemon)
//     {
//         detailPanel.SetActive(true);
//         detailPanel.GetComponent<PokemonDetailPanel>().Setup(pokemon);
//     }

//     public void ReturnItemToPool(GameObject item)
//     {
//         activeListItems.Remove(item);
//         item.GetComponent<PokemonListItem>().ReturnToPool();
//     }
// }

// [System.Serializable]
// public class PokemonList
// {
//     public Pokemon[] results;
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PokemonAPIManager : MonoBehaviour
{
    private const string API_BASE_URL = "https://pokeapi.co/api/v2/pokemon/";
    private List<PokemonData> pokemonList = new List<PokemonData>();

    public IEnumerator FetchPokemonData(int startIndex, int count)
    {
        for (int i = startIndex; i < startIndex + count; i++)
        {
            string url = API_BASE_URL + i.ToString();
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    string json = webRequest.downloadHandler.text;
                    PokemonData pokemonData = JsonUtility.FromJson<PokemonData>(json);
                    pokemonList.Add(pokemonData);
                }
                else
                {
                    Debug.LogError("Error: " + webRequest.error);
                }
            }
        }
    }

    public List<PokemonData> GetPokemonList()
    {
        return pokemonList;
    }
}

[System.Serializable]
public class PokemonData
{
    public string name;
    public Sprites sprites;
}
