using System.Collections;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class StoryPlayerTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void StoryTestsSimplePasses()
    {
        // Use the Assert class to test conditions
        MeshRenderer r = new MeshRenderer();
        
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator Start_WithAGivenStory_SetsTheImageToTheCoverAndTheSentanceToTheTitle()
    {
        // ARRANGE
        var go =
            GameObject.Instantiate<GameObject>(
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    "Assets/!Stories/Prefabs/Story.prefab"));

        var story = ScriptableObject.CreateInstance<Story>();
        var expectedName = story.name = "Test Title";
        var expectedSprite
            = story.CoverImage
            = AssetDatabase.LoadAssetAtPath<Sprite>(
                "Assets/!Stories/Stories/How the land got a pink lake/Textures/Cover.png");

        var player = go.GetComponent<StoryPlayer>();
        player.References.Story = story;

        // ACT
        yield return null; // Start occured during this time

        // ASSERT
        var actualSprite = player.Ui.Image.sprite;
        var actualName = player.Ui.Sentence.text;

        Assert.AreSame(expectedSprite, actualSprite, "When the story started, the image was not set to the cover image.");
        Assert.AreEqual(expectedName, actualName, "When the story started, the sentance shown was not the title or story name.");
    }

    [UnityTest]
    public IEnumerator Start_WithAMockedStory_SetsTheImageToTheCoverAndTheSentanceToTheTitle()
    {
        // ARRANGE
        var go =
            GameObject.Instantiate<GameObject>(
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    "Assets/!Stories/Prefabs/Story.prefab"));

        var expectedName = "Test Title";
        var expectedSprite
            = AssetDatabase.LoadAssetAtPath<Sprite>(
                "Assets/!Stories/Stories/How the land got a pink lake/Textures/Cover.png");

        var story = new Mock<IStory>();
        story.Setup((m) => m.Name).Returns(expectedName);
        story.Setup((m) => m.CoverImage).Returns(expectedSprite);

        var player = go.GetComponent<StoryPlayer>();
        player.References.Story = story.Object;

        // ACT
        yield return null; // Start occured during this time

        // ASSERT
        var actualSprite = go.transform.GetChild(0).GetComponent<Image>().sprite;
        var actualName = go.GetComponentInChildren<TextMeshProUGUI>().text;

        Assert.AreSame(expectedSprite, actualSprite, "When the story started, the image was not set to the cover image.");
        Assert.AreEqual(expectedName, actualName, "When the story started, the sentance shown was not the title or story name.");
    }
}
