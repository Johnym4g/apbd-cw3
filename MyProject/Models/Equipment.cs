namespace MyProject.Models;

public abstract class Equipment
{
    public Guid Id { get; protected set; }
    public string Name { get; protected set; }
    public bool IsAvailable { get; private set; }

    protected Equipment(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        IsAvailable = true;
    }

    public void MarkAsUnavailable()
    {
        IsAvailable = false;
    }

    public void MarkAsAvailable()
    {
        IsAvailable = true;
    }
}

public class Laptop : Equipment
{
    public string CpuModel { get; private set; }
    public int RamGb { get; private set; }

    public Laptop(string name, string cpuModel, int ramGb) : base(name)
    {
        CpuModel = cpuModel;
        RamGb = ramGb;
    }
}

public class Projector : Equipment
{
    public string Resolution { get; private set; }
    public int Lumens { get; private set; }

    public Projector(string name, string resolution, int lumens) : base(name)
    {
        Resolution = resolution;
        Lumens = lumens;
    }
}

public class Camera : Equipment
{
    public string LensType { get; private set; }
    public double Megapixels { get; private set; }

    public Camera(string name, string lensType, double megapixels) : base(name)
    {
        LensType = lensType;
        Megapixels = megapixels;
    }
}