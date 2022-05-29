using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace XstarS.IO
{
    /// <summary>
    /// 表示在释放后保持打开状态的 <see cref="Stream"/> 的包装。
    /// </summary>
    internal sealed class LeaveOpenStream : Stream
    {
        /// <summary>
        /// 表示当前包装的要保持打开状态的 <see cref="Stream"/>。
        /// </summary>
        private readonly Stream BaseStream;

        /// <summary>
        /// 以要保持打开状态的流初始化 <see cref="LeaveOpenStream"/> 类的新实例。
        /// </summary>
        /// <param name="baseStream">要保持打开状态的流。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseStream"/> 为 <see langword="null"/>。</exception>
        internal LeaveOpenStream(Stream baseStream)
        {
            this.BaseStream = baseStream ??
                throw new ArgumentNullException(nameof(baseStream));
        }

        /// <inheritdoc/>
        public override bool CanRead => this.BaseStream.CanRead;

        /// <inheritdoc/>
        public override bool CanSeek => this.BaseStream.CanSeek;

        /// <inheritdoc/>
        public override bool CanTimeout => this.BaseStream.CanTimeout;

        /// <inheritdoc/>
        public override bool CanWrite => this.BaseStream.CanWrite;

        /// <inheritdoc/>
        public override long Length => this.BaseStream.Length;

        /// <inheritdoc/>
        public override long Position
        {
            get => this.BaseStream.Position;
            set => this.BaseStream.Position = value;
        }

        /// <inheritdoc/>
        public override int ReadTimeout
        {
            get => this.BaseStream.ReadTimeout;
            set => this.BaseStream.ReadTimeout = value;
        }

        /// <inheritdoc/>
        public override int WriteTimeout
        {
            get => this.BaseStream.WriteTimeout;
            set => this.BaseStream.WriteTimeout = value;
        }

        /// <inheritdoc/>

        public override IAsyncResult BeginRead(
            byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
        {
            return this.BaseStream.BeginRead(buffer, offset, count, callback!, state);
        }

        /// <inheritdoc/>
        public override IAsyncResult BeginWrite(
            byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
        {
            return this.BaseStream.BeginWrite(buffer, offset, count, callback!, state);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <inheritdoc/>
        public override void CopyTo(Stream destination, int bufferSize)
        {
            this.BaseStream.CopyTo(destination, bufferSize);
        }
#endif

        /// <inheritdoc/>
        public override Task CopyToAsync(
            Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            return this.BaseStream.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        /// <inheritdoc/>
        public override int EndRead(IAsyncResult asyncResult)
        {
            return this.BaseStream.EndRead(asyncResult);
        }

        /// <inheritdoc/>
        public override void EndWrite(IAsyncResult asyncResult)
        {
            this.BaseStream.EndWrite(asyncResult);
        }

        /// <inheritdoc/>
        public override void Flush() => this.BaseStream.Flush();

        /// <inheritdoc/>
        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return this.BaseStream.FlushAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.BaseStream.Read(buffer, offset, count);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <inheritdoc/>
        public override int Read(Span<byte> buffer) => this.BaseStream.Read(buffer);
#endif

        /// <inheritdoc/>
        public override Task<int> ReadAsync(
            byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return this.BaseStream.ReadAsync(buffer, offset, count, cancellationToken);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <inheritdoc/>
        public override ValueTask<int> ReadAsync(
            Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            return this.BaseStream.ReadAsync(buffer, cancellationToken);
        }
#endif

        /// <inheritdoc/>
        public override int ReadByte() => this.BaseStream.ReadByte();

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.BaseStream.Seek(offset, origin);
        }

        /// <inheritdoc/>
        public override void SetLength(long value) => this.BaseStream.SetLength(value);

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            this.BaseStream.Write(buffer, offset, count);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <inheritdoc/>
        public override void Write(ReadOnlySpan<byte> buffer) => this.BaseStream.Write(buffer);
#endif

        /// <inheritdoc/>
        public override Task WriteAsync(
            byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return this.BaseStream.WriteAsync(buffer, offset, count, cancellationToken);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <inheritdoc/>
        public override ValueTask WriteAsync(
            ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            return this.BaseStream.WriteAsync(buffer, cancellationToken);
        }
#endif

        /// <inheritdoc/>
        public override void WriteByte(byte value) => this.BaseStream.WriteByte(value);
    }
}
