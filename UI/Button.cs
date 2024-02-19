using AdvancedEdit.Types;
using static SDL2.SDL;
namespace AdvancedEdit.UI{
    public delegate void OnClick();
    class Button : UIElement {
        public IntPtr texture;
        public OnClick onClick;

        public Button(SDL_Rect elementPosition, IntPtr texture, OnClick? onClick){
            ElementPosition = elementPosition;
            this.texture = texture;
            this.onClick = onClick;
        }
        public override void Update()
        {}
        public override void Draw(){
            SDL_RenderCopy(Program.Renderer,texture,IntPtr.Zero,ref ElementPosition);
        }
        public override void Events(SDL_Event e){
            switch (e.type)
            {
                case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    if (ElementPosition.Contains(e.motion.x, e.motion.y))
                    {
                        if (onClick != null)
                        {
                            onClick();
                        }
                    }
                    break;
            }
            
        }

    }
}