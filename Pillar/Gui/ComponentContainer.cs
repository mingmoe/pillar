using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pillar.Gui
{
    public abstract class ComponentContainer(Box initialBox,IComponent? parent,SpriteBatch spriteBatch)
        :List<IComponent>,IComponentContainer
    {
        protected bool BoxChanged { get; set; } = false;
        public SpriteBatch SpriteBatch { get; init; } = spriteBatch;

        private Box _box = initialBox;

        public Box Box
        {
            get
            {
                return _box;
            }
            set
            {
                _box = value;
                BoxChanged = true;
            }
        }

        public IComponent? Parent { get; set; } = parent;
        public abstract bool IsFocused { get; set; }
        public abstract bool IsFocusable { get; }
        public abstract void UpdateSetup(GameTime gameTime);
        public abstract void UpdateInput(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
        public abstract bool UpdateBox(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, Point position);
    }
}
