using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Helpers.Translation
{
    public class ErrorTranslator : Translator
    {
        protected override Dictionary<string, Func<string>> Sentences => new Dictionary<string, Func<string>>
        {
            ["Passwords must have at least one non letter or digit character."]
            =
            () => "Hasło musi zawierać co najmniej jeden znak specjalny.",

            ["Passwords must have at least one digit ('0'-'9')."]
            =
            () => "Hasło musi zawierać co najmniej jedną cyfrę.",

            ["Passwords must have at least one uppercase ('A'-'Z')."]
            =
            () => "Hasło musi zawierać co najmniej jedną wielką literę.",

            ["Invalid token."] = () => "Niepoprawny token."
        };

        protected override Dictionary<string, Func<string>> Words => new Dictionary<string, Func<string>>
        {
            ["Name"] = () => "E-mail",
            ["is"] = () => "jest",
            ["already"] = () => "już",
            ["taken"] = () => "zajęty"
        };
    }
}
