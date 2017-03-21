using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Racing
{
	public class template
	{
		public Texture2D texture;
		public Vector2 position;
		public float speed;
		public bool Active;

		public void Initialize(Texture2D texture, Vector2 position, float speed)
		{
			this.texture = texture;
			this.position = position;
			this.speed = speed;
			this.Active = true;
		}

		public void Update(GameTime gameTime)
		{

		}

		public void Draw(SpriteBatch spriteBatch)
		{

		}
	}
}
