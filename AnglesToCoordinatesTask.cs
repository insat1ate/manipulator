using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        /// <summary>
        /// По значению углов суставов возвращает массив координат суставов
        /// в порядке new []{elbow, wrist, palmEnd}
        /// </summary>
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var elbowX = Manipulator.UpperArm * Math.Cos(shoulder);
            var elbowY = Manipulator.UpperArm * Math.Sin(shoulder);
            var elbowPos = new PointF((float)elbowX, (float)elbowY);

            var angle2 = elbow + shoulder - Math.PI;
            var wristX = Manipulator.Forearm * Math.Cos(angle2) + elbowX;
            var wristY = Manipulator.Forearm * Math.Sin(angle2) + elbowY;
            var wristPos = new PointF((float)wristX, (float)wristY);

            var angle3 = wrist + angle2 - Math.PI;
            var palmX = Manipulator.Palm * Math.Cos(angle3) + wristX;
            var palmY = Manipulator.Palm * Math.Sin(angle3) + wristY;
            var palmEndPos = new PointF((float)palmX, (float)palmY);
            return new PointF[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        // Доработайте эти тесты!
        // С помощью строчки TestCase можно добавлять новые тестовые данные.
        // Аргументы TestCase превратятся в аргументы метода.
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
            Assert.AreEqual(GetDistance(joints[0], new PointF(0, 0)), Manipulator.UpperArm);
            Assert.AreEqual(GetDistance(joints[0], joints[1]), Manipulator.Forearm);
            Assert.AreEqual(GetDistance(joints[1], joints[2]), Manipulator.Palm);
        }
        public double GetDistance(PointF point1, PointF point2)
        {
            var differenceX = (point1.X - point2.X) * (point1.X - point2.X);
            var differenceY = (point1.Y - point2.Y) * (point1.Y - point2.Y);
            return Math.Sqrt(differenceX + differenceY);
        }
    }

}