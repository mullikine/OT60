using System;
using System.Collections.Generic;
using System.Collections;

namespace Yagan
{
  public class CloneSprite : IPixel
  {
    readonly IPixel sprite;
    public CloneSprite(Sprite sprite)
    {
      this.sprite = sprite.Clone();
    }
    public void Set(double x, double y)
    {
      sprite.Set(x, y);
    }

    public void Move(double dx, double dy)
    {
      sprite.Move(dx, dy);
    }

    public void Accept(IPixelVisitor visitor)
    {
      sprite.Accept(visitor);
    }

    public void Draw(IPainter painter)
    {
      sprite.Draw(painter);
    }

    public IPixel Clone()
    {
      return sprite.Clone();
    }

    public double X { get { return sprite.X; } }

    public double Y { get { return sprite.Y; } }

    public ConsoleColor Color { get { return sprite.Color; } }

    public int Width { get { return sprite.Width; } }

    public int Height { get { return sprite.Height; } }
  }


  // создать две разновидности спрайта:
  // Plain - без вращения
  // Sprite - с углом
  // соот-но Painter будет принимать на вход два разных объекта и по разному рисовать
  // аналоги Rotate strategy и  Plain strategy

  public class Sprite : IPixel, IEnumerable<IPixel>
  {
    protected readonly List<IPixel> pixels = new List<IPixel>();
    private double x, y;

    public virtual double X { get { return width == 0 ? x : xmin; } protected set { xmin = value; } }
    public virtual double Y { get { return height == 0 ? y : ymax; } protected set { ymax = value; } }
    public ConsoleColor Color { get; }
    public double Angle { get; protected set; }

    private int width;
    private int height;
    public virtual int Width { get { return width; } }
    public virtual int Height { get { return height; } }

    public void Add(IPixel pixel, bool show = true)
    {
      CalculateBounds(pixel);
      if (show) pixels.Add(pixel);
    }

    private double xmin = 1e6, xmax = -1e6, ymin = 1e6, ymax = -1e6;
    void CalculateBounds(IPixel pixel)
    {
      xmin = Math.Min(xmin, pixel.X);
      xmax = Math.Max(xmax, pixel.X + pixel.Width);
      ymin = Math.Min(ymin, pixel.Y - pixel.Height);
      ymax = Math.Max(ymax, pixel.Y);
      width = (int) (xmax - xmin);
      height = (int) (ymax - ymin);
    }

    public Sprite(List<IPixel> pixels = null, double angle = 0, ConsoleColor color = ConsoleColor.White)
    {
      X = 0;
      Y = 0;
      Angle = angle;
      Color = color;
      height = 0;
      width = 0;
      if (pixels != null) foreach (var pixel in pixels) Add(pixel);
    }

    public void Move(double dx, double dy)
    {
      foreach (var pixel in pixels) {
        pixel.Move(dx, dy);
      }
      X += dx;
      Y += dy;
    }
    public virtual void Accept(IPixelVisitor visitor)
    {
      foreach (var pixel in pixels) {
        pixel.Accept(visitor);
      }
      visitor.Visit(this);
    }
    public void Draw(IPainter painter)
    {
      painter.Draw(this);
    }

    public virtual bool ShowLabel { get; set; } = true;
    public override string ToString()
    {
      return ShowLabel ? string.Format("[Sprite: {0}° {1}:{2} ({3:F2},{4:F2})]", (int) Math.Round(Angle / Math.PI * 180), Width, Height, X, Y) : null;
    }

    public void Set(double x, double y)
    {
    }

    public virtual IPixel Clone()
    {
      return new Sprite(pixels, Angle, Color){ ShowLabel = ShowLabel };
    }

    public IEnumerator<IPixel> GetEnumerator()
    {
      foreach (var pixel in pixels) yield return pixel;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

  }
}
