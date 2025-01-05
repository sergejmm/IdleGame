using Godot;
using System;
using IdleGame.scripts;

public partial class GeneratorMenu : Control
{
    // Gold Label
    public int gold = 0;
    public bool GoldPerSecond = false;
    [Export] public Label label;

    // Gold Generators
    [Export] public Button WardButton, ShroomButton, VoidGrubButton, QuitButton;
    public int WardPrice = 0, ShroomPrice = 10, VoidGrubPrice = 1000;
    public int WardValue = 1, ShroomValue = 8, VoidGrubValue = 800;
    public int WardCount = 0, ShroomCount = 0, VoidGrubCount = 0;
    [Export] public Timer WardTimer, ShroomTimer, VoidGrubTimer;
    [Export] public ProgressBar WardProgressBar, ShroomProgressBar, VoidGrubProgressBar;

    // Audio Player
    [Export] public AudioStreamPlayer AudioPlayer;

    // Check Button
    [Export] public CheckButton CheckButton;


    // New Game WORK IN PROGRESS
    public void CreateGenerators()
    {
        var WardGenerator = new GoldGenerator(WardButton, WardPrice, WardValue, WardCount, WardTimer, WardProgressBar,
            "res://Sounds/Control_Ward_active_SFX.ogg");
        var ShroomGenerator = new GoldGenerator(ShroomButton, ShroomPrice, ShroomValue, ShroomCount, ShroomTimer,
            ShroomProgressBar, "res://Sounds/Teemo_Select.ogg");
        var VoidGrubGenerator = new GoldGenerator(null, VoidGrubPrice, VoidGrubValue, VoidGrubCount, VoidGrubTimer,
            VoidGrubProgressBar, "res://Sounds/Void_Grub_SFX.ogg");
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        TimersStart();
        ActivateProgressBars();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        UpdateUI();
        ActivateProgressBars();

        if (Input.IsKeyPressed(Key.Q)) //TODO Should be button press but i can't connect it 
            SceneManager.instance.changeScene(eSceneNames.MainMenu);
    }

    // Activate Progress Bars
    public void ActivateProgressBars()
    {
        ActivateProgressBar(WardCount, WardProgressBar);
        ActivateProgressBar(ShroomCount, ShroomProgressBar);
        ActivateProgressBar(VoidGrubCount, VoidGrubProgressBar);
    }

    // Only show progress bar if generator is purchased
    public void ActivateProgressBar(int count, ProgressBar progressBar)
    {
        progressBar.Visible = count > 0;
    }

    // Start Timers
    public void TimersStart()
    {
        WardTimer.Start();
        ShroomTimer.Start();
        VoidGrubTimer.Start();
    }

    // Called when gold is modified
    public void ModifyGold(int amount)
    {
        gold += amount;
    }

    // Updates the UI
    public void UpdateUI()
    {
        UpdateProgressBars();
        UpdateLabels();
    }

    // Update Progress Bars
    public void UpdateProgressBars()
    {
        WardProgressBar.Value = 100 - (WardTimer.TimeLeft / WardTimer.WaitTime * 100);
        ShroomProgressBar.Value = 100 - (ShroomTimer.TimeLeft / ShroomTimer.WaitTime * 100);
        VoidGrubProgressBar.Value = 100 - (VoidGrubTimer.TimeLeft / VoidGrubTimer.WaitTime * 100);
    }

    // Update Labels
    public void UpdateLabels()
    {
        if (GoldPerSecond)
        {
            label.Text = $"Gold : {gold} ({GetTotalGoldPerSecond()} g/s)";
            WardButton.Text =
                $"{WardCount}x Wards {WardPrice}g {GetGoldPerSecond(WardValue, WardCount, WardTimer)} g/s";
            ShroomButton.Text =
                $"{ShroomCount}x Shrooms: {ShroomPrice}g {GetGoldPerSecond(ShroomValue, ShroomCount, ShroomTimer)} g/s";
            VoidGrubButton.Text =
                $"{VoidGrubCount}x Voidgrubs: {VoidGrubPrice}g {GetGoldPerSecond(VoidGrubValue, VoidGrubCount, VoidGrubTimer)} g/s";
        }
        else
        {
            label.Text = $"Gold : {gold}";
            WardButton.Text = $"{WardCount}x Wards: {WardPrice}g";
            ShroomButton.Text = $"{ShroomCount}x Shrooms: {ShroomPrice}g";
            VoidGrubButton.Text = $"{VoidGrubCount}x Voidgrubs: {VoidGrubPrice}g";
        }
    }

