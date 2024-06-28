using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TiledMapParser;
using System.Timers;

namespace GXPEngine
{

    public class Hud : GameObject
    {
        EasyDraw canvas;
        float sliderMaxWidth = 500;
        float sliderHeight = 75;
        Bottle bottle;
        float bottleCharge;
        float bottleMaxCharge;

        public static float score;
        float multiplier = 1;
        float multiplierTimer = 0;
        float multiplierTimerMax = 7.5f;
        float nextSceneDelay;

        public float gameTimer;
        public float gameTimerMax = 100;
        float visualGameTimer;
        bool  gameTimerOn = false;
        Sprite scoreUI = new Sprite("UI/scoreboard.png");
        Sprite timerUI = new Sprite("UI/timer.png");
        Sprite multiplierUI = new Sprite("UI/multiplier.png");
        Sprite timesupUI = new Sprite("UI/timesup.png");
        AnimationSprite sliderUI;
        int sliderFrame;

        MyGame myGame;

        public Hud() : base()
        {
            myGame = (MyGame)game;
            canvas = new EasyDraw(game.width,game.height,false);
            sliderUI = new AnimationSprite("UI/Charge_spritesheet.png", 2, 4);
            sliderMaxWidth = game.width / 3.84f;
            sliderHeight = game.height / 14.4f;
            AddChild(canvas);
            StartTimer();

            scoreUI.SetScaleXY(3);
            timerUI.SetScaleXY(3);
            timerUI.SetXY(game.width - timerUI.width,0);
            multiplierUI.SetScaleXY(3);
            multiplierUI.SetXY(0, scoreUI.height + 50);
            timesupUI.SetOrigin(timesupUI.width/2,timesupUI.height/2);
            timesupUI.SetScaleXY(1);
            timesupUI.SetXY(game.width/2,game.height/2);
            sliderUI.SetOrigin(0, sliderUI.height);
            sliderUI.SetXY(x, game.height);
            sliderUI.SetScaleXY(2);
        }

        void Update()
        {
            bottle = game.FindObjectOfType<Bottle>();
            if(bottle != null )
            {
                bottleCharge = bottle.charge;
                bottleMaxCharge = bottle.maxCharge;
            }
            canvas.ClearTransparent();
            if (myGame.currentLevel == "Game")
            {
                DrawSlider();
                DrawScore();
                DrawTimer();
                DrawMultiplier();
            }
            if (myGame.currentLevel == "Leaderboard")
            {
                DrawLeaderboard();
            }
        }

        void DrawSlider()
        {
            sliderFrame = (int)(bottleCharge / 100 * 6);
            if (sliderFrame > 5)
            {
                sliderFrame = 5;
            }
            sliderUI.SetCycle(sliderFrame);
            canvas.DrawSprite(sliderUI);
            //canvas.NoStroke();
            //canvas.Fill(255,255,0,255);
            //canvas.ShapeAlign(CenterMode.Min,CenterMode.Min);
            //canvas.Rect(game.height / 48, game.height - sliderHeight - game.height / 48, bottleCharge / bottleMaxCharge * sliderMaxWidth , sliderHeight);
        }

        void DrawScore()
        {
            canvas.DrawSprite(scoreUI);
            canvas.NoStroke();
            canvas.Fill(208,0,0);
            canvas.TextSize(game.height * 0.05f);
            canvas.TextAlign(CenterMode.Center, CenterMode.Center);
            canvas.Text("" + score,scoreUI.width/2,scoreUI.height/2);
        }

        public void AddScore(int aScore)
        {
            score += aScore * multiplier;
        }

        void DrawMultiplier()
        {
            if(multiplier == 2)
            {
                multiplierTimer -= Time.deltaTime;
                if (multiplierTimer <= 0)
                {
                    multiplierTimer = 0;
                    multiplier = 1;
                }
                canvas.DrawSprite(multiplierUI);
            }
        }

        public void SetMultiplier(int setMultiplier)
        {
            multiplier = setMultiplier;
            multiplierTimer = multiplierTimerMax * 1000;
        }

        void StartTimer()
        {
            gameTimer = gameTimerMax * 1000;
            gameTimerOn = true;
        }

        public void AddTime(int addedTime)
        {
            if (gameTimerOn)
            {
                gameTimer += addedTime * 1000;
            }
        }

        void DrawTimer()
        {
            if (gameTimerOn)
            {
                gameTimer -= Time.deltaTime;
                if (gameTimer <= 0)
                {
                    gameTimerOn = false;
                    gameTimer = 0;
                    Console.WriteLine("GAMOVER");
                }
            }
            canvas.DrawSprite(timerUI);
            canvas.NoStroke();
            canvas.Fill(208,0,0);
            canvas.TextSize(game.height * 0.05f);
            canvas.TextAlign(CenterMode.Center, CenterMode.Center);
            visualGameTimer = Mathf.Round(gameTimer/1000);
            canvas.Text("" + visualGameTimer, timerUI.x + timerUI.width/2,timerUI.y + timerUI.height/2);
            if (visualGameTimer <= 0)
            {
                gameTimerOn = false;
                canvas.DrawSprite(timesupUI);
                nextSceneDelay += Time.deltaTime;
                if (nextSceneDelay >= 2000)
                    myGame.LoadLevel("Leaderboard");
            }
        }

        void DrawLeaderboard()
        {
            canvas.NoStroke();
            canvas.Fill(255);
            canvas.TextAlign(CenterMode.Center, CenterMode.Min);
            canvas.TextSize(100);
            canvas.Text("Leaderboard",game.width/2,100);
            canvas.TextSize(75);
            canvas.Text("Player: " + score, game.width / 2, 250);
            for (int i = 5; i >= 0; i--)
            {
                canvas.Text("npc: " + (int)((5 - i)*50+ 100), game.width / 2, 350 + i * 100);
            }
            canvas.TextAlign(CenterMode.Max, CenterMode.Max);
            canvas.Text("Shoot To Play Again");
            if (Input.GetKeyDown(Key.SPACE))
            {
                myGame.LoadLevel("Menu");
                score = 0;
            }
            if (Controller.main.buttonDOWN)
            {
                myGame.LoadLevel("Menu");
                score = 0;
            }
        }
    }
}