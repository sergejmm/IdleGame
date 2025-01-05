using Godot;
using System;

public partial class SaverLoader : Node
{
    private string savePath = "user://savegame.tres";
    [Export] private GeneratorMenu generatorMenu;


    public void SaveGameButton()
    {
        GD.Print("Saved Game");

        SaveGame savedGame = new SaveGame();

        savedGame.gold = generatorMenu.gold;
        savedGame.shroomCount = generatorMenu.ShroomCount;
        savedGame.shroomPrice = generatorMenu.ShroomPrice;
        savedGame.wardCount = generatorMenu.WardCount;
        savedGame.wardPrice = generatorMenu.WardPrice;
        savedGame.voidGrubCount = generatorMenu.VoidGrubCount;
        savedGame.voidGrubPrice = generatorMenu.VoidGrubPrice;

        ResourceSaver.Save(savedGame, savePath);
    }

    public void LoadGameButton()
    {
        GD.Print("Loading Game");

        SaveGame loadGame = ResourceLoader.Load<SaveGame>(savePath, savePath, 0);

        generatorMenu.gold = loadGame.gold;
        generatorMenu.ShroomCount = loadGame.shroomCount;
        generatorMenu.ShroomPrice = loadGame.shroomPrice;
        generatorMenu.WardCount = loadGame.wardCount;
        generatorMenu.WardPrice = loadGame.wardPrice;
        generatorMenu.VoidGrubCount = loadGame.voidGrubCount;
        generatorMenu.VoidGrubPrice = loadGame.voidGrubPrice;
    }
}