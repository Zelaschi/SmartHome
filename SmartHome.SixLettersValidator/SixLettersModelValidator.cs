using System.Text.RegularExpressions;
using ModeloValidador.Abstracciones;

namespace SmartHome.SixLettersValidator;

public sealed class SixLettersModelValidator : IModeloValidador
{
    public required string ValidatorName = "SixLettersValidator";
    public bool EsValido(Modelo modelo)
    {
        var sixLettersRegex = "^[A-Za-z]{6}$";
        return Regex.IsMatch(modelo.Value, sixLettersRegex);
    }
}
