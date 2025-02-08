using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ollama.Core.Services
{
    public class OllamaService
    {
        public string GetOllamaIpAddress()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c netstat -ano | findstr :11434";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            Match match = Regex.Match(output, @"(\d+\.\d+\.\d+\.\d+):11434");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return "127.0.0.1";
        }

        public List<string> GetOllamaModelList()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c ollama list";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            List<string> models = new List<string>();

            foreach (string line in output.Split('\n'))
            {
                string modelName = line.Split(' ')[0].Trim();
                if (!string.IsNullOrEmpty(modelName))
                {
                    models.Add(modelName);
                }
            }

            return models;
        }
    }
}
