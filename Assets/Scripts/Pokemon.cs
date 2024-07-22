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
