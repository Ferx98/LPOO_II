using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data; //Necesario para implementar la interfase IValueConverter
using System.Windows.Media; //Para que tome brushes y cambiar el color del rectangle, es el namespace de la clase BrushConverter
namespace Vistas
{
    public class ConversorDeEstados:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Brushes.Gray; // Color por defecto si el valor es nulo

            string estado = value.ToString();

            switch (estado)
            {
                case "programado":
                    return Brushes.Green;
                case "cancelado":
                    return Brushes.Red;
                case "en_curso":
                    return Brushes.Orange;
                default:
                    return Brushes.DodgerBlue; // color para Finalizado
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
