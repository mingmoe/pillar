using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pillar.Gui
{
    public abstract class Component(Box initialBox,IComponent? parent,SpriteBatch spriteBatch):IComponent
    {
        public SpriteBatch SpriteBatch { get; init; } = spriteBatch;
        public Box Box { get; set; } = initialBox;
        public IComponent? Parent { get; set; } = parent;
        public abstract bool IsFocused { get; set; }
        public abstract bool IsFocusable { get; }
        public abstract void UpdateSetup(GameTime gameTime);
        public abstract void UpdateInput(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
        public abstract void UpdatePrefSize(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, Point position);
    }
}
