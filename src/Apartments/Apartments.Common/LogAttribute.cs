using MethodBoundaryAspect.Fody.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Apartments.Common
{
    public class LogAttribute : OnMethodBoundaryAspect
    {
        private string _extension = ".txt";

        public override void OnEntry(MethodExecutionArgs args)
        {
            string fileName = $"{DateTime.UtcNow.ToString("ddmmyyyyhhmmssffffff")}_{args.Method.Name}"
                                + Guid.NewGuid().ToString() + _extension;
            var directiryPath = $"Resources\\Logs\\OnEntry";
            var fullDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), directiryPath);
            var fullPath = Path.Combine(fullDirectoryPath, fileName);

            DirectoryInfo dirInfo = new DirectoryInfo(fullDirectoryPath);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            var log = $"{DateTime.Now} - CLASS: {args.Method.DeclaringType.Name} \n" +
                $"METHOD: {args.Method.Name} started.";

            using (StreamWriter sw = new StreamWriter(fullPath, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(log);
            }

            Debug.WriteLine($"{DateTime.Now} - CLASS: {args.Method.DeclaringType.Name} \n" +
                $"METHOD: {args.Method.Name} started.");
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            string fileName = $"{DateTime.UtcNow.ToString("ddmmyyyyhhmmssffffff")}_{args.Method.Name}"
                                + Guid.NewGuid().ToString() + _extension;
            var directiryPath = $"Resources\\Logs\\OnExit";
            var fullDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), directiryPath);
            var fullPath = Path.Combine(fullDirectoryPath, fileName);

            DirectoryInfo dirInfo = new DirectoryInfo(fullDirectoryPath);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            var log = $"{DateTime.Now} - CLASS: {args.Method.DeclaringType.Name} \n" +
                $"METHOD: {args.ReturnValue} started.";

            using (StreamWriter sw = new StreamWriter(fullPath, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(log);
            }

            Debug.WriteLine($"{DateTime.Now} - {args.ReturnValue.ToString()} \n" +
                $"METHOD Finished.");
        }

        public override void OnException(MethodExecutionArgs args)
        {
            string fileName = $"{DateTime.UtcNow.ToString("ddmmyyyyhhmmssffffff")}_{args.Method.Name}"
                                + Guid.NewGuid().ToString() + _extension;
            var directiryPath = $"Resources\\Logs\\OnException";
            var fullDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), directiryPath);
            var fullPath = Path.Combine(fullDirectoryPath, fileName);

            DirectoryInfo dirInfo = new DirectoryInfo(fullDirectoryPath);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            var log = $"{DateTime.Now} - CLASS: {args.Method.DeclaringType.Name} \n" +
                $"METHOD: {args.Method.Name} has exception: " +
                $"\n{args.Exception.Data}\n" +
                $"{args.Exception.Message}\n" +
                $"{args.Exception.InnerException?.Message}";

            using (StreamWriter sw = new StreamWriter(fullPath, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(log);
            }

            Debug.WriteLine($"{DateTime.Now} - CLASS: {args.Method.DeclaringType.Name} \n" +
                $"METHOD: {args.Method.Name} has exception: " +
                $"\n{args.Exception.Data}\n" +
                $"{args.Exception.Message}\n" +
                $"{args.Exception.InnerException?.Message}");
        }
    }
}