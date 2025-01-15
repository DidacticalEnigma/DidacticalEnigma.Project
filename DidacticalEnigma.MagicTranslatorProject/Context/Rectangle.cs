namespace MagicTranslatorProject.Context;

public readonly record struct Rectangle<T>
{
    public T X => Position.X;

    public T Y => Position.Y;

    public T Width => Size.Width;

    public T Height => Size.Height;
    
    public Position<T> Position { get; }
    
    public Size<T> Size { get; }

    public Rectangle(Position<T> position, Size<T> size)
    {
        Position = position;
        Size = size;
    }

    public Rectangle(T x, T y, T width, T height)
    {
        Position = new Position<T>(x, y);
        Size = new Size<T>(width, height);
    }
}

public readonly record struct Position<T>
{
    public Position(T x, T y)
    {
        X = x;
        Y = y;
    }

    public T X { get; }
    
    public T Y { get; }
}

public readonly record struct Size<T>
{
    public Size(T width, T height)
    {
        Width = width;
        Height = height;
    }

    public T Width { get; }

    public T Height { get; }
}