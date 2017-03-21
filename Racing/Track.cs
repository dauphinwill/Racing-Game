using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Racing
{
	public class Track
	{
		public Track()
		{
		}

		float bgWidth;
		float bgHeight;

		Texture2D texture;
		Vector2[] positions;

		int speed;

		public void Initialize(float screenWidth, float screenHeight, Texture2D texture, int speed)
		{
			this.texture = texture;
			bgHeight = screenHeight;
			bgWidth = texture.Width * (screenHeight / texture.Height);

			this.speed = speed;

			positions = new Vector2[(int)screenWidth / (int)bgWidth + 2];

			for (int i = 0; i < positions.Length; i++)
				positions[i] = new Vector2(i * bgWidth, 0);
		}

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < positions.Length; i++)
			{
				positions[i].X -= speed;

				if (positions[i].X <= -bgWidth) positions[i].X = (positions.Length - 1) * bgWidth;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < positions.Length; i++)
			{
				Rectangle rect = new Rectangle((int)positions[i].X, (int)positions[i].Y, (int)bgWidth, (int)bgHeight);
				spriteBatch.Draw(texture, rect, Color.Azure);
			}
		}
	}
}
