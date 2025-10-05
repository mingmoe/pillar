using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pillar.Gui
{
    /// <summary>
    /// It stands a component of the UI.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// The box model of the component
        /// </summary>
        Box Box { get; }

        /// <summary>
        /// The parent of the component, if any.
        /// </summary>
        IComponent? Parent { get; set; }

        /// <summary>
        /// The component's current focus state.
        /// </summary>
        bool IsFocused { get; set; }

        /// <summary>
        /// Used when cycling over components and picking which one should have focus.
        /// </summary>
        bool IsFocusable { get; }

        /// <summary>
        /// Good place to setup states before inputs are processed.
        /// </summary>
        void UpdateSetup(GameTime gameTime);

        /// <summary>
        /// Might or might not be called. Handles user inputs.
        /// Inputs are separated to allow for freezing inputs while keeping the UI responsive.
        /// </summary>
        void UpdateInput(GameTime gameTime);

        /// <summary>
        /// Final update step. Usually for doing animations.
        /// This is where everything that doesn't rely on inputs gets updated.
        /// </summary>
        void Update(GameTime gameTime);

        /// <summary>
        /// First pass for layout management.
        /// Components can determine their preferred sizes.
        /// </summary>
        /// <returns>if the box updated,return true.</returns>
        bool UpdateBox(GameTime gameTime);

        /// <summary>
        /// draw content,padding,border at the position,ignore margin.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="position"></param>
        void Draw(GameTime gameTime, Point position);
    }
}
