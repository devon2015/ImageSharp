// <copyright file="PeekableStream.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.IO
{
    using System;
    using System.Buffers;
    using System.IO;

    /// <summary>
    /// A Stream that support peeking readonly none seekable streams, with minimal buffering
    /// </summary>
    internal class PeekableStream : Stream
    {
        private const int MinBufferGrowSize = 10;
        private readonly Stream stream;

        /// <summary>
        /// Indicates that this stream manages the underlying stream, thus disposing it when this is disposed.
        /// </summary>
        private readonly bool manageStream;

        private int peekedAmount = 0;
        private int peekedOffset = 0;
        private byte[] peekBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeekableStream" /> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="manageStream">if set to <c>true</c> [manage stream].</param>
        /// <exception cref="System.ArgumentException">Stream must be readable - stream</exception>
        public PeekableStream(Stream stream, bool manageStream)
        {
            this.manageStream = manageStream;

            if (stream.CanRead == false)
            {
                throw new ArgumentException("Stream must be readable", nameof(stream));
            }

            this.stream = stream;
        }

        /// <summary>
        /// Gets the peek position.
        /// </summary>
        /// <value>
        /// The peek position.
        /// </value>
        public long PeekPosition => this.stream.Position;

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead => true;

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek => false;

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite => false;

        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>
        public override long Length => this.stream.Length;

        /// <summary>
        /// Gets or sets the position within the current stream.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Stream is not seekable</exception>
        public override long Position
        {
            get
            {
                return this.stream.Position - this.peekedAmount + this.peekedOffset;
            }

            set
            {
                throw new InvalidOperationException("Stream is not seekable");
            }
        }

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
            this.stream.Flush();
        }

        /// <summary>
        /// Peeks the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>the number of bytes peeked</returns>
        public int Peek(byte[] buffer, int offset, int count)
        {
            this.EnsurePeekBuffer(count);

            var read = this.stream.Read(this.peekBuffer, this.peekedAmount, count);

            Array.Copy(this.peekBuffer, this.peekedAmount, buffer, offset, read);
            this.peekedAmount += read;
            return read;
        }

        /// <summary>
        /// Peeks the byte.
        /// </summary>
        /// <returns>the byte value of the peeks data or -1 if no data found</returns>
        public int PeekByte()
        {
            this.EnsurePeekBuffer(1);
            var b = this.stream.ReadByte();
            if (b > -1)
            {
                this.peekBuffer[this.peekedAmount] = (byte)b;
                this.peekedAmount++;
            }

            return b;
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            var unreadBuffer = this.peekedAmount - this.peekedOffset;
            var readamount = Math.Min(unreadBuffer, count);
            count -= readamount;

            // read from peek buffer;
            if (unreadBuffer > 0)
            {
                // let release the buffer
                Array.Copy(this.peekBuffer, this.peekedOffset, buffer, offset, readamount);
                this.peekedOffset += readamount;
                unreadBuffer -= readamount;
            }

            // read remainign from underlying stream
            var streamREadAmount = this.stream.Read(buffer, offset + readamount, count);

            // empty buffer drop it
            if (this.peekBuffer != null && unreadBuffer <= 0)
            {
                ArrayPool<byte>.Shared.Return(this.peekBuffer);
                this.peekBuffer = null;
                this.peekedAmount = 0;
                this.peekedOffset = 0;
            }

            return streamREadAmount + readamount;
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Stream is not seekable</exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new InvalidOperationException("Stream is not seekable");
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="System.InvalidOperationException">Streams length cannot be set</exception>
        public override void SetLength(long value)
        {
            throw new InvalidOperationException("Streams length cannot be set");
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="System.InvalidOperationException">Stream is readonly</exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException("Stream is readonly");
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.Stream" /> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.peekBuffer != null)
                {
                    ArrayPool<byte>.Shared.Return(this.peekBuffer);
                    this.peekBuffer = null;
                }
            }

            if (this.manageStream)
            {
                this.stream.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Ensures the peek buffer.
        /// </summary>
        /// <param name="requestedBytes">The requested bytes.</param>
        private void EnsurePeekBuffer(int requestedBytes)
        {
            if (this.peekBuffer == null)
            {
                if (requestedBytes < MinBufferGrowSize)
                {
                    requestedBytes = MinBufferGrowSize;
                }

                this.peekBuffer = ArrayPool<byte>.Shared.Rent(requestedBytes);

                // the peek bufffer is fine;
                return;
            }

            // buffer is set lets see if there is enought spare space in the buffer

            // peeked amount is the amount of bytes in the buffer that have been set
            var len = this.peekedAmount + requestedBytes;
            if (this.peekBuffer.Length >= len)
            {
                // the current buffer is large enough
                return;
            }

            // buffer needs replacing
            // will the current buffer offset to 0 work?
            var actualyRequiredLength = requestedBytes + (this.peekedAmount - this.peekedOffset);
            if (this.peekBuffer.Length >= len)
            {
                this.CopyToArrayStart(this.peekBuffer, this.peekBuffer, this.peekedOffset, this.peekedAmount);
                this.peekedAmount -= this.peekedOffset;
                this.peekedOffset = 0;
                return;
            }

            if (requestedBytes < MinBufferGrowSize)
            {
                requestedBytes = MinBufferGrowSize;
            }

            actualyRequiredLength = requestedBytes + (this.peekedAmount - this.peekedOffset);
            var newBuffer = ArrayPool<byte>.Shared.Rent(actualyRequiredLength);

            this.CopyToArrayStart(newBuffer, this.peekBuffer, this.peekedOffset, this.peekedAmount);
            this.peekedAmount -= this.peekedOffset;
            this.peekedOffset = 0;

            ArrayPool<byte>.Shared.Return(this.peekBuffer);
            this.peekBuffer = newBuffer;
        }

        /// <summary>
        /// Copies to array start.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        private void CopyToArrayStart(byte[] target, byte[] source, int offset, int count)
        {
            for (var i = 0; i < count; i++)
            {
                target[i] = source[i + offset];
            }
        }
    }
}
