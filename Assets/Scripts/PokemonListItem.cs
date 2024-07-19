using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class PokemonListItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image pokemonImage;
    private Pokemon pokemon;
    private PokemonObjectPool objectPool;

    public void Setup(Pokemon pokemon, PokemonObjectPool pool)
    {
        this.pokemon = pokemon;
        this.objectPool = pool;
        nameText.text = pokemon.name;
        StartCoroutine(LoadPokemonDetails(pokemon.url));
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

    public void ReturnToPool()
    {
        objectPool.Release(gameObject);
    }
}

[System.Serializable]
public class Pokemon
{
    public string name;
    public string url;
}

[System.Serializable]
public class PokemonDetails
{
    public Sprites sprites;
}

[System.Serializable]
public class Sprites
{
    public string front_default;
}
