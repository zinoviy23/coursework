using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LessonLibrary.Visualisation3D.Animations;
using LessonLibrary.Visualisation3D.Geometry;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using MaterialFace = OpenTK.Graphics.OpenGL.MaterialFace;
using MaterialParameter = OpenTK.Graphics.OpenGL.MaterialParameter;

namespace LessonLibrary.Visualisation3D
{
    /// <summary>
    /// Класс для визуализаций
    /// </summary>
    public abstract class VisualisationLesson
    {
        /// <summary>
        /// Вершины, которые отрисовываются сейчас
        /// </summary>
        protected Vector3[] Vertices;

        /// <summary>
        /// Вершины без всяких преобразований
        /// </summary>
        protected Vector3[] InitVertices;

        /// <summary>
        /// Предыдущие вершины
        /// </summary>
        protected Vector3[] PrevVertices;

        /// <summary>
        /// Нормали, которые используются сейчас
        /// </summary>
        protected Vector3[] Normals;

        /// <summary>
        /// Нормали без всяких преобразований
        /// </summary>
        protected Vector3[] InitNormals;

        /// <summary>
        /// Предыдущиие нормали
        /// </summary>
        protected Vector3[] PrevNormals;

        /// <summary>
        /// Анимации
        /// </summary>
        protected List<IAnimation> Animations;

        /// <summary>
        /// Отрисовывает фигуру
        /// </summary>
        public abstract void Render();

        /// <summary>
        /// Грани объекта
        /// </summary>
        protected Face[] Faces;

        /// <summary>
        /// Рёбра визуализации
        /// </summary>
        protected Tuple<Vector3, Vector3>[] Edges { get; set; }

        /// <summary>
        /// Задаёт грани
        /// </summary>
        [UsedImplicitly]
        protected abstract void InitFaces();

        /// <summary>
        /// Задаёт анимации
        /// </summary>
        /// <param name="animations">Анимации</param>
        public void SetAnimations(IAnimation[] animations)
        {
            Animations = new List<IAnimation>((IAnimation[])animations.Clone());
        }

        /// <summary>
        /// Возвращает анимации, в виде неизменяемого лист
        /// </summary>
        [CanBeNull]
        public IReadOnlyList<IAnimation> ReadOnlyAnimations => Animations?.AsReadOnly();

        /// <summary>
        /// Положение в пространствее данной визуализации
        /// </summary>
        public VisualisationTransform Transform { get; set; }

        // Анимация, которая сейчас проигрывается
        public IAnimation CurrentAnimation { get; set; }

        /// <summary>
        /// Конструтор, общий для всех визуализаций
        /// </summary>
        [UsedImplicitly]
        protected VisualisationLesson() => Transform = new VisualisationTransform();

        /// <summary>
        /// Задаёт координатную сетку
        /// </summary>
        public void InitGrid()
        {
            GL.Begin(PrimitiveType.Lines);

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Blue);
            GL.Normal3(0, 1, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(3, 0, 0);

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Red);
            GL.Normal3(0, 1, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 3, 0);

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Green);
            GL.Normal3(0, 1, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, -3);

