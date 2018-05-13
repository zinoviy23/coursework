namespace WinFormCourseWork
{
    /// <summary>
    /// Класс для путей приложения
    /// </summary>
    internal static class MainFormPathes
    {
        /// <summary>
        /// Временный путь до таблицы
        /// </summary>
        public const string TablesFolderPath = @"lessons\CayleyTables";

        /// <summary>
        /// Путь до файла с деревом уроков
        /// </summary>
        public const string LessonsTreeInfoPath = @"lessons\lessonstree.xml";

        /// <summary>
        /// Путь до шаблона с визуализациями подстановок
        /// </summary>
        public const string PermutationVisualisationFilePath = @"lessons\default\permutation_visualisation.xml";

        /// <summary>
        /// Путь до шаблона калькулятора подстановок
        /// </summary>
        public const string PermutationCalculatorFilePath = @"lessons\default\permutation_calculator.xml";

        /// <summary>
        /// путь до папки со стандартными файлами
        /// </summary>
        public const string DefultFilesPath = @"lessons\default";

        /// <summary>
        /// путь до файла с шаблоном разметки для визуализации 3d
        /// </summary>
        public const string Visualisation3DMarkupFilePath = @"lessons\default\visualisation_3d.xml";

        /// <summary>
        /// Путь до файла с настройками
        /// </summary>
        public const string SettingsFilePath = @"settings.config";

        /// <summary>
        /// Путь до папки с пользователями
        /// </summary>
        public const string UserFolderPath = @"Users\";

        /// <summary>
        /// Путь до файла со списком пользователей
        /// </summary>
        public const string UsersFile = UserFolderPath + @"users.json";
    }
}