using System.Text;

namespace HtmlMaker
{
    partial class Program
    {
        static string GetJavascript()
        {
            var sb = new StringBuilder();
			sb.AppendLine("<script>");
			sb.AppendLine("	window.onload = function(){");
			sb.AppendLine("		var windowScreenHeight = window.screen.height;");
			sb.AppendLine("		var avg = windowScreenHeight + 100; // (windowScreenWidth + windowScreenHeight) / 2;");
			sb.AppendLine("		console.log('windowScreenHeight: ' + windowScreenHeight);");
			sb.AppendLine("		console.log('               avg: ' + avg);");
			sb.AppendLine("");
			sb.AppendLine("		function setMax(image) {");
			sb.AppendLine("			// portrait");
			sb.AppendLine("			if (image.naturalHeight > image.naturalWidth) {");
			sb.AppendLine("				image.style.width = avg;");
			sb.AppendLine("				image.style.maxWidth = '100%';");
			sb.AppendLine("			}");
			sb.AppendLine("");
			sb.AppendLine("			else { // landscape");
			sb.AppendLine("				image.style.maxWidth = '100vw';");
			sb.AppendLine("				image.style.maxHeight = '100vh';");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine("");
			sb.AppendLine("		const images = document.querySelectorAll('img');");
			sb.AppendLine("");
			sb.AppendLine("		images.forEach(image => {");
			sb.AppendLine("			setMax(image);");
			sb.AppendLine("		});");
			sb.AppendLine("	};");
			sb.AppendLine("</script>");

			return sb.ToString();
        }
    }
}