using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class PokemonDetailsUI : MonoBehaviour
{
    public static PokemonDetailsUI Instance;
    public GameObject detailsPanel;
    public TextMeshProUGUI nameText;
    public Image pokemonImage;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI typesText;

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

    public void ShowDetails(Pokemon pokemon)
    {
        detailsPanel.SetActive(true);
        nameText.text = pokemon.name;
        StartCoroutine(LoadPokemonDetails(pokemon.url));
        StartCoroutine(LoadPokemonImage(pokemon.url));
    }

    private IEnumerator LoadPokemonImage(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            PokemonDetails details = JsonUtility.FromJson<PokemonDetails>(request.downloadHandler.text);
            StartCoroutine(DownloadImage(details.sprites.front_default));
        }
    }

    private IEnumerator DownloadImage(string imageUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            pokemonImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }

    private IEnumerator LoadPokemonDetails(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            PokemonFullDetails fullDetails = JsonUtility.FromJson<PokemonFullDetails>(request.downloadHandler.text);
            PopulateStats(fullDetails.stats);
            PopulateTypes(fullDetails.types);
        }
    }

    private void PopulateStats(Stat[] stats)
    {
        foreach (Stat stat in stats)
        {
            statsText.text += $"{stat.stat.name}: {stat.base_stat}, ";
        }
    }

    private void PopulateTypes(Type[] types)
    {
        foreach (Type type in types)
        {
            typesText.text += $"{type.type.name},";
        }
    }
    public void HideDetails()
    {
        detailsPanel.SetActive(false);
        statsText.text = "";
        typesText.text = "";
        pokemonImage.sprite = null;
        nameText.text = "";
    }
}

[System.Serializable]
public class PokemonFullDetails
{
    public Stat[] stats;
    public Type[] types;
}

[System.Serializable]
public class Stat
{
    public int base_stat;
    public StatInfo stat;
}

[System.Serializable]
public class StatInfo
{
    public string name;
}

[System.Serializable]
public class Type
{
    public TypeInfo type;
}

[System.Serializable]
public class TypeInfo
{
    public string name;
}

