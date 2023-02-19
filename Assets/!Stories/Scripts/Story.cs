using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStory
{
    Sprite CoverImage { get; set; }
    string Name { get; }
    IStoryPart[] Parts { get; set; }
}

public interface IStoryPart
{
    Sprite Image { get; set; }
    string Sentance { get; set; }
}

[CreateAssetMenu(menuName = "Data/Story")]
public class Story : ScriptableObject, IStory
{
    [Serializable]
    public class Part : IStoryPart
    {
        [SerializeField]
        private string _Sentance;

        [SerializeField]
        private Sprite _Image;

        public string Sentance
        {
            get => _Sentance;
            set => _Sentance = value;
        }

        public Sprite Image
        {
            get => _Image; 
            set => _Image = value;
        }
    }

    [SerializeField]
    public Sprite _CoverImage;

    [SerializeField]
    public List<Part> _Parts = new List<Part>();

    public Sprite CoverImage
    {
        get => _CoverImage;
        set => _CoverImage = value;
    }

    public IStoryPart[] Parts
    {
        get => (IStoryPart[])_Parts.ToArray();
        set => _Parts = new List<Part>((Part[])value);
    }

    public string Name => this.name;
}
