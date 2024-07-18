using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class PokemonDetailPanel : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image pokemonImage;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI typesText;

    public void Setup(Pokemon pokemon)
    {
        nameText.text = pokemon.name;
        StartCoroutine(LoadPokemonImage(pokemon.url));
        StartCoroutine(LoadPokemonDetails(pokemon.url));
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
        statsText.text = "Stats:\n";
        foreach (Stat stat in stats)
        {
            statsText.text += $"{stat.stat.name}: {stat.base_stat}\n";
        }
    }

    private void PopulateTypes(Type[] types)
    {
        typesText.text = "Types:\n";
        foreach (Type type in types)
        {
            typesText.text += $"{type.type.name}\n";
        }
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
