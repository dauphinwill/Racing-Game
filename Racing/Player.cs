using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Racing
{
	public class Player
	{
		public Player()
		{
		}

		public Texture2D playerTexture;
		public Vector2 playerPosition;

		public void Initialize(Texture2D texture, Vector2 position)
		{
			playerTexture = texture;
			playerPosition = position;
		}

		public void Update()
		{

		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(playerTexture, playerPosition, Color.Azure);
		}
	}
}
