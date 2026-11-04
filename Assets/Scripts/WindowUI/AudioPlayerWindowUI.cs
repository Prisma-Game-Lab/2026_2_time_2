using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayerWindowUI : MonoBehaviour
{
    [Header("Componentes de Interface")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Button playPauseButton;
    [SerializeField] private TextMeshProUGUI playPauseButtonText;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Button closeButton;

    private AudioSource audioSource;
    private bool isDraggingSlider = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void Setup(VirtualAudioFile file)
    {
        if (titleText != null)
            titleText.text = file.itemName;

        if (file.audioContent != null)
        {
            audioSource.clip = file.audioContent;

            if (progressSlider != null)
            {
                progressSlider.minValue = 0f;
                progressSlider.maxValue = file.audioContent.length;
                progressSlider.value = 0f;
            }

            PlayAudio();
        }

        // Configura botões
        if (playPauseButton != null)
        {
            playPauseButton.onClick.RemoveAllListeners();
            playPauseButton.onClick.AddListener(TogglePlayPause);
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseWindow);
        }

        // Eventos do Slider
        if (progressSlider != null)
        {
            progressSlider.onValueChanged.RemoveAllListeners();
            progressSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    void Update()
    {
        if (audioSource.clip == null) return;

        // Atualiza a barra e o tempo se estiver tocando e o jogador não estiver arrastando a barra
        if (audioSource.isPlaying && !isDraggingSlider)
        {
            if (progressSlider != null)
            {
                progressSlider.SetValueWithoutNotify(audioSource.time);
            }
        }

        UpdateTimeDisplay();

        // Se o áudio terminou sozinho, atualiza o botão
        if (!audioSource.isPlaying && audioSource.time >= audioSource.clip.length)
        {
            if (playPauseButtonText != null) playPauseButtonText.text = "Play";
        }
    }

    public void TogglePlayPause()
    {
        if (audioSource.isPlaying)
        {
            PauseAudio();
        }
        else
        {
            // Se chegou ao fim e clicou em play, reinicia do começo
            if (audioSource.time >= audioSource.clip.length)
            {
                audioSource.time = 0f;
            }
            PlayAudio();
        }
    }

    private void PlayAudio()
    {
        audioSource.Play();
        if (playPauseButtonText != null) playPauseButtonText.text = "Pause";
    }

    private void PauseAudio()
    {
        audioSource.Pause();
        if (playPauseButtonText != null) playPauseButtonText.text = "Play";
    }

    public void OnSliderValueChanged(float value)
    {
        if (audioSource.clip != null)
        {
            audioSource.time = Mathf.Clamp(value, 0f, audioSource.clip.length - 0.01f);
        }
    }

    private void UpdateTimeDisplay()
    {
        if (timeText == null || audioSource.clip == null) return;

        float current = audioSource.time;
        float total = audioSource.clip.length;

        timeText.text = $"{FormatTime(current)} / {FormatTime(total)}";
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public void CloseWindow()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}