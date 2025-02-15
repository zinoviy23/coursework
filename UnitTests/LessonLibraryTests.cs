using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LessonLibrary;
using LessonLibrary.Permutations;
using LessonLibrary.Visualisation3D;
using LessonLibrary.Visualisation3D.Animations;
using LessonLibrary.Visualisation3D.Geometry;
using OpenTK;
using WinFormCourseWork;

namespace UnitTests
{
    [TestClass]
    public class LessonLibraryTests
    {
        /// <summary>
        /// Проверяет на правильные ответы в checkbox
        /// </summary>
        [TestMethod]
        public void CompareCheckBoxAnswersTestTrue()
        {
            Assert.IsTrue(TestAnswers.CompareCheckBoxAnswers("132", "123"),
                "TestAnswers.CompareCheckBoxAnswers('132', '123') не работает");
            Assert.IsTrue(TestAnswers.CompareCheckBoxAnswers("12345", "31425"),
                "TestAnswers.CompareCheckBoxAnswers('12345', '31425') не работает");
        }

        /// <summary>
        /// Проверяет на неправильные ответы в checkbox
        /// </summary>
        [TestMethod]
        public void CompareCheckBoxAnswersTestFalse()
        {
            Assert.IsFalse(TestAnswers.CompareCheckBoxAnswers("12345", "324"),
                "TestAnswers.CompareCheckBoxAnswers('12345', '324') не работает");
            Assert.IsFalse(TestAnswers.CompareCheckBoxAnswers("123", "3214"), 
                "TestAnswers.CompareCheckBoxAnswers('123', '3214') не работает");
        }

        /// <summary>
        /// Проверяет "проверку" на группу(правилтные таблицы)
        /// </summary>
        [TestMethod]
        public void CheckTableOnGroupTestSuccess()
        {
            var table = new[,]
            {
                {" ", "0", "1", "2" },
                {"0", "0", "1", "2" },
                {"1", "1", "2", "0" },
                {"2", "2", "0", "1" }
            };
            Assert.AreEqual(CayleyTableTestLesson.CheckResult.Success,
                CayleyTableTestLesson.CheckTableOnGroup(table));

         
            var table2 = new[,]
            {
                {" ", "0", "1"},
                {"1", "1", "0" },
                {"0", "0", "1" }
            };
            Assert.AreEqual(CayleyTableTestLesson.CheckResult.Success,
                CayleyTableTestLesson.CheckTableOnGroup(table2));
        }

        /// <summary>
        /// Проверяет "проверку" на группу(не выполняется ассоциативность)
        /// </summary>
        [TestMethod]
        public void CheckTableOnGroupTestNotAssociativity()
        {
            var table1 = new[,]
            {
                {" ", "0", "1", "2" },
                {"0", "0", "2", "1" },
                {"1", "1", "2", "0" },
                {"2", "2", "0", "1" }
            };
            Assert.AreEqual(CayleyTableTestLesson.CheckResult.NotAssociativity,
                CayleyTableTestLesson.CheckTableOnGroup(table1));
        }
        
        /// <summary>
        /// Проверяет "проверку" на группу(нет нейтрального элемента)
        /// </summary>
        [TestMethod]
        public void CheckTableOnGroupTestDontContainsNeutral()
        {
            var table3 = new[,]
            {
                {" ", "0", "1"},
                {"1", "1", "1" },
                {"0", "1", "1" }
            };
            Assert.AreEqual(CayleyTableTestLesson.CheckResult.DontContainsNeutral,
                CayleyTableTestLesson.CheckTableOnGroup(table3));
        }

        /// <summary>
        /// Проверяет "проверку" на группу(не для всех есть обратные)
        /// </summary>
        [TestMethod]
        public void CheckTableOnGroupTestDontContainsInverts()
        {
            var table4 = new[,]
            {
                {" ", "0", "1"},
                {"0", "0", "1" },
                {"1", "1", "1" }
            };
            Assert.AreEqual(CayleyTableTestLesson.CheckResult.DontContainsInverts,
                CayleyTableTestLesson.CheckTableOnGroup(table4));
        }

