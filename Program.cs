using NAudio.Wave;

ShowMenuOptions();

var audio = new WaveOutEvent();
var whiteNoiseProvider = new WhiteNoiseProvider();
audio.Init(whiteNoiseProvider);
audio.Play();

while (IsPlayingAudio(audio))
{
    {
        var key = Console.ReadKey(intercept: true).Key;
        switch (key)
        {
            case ConsoleKey.OemPlus:
                whiteNoiseProvider.IncreaseAmplitude();
                break;
            case ConsoleKey.OemMinus:
                whiteNoiseProvider.DecreaseAmplitude();
                break;
            case ConsoleKey.E:
                audio.Stop();
                break;
        }
    }
}

void ShowMenuOptions()
{
    Console.WriteLine("Use [+ & -] to change amplitude.");
    Console.WriteLine("[E]xit");
}

Boolean IsPlayingAudio(WaveOutEvent audio)
{
    return audio.PlaybackState == PlaybackState.Playing;
}

public class WhiteNoiseProvider : WaveProvider32
{
    private Random _random = new Random();
    private static int HIGH_DEFINITION_AUDIO = 96000; // 96 kHZ
    private int _sampleRate = HIGH_DEFINITION_AUDIO;
    private int _totalSamples;
    private static int ONE_HOUR = 60 * 60; // Seconds
    private int playDuration = ONE_HOUR;
    private float _amplitude { get; set; } = 0.1f;

    public WhiteNoiseProvider()
    {
        _totalSamples = _sampleRate * playDuration;
        SetWaveFormat(_sampleRate, 1);
    }

    public override int Read(float[] buffer, int offset, int sampleCount)
    {
        int samplesToWrite = Math.Min(sampleCount, _totalSamples);
        for (int i = 0; i < samplesToWrite; i++)
        {
            buffer[offset + i] = (float)(_random.NextDouble() * 2.0 - 1.0) * _amplitude;
        }
        _totalSamples -= samplesToWrite;
        return samplesToWrite;
    }

    public void PrintAmplitude()
    {
        Console.WriteLine($"Amplitude: {_amplitude}");
    }

    public void IncreaseAmplitude()
    {
        _amplitude += 0.01f;
        PrintAmplitude();
    }

    public void DecreaseAmplitude()
    {
        _amplitude -= Math.Min(0.01f, _amplitude - 0.01f);
        PrintAmplitude();
    }
}

