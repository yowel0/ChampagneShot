using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using System.Collections.Generic;                           // System.Drawing contains drawing tools such as Color definitions
using System.IO.Ports;
using System.Drawing.Printing;

public class MyGame : Game {

	Random random = new Random();

	Controller controller;
	SoundManager soundManager;

	Pivot level;
	Pivot platesGroup;
	Hud hud;
	Sprite background;
	Sprite title;
	Sprite insertCork;
    Bottle bottle;
	Plate plate1;
    Plate plate2;
    Plate plate3;
	AnimationSprite tutorialBottle;
	PlateSpawner plateSpawner;


    public String currentLevel;

    public MyGame() : base(1920,1080, false, false, 1920,1080, true)     // Create a window that's 800x600 and NOT fullscreen
	{
        // Load controller
        controller = new Controller();
		//AddChild(controller);
		soundManager = new SoundManager();
		AddChild(soundManager);
		
		LoadLevel("Menu");
		Console.WriteLine("MyGame initialized");
	}

	// For every game object, Update is called every frame, by the engine:
	void Update() {
        // Empty
		//if (Input.GetMouseButtonDown(0))
		//{
		//	plate1 = new Plate("","random");
		//		platesGroup.AddChild(plate1);
		//}
		if (Input.GetKeyDown(Key.COMMA))
		{
			LoadLevel("Menu");
		}
		if (Input.GetKeyDown(Key.DOT))
		{
			LoadLevel("Game");
		}
        if (currentLevel == "Menu")
		{
			//title.SetScaleXY(0.05f * Mathf.Sin((float)Time.time / 800) + .7f);
			title.rotation = 8 * Mathf.Sin((float)Time.time / 800);
            title.SetScaleXY(0.05f * Mathf.Sin(((float)2*Time.time + 0.5f * Mathf.PI)/ 800) + .7f);
            insertCork.SetScaleXY(0.05f * Mathf.Sin(((float)Time.time + 6 * Mathf.PI)/ 400) + .7f);
			tutorialBottle.SetXY(100, 80 * Mathf.Sin((float)Time.time / 400) + height - 150);
			tutorialBottle.rotation = 40 * Mathf.Sin((float)Time.time / 400);
        }

    }

	static void Main()                          // Main() is the first method that's called when the program is run
	{
		new MyGame().Start();                   // Create a "MyGame" and start it
	}

	void DestroyAll()
	{
		//destroy all game object except for the controller

		List<GameObject> Children = new List<GameObject>();
		Children = GetChildren();
		foreach (GameObject child in Children)
		{
			if (child.GetType().Name == "Controller" || child.GetType().Name == "SoundManager")
			{
                
            }
			else
				child.Destroy();
		}

	}

	public void LoadLevel(String levelName = "Empty")
	{
		DestroyAll();
		currentLevel = levelName;
        if (levelName == "Empty")
		{

		}
		if (levelName == "Menu")
		{
			level = new Pivot();

            background = new Sprite("Background.png");
            background.SetOrigin(background.width / 2, background.height / 2);
            background.SetXY(width / 2, height / 2);
            background.scale = 1.41f;
			Plate menuPlate = new Plate("Menu");
			
            CorkParent corkParent = new CorkParent();

            bottle = new Bottle();

			title = new Sprite("Title.png");
			title.SetOrigin(title.width/2, title.height/2);
			title.SetXY(title.width / 2 * 0.7f, title.height / 2 * 0.7f);

			insertCork = new Sprite("Insert cork.png");
			insertCork.SetOrigin(insertCork.width/2,insertCork.height/2);
			insertCork.SetXY(width - insertCork.width/3, height - insertCork.height/2);

			tutorialBottle = new AnimationSprite("BottleSpritesheet.png", 3, 1);
			tutorialBottle.SetOrigin(0, tutorialBottle.height);

            AddChild(level);
            level.AddChild(background);
            level.AddChild(menuPlate);
            level.AddChild(corkParent);
            level.AddChild(bottle);
			level.AddChild(title);
			level.AddChild(insertCork);
			level.AddChild(tutorialBottle);
        }
		if (levelName == "Game")
		{
            level = new Pivot();
            platesGroup = new Pivot();
            hud = new Hud();
            background = new Sprite("Background.png");
			background.SetOrigin(background.width/2,background.height/2);
			background.SetXY(width/2,height/2);
            background.scale = 1.41f;
			CorkParent corkParent= new CorkParent();
			Sprite curtains = new Sprite("Curtains.png");
			curtains.SetOrigin(curtains.width / 2, curtains.height / 2);
			curtains.SetXY(width/2,height/2);
			curtains.scale = 1.41f;
			plateSpawner = new PlateSpawner();

            bottle = new Bottle();
            plate1 = new Plate();
            plate1.SetXY(width / 2, 400);
            plate2 = new Plate();
            plate2.SetXY(width / 2, 400);
            plate3 = new Plate();
            plate3.SetXY(width / 2 - 450, 400);

            // Add the canvas to the engine to display it:
            AddChild(level);
            level.AddChild(background);
            level.AddChild(plateSpawner);
			level.AddChild(corkParent);
			level.AddChild(curtains);
            level.AddChild(bottle);
            level.AddChild(hud);
            //plateSpawner.AddChild(plate1);
            //platesGroup.AddChild(plate2);
            //platesGroup.AddChild(plate3);

            soundManager.PlaySound("theme");
            Console.WriteLine("Game initialized");
        }
		if (levelName == "Leaderboard")
		{
			background = new Sprite("Background.png");
            background.SetOrigin(background.width / 2, background.height / 2);
            background.SetXY(width / 2, height / 2);
            background.scale = 1.41f;
			hud = new Hud();

			AddChild(background);
			AddChild(hud);
        }
	}
}