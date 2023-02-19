using Krivodeling.UI.Effects;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlasticPipe.PlasticProtocol.Messages.Serialization.ItemHandlerMessagesSerialization;

public class StoryPlayer : MonoBehaviour
{
    [System.Serializable]
    public class UiFields
    {
        public Image Image;
        public UIBlur Blur;
        public TextMeshProUGUI Sentence;
        public TextMeshProUGUI SentenceShadow;
        public Button BackButton;
        public Button ShowButton;
        public Button NextButton;
        public RectTransform Completed;
    }

    [System.Serializable]
    public class DebugFields
    {
        public int Index = -1;
    }

    public interface IReferenceFields
    {
        public Story Story {get; set;}
    }

    [System.Serializable]
    public class ReferenceFields
    {
        [SerializeField]
        private Story _Story;
        private IStory _IStory;
        public IStory Story
        {
            get => _Story == null ? _IStory : _Story;
            set
            {
                if (value is Story) _Story = (Story)value;
                _IStory = value;
            }
        }

        [MethodButton()]
        public void Apply()
        {
            _Story = (Story)_IStory;
        }
    }

    public ReferenceFields References;
    public UiFields Ui;
    public DebugFields _Debug;

    private void Awake()
    {
        References.Story = References.Story;
    }

    [MethodButton()]
    public void MyMethod()
    {
        Debug.Log("trest");
    }
    private void Start()
    {
        _Debug.Index = -1;
        ShowCover();
        HideBackButton();
        HideShowButton();
        ShowNextButton();
        Ui.BackButton.onClick.AddListener(OnBackClicked);
        Ui.ShowButton.onClick.AddListener(OnShowClicked);
        Ui.NextButton.onClick.AddListener(OnNextClicked);
    }

    private void ShowNextButton()
    {
        SetButton(Ui.NextButton, true);
    }

    private void HideNextButton()
    {
        SetButton(Ui.NextButton, false);
    }

    private void ShowShowButton()
    {
        SetButton(Ui.ShowButton, true);
    }

    private void HideShowButton()
    {
        SetButton(Ui.ShowButton, false);
    }

    private void ShowBackButton()
    {
        SetButton(Ui.BackButton, true);
    }

    private void HideBackButton()
    {
        SetButton(Ui.BackButton, false);
    }

    private void OnNextClicked()
    {
        _Debug.Index += 1;
        ShowPart();
    }

    private void ShowPart()
    {
        if(_Debug.Index < 0)
        {
            _Debug.Index = -1;
            ShowCover();
            UnBlurImage();
            ShowNextButton();
            HideShowButton();
            HideBackButton();
            PercentDone();
            return;
        }

        SetImage(References.Story.Parts[_Debug.Index].Image);
        SetText(References.Story.Parts[_Debug.Index].Sentance);
        BlurImage();
        ShowBackButton();
        ShowShowButton();

        
        if (_Debug.Index < References.Story.Parts.Length - 1 ) ShowNextButton();
        else HideNextButton();
    }

    private void PercentDone()
    {
        var v = _Debug.Index / References.Story.Parts.Length;

        var ls = Ui.Completed.localScale;
        ls.x = v;
        Ui.Completed.localScale = ls;
    }

    private void BlurImage()
    {
        Ui.Blur.SetBlur(new Color(.1f, .2f, .2f), 1, 1);
    }

    private void OnShowClicked()
    {
        UnBlurImage();
        HideShowButton();

        if (_Debug.Index < References.Story.Parts.Length - 1) ShowNextButton();
        else HideNextButton();
    }

    private void UnBlurImage()
    {
        Ui.Blur.EndBlur(1);
    }

    private void OnBackClicked()
    {
        _Debug.Index-= 1;
        ShowPart();
    }

    private void ShowCover()
    {
        SetText(References.Story.Name);
        SetImage(References.Story.CoverImage);
    }

    private void SetImage(Sprite coverImage)
    {
        Ui.Image.sprite = coverImage;
    }

    private void SetText(string text)
    {
        Ui.Sentence.text = text;
        Ui.SentenceShadow.text = text;
    }

    private void SetButton(Button button, bool visible)
    {
        button.gameObject.SetActive(visible);
    }
}
