using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MagazineList
{
    public List<Magazine> data;
    public List<Magazine> magazineList {
        get {
            if (data != null)
            {
                data.Sort();
                return data;
            }
            else
                return null;
        }
    }
}
[Serializable]
public class Magazine: IEquatable<Magazine>, IComparable<Magazine>
{
    public double index;
    public string name;
    public string id;
    public double size;
    public string url;
    public string fileName {

        get {
            return id.Replace(" ", "_") + ".dlc";
        }
    }
    public bool Equals(Magazine other)
    {
        if (other is null)
            return false;
        return this.id == other.id;
    }
    public int CompareTo(Magazine value)
    {
        return value.index.CompareTo(this.index);
    }

    public override string ToString()
    {
        return string.Format("index: {0}, name: {1}, id: {2}, size: {3}, url: {4}", index, name, id, size, url);
    }
}