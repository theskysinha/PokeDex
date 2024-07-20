using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class PokemonItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image pokemonImage;
    private Pokemon pokemon;

    public void Setup(Pokemon newPokemon)
    {
        pokemon = newPokemon;
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

    public void OnClick()
    {
        PokemonDetailsUI.Instance.ShowDetails(pokemon);
    }
}
