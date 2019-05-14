/*
 * MXG - Minimalist XML Generator
 * Copyright Raphael Stoeckli © 2019
 * This library is licensed under the MIT License.
 * You find a copy of the license in project folder or on: http://opensource.org/licenses/MIT
 */

using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MXG.Core
{
    /// <summary>
    /// /Abstract class representing an XML document of any kind
    /// </summary>
    public abstract class AbstractXmlDocument
    {
        /// <summary>
        /// Abstract method to get the string of the entire XML document
        /// </summary>
        /// <returns>Non-beautified XML string</returns>
        public abstract string GetXmlString();

        /// <summary>
        /// Saves the XML document under the specified file name
        /// </summary>
        /// <param name="fileName">Full path of the XML file</param>
        /// <remarks>This method is does not use try-catch internally and may throw an Exception. Wrap the call into a try-catch block to avoid runtime errors</remarks>
        public void Save(string fileName)
        {
            this.Save(fileName, Encoding.UTF8);
        }

        /// <summary>
        /// Saves the XML document under the specified file name using a stream writer
        /// </summary>
        /// <param name="fileName">Full path of the XML file</param>
        /// <param name="encoding">Encoding of the XML text</param>
        /// <param name="bufferSize">Optional size of the buffer for the internal stream writer</param>
        /// <remarks>This method is does not use try-catch internally and may throw an Exception. Wrap the call into a try-catch block to avoid runtime errors</remarks>
        public void Save(string fileName, Encoding encoding, int bufferSize = Constants.DEFAULT_STREAM_BUFFER_SIZE)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, encoding, bufferSize))
            {
                writer.Write(this.GetXmlString());
                writer.Flush();
            }
        }

        /// <summary>
        /// Saves the XML document asynchronous under the specified file name
        /// </summary>
        /// <param name="fileName">Full path of the XML file</param>
        /// <returns>Async Task</returns
        /// <remarks>This method is does not use try-catch internally and may throw an Exception. Wrap the call into a try-catch block to avoid runtime errors</remarks>
        public async Task SaveAsync(string fileName)
        {
            await this.SaveAsync(fileName, Encoding.UTF8);
        }

        /// <summary>
        /// Saves the XML document asynchronous under the specified file name using an asynchronous stream writer
        /// </summary>
        /// <param name="fileName">Full path of the XML file</param>
        /// <param name="encoding">Encoding of the XML text</param>
        /// <param name="bufferSize">Optional size of the buffer for the internal stream writer</param>
        /// <returns>Async Task</returns>
        /// <remarks>This method is does not use try-catch internally and may throw an Exception. Wrap the call into a try-catch block to avoid runtime errors</remarks>
        public async Task SaveAsync(string fileName, Encoding encoding, int bufferSize = Constants.DEFAULT_STREAM_BUFFER_SIZE)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, encoding, bufferSize))
            {
                await writer.WriteAsync(this.GetXmlString());
                await writer.FlushAsync();
            }
        }

        /// <summary>
        /// Saves the XML document as stream. The stream will be closed automatically.
        /// </summary>
        /// <param name="stream">Writable stream</param>
        /// <remarks>This method is does not use try-catch internally and may throw an Exception. Wrap the call into a try-catch block to avoid runtime errors</remarks>
        public void SaveAsStream(Stream stream)
        {
            this.SaveAsStream(stream, Encoding.UTF8, Constants.DEFAULT_STREAM_BUFFER_SIZE, false);
        }

        /// <summary>
        /// Saves the XML document as stream. The stream can be left open optionally to copy the stream content after writing
        /// </summary>
        /// <param name="stream">Writable stream. If the stream shall be copied after the save action, its position has to be reset to zero</param>
        /// <param name="leaveOpen">If set to <c>true</c>, the stream is left open</param>
        /// <remarks>This method is does not use try-catch internally and may throw an Exception. Wrap the call into a try-catch block to avoid runtime errors</remarks>
        public void SaveAsStream(Stream stream, bool leaveOpen = false)
        {
            SaveAsStream(stream, Encoding.UTF8, Constants.DEFAULT_STREAM_BUFFER_SIZE, leaveOpen);
        }

        /// <summary>
        /// Saves the XML document as stream using a stream writer. The stream can be left open optionally to copy the stream content after writing
        /// </summary>
        /// <param name="stream">Writable stream. If the stream shall be copied after the save action, its position has to be reset to zero</param>
        /// <param name="encoding">Encoding of the XML text</param>
        /// <param name="bufferSize">Optional size of the buffer for the internal stream writer</param>
        /// <param name="leaveOpen">If set to <c>true</c>, the stream is left open</param>
        /// <remarks>This method is does not use try-catch internally and may throw an Exception. Wrap the call into a try-catch block to avoid runtime errors</remarks>
        public void SaveAsStream(Stream stream, Encoding encoding, int bufferSize = Constants.DEFAULT_STREAM_BUFFER_SIZE, bool leaveOpen = false)
        {
            using (StreamWriter writer = new StreamWriter(stream, encoding, bufferSize, leaveOpen))
             {
            writer.Write(this.GetXmlString());
            stream.Flush();
             }
        }

        /// <summary>
        /// Saves the XML document as stream asynchronous. The stream will be closed automatically.
        /// </summary>
        /// <param name="stream">Writable stream</param>
        /// <returns>Async Task</returns>
        /// <remarks>This method is does not use try-catch internally and may throw an Exception. Wrap the call into a try-catch block to avoid runtime errors</remarks>
        public async Task SaveAsStreamAsync(Stream stream)
        {
            await this.SaveAsStreamAsync(stream, Encoding.UTF8, Constants.DEFAULT_STREAM_BUFFER_SIZE, false);
        }

        /// <summary>
        /// Saves the XML document as stream asynchronous. The stream can be left open optionally to copy the stream content after writing
        /// </summary>
        /// <param name="stream">Writable stream. If the stream shall be copied after the save action, its position has to be reset to zero</param>
        /// <param name="leaveOpen">If set to <c>true</c>, the stream is left open</param>
        /// <returns>Async Task</returns>
        /// <remarks>This method is does not use try-catch internally and may throw an Exception. Wrap the call into a try-catch block to avoid runtime errors</remarks>
        public async Task SaveAsStreamAsync(Stream stream, bool leaveOpen = false)
        {
            await SaveAsStreamAsync(stream, Encoding.UTF8, Constants.DEFAULT_STREAM_BUFFER_SIZE, leaveOpen);
        }

        /// <summary>
        /// Saves the XML document as stream asynchronous using an asynchronous stream writer. The stream can be left open optionally to copy the stream content after writing
        /// </summary>
        /// <param name="stream">Writable stream. If the stream shall be copied after the save action, its position has to be reset to zero</param>
        /// <param name="encoding">Encoding of the XML text</param>
        /// <param name="bufferSize">Optional size of the buffer for the internal stream writer</param>
        /// <param name="leaveOpen">If set to <c>true</c>, the stream is left open</param>
        /// <returns>Async Task</returns>
        /// <remarks>This method is does not use try-catch internally and may throw an Exception. Wrap the call into a try-catch block to avoid runtime errors</remarks>
        public async Task SaveAsStreamAsync(Stream stream, Encoding encoding, int bufferSize = Constants.DEFAULT_STREAM_BUFFER_SIZE, bool leaveOpen = false)
        {
            using (StreamWriter writer = new StreamWriter(stream, encoding, bufferSize, leaveOpen))
            {
                await writer.WriteAsync(this.GetXmlString());
                await stream.FlushAsync();
            }
        }

    }
}