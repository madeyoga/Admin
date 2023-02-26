/***
 * Copied from
 * https://github.com/DamianEdwards/MinimalRazorComponents/blob/main/src/MinimalRazorComponents/Infrastructure/BufferWriterExtensions.cs
 */
using System.Buffers;
using System.Diagnostics;
using System.Text.Unicode;

namespace MyAdmin.Admin;

public static class BufferWriterExtensions
{
    public static void WriteHtml(this IBufferWriter<byte> bufferWriter, string? encoded)
    {
        if (string.IsNullOrEmpty(encoded))
        {
            return;
        }

        ReadOnlySpan<char> textSpan = encoded;
        WriteHtml(bufferWriter, textSpan);
    }

    private static void WriteHtml(IBufferWriter<byte> bufferWriter, ReadOnlySpan<char> encoded)
    {
        Span<byte> writerSpan = bufferWriter.GetSpan();
        var status = OperationStatus.Done;

        while (encoded.Length > 0)
        {
            if (writerSpan.Length == 0)
            {
                writerSpan = bufferWriter.GetSpan();
            }

            status = Utf8.FromUtf16(encoded, writerSpan, out var charsWritten, out var bytesWritten);

            encoded = encoded[charsWritten..];
            writerSpan = writerSpan[bytesWritten..];
            bufferWriter.Advance(bytesWritten);
        }

        Debug.Assert(status == OperationStatus.Done, "Bad math in IBufferWriter HTML writing extensions");
    }
}
