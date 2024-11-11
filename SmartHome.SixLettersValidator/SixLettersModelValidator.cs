using System.Text.RegularExpressions;
using ModeloValidador.Abstracciones;

namespace SmartHome.SixLettersValidator;

public sealed class SixLettersModelValidator : IModeloValidador
{
    public required string ValidatorName = "SixLettersValidator";
    public bool EsValido(Modelo modelo)
    {
        var modelToString = modelo.ToString()!;
        return Regex.IsMatch(modelToString, "^[A-Za-z]{6}$");
    }
}
