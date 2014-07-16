using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Input;
using Microsoft.Test;
using Mouse=Microsoft.Test.Mouse;

namespace AutomationExample
{
    public class Script
    {
        private string PaintDotNetPath = @"C:\Program Files\Paint.NET\PaintDotNet.exe";

        public void Run(string paintDotNetPath)
        {
            var processStartInfo = new ProcessStartInfo(paintDotNetPath);
            var process = Process.Start(processStartInfo);

            process.WaitForInputIdle();
            Delay();

            var mainWindow = AutomationElement.RootElement.FindChildByProcessId(process.Id);

            Delay();

            AutomateMainWindow(mainWindow);
        }

        private void AutomateMainWindow(AutomationElement mainWindow)
        {
            // find the Paint.Net drawing Canvas
            var canvas = mainWindow.FindDescendentByIdPath(new[] { "appWorkspace", "workspacePanel", "DocumentView", "scrollableCanvasControl", "canvasView" });

            DrawSpirographWaveOnCanvas(canvas);

            // the the audience appreciate the masterpiece!
            Delay(5000);

            var closeButton = mainWindow.FindDescendentByIdPath(new[] {"TitleBar", "Close"});
            closeButton.GetInvokePattern().Invoke();

            // give chance for the close dialog to open
            Delay();

            var dontSaveButton = mainWindow.FindDescendentByNamePath(new[] {"Unsaved Changes", "Don't Save"});

            Mouse.MoveTo(dontSaveButton.GetClickablePoint().ToDrawingPoint());
            Mouse.Click(MouseButton.Left);
        }

        private void DrawSpirographWaveOnCanvas(AutomationElement canvasElement)
        {
            var bounds = canvasElement.Current.BoundingRectangle;

            var centerX = (int)(bounds.X + bounds.Width / 2);
            int centerY = (int)(bounds.Y + bounds.Height / 2);

            var points = GetPointsForSpirograph(centerX, centerY, 1.02, 5, 2, 0, 300);

            Mouse.MoveTo(points.First());
            Mouse.Down(MouseButton.Left);

            AnimateMouseThroughPoints(points);

            Mouse.Up(MouseButton.Left);
        }

        private void AnimateMouseThroughPoints(IEnumerable<Point> points)
        {
            foreach (var point in points)
            {
                Mouse.MoveTo(point);
                Delay(5);
            }
        }

        private IEnumerable<Point> GetPointsForSpirograph(int centerX, int centerY, double littleR, double bigR, double a, int tStart, int tEnd)
        {
            // Equations from http://www.mathematische-basteleien.de/spirographs.htm
            for (double t = tStart; t < tEnd; t += 0.1)
            {
                var rDifference = bigR - littleR;
                var rRatio = littleR / bigR;
                var x = (rDifference * Math.Cos(rRatio * t) + a * Math.Cos((1 - rRatio) * t)) * 25;
                var y = (rDifference * Math.Sin(rRatio * t) - a * Math.Sin((1 - rRatio) * t)) * 25;

                yield return new Point(centerX + (int)x, centerY + (int)y);
            }
        }

        private void DrawSineWaveOnCanvas(AutomationElement canvasElement)
        {
            var bounds = canvasElement.Current.BoundingRectangle;

            var left = (int)bounds.Left;
            int center = (int)(bounds.Y + bounds.Height / 2);

            Mouse.MoveTo(new Point(left, center));
            Mouse.Down(MouseButton.Left);

            AnimateMouseThroughPoints(GetPointsForSineWave(left, (int) bounds.Right, center));

            Mouse.Up(MouseButton.Left);
        }

        private IEnumerable<Point> GetPointsForSineWave(int left, int right, int verticalCenter)
        {
            for (int x = left; x < right; x++)
            {
                var y = Math.Sin(x * 0.1) * 100;
                yield return new Point(x, (int)(verticalCenter + y));
            }
        }

        private void Delay(int milliseconds = 1000)
        {
            Thread.Sleep(milliseconds);
        }
    }
}
