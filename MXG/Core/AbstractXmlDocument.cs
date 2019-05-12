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

        public void Save(string fileName)
        {
            this.Save(fileName, Encoding.UTF8);
        }

        public void Save(string fileName, Encoding encoding, int bufferSize = Constants.DEFAULT_STREAM_BUFFER_SIZE)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, encoding, bufferSize))
            {
                writer.Write(this.GetXmlString());
            }
        }

        public async Task SaveAsync(string fileName)
        {
            await this.SaveAsync(fileName, Encoding.UTF8);
        }

        public async Task SaveAsync(string fileName, Encoding encoding, int bufferSize = Constants.DEFAULT_STREAM_BUFFER_SIZE)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, encoding, bufferSize))
            {
                await writer.WriteAsync(this.GetXmlString());
            }
        }

        public void SaveAsStream(Stream stream)
        {
            this.SaveAsStream(stream, Encoding.UTF8);
        }

        public void SaveAsStream(Stream stream, Encoding encoding, int bufferSize = Constants.DEFAULT_STREAM_BUFFER_SIZE)
        {
            using (StreamWriter writer = new StreamWriter(stream, encoding, bufferSize))
            {
                writer.Write(this.GetXmlString());
            }
        }

        public async Task SaveAsStreamAsync(Stream stream)
        {
            await this.SaveAsStreamAsync(stream, Encoding.UTF8);
        }

        public async Task SaveAsStreamAsync(Stream stream, Encoding encoding, int bufferSize = Constants.DEFAULT_STREAM_BUFFER_SIZE)
        {
            using (StreamWriter writer = new StreamWriter(stream, encoding, bufferSize))
            {
                await writer.WriteAsync(this.GetXmlString());
            }
        }

    }
}