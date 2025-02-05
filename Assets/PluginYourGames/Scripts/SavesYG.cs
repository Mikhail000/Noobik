
using System.Diagnostics;

namespace YG
{
    public partial class SavesYG
    {
        public bool[] openLevels = new bool[30];


        // Ваши сохранения

        // ...

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            openLevels[0] = true;
        }
    }
}
