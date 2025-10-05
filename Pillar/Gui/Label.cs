using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moe.TextEngine;

namespace Pillar.Gui
{
    public class Label(IComponent parent,RenderEngine engine,ShapeRun run)
    {
        public RenderEngine TextRenderEngine { get; init; } = engine;

        public IComponent? Parent { get; init; } = parent;

        public ShapeRun Text { get; set; } = run;

        public void Draw(Rectangle box, SpriteBatch batch)
        {
            
        }

        public Point GetPosition(Rectangle box)
        {
            return new(box.X, box.Y);
        }

        public Point GetSize(Rectangle box)
        {
            return TextRenderEngine.MeasureSize(Text);
        }
    }
}
