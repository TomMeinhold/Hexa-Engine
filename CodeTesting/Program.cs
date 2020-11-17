namespace CodeTesting
{
    using System;
    using System.IO;
    using System.Text;

    internal class Program
    {
        private static void Main(string[] args)
        {
            byte[] data = Encoding.UTF8.GetBytes("ABCDEFGHIJ");
            Dump(data);
            var stream = new MemoryStream();
        }

        private static void Dump(byte[] data)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{ ");
            foreach (byte b in data)
            {
                builder.Append($" {b}, ");
            }
            builder.Append(" }");
            Console.WriteLine(builder);
        }
    }
}