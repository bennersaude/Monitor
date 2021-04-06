using System;

namespace Monitor.Data.Infra.Attributes
{
    /// <summary>
	///	Atributo utilizado para remover os espaços a mais antes, no meio e depois do texto.
	/// </summary>
    [AttributeUsage(AttributeTargets.Property)]
	public class RemoveExtraSpacesAttribute : Attribute
	{
		
	}
}