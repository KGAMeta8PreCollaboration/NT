using UnityEngine;

public class Utility
{
	public static Sprite ByteToSprite(byte[] bytes, string filePath = null)
	{
		Texture2D texture = new Texture2D(100, 100);
		texture.LoadImage(bytes);
		Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
		sprite.name = texture.name;
		return sprite;
	}	
}