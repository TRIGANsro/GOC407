namespace LOHBuffers;

public class LOHBuffer : IBuffer
{

    private readonly byte[] buffer;
    private const int LOHBufferMin = 85000;
    internal bool InUse { get; set; }

    public LOHBuffer()
    {
        buffer = new byte[LOHBufferMin];
    }

    public byte[] Buffer { get { return buffer; } }
}
