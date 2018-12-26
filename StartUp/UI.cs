namespace StartUp
{
    using System;
    using System.Collections.Generic;
    using static StartUp;
    using static Consts;

    public class UI
    {
        private readonly List<Vector2> Elements;

        public UI()
        {
            this.Elements = new List<Vector2>();

            for (int i = 0; i < PLAYFIELD_HEIGHT; i++)
            {
                Elements.Add(new Vector2(PLAYFIELD_WIDTH, i));
            }
        }

        public void Draw()
        {
            for (int i = 0; i < this.Elements.Count; i++)
            {
                Console.SetCursorPosition(Elements[i].X, Elements[i].Y);
                Console.Write("|");
            }

            Console.SetCursorPosition(PLAYFIELD_WIDTH + 1, PLAYFIELD_HEIGHT - 15);
            Console.Write($"Score:{score}");
        }

        public void Delete()
        {
            for (int i = 0; i < this.Elements.Count; i++)
            {
                Console.SetCursorPosition(Elements[i].X, Elements[i].Y);
                Console.Write(" ");
            }
        }
    }
}
