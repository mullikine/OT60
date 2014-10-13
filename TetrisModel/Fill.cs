﻿using System;
using System.Threading.Tasks;

namespace TetrisModel
{
  /// <summary>
  /// Cell.
  /// </summary>
  public sealed class Fill : CompositeUnit
  {
    private int w = 0;
    private int h = 0;

    private int N = 0;
    private int M = 0;


    private Color color;

    private static Random rnd = new Random();

    public Fill(double x, double y, int n, int m, Color c, Func<GameUnitImplementation> creator) : base(x, y)
    {
      color = c;
      N = n;
      M = m;
      var tmp = creator();
      w = tmp.Width;
      h = tmp.Height;
      for (var i = 0; i < N; i++) for (var j = 0; j < M; j++) AddUnit(new Cell(x + i * w, y + j * h, (Color) (1 + rnd.Next(15)), creator));
      for (var i = 0; i < N; i++) for (var j = 0; j < M; j++) AddUnit(new Cell(x + i * w, y + j * h, color, creator));
    }


    public Fill(double x, double y, int n, int m, Color c, GraphicsFactory factory) : base(x, y)
    {
      color = c;
      N = n;
      M = m;
      var tmp = factory.CreateFillImplementation();
      w = tmp.Width;
      h = tmp.Height;
      for (var i = 0; i < N; i++) for (var j = 0; j < M; j++) AddUnit(new Cell(x + i * w, y + j * h, (Color) (1 + rnd.Next(15)), () => tmp));
      for (var i = 0; i < N; i++) for (var j = 0; j < M; j++) AddUnit(new Cell(x + i * w, y + j * h, color, () => tmp));
    }

    public override void Draw()
    {
      var item = 0;
      foreach (var unit in units) {
        item = item == M * N ? 1 : item + 1;
        var col = (int) ((item - 1) / N);
        var raw = item - 1 - col * N;
        var xx = x + raw;
        var yy = y + col;
    
        var xc = x + 0.5 * (N - 1);
        var yc = y + 0.5 * (M - 1);
    
        var xnew = (xx - xc) * Math.Cos(angle) - (yy - yc) * Math.Sin(angle);
        var ynew = (xx - xc) * Math.Sin(angle) + (yy - yc) * Math.Cos(angle);
    
        xnew += xc;
        ynew += yc;
    
        xnew = x + (xnew - x) * w;
        ynew = y + (ynew - y) * h;
    
        unit.Position(xnew, ynew);
        //unit.Position(0);
        unit.Draw();
      }
    }
  }
}

