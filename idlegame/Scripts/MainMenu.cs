using System;
using Godot;
using IdleGame.scripts;

public partial class MainMenu : Control
{
    [Export] public Button NewGameButton, LoadGameButton, CreditsButton, QuitButton;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Q)) //TODO should be button press but i cant make it work
            GetTree().Quit();
    }

    public void OnNewGameButtonUp()
    {
        GD.Print("New Game");
        SceneManager.instance.changeScene(eSceneNames.GeneratorMenu);
    }

    public void OnLoadGameButtonUp() // Work In Progress
    { 
        GD.Print("Load Game");
        SceneManager.instance.changeScene(eSceneNames.GeneratorMenu);
    }

    public void OnCreditsButtonUp() // Work In Progress
    {
        GD.Print("Credits");
        // SceneManager.instance.changeScene(eSceneNames.Credits);
    }

    public void OnQuitGameButtonUp()
    {
        GD.Print("Quit Game");
        GetTree().Quit();
    }
}