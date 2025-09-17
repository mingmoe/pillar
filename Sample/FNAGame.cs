using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moe.TextEngine;
using NLog.Layouts;
using Color = Microsoft.Xna.Framework.Color;

namespace Sample;

internal sealed class FNAGame : Game
{
	public required ILogger<FNAGame> Logger { get; init; }

	private SpriteBatch _spriteBatch = null!;
	private RenderEngine _fontEngine = null!;
	private LayoutEngine _layoutEngine = null!;

	public GraphicsDeviceManager DeviceManager { get; init; }

	public FNAGame()
	{
		DeviceManager = new(this)
		{
			PreferredBackBufferWidth = 1280,
			PreferredBackBufferHeight = 720,
			IsFullScreen = false,
			SynchronizeWithVerticalRetrace = true
		};

	}
	protected override void Initialize()
	{
		// FontSystemDefaults.FontLoader = new FreeTypeLoader();
		_spriteBatch = new SpriteBatch(GraphicsDevice);
		_fontEngine = new(GraphicsDevice)
		{
			RasterizerEngine = new FreeTypeEngine(),
			ShapeEngine = new HarfbuzzShapeEngine(),
		};
        _layoutEngine = new(_fontEngine);
		base.Initialize();
	}

	protected override void LoadContent()
	{
		base.LoadContent();

        _fontEngine.AddFont(new(new FileFontSource(@"resources/fonts/NotoEmoji-Regular.ttf"), 0));
        _fontEngine.AddFont(new(new FileFontSource(@"resources/fonts/SarasaFixedSC-Regular.ttf"), 0));
    }

	protected override void UnloadContent()
	{
		base.UnloadContent();
	}

	protected override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		base.Draw(gameTime);

		GraphicsDevice.Clear(Color.BurlyWood);

		_spriteBatch.Begin();

		var shapeRun = new ShapeRun()
		{
			FontOptions = new FontOptions()
			{
				PixelHeight = 22,
				PixelWidth = 22,
			},
			Text = "Hello World.😄西郊有密林，助君出重围。",
			ShapeOptions = new()
			{
				Direction = TextDirection.LeftToRight,
				Region = "zh-CN",
				Script = "Hans"
			},
		};

        int yPen = 0;

        var layouts = _layoutEngine.SplitLines(shapeRun, 200);

		foreach(var layout in layouts)
        {
			_fontEngine.DrawString(layout, new(0,yPen), _spriteBatch);

            yPen += shapeRun.FontOptions.PixelHeight;
        }
		
		_spriteBatch.End();
	}
}
