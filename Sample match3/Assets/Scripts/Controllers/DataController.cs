#if !UNITY_EDITOR
using System.Windows.Forms;
#endif
using MatchThree.Interfaces;

/// <summary>
/// Проект билдиться не будет из-за отсутствия библиотеки System.Windows.Forms
/// </summary>
namespace MatchThree.Controllers
{
    public sealed class DataController<T> : IDataController<T> where T : class
    {
        #region PrivateVariables

        private IData<T> _loader;
#if !UNITY_EDITOR
        private OpenFileDialog _fileDialog;
        private SaveFileDialog _saveFileDialog;
#endif

        #endregion

        #region Constructor

        public DataController(IData<T> loader)
        {
            _loader = loader;

#if !UNITY_EDITOR
            _fileDialog = new OpenFileDialog();
            _fileDialog.DefaultExt = "json";
            _fileDialog.InitialDirectory = UnityEngine.Application.dataPath;

            _saveFileDialog = new SaveFileDialog();
            _saveFileDialog.DefaultExt = "json";
            _saveFileDialog.InitialDirectory = UnityEngine.Application.dataPath;

            _saveFileDialog.CreatePrompt = true;
            _saveFileDialog.OverwritePrompt = true;
#endif
        }

        #endregion

        #region PublicMethods

        /// <summary>
        /// Метод для загрузки данных
        /// </summary>
        /// <param name="data"> Загруженные данные </param>
        /// <returns></returns>
        public bool Load(out T data)
        {
            bool status = false;
#if UNITY_EDITOR
            var fileName = UnityEditor.EditorUtility.OpenFilePanel("Save as", UnityEngine.Application.dataPath, "json");

            data = null;

            if (!string.IsNullOrEmpty(fileName))
            {
                data = _loader.Load(fileName);
                status = true;
            }
#else
            var button = _fileDialog.ShowDialog();

            switch (button)
            {
                case DialogResult.OK:
                    data = _loader.Load(_fileDialog.FileName);
                    status = true;
                    break;

                case DialogResult.Cancel:
                    data = null;
                    status = false;
                    break;

                default:
                    data = null;
                    status = false;
                    break;
            }
#endif

            return status;
        }

        /// <summary>
        /// Метод сохранения данных
        /// </summary>
        /// <param name="data"> Данные для сохранения </param>
        /// <returns></returns>
        public bool Save(T data)
        {
            bool status = false;
#if UNITY_EDITOR
            var fileName = UnityEditor.EditorUtility.SaveFilePanel("Save as", "", "", "json");

            if (!string.IsNullOrEmpty(fileName))
            {
                _loader.Save(data, fileName);
                status = true;
            }
#else
            if (data == null) return false;

            
            
            var button = _saveFileDialog.ShowDialog();

            //switch (button)
            //{
            //    case DialogResult.OK:
            //        _loader.Save(data, _fileDialog.FileName);
            //        status = true;
            //        break;

            //    case DialogResult.Cancel:
            //        status = false;
            //        break;

            //    default:
            //        status = false;
            //        break;
            //}
#endif

            return status;
        }

        #endregion
    }
}