            GL.End();
        }

        /// <summary>
        /// Радиус вершины
        /// </summary>
        private const float R = 0.07f;

        /// <summary>
        /// Кол-во делений по X
        /// </summary>
        private const int Nx = 7;

        /// <summary>
        /// Кол-во делений по Y
        /// </summary>
        private const int Ny = 7;

        /// <summary>
        /// Рисует вершину
        /// </summary>
        /// <param name="position"></param>
        public static void DrawVertex(Vector3 position)
        {
            GL.Translate(position);

            const double dnx = 1.0 / Nx;
            const double dny = 1.0 / Ny;
            GL.Begin(PrimitiveType.QuadStrip);

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.BlueViolet);

            const double piy = Math.PI * dny;
            const double pix = Math.PI * dnx;
            for (var iy = 0; iy < Ny; iy++)
            {
                double diy = iy;
                var ay = diy * piy;
                var sy = Math.Sin(ay);
                var cy = Math.Cos(ay);
                var ay1 = ay + piy;
                var sy1 = Math.Sin(ay1);
                var cy1 = Math.Cos(ay1);
                //int ix;
                for (var ix = 0; ix <= Nx; ix++)
                {
                    var ax = 2.0 * ix * pix;
                    var sx = Math.Sin(ax);
                    var cx = Math.Cos(ax);
                    var x = R * sy * cx;
                    var y = R * sy * sx;
                    var z = -R * cy;
                    GL.Normal3(x, y, z);
                    GL.Vertex3(x, y, z);
                    x = R * sy1 * cx;
                    y = R * sy1 * sx;
                    z = -R * cy1;
                    GL.Normal3(x, y, z);
                    GL.Vertex3(x, y, z);
                }
            }
            GL.End();
            GL.Translate(-position);
        }

        /// <summary>
        /// Рисует все вершины
        /// </summary>
        protected void DrawVertices()
        {
            foreach (var vertex in Vertices)
            {
                DrawVertex(vertex);
            }
        }

        /// <summary>
        /// Копия вершин (чтобы не портить)
        /// </summary>
        [NotNull]
        public Vector3[] VerticesClone => (Vector3[])Vertices.Clone();

        /// <summary>
        /// Преобразует координаты вершины в оконные
        /// </summary>
        /// <param name="vertex">Координаты в пространстве</param>
        /// <returns>Координаты в окне</returns>
        public Vector3 VertexScreenPosition(Vector3 vertex)
        {
            var res = Vector3.Transform(vertex, Transform.Matrix);
            res = Vector3.Transform(res, WorldInfo.ViewMatrix);
            return Vector3.TransformPosition(res, WorldInfo.ProjectionMatrix);
        }

        /// <summary>
        /// Позиции вершин на экране
        /// </summary>
        [NotNull]
        public Vector3[] ScreenPoints
        {
            get
            {
                var result = new Vector3[Vertices.Length];
                for (var i = 0; i < result.Length; i++)
                {
                    result[i] = VertexScreenPosition(Vertices[i]);
                }

                return result;
            }
        }

        /// <summary>
        /// Возращает массив номеров исходных вершин,в которые перешли вершины после преобразование
        /// </summary>
        [CanBeNull]
        public int[] ImageVerticesIndexies
        {
            get
            {
                var result = new int[Vertices.Length];

                for (var i = 0; i < Vertices.Length; i++)
                {
                    var ind = -1;
                    for (var j = 0; j < InitVertices.Length; j++)
                    {
                        if (VectorUtils.AreVectorsEqual(Vertices[i], InitVertices[j]))
                            ind = j;
                    }

                    if (ind == -1)
                        return null;

                    result[i] = ind;
                }

                return result;
            }
        }

        /// <summary>
        /// Сбрасывает все изменения
        /// </summary>
        public void Reset()
        {
            CurrentAnimation = null;
            PrevVertices = (Vector3[]) InitVertices.Clone();
            PrevNormals = (Vector3[]) InitNormals.Clone();
            Vertices = (Vector3[]) PrevVertices.Clone();
            Normals = PrevNormals.Clone() as Vector3[];
        }

        /// <summary>
        /// Применяет анимацию к каждой вершине и нормали
        /// </summary>
        protected void ApplyCurrentAnimationInRender()
        {
            switch (CurrentAnimation)
            {
                case null:
                    return;
                case RotationAnimation rotation:
                    GL.Begin(PrimitiveType.Lines);

                    GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Black);
                    GL.Normal3(Vector3.Cross(Vector3.One, rotation.Axis));
                    GL.Vertex3(-rotation.Axis.Normalized() * 2);
                    GL.Vertex3(rotation.Axis.Normalized() * 2);

                    GL.End();
                    break;
                case SymmetryAnimation symmetry:
                    GL.Begin(PrimitiveType.Lines);

                    GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color4.Black);
                    GL.Normal3(symmetry.Plane.Normal);
                    GL.Vertex3(-Vector3.Cross(-Vector3.UnitZ, symmetry.Plane.Normal).Normalized() * 3);
                    GL.Vertex3(Vector3.Cross(-Vector3.UnitZ, symmetry.Plane.Normal).Normalized() * 3);

                    GL.End();
                    break;
            }

            for (var i = 0; i < Vertices.Length; i++)
                Vertices[i] = CurrentAnimation.Apply(PrevVertices[i]);

            for (var i = 0; i < Normals.Length; i++)
            {
                Normals[i] = CurrentAnimation.Apply(PrevNormals[i]);
            }
        }

        /// <summary>
        /// Задаёт анимацию
        /// </summary>
        /// <param name="animation">новая анимация</param>
        public void SetAnimation(IAnimation animation)
        {
            CurrentAnimation = animation;
            CurrentAnimation.Reset();
            PrevVertices = (Vector3[]) Vertices.Clone();
            PrevNormals = (Vector3[]) Normals.Clone();
        }

        /// <summary>
        /// Отображает движение пространства в подстановку
        /// </summary>
        /// <param name="animation">Движение ввиде анимации</param>
        /// <returns>Подстановка</returns>
        [NotNull]
        public Permutations.Permutation ConvertAnimationToPermuation([NotNull] IAnimation animation)
        {
            var permutationBottom = new List<int>(InitVertices.Length);

            foreach (var vertex in InitVertices)
            {
                var ind = -1;
                var appliedVertex = animation.ApplyToEnd(vertex);

                for (var i = 0; i < InitVertices.Length; i++)
                {
                    if (!VectorUtils.AreVectorsEqual(appliedVertex, InitVertices[i])) continue;

                    ind = i;
                    break;
                }

                permutationBottom.Add(ind + 1);
            }

            return new Permutations.Permutation(permutationBottom);
        }

        /// <summary>
        /// Определяет рёбра по граням
        /// </summary>
        protected void InitEdgesByFaces()
        {
            var res = new List<Tuple<Vector3, Vector3>>();

            foreach (var face in Faces)
            {
                foreach (var edge in face.Edges)
                {
                    // если ещё нет ребра или перевернутого ребра, то добавить
                    if (!VectorUtils.IsVectorPairContainsInPairSequence(res, edge) &&  
                        !VectorUtils.IsVectorPairContainsInPairSequence(res,
                            new Tuple<Vector3, Vector3>(edge.Item2, edge.Item1)))
                    {
                        res.Add(edge);
                    }
                }
            }

            Edges = res.ToArray();
        }

        /// <summary>
        /// Возвращает изначальный индекс вершины, по её координатам
        /// </summary>
        /// <param name="vertex">Координаты вершины</param>
        /// <returns>Индекс, если есть вершина с такими координатами, null иначе</returns>
        [CanBeNull]
        public int? GetStarterIndexByVertex(Vector3 vertex)
        {
            for (var i = 0; i < InitVertices.Length; i++)
            {
                if (VectorUtils.AreVectorsEqual(vertex, InitVertices[i]))
                    return i;
            }

            return null;
        }

        /// <summary>
        /// Возвращает неизменяемый массив начальных вершин
        /// </summary>
        public IReadOnlyList<Vector3> ReadOnlyInitVertices => new List<Vector3>(InitVertices).AsReadOnly();

        /// <summary>
        /// Возвращает неизменяемый массив рёбер
        /// </summary>
        public IReadOnlyList<Tuple<Vector3, Vector3>> ReadOnlyEdges => new List<Tuple<Vector3, Vector3>>(Edges).AsReadOnly();

        /// <summary>
        /// Возвращает неизменяемый массив граней
        /// </summary>
        public IReadOnlyList<Face> ReadOnlyFaces => new List<Face>(Faces).AsReadOnly();

        /// <summary>
        /// Получает информацию о данной визуализации для пользователя
        /// </summary>
        public abstract string UserTutorialHtmlCode { get; }

        /// <summary>
        /// Получает поворот по индексу
        /// </summary>
        /// <param name="index">индекс</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>поворот, номер которого(среди поворотов) равен переданному индексу</returns>
        public IAnimation GetRotationByIndex(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), @"Индекс меньше 0");
            var cnt = 0;
            foreach (var anim in Animations)
            {
                if (!(anim is RotationAnimation)) continue;

                if (cnt == index)
                    return anim;

                cnt++;
            }

            throw new ArgumentOutOfRangeException(nameof(index), @"Нет столько поворотов!");
        }

        /// <summary>
        /// Находит симметрию по индексу
        /// </summary>
        /// <param name="index">индекс</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>симметрия, номер которой(среди симметрий) равен переданному индексу</returns>
        public IAnimation GetSymmetryByIndex(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), @"Индекс меньше 0");
            var cnt = 0;
            foreach (var animation in Animations)
            {
                if (!(animation is SymmetryAnimation)) continue;

                if (cnt == index)
                    return animation;

                cnt++;
            }

            throw new ArgumentOutOfRangeException(nameof(index), @"Нет столько симметрий!");
        }
    }
}