using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Racing
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		bool Active;

		SpriteFont font;

		// Player Declaration
		Player player;
		float playerSpeed;

		//Keys events Declaration
		KeyboardState currentKey;

		//Background Declaration
		Track bgLayer;
		float bgSpeed;



		//Coins Declaration
		List<Coin> coins;
		//float coinSpeed;
		float spawnTimeValue;
		TimeSpan previousSpanTime;
		TimeSpan spawnTime;
		Random random;

		//Obstacles Declaration
		List<Coin> rocks;

		//Score
		SpriteFont scoreFont;
		int score;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			// Game Status
			Active = true;

			//Player Initialization
			player = new Player();
			playerSpeed = 6.0f;

			//Background Initialization
			bgLayer = new Track();
			bgSpeed = 4.0f;

			//Coins Initialization
			coins = new List<Coin>();
			//coinSpeed = bgSpeed;
			spawnTimeValue = 2.0f;
			spawnTime = TimeSpan.FromSeconds(spawnTimeValue);
			previousSpanTime = TimeSpan.Zero;
			random = new Random();

			//Obstacle Initialization
			rocks = new List<Coin>();

			//Score Initialization
			score = 0;


			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			//TODO: use this.Content to load your game content here 
			spriteBatch = new SpriteBatch(GraphicsDevice);
			font = Content.Load<SpriteFont>("Texts/GameOver");
			scoreFont = Content.Load<SpriteFont>("Texts/Score");

			Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + 
			                       GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
			player.Initialize(Content.Load<Texture2D>("Graphics/car"), playerPosition);

			//float screenWidth, float screenHeight, Texture2D texture, int speed
			bgLayer.Initialize(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, Content.Load<Texture2D>("Graphics/track"), bgSpeed);


		}


		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
#endif

			// TODO: Add your update logic here
			currentKey = Keyboard.GetState();
			keyUpdate(gameTime);

			if (!Active) return;

			PlayerUpdate(gameTime);
			bgLayer.speed = bgSpeed;
			bgLayer.Update(gameTime);


			CoinsUpdate(gameTime);
			CollisionUpdate(gameTime);



			base.Update(gameTime);
		}

		protected void keyUpdate(GameTime gameTime)
		{
			if (currentKey.IsKeyDown(Keys.R))
			{
				//Active = true;
				//rocks = new List<Coin>();
				//coins = new List<Coin>();
				//playerSpeed = 6.0f;
				//bgSpeed = 4.0f;
				//score = 0;
				this.Initialize();
			}
		}

		protected void PlayerUpdate(GameTime gameTime)
		{
			
			if (currentKey.IsKeyDown(Keys.Right))
				player.playerPosition.X += playerSpeed;

			if (currentKey.IsKeyDown(Keys.Left))
				player.playerPosition.X -= playerSpeed;

			if (currentKey.IsKeyDown(Keys.Up))
				player.playerPosition.Y -= playerSpeed;

			if (currentKey.IsKeyDown(Keys.Down))
				player.playerPosition.Y += playerSpeed;

			player.playerPosition.X = MathHelper.Clamp(player.playerPosition.X, 0, GraphicsDevice.Viewport.Width - player.playerTexture.Width);
			player.playerPosition.Y = MathHelper.Clamp(player.playerPosition.Y, 0, GraphicsDevice.Viewport.Height - player.playerTexture.Height);
		}

		protected void AddCoin(List<Coin> things, bool isObstacle)
		{
			Coin coin = new Coin();

			Texture2D coinTexture = Content.Load<Texture2D>("Graphics/coin");
			//if (!isObstacle) coinTexture = Content.Load<Texture2D>("Graphics/coin");
			if (isObstacle)
			{
				coinTexture = Content.Load<Texture2D>("Graphics/rock");
			}

			Vector2 coinPosition = new Vector2(GraphicsDevice.Viewport.Width + coinTexture.Width / 2 + random.Next(-100, 100),
											   random.Next(0, GraphicsDevice.Viewport.Height - coinTexture.Height));

			coin.Initialize(coinTexture, coinPosition, bgSpeed, isObstacle);
			things.Add(coin);
		}

		protected void CoinsUpdate(GameTime gameTime)
		{ 
			if (gameTime.TotalGameTime - previousSpanTime >= spawnTime)
			{
				previousSpanTime = gameTime.TotalGameTime;

				AddCoin(rocks, true);
				AddCoin(coins, false);

				if (score < 1000)
				{
					bgSpeed *= 1.05f;
					playerSpeed *= 1.05f;
				}
					
				else
				{
					spawnTimeValue *= 0.98f;
					spawnTime = TimeSpan.FromSeconds(spawnTimeValue);
				}
					
				
				
				//playerSpeed *= 1.1f;
			}

			for (int i = 0; i < coins.Count; i++)
			{
				coins[i].Update(gameTime);
				if (!coins[i].Active) coins.RemoveAt(i);
			}

			for (int i = 0; i < rocks.Count; i++)
			{
				rocks[i].Update(gameTime);
				if (!rocks[i].Active)
				{
					rocks.RemoveAt(i);
					score += (int)(5 * bgSpeed / 4.0f);
				}
			}
		}

		protected void CollisionUpdate(GameTime gameTime)
		{
			CoinCollisionUpdate(gameTime);
		}

		protected void CoinCollisionUpdate(GameTime gameTime)
		{
			Rectangle rect1 = new Rectangle((int)player.playerPosition.X, (int)player.playerPosition.Y,
											player.playerTexture.Width, player.playerTexture.Height);

			foreach (Coin coin in coins)
			{
				if (!coin.Active) continue;
				Rectangle rect2 = new Rectangle((int)coin.position.X, (int)coin.position.Y,
												coin.texture.Width, coin.texture.Height);

				if (rect1.Intersects(rect2)) {
					coin.Active = false;
					score += (int)(20 * bgSpeed / 4.0f);
				}
			}

			foreach (Coin rock in rocks)
			{
				if (!rock.Active) continue;
				Rectangle rect2 = new Rectangle((int)rock.position.X, (int)rock.position.Y,
												rock.texture.Width, rock.texture.Height);

				if (rect1.Intersects(rect2))

					this.Active = false;
			}
		}




		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			//TODO: Add your drawing code here
			spriteBatch.Begin();

			bgLayer.Draw(spriteBatch);

			foreach (Coin rock in rocks) rock.Draw(spriteBatch);
			foreach (Coin coin in coins) coin.Draw(spriteBatch);
			player.Draw(spriteBatch);

			spriteBatch.DrawString(scoreFont, "Score = " + score, new Vector2(50, 50), Color.White);
			if (!Active) spriteBatch.DrawString(font, "Final Score = " + score, new Vector2(150, 200), Color.Black);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
