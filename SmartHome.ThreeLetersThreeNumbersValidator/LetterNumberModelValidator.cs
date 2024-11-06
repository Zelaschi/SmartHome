using System.Text.RegularExpressions;
using ModeloValidador.Abstracciones;

namespace SmartHome.ThreeLetersThreeNumbersValidator;

public sealed class LetterNumberModelValidator : IModeloValidador
{
    public required string ValidatorName = "LetterNumberValidator";
    public bool EsValido(Modelo modelo)
    {
        var modelToString = modelo.ToString()!;
        return Regex.IsMatch(modelToString, "^[A-Za-z]{3}[0-9]{3}$");
    }
}