        /// <summary>
        /// Проверяет считывание таблицы
        /// </summary>
        [TestMethod]
        public void ReadCayleyTableTestLessonTest()
        {
            var expectedTable = new[,]
            {
                {"\\", "0", "1"},
                {"0", "0", "1"},
                {"1", "1", "0"}
            };
            var cayleyTable = LessonReader.ReadCayleyTableTestLesson(@"..\..\lessons\CayleyTables\table_test.xml");
            for (var i = 0; i < expectedTable.GetLength(0); i++)
                for (var j = 0; j < expectedTable.GetLength(1); j++)
                    Assert.AreEqual(expectedTable[i, j], cayleyTable.StartTable[i, j]);
        }

        /// <summary>
        /// Проверяет проверку списка на перестановку
        /// </summary>
        [TestMethod]
        public void CheckListOnPermutationTest()
        {
            Assert.IsFalse(Permutation.CheckListOnPermutation(new List<int> {-1, 2, 3, 5}));
            Assert.IsFalse(Permutation.CheckListOnPermutation(new List<int> { 1, 2, 3, 5 }));
            Assert.IsFalse(Permutation.CheckListOnPermutation(new List<int>()));
            Assert.IsFalse(Permutation.CheckListOnPermutation(new List<int> { 1, 1, 2, 4 }));

            Assert.IsTrue(Permutation.CheckListOnPermutation(new List<int> { 1, 2, 3, 4 }));
            Assert.IsTrue(Permutation.CheckListOnPermutation(new List<int> { 1, 3, 2, 4 }));
            Assert.IsTrue(Permutation.CheckListOnPermutation(new List<int> { 1, 2, 3 }));
            Assert.IsTrue(Permutation.CheckListOnPermutation(new List<int> { 1 }));
        }

        /// <summary>
        /// Проверяет конструктор и индексаторы Permutation
        /// </summary>
        [TestMethod]
        public void PermutationConstructorAndIndexatorTest()
        {
            var id = new Permutation(4);
            var idFromList = new Permutation(new List<int>{1, 2, 3, 4});

            for (var i = 1; i <= id.Size; i++)
                Assert.AreEqual(id[i], idFromList[i]);

            Assert.AreEqual(id, idFromList);

            var perm1 = new Permutation(new List<int>{1, 3, 2, 4});
            
            Assert.AreNotEqual(id, perm1);
        }

        /// <summary>
        /// Проверяет умножение подстановок
        /// </summary>
        [TestMethod]
        public void PermutationCompositionTest()
        {
            var id = new Permutation(3);
            var perm = new Permutation(new List<int>{3, 2, 1});
            Assert.AreEqual(perm, id * perm);

            var perm2 = new Permutation(new List<int> {1, 3, 2});
            var res = new Permutation(new List<int> {3, 1, 2});
            Assert.AreEqual(res, perm * perm2);
        }

        /// <summary>
        /// Проверяет конструкторы и ToString PermutationCycles
        /// </summary>
        [TestMethod]
        public void PermutationCyclesToStringAndConstructorsTest()
        {
            var idCyclesString = "(1) (2) (3)";
            var cycles = new PermutationCycles(new Permutation(3));
            Assert.AreEqual(idCyclesString, cycles.ToString());

            var permCyclesString = "(1 3) (2)";
            var perm = new PermutationCycles(new Permutation(new List<int>{3, 2, 1}));
            Assert.AreEqual(permCyclesString, perm.ToString());

            permCyclesString = "(1 2) (3)";
            perm = new PermutationCycles(new List<int> {2, 1, 3});
            Assert.AreEqual(permCyclesString, perm.ToString());

            perm = new PermutationCycles(
                new List<List<int>>
                {
                    new List<int> {1, 2},
                    new List<int> {3}
                });
            Assert.AreEqual(permCyclesString, perm.ToString());
        }

