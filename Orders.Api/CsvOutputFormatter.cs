using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Orders.Shared.DataTransferObjects;
using System.Text;

namespace Orders.Api
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type? type)
        {
            if (typeof(SampleDto).IsAssignableFrom(type)
                || typeof(IEnumerable<SampleDto>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }

            return false;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context,
            Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();

            if (context.Object is IEnumerable<SampleDto>)
            {
                foreach (var sample in (IEnumerable<SampleDto>)context.Object)
                {
                    FormatCsv(buffer, sample);
                }
            }
            else
            {
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                FormatCsv(buffer, (SampleDto)context.Object);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8604 // Possible null reference argument.
            }

            await response.WriteAsync(buffer.ToString());
        }

        private static void FormatCsv(StringBuilder buffer, SampleDto sample)
        {
            //buffer.AppendLine($"{sample.Id},\"{sample.Name}\",\"{sample.FullAddress}\"");
        }

    }

}
