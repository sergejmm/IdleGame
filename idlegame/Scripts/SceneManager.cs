using Godot;
using System.Collections.Generic;

namespace IdleGame.scripts;


public enum eSceneNames
{
	MainMenu = 10,
	GeneratorMenu = 30
}
public partial class SceneManager : Node
{
	public static SceneManager instance;

	public Dictionary<eSceneNames, SceneCaster> scenes = new Dictionary<eSceneNames, SceneCaster>()
	{
		{ eSceneNames.MainMenu, new SceneCaster("res://Scenes/Main Menu.tscn", "Main Menu")},
		{ eSceneNames.GeneratorMenu, new SceneCaster("res://Scenes/GeneratorMenu.tscn", "GeneratorMenu")}

	};

	public override void _Ready()
	{
		GD.Print("SceneManager Ready");
		instance = this;
	}

	// Call this when changing scenes
	public void changeScene(eSceneNames sceneName)
	{
		string path = scenes[sceneName].path;
		GetTree().ChangeSceneToFile(path);
	}
}
