// <copyright file="StreamingMemoryStream.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>


namespace ImageSharp.Benchmarks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// This is a memory stream wrapper that doesn't support seeking.
    /// </summary>
    /// <seealso cref="System.IO.Stream" />
    public class StreamingMemoryStream : Stream
    {
        private readonly MemoryStream stream;

        public StreamingMemoryStream(byte[] data)
        {
            this.stream = new MemoryStream(data, false);
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => this.stream.Length;

        public override long Position
        {
            get
            {
                return stream.Position;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void Flush()
        {
            stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
