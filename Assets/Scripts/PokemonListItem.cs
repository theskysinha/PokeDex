using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class PokemonListItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image pokemonImage;
    public GameObject detailPanel; // Reference to the detail panel prefab
    private Pokemon pokemon;

    public void Setup(Pokemon pokemon)
    {
        this.pokemon = pokemon;
        nameText.text = pokemon.name;
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

    public void OnClick()
    {
        detailPanel = GameObject.Find("PokemonDetails");
        detailPanel.SetActive(true);
        this.gameObject.transform.parent.gameObject.SetActive(false);
        detailPanel.GetComponent<PokemonDetailPanel>().Setup(pokemon);
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
