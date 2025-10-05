using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pillar.Gui
{
    public sealed class FlexContainer(Box initialBox, IComponent? parent, SpriteBatch spriteBatch)
        : ComponentContainer(initialBox, parent, spriteBatch)
    {
        public override bool IsFocused { get; set; }

        public override bool IsFocusable { get; } = false;

        private int Redesign()
        {

        }

        public override void Draw(GameTime gameTime, Point position)
        {
            int xPen = position.X + Box.Padding.Left;
            int yPen = position.Y + Box.Padding.Top;
            
            // we need no border
            foreach (var component in this)
            {
                component
            }
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void UpdateInput(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateBox(GameTime gameTime)
        {
            return false;
        }

        public override void UpdateSetup(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
