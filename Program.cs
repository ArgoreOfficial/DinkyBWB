﻿
using Dinky_bwb;
using System;

public static class Program
{
    public static Game1 Game;

    [STAThread]
    static void Main()
    {
        using (var game = new Game1())
        {
            Game = game;
            Game.Run();
        }
    }
}