        /// <summary>
        /// Проверяет получение обратной подтсановки
        /// </summary>
        [TestMethod]
        public void PermuationNegationTest()
        {
            var permutation = new Permutation(new List<int>(new [] {3, 1, 5, 4, 2}));

            Assert.AreEqual(new Permutation(5), permutation * (-permutation));
            Assert.AreEqual(new Permutation(5), (-permutation) * permutation);
        }


        /// <summary>
        /// Проверяет преобразоавние между циклами и подстановками
        /// </summary>
        [TestMethod]
        public void PermutationCyclesToPermutationAndBackConvertionTest()
        {
            var cycles = new PermutationCycles(new List<List<int>> {new List<int> {1, 2}, new List<int> {3}});
            var perm = new Permutation(new List<int> {2, 1, 3});
            Assert.AreEqual(perm, cycles.Permutation);

            Assert.AreEqual(cycles, perm.Cycles);
        }

        /// <summary>
        /// Проверяет сериализацию анимаций вращений
        /// </summary>
        [TestMethod]
        public void RotationAnimationSerializationAndDeserializationTest()
        {
            var rotation = new RotationAnimation(MathHelper.Pi / 2, Vector3.One, 10.01f);
            var xmlSer = new DataContractSerializer(typeof(RotationAnimation));
            var writer = XmlWriter.Create("tmpRotation.xml", new XmlWriterSettings {Indent = true}); 
            xmlSer.WriteObject(writer, rotation);
            writer.Close();

            var reader = new FileStream("tmpRotation.xml", FileMode.Open);
            var newRot = (RotationAnimation) xmlSer.ReadObject(reader);
            reader.Close();

            Assert.AreEqual(rotation, newRot);
        }

        /// <summary>
        /// Проверяет сереализацию симметрий
        /// </summary>
        [TestMethod]
        public void SymmetryAnimationSerializationAndDeserializationTest()
        {
            var sym = new SymmetryAnimation(new Plane(Vector3.One, Vector3.UnitZ), 10f);
            var xmlSer = new DataContractSerializer(typeof(SymmetryAnimation));

            using (var writer = XmlWriter.Create("tmpSymmetry.xml", new XmlWriterSettings {Indent = true}))
            {
                xmlSer.WriteObject(writer, sym);
            }

            using (var reader = new FileStream("tmpSymmetry.xml", FileMode.Open))
            {
                var otherSym = (SymmetryAnimation) xmlSer.ReadObject(reader);
                Assert.AreEqual(sym, otherSym);
            }
        }

        /// <summary>
        /// Пытается вывести все анимации куба
        /// </summary>
        [TestMethod]
        public void CubeAnimationWritingTest()
        {
            var vertices = new CubeVisualisation().VerticesClone;
            List<RotationAnimation> rotations = new List<RotationAnimation>();
            var id = new RotationAnimation(0, Vector3.One, 10);
            rotations.Add(id);

            var vectors = new[] {Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ};

            foreach (var axis in vectors)
            {
                for (var i = 1; i <= 3; i++)
                    rotations.Add(new RotationAnimation(MathHelper.Pi / 2 * i, axis, MathHelper.Pi / 3));
            }

            for (var first = 0; first < 4; first++)
            {
                var second = (first + 2) % 4 + 4;
                for (var angle = 1; angle <= 2; angle++)
                    rotations.Add(new RotationAnimation(MathHelper.Pi * 2 / 3 * angle, vertices[second] - vertices[first],
                        MathHelper.Pi / 3));
            }

            vectors = new[]
            {
                new Vector3(0, 1, 1), new Vector3(0, 1, -1), new Vector3(1, 0, 1), new Vector3(1, 0, -1),
                new Vector3(1, 1, 0), new Vector3(-1, 1, 0)  
            };

            rotations.AddRange(vectors.Select(axis => new RotationAnimation(MathHelper.Pi, axis, MathHelper.Pi / 3)));
            var xmlSer = new DataContractSerializer(typeof(RotationAnimation));

            EnsureClearDirectory("Cube");
            for (var i = 0; i < rotations.Count; i++)
            {
                var name = $@"Cube\r{i.ToString($"D{2}")}.xml";
                using (var writer = XmlWriter.Create(name, new XmlWriterSettings { Indent = true }))
                {
                    xmlSer.WriteObject(writer, rotations[i]);
                }
            }
        }

