using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Pokemon
{
    public string name;
    public string url;
}

[System.Serializable]
public class PokemonList
{
    public List<Pokemon> results;
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
