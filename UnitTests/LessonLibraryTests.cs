using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LessonLibrary;
using LessonLibrary.Permulation;
using LessonLibrary.Visualisation3D;
using LessonLibrary.Visualisation3D.Animations;
using LessonLibrary.Visualisation3D.Geometry;
using OpenTK;

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
            var cayleyTable = LessonReader.ReadCayleyTableTestLesson(@"lessons\CayleyTables\table_test.xml");
            for (var i = 0; i < expectedTable.GetLength(0); i++)
                for (var j = 0; j < expectedTable.GetLength(1); j++)
                    Assert.AreEqual(expectedTable[i, j], cayleyTable.StartTable[i, j]);
        }

        /// <summary>
        /// Проверяет проверку списка на перестановку
        /// </summary>
        [TestMethod]
        public void CheckListOnPermulationTest()
        {
            Assert.IsFalse(Permulation.CheckListOnPermulation(new List<int> {-1, 2, 3, 5}));
            Assert.IsFalse(Permulation.CheckListOnPermulation(new List<int> { 1, 2, 3, 5 }));
            Assert.IsFalse(Permulation.CheckListOnPermulation(new List<int>()));
            Assert.IsFalse(Permulation.CheckListOnPermulation(new List<int> { 1, 1, 2, 4 }));

            Assert.IsTrue(Permulation.CheckListOnPermulation(new List<int> { 1, 2, 3, 4 }));
            Assert.IsTrue(Permulation.CheckListOnPermulation(new List<int> { 1, 3, 2, 4 }));
            Assert.IsTrue(Permulation.CheckListOnPermulation(new List<int> { 1, 2, 3 }));
            Assert.IsTrue(Permulation.CheckListOnPermulation(new List<int> { 1 }));
        }

        /// <summary>
        /// Проверяет конструктор и индексаторы Permulation
        /// </summary>
        [TestMethod]
        public void PermulationConstructorAndIndexatorTest()
        {
            var id = new Permulation(4);
            var idFromList = new Permulation(new List<int>{1, 2, 3, 4});

            for (var i = 1; i <= id.Size; i++)
                Assert.AreEqual(id[i], idFromList[i]);

            Assert.AreEqual(id, idFromList);

            var perm1 = new Permulation(new List<int>{1, 3, 2, 4});
            
            Assert.AreNotEqual(id, perm1);
        }

        /// <summary>
        /// Проверяет умножение подстановок
        /// </summary>
        [TestMethod]
        public void PermulationCompositionTest()
        {
            var id = new Permulation(3);
            var perm = new Permulation(new List<int>{3, 2, 1});
            Assert.AreEqual(perm, id * perm);

            var perm2 = new Permulation(new List<int> {1, 3, 2});
            var res = new Permulation(new List<int> {3, 1, 2});
            Assert.AreEqual(res, perm * perm2);
        }

        /// <summary>
        /// Проверяет конструкторы и ToString PermulationCycles
        /// </summary>
        [TestMethod]
        public void PermulationCyclesToStringAndConstructorsTest()
        {
            var idCyclesString = "(1) (2) (3)";
            var cycles = new PermulationCycles(new Permulation(3));
            Assert.AreEqual(idCyclesString, cycles.ToString());

            var permCyclesString = "(1 3) (2)";
            var perm = new PermulationCycles(new Permulation(new List<int>{3, 2, 1}));
            Assert.AreEqual(permCyclesString, perm.ToString());

            permCyclesString = "(1 2) (3)";
            perm = new PermulationCycles(new List<int> {2, 1, 3});
            Assert.AreEqual(permCyclesString, perm.ToString());

            perm = new PermulationCycles(
                new List<List<int>>
                {
                    new List<int> {1, 2},
                    new List<int> {3}
                });
            Assert.AreEqual(permCyclesString, perm.ToString());
        }

        /// <summary>
        /// Проверяет преобразоавние между циклами и подстановками
        /// </summary>
        [TestMethod]
        public void PermulationCyclesToPermulationAndBackConvertionTest()
        {
            var cycles = new PermulationCycles(new List<List<int>> {new List<int> {1, 2}, new List<int> {3}});
            var perm = new Permulation(new List<int> {2, 1, 3});
            Assert.AreEqual(perm, cycles.Permulation);

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
                    rotations.Add(new RotationAnimation(MathHelper.Pi / 2 * i, axis, MathHelper.Pi / 2));
            }

            for (var first = 0; first < 4; first++)
            {
                var second = (first + 2) % 4 + 4;
                for (var angle = 1; angle <= 2; angle++)
                    rotations.Add(new RotationAnimation(MathHelper.Pi * 2 / 3 * angle, vertices[second] - vertices[first],
                        MathHelper.Pi / 2));
            }

            vectors = new[]
            {
                new Vector3(0, 1, 1), new Vector3(0, 1, -1), new Vector3(1, 0, 1), new Vector3(1, 0, -1),
                new Vector3(1, 1, 0), new Vector3(-1, 1, 0)  
            };

            rotations.AddRange(vectors.Select(axis => new RotationAnimation(MathHelper.Pi, axis, MathHelper.Pi / 2)));
            var xmlSer = new DataContractSerializer(typeof(RotationAnimation));

            for (var i = 0; i < rotations.Count; i++)
            {
                var name = $@"Cube\r{i.ToString($"D{2}")}.xml";
                using (var writer = XmlWriter.Create(name, new XmlWriterSettings { Indent = true }))
                {
                    xmlSer.WriteObject(writer, rotations[i]);
                }
            }
        }
    }
}