        private static void EnsureClearDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            else
            {
                Directory.Delete(dir, true);
                Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// Пытается вывести все анимации октаэдра
        /// </summary>
        [TestMethod]
        public void OctahedronAnimationWritingTest()
        {
            var rotations = new List<RotationAnimation> { new RotationAnimation(0, Vector3.UnitY, MathHelper.Pi / 3) };

            var vertices = new OctahedronVisualisation().VerticesClone;

            var verticesTuples = new[]
                {new Tuple<int, int>(0, 2), new Tuple<int, int>(1, 3), new Tuple<int, int>(4, 5)};

            foreach (var tuple in verticesTuples)
            {
                for (var angleMult = 1; angleMult <= 3; angleMult++)
                {
                    rotations.Add(new RotationAnimation(MathHelper.Pi / 2 * angleMult,
                        vertices[tuple.Item1] - vertices[tuple.Item2], MathHelper.Pi / 3));
                }
            }

            var verticesTuple3 = new[]
            {
                new Tuple<int, int, int>(0, 1, 4), new Tuple<int, int, int>(1, 2, 4),
                new Tuple<int, int, int>(2, 3, 4), new Tuple<int, int, int>(3, 0, 4)
            };

            rotations.AddRange(verticesTuple3.SelectMany(tuple => new []
            {
                new RotationAnimation(MathHelper.Pi / 3 * 2,
                    (vertices[tuple.Item1] + vertices[tuple.Item2] + vertices[tuple.Item3]) / 3, MathHelper.Pi / 3),
                new RotationAnimation(MathHelper.Pi / 3 * 4,
                    (vertices[tuple.Item1] + vertices[tuple.Item2] + vertices[tuple.Item3]) / 3, MathHelper.Pi / 3)
            }));

            verticesTuples = new[]
            {
                new Tuple<int, int>(0, 4), new Tuple<int, int>(1, 4), new Tuple<int, int>(2, 4),
                new Tuple<int, int>(3, 4), new Tuple<int, int>(0, 1), new Tuple<int, int>(1, 2)
            };

            rotations.AddRange(verticesTuples.Select(tuple => new RotationAnimation(MathHelper.Pi, 
                (vertices[tuple.Item1] + vertices[tuple.Item2]) / 2, MathHelper.Pi / 3)));

            var xmlSer = new DataContractSerializer(typeof(RotationAnimation));

            EnsureClearDirectory("Octahedron");
            for (var i = 0; i < rotations.Count; i++)
            {
                var name = $@"Octahedron\r{i.ToString($"D{2}")}.xml";
                using (var writer = XmlWriter.Create(name, new XmlWriterSettings { Indent = true }))
                {
                    xmlSer.WriteObject(writer, rotations[i]);
                }
            }
        }

        /// <summary>
        /// Пытается вывести все анимации икосаэдра
        /// </summary>
        [TestMethod]
        public void IcosahedronAnimationsWritingTest()
        {
            var rotations = new List<RotationAnimation> { new RotationAnimation(0, Vector3.UnitY, MathHelper.Pi / 3) };

            var vertices = new IcosahedronVisualisation().VerticesClone;

            var verticesTuples = new[] {
                new Tuple<int, int>(5, 11), new Tuple<int, int>(0, 8),
                new Tuple<int, int>(1, 9), new Tuple<int, int>(2, 10), 
                new Tuple<int, int>(3, 6), new Tuple<int, int>(4, 7)  
            };

            foreach (var tuple in verticesTuples)
            {
                const float angle = MathHelper.Pi / 5;
                for (var i = 1; i <= 4; i++ )
                    rotations.Add(new RotationAnimation(angle * 2 * i, vertices[tuple.Item1] - vertices[tuple.Item2],
                        MathHelper.Pi / 3));
            }

            var verticesTuple3 = new[]
            {
                new Tuple<int, int, int>(5, 0, 1), new Tuple<int, int, int>(5, 1, 2), new Tuple<int, int, int>(5, 2, 3),
                new Tuple<int, int, int>(5, 3, 4), new Tuple<int, int, int>(5, 4, 0),
                new Tuple<int, int, int>(0, 1, 6), new Tuple<int, int, int>(1, 2, 7), new Tuple<int, int, int>(2, 3, 8),
                new Tuple<int, int, int>(3, 4, 9), new Tuple<int, int, int>(4, 0, 10)  
            };

            rotations.AddRange(verticesTuple3.SelectMany(tuple => new []
            {
                new RotationAnimation(2 * MathHelper.Pi / 3,
                    (vertices[tuple.Item1] + vertices[tuple.Item2] + vertices[tuple.Item3]) / 3, MathHelper.Pi / 3),
                new RotationAnimation(4 * MathHelper.Pi / 3,
                    (vertices[tuple.Item1] + vertices[tuple.Item2] + vertices[tuple.Item3]) / 3, MathHelper.Pi / 3)
            }));

            verticesTuples = new[]
            {
                new Tuple<int, int>(0, 1), new Tuple<int, int>(1, 2), new Tuple<int, int>(2, 3),
                new Tuple<int, int>(3, 4), new Tuple<int, int>(4, 0),
                new Tuple<int, int>(5, 0), new Tuple<int, int>(5, 1), new Tuple<int, int>(5, 2),
                new Tuple<int, int>(5, 3), new Tuple<int, int>(5, 4),
                new Tuple<int, int>(0, 10), new Tuple<int, int>(1, 6), new Tuple<int, int>(2, 7),
                new Tuple<int, int>(3, 8), new Tuple<int, int>(4, 9),  
            };

            rotations.AddRange(verticesTuples.Select(tuple => 
                new RotationAnimation(MathHelper.Pi, (vertices[tuple.Item1] + vertices[tuple.Item2]) / 2,
                    MathHelper.Pi / 3)));

            var xmlSer = new DataContractSerializer(typeof(RotationAnimation));

            EnsureClearDirectory("Icosahedron");
            for (var i = 0; i < rotations.Count; i++)
            {
                var name = $@"Icosahedron\r{i.ToString($"D{2}")}.xml";
                using (var writer = XmlWriter.Create(name, new XmlWriterSettings { Indent = true }))
                {
                    xmlSer.WriteObject(writer, rotations[i]);
                }
            }
        }

        /// <summary>
        /// проверяет построение вершин икосаэдра
        /// </summary>
        [TestMethod]
        public void IcosahedronConstructorTest()
        {
            var vertices = new IcosahedronVisualisation().VerticesClone;
            const float delta = 0.0001f;

            Assert.AreEqual(1.0f, (vertices[0] - vertices[1]).Length, delta);
            Assert.AreEqual(1.0f, (vertices[5] - vertices[1]).Length, delta);
            Assert.AreEqual(1.0f, (vertices[3] - vertices[9]).Length, delta);
        }

        /// <summary>
        /// Проверяет построение вершин додекаэдра
        /// </summary>
        [TestMethod]
        public void DodecahedronConstructorTest()
        {
            const float delta = 0.0001f;

            var vertices = new DodecahedronVisualisation().VerticesClone;

            Assert.AreEqual(1.0f, (vertices[0] - vertices[1]).Length, delta);
            Assert.AreEqual(1.0f, (vertices[0] - vertices[4]).Length, delta);
            Assert.AreEqual(1.0f, (vertices[0] - vertices[5]).Length, delta);
            Assert.AreEqual(1.0f, (vertices[5] - vertices[10]).Length, delta);
        }

        /// <summary>
        /// Проверяет преобразование угла в дробь с пи
        /// </summary>
        [TestMethod]
        public void AngleToFracWithPiTest()
        {
            const float angle = MathHelper.Pi * 2 / 3;
            var frac = Visualisation3DController.AngleToFracWithPi(angle);

            Assert.IsNotNull(frac);
            Assert.AreEqual(2, frac.Item1);
            Assert.AreEqual(3, frac.Item2);

            const float angle2 = MathHelper.Pi * 8 / 5;
            frac = Visualisation3DController.AngleToFracWithPi(angle2);

            Assert.IsNotNull(frac);
            Assert.AreEqual(8, frac.Item1);
            Assert.AreEqual(5, frac.Item2);
        }

        /// <summary>
        /// Тестирует класс грани
        /// </summary>
        [TestMethod]
        public void FaceTest()
        {
            var face = new Face(new Vector3(1, 1, 1), new Vector3(1, -1, 1), new Vector3(-1, -1, 1),
                new Vector3(-1, 1, 1));

            Assert.IsTrue(VectorUtils.AreVectorsEqual(new Vector3(0, 0, 1), face.Center));
        }

        /// <summary>
        /// Проверяет метод IsVectorPairContainsInPairSequence
        /// </summary>
        [TestMethod]
        public void IsVectorPairContainsInPairSequenceTest()
        {
            var tuples = new[]
            {
                new Tuple<Vector3, Vector3>(Vector3.UnitX, Vector3.UnitY),
                new Tuple<Vector3, Vector3>(Vector3.One, Vector3.Zero)
            };

            var tuple = new Tuple<Vector3, Vector3>(new Vector3(1, 1.0000000001f, 1), new Vector3(0, 0, 0));

            Assert.IsTrue(VectorUtils.IsVectorPairContainsInPairSequence(tuples, tuple));

            tuple = new Tuple<Vector3, Vector3>(Vector3.One, Vector3.One);

            Assert.IsFalse(VectorUtils.IsVectorPairContainsInPairSequence(tuples, tuple));
        }

        /// <summary>
        /// проверяет метод GetStarterIndexByVertex
        /// </summary>
        [TestMethod]
        public void GetStarterIndexByVertexTest()
        {
            var vis = new OctahedronVisualisation();

            Assert.IsNull(vis.GetStarterIndexByVertex(Vector3.Zero));

            var ind = vis.GetStarterIndexByVertex(new Vector3(0.0f, (float)Math.Sqrt(2) / 2.00000001f, 0.0000001f));

            Assert.IsNotNull(ind);
            Assert.AreEqual(4, ind.Value);
        }

        /// <summary>
        /// Проверяет метод IsVertexOnLineByPointAndDirection
        /// </summary>
        [TestMethod]
        public void IsVertexOnLineByPointAndDirectionTest()
        {
            Assert.IsTrue(VectorUtils.IsVertexOnLineByPointAndDirection(Vector3.Zero, Vector3.UnitX, Vector3.UnitX));
            Assert.IsFalse(
                VectorUtils.IsVertexOnLineByPointAndDirection(Vector3.Zero, Vector3.One, new Vector3(2, 1, 1)));

            Assert.IsFalse(VectorUtils.IsVertexOnLineByPointAndDirection(Vector3.UnitZ, Vector3.Zero, Vector3.UnitX));
            Assert.IsFalse(VectorUtils.IsVertexOnLineByPointAndDirection(new Vector3(0, -0.2653614f, 0.7505553f), Vector3.Zero, new Vector3(0, 1, 0)));
        }
    }
}
