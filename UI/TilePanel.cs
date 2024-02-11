using System.Drawing;
using MkscEdit.Extract;
using MkscEdit.Types;
using static SDL2.SDL;
namespace MkscEdit.UI{
    class TilePanel{
        public byte[,] indicies;
        public Track trackId;
        public IntPtr tileAtlas;
        public SDL_Rect ElementPosition;
        public Point ContentPosition;
        public int tileSize = 16;
        public int columns = 16;
        public int rows = 16;
        
        public TilePanel(SDL_Rect elementPosition, Point contentPosition) {
            indicies = new byte[0,0];
            tileAtlas = SDL_CreateTextureFromSurface(Program.Renderer,SDL_CreateRGBSurface(0, 8, 2048, 32, 0, 0, 0, 0));
            ElementPosition = elementPosition;
            ContentPosition = contentPosition;
        }
        public unsafe void SetTrack(Track track){
            trackId = track;
            IntPtr ta = SDL_CreateRGBSurface(0,8,2048,32,0,0,0,0);
            for (int i = 0; i < Program.rom.tiles[(int)track].Length; i++)
            {
                var t = Program.rom.tiles[(int)track][i];
                IntPtr s = (IntPtr)t.ToImage();
                SDL_Rect d = new SDL_Rect { x = 0, y = i * 8, w = 8, h = 8 };
                SDL_BlitSurface(s, IntPtr.Zero, ta, ref d);
                SDL_FreeSurface(s);
            }
            SDL_DestroyTexture(tileAtlas);
            tileAtlas = SDL_CreateTextureFromSurface(Program.Renderer, ta);
        }
        public int GetTile(int x, int y){
            if (new SDL_Rect() {x=ContentPosition.X,y=ContentPosition.Y,w=tileSize*columns,h=tileSize*rows}.Contains(x,y)){
                int tilex = (int)Math.Floor((decimal)(x-ContentPosition.X)/tileSize);
                int tiley = (int)Math.Floor((decimal)(y-ContentPosition.Y)/tileSize);
                int temp = tilex + tiley * columns;
                return indicies[(int)temp/16, temp%16];
            } else {
                return -1;
            }
        }
        public void SetTile(byte idx, int x, int y) { 
            if (new SDL_Rect() {x=ContentPosition.X,y=ContentPosition.Y,w=tileSize*columns,h=tileSize*rows}.Contains(x,y)){
                int tilex = (int)Math.Floor((decimal)(x-ContentPosition.X)/tileSize);
                int tiley = (int)Math.Floor((decimal)(y-ContentPosition.Y)/tileSize);
                int temp = tilex + tiley * columns;
                indicies[(int)temp / 16, temp%16] = (byte)idx;
            }
        }
        public void DrawElement(){
            SDL_RenderSetClipRect(Program.Renderer,ref ElementPosition);
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    SDL_Rect s = new SDL_Rect() { x = 0, y = indicies[x,y]*8, w = 8, h = 8 };
                    SDL_Rect d = new SDL_Rect() { x = x * tileSize + ContentPosition.X, y = y * tileSize + ContentPosition.Y, w = tileSize, h = tileSize };
                    SDL_RenderCopy(Program.Renderer, tileAtlas, ref s, ref d);
                }
            }
            SDL_RenderSetClipRect(Program.Renderer, IntPtr.Zero);
        }
    }
}