    // Get Gold Per Second
    public double GetTotalGoldPerSecond()
    {
        return GetGoldPerSecond(WardValue, WardCount, WardTimer) + GetGoldPerSecond(ShroomValue, ShroomCount,
                                                                     ShroomTimer)
                                                                 + GetGoldPerSecond(VoidGrubValue, VoidGrubCount,
                                                                     VoidGrubTimer);
    }

    public double GetGoldPerSecond(int value, int count, Timer timer)
    {
        return value * count / timer.WaitTime;
    }

    // Timer Timeout
    public void OnWardTimerTimeout()
    {
        ModifyGold(WardValue * WardCount);
    }

    public void OnShroomTimerTimeout()
    {
        ModifyGold(ShroomValue * ShroomCount);
    }

    public void OnVoidGrubTimerTimeout()
    {
        ModifyGold(VoidGrubValue * VoidGrubCount);
    }

    // Any Button Pressed
    public void OnButtonPressed(ref int price, ref int value, ref int count, Button button, Timer timer,
        string AudioStreamPath = null)
    {
        if (gold < price) return;
        PlayAudio(AudioStreamPath);
        ModifyGold(-price);
        timer.Start();
        count++;
        button.Modulate = new Color(1, 1, 1); // Change button color to white
        if (count == 10 || count == 25 || count == 50 || count == 100 || count == 200)
        {
            button.Modulate = new Color(1, 1, 0); // Change button color to yellow
            value *= 5; // Increase value by 5
            timer.WaitTime /= 2; // Decrease timer by 2
        }

        price = (int)Math.Ceiling(1 + price * 1.5);
    }

    public void OnWardButtonPressed()
    {
        OnButtonPressed(ref WardPrice, ref WardValue, ref WardCount, WardButton, WardTimer,
            "res://Sounds/Control_Ward_active_SFX.ogg");
    }

    public void OnShroomButtonPressed()
    {
        OnButtonPressed(ref ShroomPrice, ref ShroomValue, ref ShroomCount, ShroomButton, ShroomTimer,
            "res://Sounds/Teemo_Select.ogg");
    }

    public void OnVoidGrubButtonPressed()
    {
        OnButtonPressed(ref VoidGrubPrice, ref VoidGrubValue, ref VoidGrubCount, VoidGrubButton, VoidGrubTimer,
            "res://Sounds/Void_Grub_SFX.ogg");
    }

    // Play Audio
    private void PlayAudio(string AudioStreamPath)
    {
        if (AudioStreamPath == null) return;
        AudioPlayer.Stream = GD.Load<AudioStream>(AudioStreamPath);
        AudioPlayer.Play();
    }

    // Check Button Toggled
    public void OnCheckButtonPressed()
    {
        GoldPerSecond = !GoldPerSecond;
    }

    public void OnQuitButtonUp()
    {
        GD.Print("Quit");
        SceneManager.instance.changeScene(eSceneNames.MainMenu);
    }
}

public class GoldGenerator
{
    public Button Button;
    public int Price;
    public int Value;
    public int Count;
    public Timer Timer;
    public ProgressBar ProgressBar;
    public string Sound;

    public GoldGenerator(Button button, int price, int value, int count, Timer timer, ProgressBar progressBar,
        string sound)
    {
        Button = button;
        Price = price;
        Value = value;
        Count = count;
        Timer = timer;
        ProgressBar = progressBar;
        Sound = sound;
    }
}