using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TableDriver
{
	// Version of field that uses native .net datatypes
	public class Field
	{
		public Field( string in_name, Type in_type ) {
			m_name = in_name;
			m_type = in_type;
		}

		public string Name {
			get { return m_name; }
		} string m_name;

		public Type Type {
			get { return m_type; }
		} Type m_type;
	}
}
